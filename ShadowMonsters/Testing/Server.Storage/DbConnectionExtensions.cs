using System;
using System.Data;
using System.Threading.Tasks;
using Insight.Database;

namespace Server.Storage
{
    internal static class DbConnectionExtensions
    {
        static DbConnectionExtensions()
        {
            SqlInsightDbProvider.RegisterProvider();
        }

        public static async Task<dynamic> ExecuteSimpleProcedureAsync(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            dynamic output = new FastExpando();
            await DbUtilities.ExecuteDbTaskWithRetryAsync(() => connection.ExecuteAsync(procedureName, input, outputParameters: (object)output, commandTimeout: commandTimeout), r => GetReturnValue(output), procedureName);
            return output;
        }

        public static dynamic ExecuteSimpleProcedure(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            dynamic output = new FastExpando();
            DbUtilities.ExecuteDbTaskWithRetry(() => connection.Execute(procedureName, input, outputParameters: (object)output, commandTimeout: commandTimeout), r => GetReturnValue(output), procedureName);
            return output;
        }

        public static Task<Results<T1>> ExecuteProcedureAsync<T1>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<Results<T1>>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2>> ExecuteProcedureAsync<T1, T2>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3>> ExecuteProcedureAsync<T1, T2, T3>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4>> ExecuteProcedureAsync<T1, T2, T3, T4>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5>> ExecuteProcedureAsync<T1, T2, T3, T4, T5>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7, T8>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7, T8>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> ExecuteProcedureAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResultsAsync(procedureName, () => connection.QueryResultsAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1> ExecuteProcedure<T1>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<Results<T1>>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2> ExecuteProcedure<T1, T2>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3> ExecuteProcedure<T1, T2, T3>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4> ExecuteProcedure<T1, T2, T3, T4>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5> ExecuteProcedure<T1, T2, T3, T4, T5>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6> ExecuteProcedure<T1, T2, T3, T4, T5, T6>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7, T8> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7, T8>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7, T8>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7, T8, T9> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7, T8, T9>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(procedureName, input, commandTimeout: commandTimeout));
        }

        public static Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ExecuteProcedure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IDbConnection connection, string procedureName, object input = null, int? commandTimeout = null)
        {
            return QueryResults(procedureName, () => connection.QueryResults<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(procedureName, input, commandTimeout: commandTimeout));
        }

        private static TResult QueryResults<TResult>(string procedureName, Func<TResult> action)
            where TResult : Results
        {
            return DbUtilities.ExecuteDbTaskWithRetry(action, r => (int)GetReturnValue(r.Outputs), procedureName);
        }

        private static Task<TResult> QueryResultsAsync<TResult>(string procedureName, Func<Task<TResult>> task)
            where TResult : Results
        {
            return DbUtilities.ExecuteDbTaskWithRetryAsync(task, r => (int)GetReturnValue(r.Outputs), procedureName);
        }

        private static int GetReturnValue(dynamic outputs)
        {
            return outputs.Return_Value;
        }
    }
}