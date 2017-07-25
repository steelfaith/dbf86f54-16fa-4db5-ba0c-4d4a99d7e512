using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Server.Common;

namespace Server.Storage
{
    internal static class DbUtilities
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public enum SqlErrorNumber
        {
            NoLock = 1204,
            Deadlock = 1205,
            WordbreakerTimeout = 30053,
            User = 50000
        }

        public const int DbUserErrorNumber = 50000;
        private const int TransientErrorRetryWaitMilliseconds = 500;
        public const int TransientErrorMaximumRetries = 3;
        private const int OperationWarnThresholdMilliseconds = 500;

        private static readonly HashSet<int> TransientSqlErrors = new HashSet<int>
                                                                             {
                                                                                     (int)SqlErrorNumber.NoLock,
                                                                                     (int)SqlErrorNumber.Deadlock,
                                                                                     (int)SqlErrorNumber.WordbreakerTimeout
                                                                             };

        public static readonly DateTime MinDateTime = new DateTime(1900, 1, 1);
        public static readonly byte[] MaxVersion = BitConverter.GetBytes(long.MaxValue);

        public static string ConvertBlankToNull(string input)
        {
            return !string.IsNullOrEmpty(input) && input.Trim().Length > 0 ? input : null;
        }

        public static int? ConvertNegativeOneToNull(int? input)
        {
            return input.HasValue && input == -1 ? null : input;
        }

        public static int? ConvertZeroToNull(int? input)
        {
            return input.HasValue && input == 0 ? null : input;
        }

        public static int? ConvertNegativeToNull(int? input)
        {
            return input < 0 ? null : input;
        }

        public static decimal? ConvertNegativeToNull(decimal input)
        {
            return input < 0 ? (decimal?)null : input;
        }


        private static StorageProviderException CreateStorageProviderException(string procedureName, int errorNumber, Exception innerException = null)
        {
            return new StorageProviderException($"Executing stored procedure '{procedureName}' returned error code {errorNumber}.", innerException, errorNumber);
        }

        public static Exception TranslateSqlException(DbException dbException, string procedureName = null)
        {
            // User Friendly Message
            var se = dbException as SqlException;
            int? errorCode = null;
            if (se != null)
                errorCode = se.Number;
            if (errorCode == DbUserErrorNumber)
                return new StorageProviderException(se.Message, CreateStorageProviderException(procedureName, errorCode.Value, dbException));
            // Unknown Message
            if (procedureName != null && errorCode != null)
                return CreateStorageProviderException(procedureName, errorCode.Value, dbException);
            return new StorageProviderException("Unknown storage error.", dbException);
        }

        public static T ExecuteDbTaskWithRetry<T>(Func<T> action, Func<T, int> returnValueFunc, string procedureName = null, int retryDelayMilliseconds = TransientErrorRetryWaitMilliseconds)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (returnValueFunc == null) throw new ArgumentNullException(nameof(returnValueFunc));
            int tries = 0;

            if (string.IsNullOrEmpty(procedureName))
                procedureName = "<UNKNOWN>";

            while (true)
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    tries++;
                    var result = action();
                    var returnValue = returnValueFunc(result);
                    if (returnValue == 0)
                        return result;

                    CheckReturnValue(returnValue, tries, procedureName, retryDelayMilliseconds);
                }
                catch (SqlException ex)
                {
                    CheckRetryOnException(ex, tries, procedureName, retryDelayMilliseconds);
                }
                finally
                {
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds > OperationWarnThresholdMilliseconds)
                        Logger.Warn("Database method {0} took {1} ms", procedureName, stopwatch.ElapsedMilliseconds);
                }

                Thread.Sleep(retryDelayMilliseconds);
            }
        }

        public static async Task<T> ExecuteDbTaskWithRetryAsync<T>(Func<Task<T>> taskFactory, Func<T, int> returnValueFunc, string procedureName = null, int retryDelayMilliseconds = TransientErrorRetryWaitMilliseconds)
        {
            int tries = 0;

            if (string.IsNullOrEmpty(procedureName))
                procedureName = "<UNKNOWN>";

            while (true)
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    tries++;
                    var result = await taskFactory();
                    var returnValue = returnValueFunc(result);
                    if (returnValue == 0)
                        return result;

                    CheckReturnValue(returnValue, tries, procedureName, retryDelayMilliseconds);
                }
                catch (AggregateException ex)
                {
                    // The way Insight.Database handles async tasks prevents the await keyword from correctly unwrapping exceptions.
                    // Instead we get an AggregateException, which we need to unwrap here so that our upstream exception handlers work properly.

                    var sqlEx = ex.GetBaseException() as SqlException;
                    if (sqlEx == null)
                        throw;

                    CheckRetryOnException(sqlEx, tries, procedureName, retryDelayMilliseconds);
                }
                catch (SqlException ex)
                {
                    CheckRetryOnException(ex, tries, procedureName, retryDelayMilliseconds);
                }
                finally
                {
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds > OperationWarnThresholdMilliseconds)
                        Logger.Warn("Database method {0} took {1} ms", procedureName, stopwatch.ElapsedMilliseconds);
                }

                await Task.Delay(retryDelayMilliseconds);
            }
        }

        public static T ConvertOutputStruct<T>(dynamic outputs, string procedureName, string parameterName)
                where T : struct
        {
            bool present;
            object value = GetOutputValue(outputs, parameterName, out present);
            if (value == null)
                throw new StorageProviderException(string.Format("Stored procedure '{0}' returned null for value-type OUTPUT parameter '{1}'.", procedureName, parameterName));
            CheckOutputType<T>(procedureName, parameterName, value);
            return (T)value;
        }

        public static T? ConvertOutputNullable<T>(dynamic outputs, string procedureName, string parameterName)
                where T : struct
        {
            bool present;
            object value = GetOutputValue(outputs, parameterName, out present);
            CheckOutputType<T>(procedureName, parameterName, value);
            return (T?)value;
        }

        public static T ConvertOutput<T>(dynamic outputs, string procedureName, string parameterName, bool required = false)
                where T : class
        {
            bool present;
            object value = GetOutputValue(outputs, parameterName, out present);
            if (!present && required)
                throw new StorageProviderException(string.Format("Stored procedure '{0}' did not return a value for required OUTPUT parameter '{1}'.", procedureName, parameterName));
            if (value == null && required)
                throw new StorageProviderException(string.Format("Stored procedure '{0}' returned null for required OUTPUT parameter '{1}'.", procedureName, parameterName));
            CheckOutputType<T>(procedureName, parameterName, value);
            return (T)value;
        }

        private static object GetOutputValue(dynamic outputs, string parameterName, out bool present)
        {
            object value;
            present = ((IDictionary<string, object>)outputs).TryGetValue(parameterName, out value);
            if (Convert.IsDBNull(value))
                value = null;
            return value;
        }

        private static void CheckOutputType<T>(string procedureName, string parameterName, object value)
        {
            if (value == null || value is T)
                return;
            throw new StorageProviderException(string.Format("Stored procedure '{0}' return an unexpected value type for OUTPUT parameter '{1}'.  Expected type = '{2}', Actual type = '{3}'.", procedureName, parameterName, typeof(T).Name, value.GetType().Name));
        }

        // ReSharper disable once UnusedParameter.Local
        private static void CheckRetryOnException(SqlException ex, int tries, string procedureName, int retryDelayMilliseconds)
        {
            // if it is not a transient error or we've exceeded the number of retries, then raise a translated exception
            if (!TransientSqlErrors.Contains(ex.Number) || tries > TransientErrorMaximumRetries)
                throw TranslateSqlException(ex, procedureName);
            Logger.Warn("A transient database error ({0}) has occurred in procedure {1}. The operation will be attempted again in {2}ms.", ex, ex.Number, procedureName, retryDelayMilliseconds);
        }

        // ReSharper disable once UnusedParameter.Local
        private static void CheckReturnValue(int returnValue, int tries, string procedureName, int retryDelayMilliseconds)
        {
            // if it is not a transient error or we've exceeded the number of retries, then raise an exception with the non-zero return value
            if (!TransientSqlErrors.Contains(returnValue) || tries > TransientErrorMaximumRetries)
                throw CreateStorageProviderException(procedureName, returnValue);
            Logger.Warn("A transient database error ({0}) has occurred in procedure {1}. The operation will be attempted again in {2}ms.", returnValue, procedureName, retryDelayMilliseconds);
        }
    }
}