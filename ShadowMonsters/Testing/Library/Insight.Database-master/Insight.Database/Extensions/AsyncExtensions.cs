﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Insight.Database.CodeGenerator;
using Insight.Database.Providers;
using Insight.Database.Reliable;
using Insight.Database.Structure;

namespace Insight.Database
{
	/// <summary>
	/// Extension methods to support asynchronous database operations.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Documenting the internal properties reduces readability without adding additional information.")]
	public static partial class DBConnectionExtensions
	{
		#region Execute Members
		/// <summary>
		/// Create a command and execute it. This method supports auto-open.
		/// </summary>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <param name="closeConnection">True to close the connection after the query.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A data reader with the results.</returns>
		public static Task<int> ExecuteAsync(
			this IDbConnection connection,
			string sql,
			object parameters = null,
			CommandType commandType = CommandType.StoredProcedure,
			bool closeConnection = false,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return connection.ExecuteAsyncAndAutoClose(
				parameters,
				c => c.CreateCommand(sql, parameters, commandType, commandTimeout, transaction),
				false,
				(cmd, r) =>
				{
#if NODBASYNC
					// Only SqlCommand supports execute async
					var sqlCommand = cmd as System.Data.SqlClient.SqlCommand;
					if (sqlCommand != null)
						return Task<int>.Factory.FromAsync(sqlCommand.BeginExecuteNonQuery(), ar => sqlCommand.EndExecuteNonQuery(ar));
					else
						return Task<int>.Factory.StartNew(() => cmd.ExecuteNonQuery(), ct);
#else
					// DbCommand now supports async execute
					DbCommand dbCommand = cmd as DbCommand;
					if (dbCommand != null)
						return dbCommand.ExecuteNonQueryAsync(ct);
					else
						return Task<int>.Factory.StartNew(() => cmd.ExecuteNonQuery(), ct);
#endif
				},
				closeConnection,
				ct,
				outputParameters);
		}

		/// <summary>
		/// Create a command and execute it. This method supports auto-open.
		/// </summary>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="closeConnection">True to close the connection after the query.</param>
		/// <param name="commandTimeout">The timeout for the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A data reader with the results.</returns>
		public static Task<int> ExecuteSqlAsync(
			this IDbConnection connection,
			string sql,
			object parameters = null,
			bool closeConnection = false,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			return connection.ExecuteAsync(sql, parameters, CommandType.Text, closeConnection, commandTimeout, transaction, cancellationToken, outputParameters);
		}
		#endregion

		#region ExecuteScalar Members
		/// <summary>
		/// Create a command and execute it, returning the first column of the first row. This method supports auto-open.
		/// </summary>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <param name="closeConnection">True to close the connection after the query.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A data reader with the results.</returns>
		/// <typeparam name="T">The type of the data to be returned.</typeparam>
		public static Task<T> ExecuteScalarAsync<T>(
			this IDbConnection connection,
			string sql,
			object parameters = null,
			CommandType commandType = CommandType.StoredProcedure,
			bool closeConnection = false,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return connection.ExecuteAsyncAndAutoClose(
				parameters,
				c => connection.CreateCommand(sql, parameters, commandType, commandTimeout, transaction),
				false,
				(cmd, r) =>
				{
#if NODBASYNC
					// not supported in .NET 4.0
					return Task<T>.Factory.StartNew(() => ConvertScalar<T>(cmd, parameters, outputParameters, cmd.ExecuteScalar()), ct);
#else
					// DbCommand now supports async execute
					DbCommand dbCommand = cmd as DbCommand;
					if (dbCommand != null)
						return dbCommand.ExecuteScalarAsync(ct).ContinueWith(t => ConvertScalar<T>(cmd, parameters, outputParameters, t.Result), TaskContinuationOptions.ExecuteSynchronously);
					else
						return Task<T>.Factory.StartNew(() => ConvertScalar<T>(cmd, parameters, outputParameters, cmd.ExecuteScalar()), ct);
#endif
				},
				closeConnection,
				ct,
				outputParameters);
		}

		/// <summary>
		/// Create a command and execute it, returning the first column of the first row. This method supports auto-open.
		/// </summary>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="closeConnection">True to close the connection after the query.</param>
		/// <param name="commandTimeout">The timeout for the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A data reader with the results.</returns>
		/// <typeparam name="T">The type of the data to be returned.</typeparam>
		public static Task<T> ExecuteScalarSqlAsync<T>(
			this IDbConnection connection,
			string sql,
			object parameters = null,
			bool closeConnection = false,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			return connection.ExecuteScalarAsync<T>(sql, parameters, CommandType.Text, closeConnection, commandTimeout, transaction, cancellationToken, outputParameters);
		}
		#endregion

		#region Query Connection Methods
		/// <summary>
		/// Create a command, execute it, and translate the result set. This method supports auto-open.
		/// </summary>
		/// <typeparam name="T">The type of object to return from the query.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="returns">The reader to use to return the data.</param>
		/// <param name="commandType">The type of the command.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A data reader with the results.</returns>
		public static Task<T> QueryAsync<T>(
			this IDbConnection connection,
			string sql,
			object parameters,
			IQueryReader<T> returns,
			CommandType commandType = CommandType.StoredProcedure,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return connection.ExecuteAsyncAndAutoClose(
				parameters,
				c => c.CreateCommand(sql, parameters, commandType, commandTimeout, transaction),
				true,
				(cmd, r) => returns.ReadAsync(cmd, r, ct),
				commandBehavior,
				ct,
				outputParameters);
		}

		/// <summary>
		/// Create a command, execute it, and translate the result set. This method supports auto-open.
		/// </summary>
		/// <typeparam name="T">The type of object to return from the query.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="returns">The reader to use to return the data.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A data reader with the results.</returns>
		public static Task<T> QuerySqlAsync<T>(
			this IDbConnection connection,
			string sql,
			object parameters,
			IQueryReader<T> returns,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			return connection.QueryAsync(sql, parameters, returns, CommandType.Text, commandBehavior, commandTimeout, transaction, cancellationToken, outputParameters);
		}

		/// <summary>
		/// Create a command, execute it, and translate the result set. This method supports auto-open.
		/// </summary>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandType">The type of the command.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A data reader with the results.</returns>
		public static Task<IList<FastExpando>> QueryAsync(
			this IDbConnection connection,
			string sql,
			object parameters = null,
			CommandType commandType = CommandType.StoredProcedure,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			return connection.QueryAsync(sql, parameters, ListReader<FastExpando>.Default, commandType, commandBehavior, commandTimeout, transaction, cancellationToken, outputParameters);
		}

		/// <summary>
		/// Create a command, execute it, and translate the result set. This method supports auto-open.
		/// </summary>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A data reader with the results.</returns>
		public static Task<IList<FastExpando>> QuerySqlAsync(
			this IDbConnection connection,
			string sql,
			object parameters = null,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			return connection.QueryAsync(sql, parameters, ListReader<FastExpando>.Default, CommandType.Text, commandBehavior, commandTimeout, transaction, cancellationToken, outputParameters);
		}
		#endregion

		#region Query Command Methods
		/// <summary>
		/// Run a command asynchronously and return a list of objects as FastExpandos. This method supports auto-open.
		/// The Connection property of the command should be initialized before calling this method.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <param name="commandBehavior">The command behavior.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task that returns a list of objects as the result of the query.</returns>
		public static Task<IList<FastExpando>> QueryAsync(
			this IDbCommand command,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			return command.QueryAsync(ListReader<FastExpando>.Default, commandBehavior, cancellationToken, outputParameters);
		}

		/// <summary>
		/// Run a command asynchronously and return a list of objects. This method supports auto-open.
		/// The Connection property of the command should be initialized before calling this method.
		/// </summary>
		/// <typeparam name="T">The type of objects to return.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <param name="returns">The reader to use to return the data.</param>
		/// <param name="commandBehavior">The command behavior.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task that returns a list of objects as the result of the query.</returns>
		public static Task<T> QueryAsync<T>(
			this IDbCommand command,
			IQueryReader<T> returns,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			if (command == null) throw new ArgumentNullException("command");
			if (returns == null) throw new ArgumentNullException("returns");

			var ct = cancellationToken ?? CancellationToken.None;

			return command.Connection.ExecuteAsyncAndAutoClose(
				null,
				c => command,
				true,
				(cmd, r) => returns.ReadAsync(command, r, ct),
				commandBehavior.HasFlag(CommandBehavior.CloseConnection),
				ct,
				outputParameters);
		}
		#endregion

		#region Query Results Methods
		/// <summary>
		/// Asynchronously executes a query and returns a result object.
		/// </summary>
		/// <typeparam name="T">The type of the results object to return.</typeparam>
		/// <param name="connection">The connection to execute on.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameters for the query.</param>
		/// <param name="commandType">The type of the command.</param>
		/// <param name="commandBehavior">The CommandBehavior to use.</param>
		/// <param name="commandTimeout">The timeout for the command.</param>
		/// <param name="transaction">The transaction to execute in.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <param name="outputParameters">An object to write the output parameters to.</param>
		/// <returns>The result of the query.</returns>
		public static Task<T> QueryResultsAsync<T>(
			this IDbConnection connection,
			string sql,
			object parameters = null,
			CommandType commandType = CommandType.StoredProcedure,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null) where T : Results, new()
		{
			return connection.QueryAsync(
				sql,
				parameters,
				DerivedResultsReader<T>.Default,
				commandType,
				commandBehavior,
				commandTimeout,
				transaction,
				cancellationToken,
				outputParameters);
		}

		/// <summary>
		/// Asynchronously executes a query and returns a result object.
		/// </summary>
		/// <typeparam name="T">The type of the results object to return.</typeparam>
		/// <param name="connection">The connection to execute on.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameters for the query.</param>
		/// <param name="commandBehavior">The CommandBehavior to use.</param>
		/// <param name="commandTimeout">The timeout for the command.</param>
		/// <param name="transaction">The transaction to execute in.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <param name="outputParameters">An object to write the output parameters to.</param>
		/// <returns>The result of the query.</returns>
		public static Task<T> QueryResultsSqlAsync<T>(
			this IDbConnection connection,
			string sql,
			object parameters = null,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null) where T : Results, new()
		{
			return connection.QueryResultsAsync<T>(
				sql,
				parameters,
				CommandType.Text,
				commandBehavior,
				commandTimeout,
				transaction,
				cancellationToken, 
				outputParameters);
		}
		#endregion

		#region Query Reader Methods
		/// <summary>
		/// Asynchronously executes a query and performs a callback to read the data in the IDataReader.
		/// </summary>
		/// <param name="connection">The connection to execute on.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameters for the query.</param>
		/// <param name="read">The reader callback.</param>
		/// <param name="commandType">The type of the command.</param>
		/// <param name="commandBehavior">The behavior of the command.</param>
		/// <param name="commandTimeout">An optional timeout for the command.</param>
		/// <param name="transaction">An optiona transaction to participate in.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task representing the completion of the query and read operation.</returns>
		public static Task QueryAsync(
			this IDbConnection connection,
			string sql,
			object parameters,
			Action<IDataReader> read,
			CommandType commandType = CommandType.StoredProcedure,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return connection.ExecuteAsyncAndAutoClose(
				parameters,
				c => c.CreateCommand(sql, parameters, commandType, commandTimeout, transaction),
				true,
				(cmd, r) => { read(r); return Helpers.FalseTask; },
				commandBehavior.HasFlag(CommandBehavior.CloseConnection),
				ct,
				outputParameters);
		}

		/// <summary>
		/// Asynchronously executes a query and performs a callback to read the data in the IDataReader.
		/// </summary>
		/// <param name="connection">The connection to execute on.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameters for the query.</param>
		/// <param name="read">The reader callback.</param>
		/// <param name="commandBehavior">The behavior of the command.</param>
		/// <param name="commandTimeout">An optional timeout for the command.</param>
		/// <param name="transaction">An optiona transaction to participate in.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task representing the completion of the query and read operation.</returns>
		public static Task QuerySqlAsync(
			this IDbConnection connection,
			string sql,
			object parameters,
			Action<IDataReader> read,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			return connection.QueryAsync(sql, parameters, read, CommandType.Text, commandBehavior, commandTimeout, transaction, cancellationToken, outputParameters);
		}

		/// <summary>
		/// Asynchronously executes a query and performs a callback to read the data in the IDataReader.
		/// </summary>
		/// <typeparam name="T">The type returned from the reader callback.</typeparam>
		/// <param name="connection">The connection to execute on.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameters for the query.</param>
		/// <param name="read">The reader callback.</param>
		/// <param name="commandType">The type of the command.</param>
		/// <param name="commandBehavior">The behavior of the command.</param>
		/// <param name="commandTimeout">An optional timeout for the command.</param>
		/// <param name="transaction">An optiona transaction to participate in.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task representing the completion of the query and read operation.</returns>
		public static Task<T> QueryAsync<T>(
			this IDbConnection connection,
			string sql,
			object parameters,
			Func<IDataReader, T> read,
			CommandType commandType = CommandType.StoredProcedure,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return connection.ExecuteAsyncAndAutoClose(
				parameters,
				c => c.CreateCommand(sql, parameters, commandType, commandTimeout, transaction),
				true,
				(cmd, r) => Helpers.FromResult(read(r)),
				commandBehavior.HasFlag(CommandBehavior.CloseConnection),
				ct,
				outputParameters);
		}

		/// <summary>
		/// Asynchronously executes a query and performs a callback to read the data in the IDataReader.
		/// </summary>
		/// <typeparam name="T">The type returned from the reader callback.</typeparam>
		/// <param name="connection">The connection to execute on.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="parameters">The parameters for the query.</param>
		/// <param name="read">The reader callback.</param>
		/// <param name="commandBehavior">The behavior of the command.</param>
		/// <param name="commandTimeout">An optional timeout for the command.</param>
		/// <param name="transaction">An optiona transaction to participate in.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task representing the completion of the query and read operation.</returns>
		public static Task<T> QuerySqlAsync<T>(
			this IDbConnection connection,
			string sql,
			object parameters,
			Func<IDataReader, T> read,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			return connection.QueryAsync(sql, parameters, read, CommandType.Text, commandBehavior, commandTimeout, transaction, cancellationToken, outputParameters);
		}
		#endregion

		#region Translation Methods
#if NODBASYNC
		/// <summary>
		/// Chain an asynchronous data reader task with a translation to a list of objects as FastExpandos.
		/// </summary>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <returns>A task that returns the list of objects.</returns>
		public static Task<IList<FastExpando>> ToListAsync(this IDataReader reader, CancellationToken? cancellationToken = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return Task<IList<FastExpando>>.Factory.StartNew(() => reader.ToList(), ct);
		}

		/// <summary>
		/// Chain an asynchronous data reader task with a translation to a list of objects as FastExpandos.
		/// </summary>
		/// <typeparam name="T">The type of object to deserialize from the reader.</typeparam>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="recordReader">The record reader to use to read the objects.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <returns>A task that returns the list of objects.</returns>
		public static Task<IList<T>> ToListAsync<T>(this IDataReader reader, IRecordReader<T> recordReader = null, CancellationToken? cancellationToken = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return Task<IList<T>>.Factory.StartNew(() => reader.ToList(recordReader), ct);
		}
#else
		/// <summary>
		/// Chain an asynchronous data reader task with a translation to a list of objects as FastExpandos.
		/// </summary>
		/// <param name="reader">The data reader to read from.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <returns>A task that returns the list of objects.</returns>
		public static Task<IList<FastExpando>> ToListAsync(this IDataReader reader, CancellationToken? cancellationToken = null)
		{
			return reader.ToListAsync(OneToOne<FastExpando>.Records, cancellationToken);
		}

		/// <summary>
		/// Chain an asynchronous data reader task with a translation to a list of objects.
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="reader">The data reader to read from.</param>
		/// <param name="recordReader">The reader to use to read the record.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <returns>A task that returns the list of objects.</returns>
		public static Task<IList<T>> ToListAsync<T>(this IDataReader reader, IRecordReader<T> recordReader, CancellationToken? cancellationToken = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return reader.ToListAsync(recordReader, ct, firstRecordOnly: false);
		}
#endif
		
		/// <summary>
		/// Chain an asynchronous data reader task with a translation to a list of objects as FastExpandos.
		/// </summary>
		/// <typeparam name="T">The type of object to deserialize from the reader.</typeparam>
		/// <param name="task">The task returning the reader to read from.</param>
		/// <param name="recordReader">The reader to use to read the record.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <returns>A task that returns the list of objects.</returns>
		public static Task<IList<T>> ToListAsync<T>(this Task<IDataReader> task, IRecordReader<T> recordReader, CancellationToken? cancellationToken = null)
		{
			if (task == null) throw new ArgumentNullException("task");

			var ct = cancellationToken ?? CancellationToken.None;
			ct.ThrowIfCancellationRequested();

			return task.ContinueWith(reader => reader.ToListAsync(recordReader), ct).Unwrap();
		}
		#endregion

		#region Insert Methods
		/// <summary>
		/// Asynchronously executes the specified query and merges the results into the specified existing object.
		/// This is commonly used to retrieve identity values from the database upon an insert.
		/// The result set is expected to contain one record that is merged into the object upon return.
		/// </summary>
		/// <typeparam name="T">The type of the object to merge into.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="inserted">
		/// The object that is being inserted and should be merged.
		/// If null, then the results are merged into the parameters object.
		/// </param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandType">The type of the command.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task whose completion is the object after merging the results.</returns>
		public static Task<T> InsertAsync<T>(
			this IDbConnection connection,
			string sql,
			T inserted,
			object parameters = null,
			CommandType commandType = CommandType.StoredProcedure,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return connection.ExecuteAsyncAndAutoClose(
				parameters ?? inserted,
				c => c.CreateCommand(sql, parameters ?? inserted, commandType, commandTimeout, transaction),
				true,
				(cmd, r) => r.MergeAsync(inserted, cancellationToken),
				commandBehavior.HasFlag(CommandBehavior.CloseConnection),
				ct,
				outputParameters);
		}

		/// <summary>
		/// Asynchronously executes the specified query and merges the results into the specified existing object.
		/// This is commonly used to retrieve identity values from the database upon an insert.
		/// The result set is expected to contain one record that is merged into the object upon return.
		/// </summary>
		/// <typeparam name="T">The type of the object to merge into.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="inserted">
		/// The object that is being inserted and should be merged.
		/// If null, then the results are merged into the parameters object.
		/// </param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <returns>A task whose completion is the object after merging the results.</returns>
		public static Task<T> InsertSqlAsync<T>(
			this IDbConnection connection,
			string sql,
			T inserted,
			object parameters = null,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null)
		{
			return connection.InsertAsync(sql, inserted, parameters, CommandType.Text, commandBehavior, commandTimeout, transaction, cancellationToken);
		}

		/// <summary>
		/// Asynchronously executes the specified query and merges the results into the specified existing object.
		/// This is commonly used to retrieve identity values from the database upon an insert.
		/// The result set is expected to contain one record per insertedObject, in the same order as the insertedObjects.
		/// </summary>
		/// <typeparam name="T">The type of the object to merge into.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="inserted">
		/// The list of objects that is being inserted and should be merged.
		/// If null, then the results are merged into the parameters object.
		/// </param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandType">The type of the command.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task whose completion is the list of objects after merging the results.</returns>
		public static Task<IEnumerable<T>> InsertListAsync<T>(
			this IDbConnection connection,
			string sql,
			IEnumerable<T> inserted,
			object parameters = null,
			CommandType commandType = CommandType.StoredProcedure,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return connection.ExecuteAsyncAndAutoClose(
				parameters ?? inserted,
				c => c.CreateCommand(sql, parameters ?? inserted, commandType, commandTimeout, transaction),
				true,
				(cmd, r) => r.MergeAsync(inserted, cancellationToken),
				commandBehavior.HasFlag(CommandBehavior.CloseConnection),
				ct,
				outputParameters);
		}

		/// <summary>
		/// Asynchronously executes the specified query and merges the results into the specified existing object.
		/// This is commonly used to retrieve identity values from the database upon an insert.
		/// The result set is expected to contain one record per insertedObject, in the same order as the insertedObjects.
		/// </summary>
		/// <typeparam name="T">The type of the object to merge into.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="inserted">
		/// The list of objects that is being inserted and should be merged.
		/// If null, then the results are merged into the parameters object.
		/// </param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <returns>A task whose completion is the list of objects after merging the results.</returns>
		public static Task<IEnumerable<T>> InsertListSqlAsync<T>(
			this IDbConnection connection,
			string sql,
			IEnumerable<T> inserted,
			object parameters = null,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null)
		{
			return connection.InsertListAsync(sql, inserted, parameters, CommandType.Text, commandBehavior, commandTimeout, transaction, cancellationToken);
		}
		#endregion

		#region QueryOnto Methods
		/// <summary>
		/// Asynchronously executes the specified query and merges the results into the specified existing object.
		/// This is the same behavior as InsertAsync.
		/// </summary>
		/// <typeparam name="T">The type of the object to merge into.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="onto">
		/// The list of objects to be merged onto.
		/// If null, then the results are merged into the parameters object.
		/// </param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandType">The type of the command.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <returns>A task whose completion is the object after merging the results.</returns>
		public static Task<T> QueryOntoAsync<T>(
			this IDbConnection connection,
			string sql,
			T onto,
			object parameters = null,
			CommandType commandType = CommandType.StoredProcedure,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null)
		{
			return connection.InsertAsync(sql, onto, parameters, commandType, commandBehavior, commandTimeout, transaction, cancellationToken);
		}

		/// <summary>
		/// Asynchronously executes the specified query and merges the results into the specified existing object.
		/// This is the same behavior as InsertAsync.
		/// </summary>
		/// <typeparam name="T">The type of the object to merge into.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="onto">
		/// The list of objects to be merged onto.
		/// If null, then the results are merged into the parameters object.
		/// </param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <returns>A task whose completion is the object after merging the results.</returns>
		public static Task<T> QueryOntoSqlAsync<T>(
			this IDbConnection connection,
			string sql,
			T onto,
			object parameters = null,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null)
		{
			return connection.InsertSqlAsync(sql, onto, parameters, commandBehavior, commandTimeout, transaction, cancellationToken);
		}

		/// <summary>
		/// Asynchronously executes the specified query and merges the results into the specified existing object.
		/// This is the same behavior as InsertAsync.
		/// </summary>
		/// <typeparam name="T">The type of the object to merge into.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="onto">
		/// The list of objects to be merged onto.
		/// If null, then the results are merged into the parameters object.
		/// </param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandType">The type of the command.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <returns>A task whose completion is the list of objects after merging the results.</returns>
		public static Task<IEnumerable<T>> QueryOntoListAsync<T>(
			this IDbConnection connection,
			string sql,
			IEnumerable<T> onto,
			object parameters = null,
			CommandType commandType = CommandType.StoredProcedure,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null)
		{
			return connection.InsertListAsync(sql, onto, parameters, commandType, commandBehavior, commandTimeout, transaction, cancellationToken);
		}

		/// <summary>
		/// Asynchronously executes the specified query and merges the results into the specified existing object.
		/// This is the same behavior as InsertAsync.
		/// </summary>
		/// <typeparam name="T">The type of the object to merge into.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="sql">The sql to execute.</param>
		/// <param name="onto">
		/// The list of objects to be merged onto.
		/// If null, then the results are merged into the parameters object.
		/// </param>
		/// <param name="parameters">The parameter to pass.</param>
		/// <param name="commandBehavior">The behavior of the command when executed.</param>
		/// <param name="commandTimeout">The timeout of the command.</param>
		/// <param name="transaction">The transaction to participate in it.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <returns>A task whose completion is the list of objects after merging the results.</returns>
		public static Task<IEnumerable<T>> QueryOntoListSqlAsync<T>(
			this IDbConnection connection,
			string sql,
			IEnumerable<T> onto,
			object parameters = null,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			int? commandTimeout = null,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null)
		{
			return connection.InsertListSqlAsync(sql, onto, parameters, commandBehavior, commandTimeout, transaction, cancellationToken);
		}
		#endregion

		#region Merge Methods
#if NODBASYNC
		/// <summary>
		/// Merges the results of a recordset into an existing object.
		/// </summary>
		/// <typeparam name="T">The type of object to merge into.</typeparam>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="item">The item to merge into.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <returns>The item that has been merged.</returns>
		/// <remarks>
		/// This method reads a single record from the reader and overwrites the values of the object.
		/// The reader is then advanced to the next result or disposed.
		/// To merge multiple records from the reader, pass an IEnumerable&lt;T&gt; to the method.
		/// </remarks>
		public static Task<T> MergeAsync<T>(this IDataReader reader, T item, CancellationToken? cancellationToken = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return Task<T>.Factory.StartNew(() => reader.Merge(item), ct);
		}

		/// <summary>
		/// Merges the results of a recordset into an existing object.
		/// </summary>
		/// <typeparam name="T">The type of object to merge into.</typeparam>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="items">The list of items to merge into.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <returns>The item that has been merged.</returns>
		/// <remarks>
		/// This method reads a single record from the reader and overwrites the values of the object.
		/// The reader is then advanced to the next result or disposed.
		/// To merge multiple records from the reader, pass an IEnumerable&lt;T&gt; to the method.
		/// </remarks>
		public static Task<IEnumerable<T>> MergeAsync<T>(this IDataReader reader, IEnumerable<T> items, CancellationToken? cancellationToken = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;

			return Task<IEnumerable<T>>.Factory.StartNew(() => reader.Merge(items), ct);
		}
#else
		/// <summary>
		/// Merges the results of a recordset into an existing object.
		/// </summary>
		/// <typeparam name="T">The type of object to merge into.</typeparam>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="item">The item to merge into.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <returns>The item that has been merged.</returns>
		/// <remarks>
		/// This method reads a single record from the reader and overwrites the values of the object.
		/// The reader is then advanced to the next result or disposed.
		/// To merge multiple records from the reader, pass an IEnumerable&lt;T&gt; to the method.
		/// </remarks>
		public static async Task<T> MergeAsync<T>(this IDataReader reader, T item, CancellationToken? cancellationToken = null)
		{
			await reader.MergeAsync<T>(new T[] { item }, cancellationToken).ConfigureAwait(false);

			return item;
		}

		/// <summary>
		/// Merges the results of a recordset into an existing object.
		/// </summary>
		/// <typeparam name="T">The type of object to merge into.</typeparam>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="items">The list of items to merge into.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <returns>The item that has been merged.</returns>
		/// <remarks>
		/// This method reads a single record from the reader and overwrites the values of the object.
		/// The reader is then advanced to the next result or disposed.
		/// To merge multiple records from the reader, pass an IEnumerable&lt;T&gt; to the method.
		/// </remarks>
		public static async Task<IEnumerable<T>> MergeAsync<T>(this IDataReader reader, IEnumerable<T> items, CancellationToken? cancellationToken = null)
		{
			var ct = cancellationToken ?? CancellationToken.None;
			ct.ThrowIfCancellationRequested();

			bool moreResults = false;

			try
			{
				// see if the reader support async reads
				DbDataReader dbReader = reader as DbDataReader;
				if (dbReader == null)
					return reader.Merge(items);

				var merger = DbReaderDeserializer.GetMerger<T>(reader);

				// read the identities of each item from the recordset and merge them into the objects
				foreach (T item in items)
				{
					await dbReader.ReadAsync(ct).ConfigureAwait(false);

					ct.ThrowIfCancellationRequested();

					merger(reader, item);
				}

				// we are done with this result set, so move onto the next or clean up the reader
				moreResults = await dbReader.NextResultAsync(ct).ConfigureAwait(false);

				return items;
			}
			finally
			{
				if (!moreResults)
					reader.Dispose();
			}
		}
#endif
		#endregion

		#region GetReader Methods
		/// <summary>
		/// Executes a command and returns a task that generates a SqlDataReader. This method does not support auto-open.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <param name="commandBehavior">The behavior for the command.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <returns>A task that returns a SqlDataReader upon completion.</returns>
		public static Task<IDataReader> GetReaderAsync(this IDbCommand command, CommandBehavior commandBehavior, CancellationToken cancellationToken)
		{
#if NODBASYNC
			// Only SqlCommand supports async
			var sqlCommand = command as System.Data.SqlClient.SqlCommand;
			if (sqlCommand != null)
				return Task<IDataReader>.Factory.FromAsync(sqlCommand.BeginExecuteReader(commandBehavior), ar => sqlCommand.EndExecuteReader(ar));

			// allow reliable commands to handle the icky task logic
			ReliableCommand reliableCommand = command as ReliableCommand;
			if (reliableCommand != null)
				return reliableCommand.GetReaderAsync(commandBehavior, cancellationToken);
#else
			// DbCommand now supports async
			DbCommand dbCommand = command as DbCommand;
			if (dbCommand != null)
				return dbCommand.ExecuteReaderAsync(commandBehavior, cancellationToken).ContinueWith(t => (IDataReader)t.Result, TaskContinuationOptions.ExecuteSynchronously);
#endif

			// the command doesn't support async so stick it in a dumb task
			return Task<IDataReader>.Factory.StartNew(() => command.ExecuteReader(commandBehavior), cancellationToken);
		}
		#endregion

		#region BulkCopy Methods
		/// <summary>
		/// Bulk copy a list of objects to the server. This method supports auto-open.
		/// </summary>
		/// <typeparam name="T">The type of the objects.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="tableName">The name of the table.</param>
		/// <param name="list">The list of objects.</param>
		/// <param name="configure">A callback for initialization of the BulkCopy object. The object is provider-dependent.</param>
		/// <param name="closeConnection">True to close the connection when complete.</param>
		/// <param name="options">The options to use for the bulk copy.</param>
		/// <param name="transaction">An optional external transaction.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>Number of rows copied.</returns>
		public static Task<int> BulkCopyAsync<T>(
			this IDbConnection connection,
			string tableName,
			IEnumerable<T> list,
			Action<InsightBulkCopy> configure = null,
			bool closeConnection = false,
			InsightBulkCopyOptions options = InsightBulkCopyOptions.Default,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null)
		{
			var reader = GetObjectReader(connection, tableName, transaction, list);

			return connection.BulkCopyAsync(
				tableName,
				reader,
				configure,
				closeConnection,
				options,
				transaction,
				cancellationToken ?? CancellationToken.None);
		}

		/// <summary>
		/// Bulk copy a list of objects to the server. This method supports auto-open.
		/// </summary>
		/// <param name="connection">The connection to use.</param>
		/// <param name="tableName">The name of the table.</param>
		/// <param name="source">The list of data to read.</param>
		/// <param name="configure">A callback for initialization of the BulkCopy object. The object is provider-dependent.</param>
		/// <param name="closeConnection">True to close the connection when complete.</param>
		/// <param name="options">The options to use for the bulk copy.</param>
		/// <param name="transaction">An optional external transaction.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>Number of rows copied.</returns>
		public static Task<int> BulkCopyAsync(
			this IDbConnection connection,
			string tableName,
			IDataReader source,
			Action<InsightBulkCopy> configure = null,
			bool closeConnection = false,
			InsightBulkCopyOptions options = InsightBulkCopyOptions.Default,
			IDbTransaction transaction = null,
			CancellationToken? cancellationToken = null)
		{
			if (source == null) throw new ArgumentNullException("source");

			// see if there are any invalid bulk copy options set
			var provider = InsightDbProvider.For(connection);
			var invalidOptions = (options & ~(provider.GetSupportedBulkCopyOptions(connection)));
			if (invalidOptions != 0)
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "BulkCopyOption {0} is not supported for this provider", invalidOptions));

			return connection.ExecuteAsyncAndAutoClose(
					closeConnection,
					(c, ct) =>
					{
						return provider.BulkCopyAsync(connection, tableName, source, configure, options, transaction, ct)
							.ContinueWith(
								t =>
								{
									source.Dispose();
									t.Wait();
									return source.RecordsAffected;
								},
								TaskContinuationOptions.ExecuteSynchronously);
					},
					cancellationToken ?? CancellationToken.None);
		}
		#endregion

		#region Helper Methods
#if NODBASYNC
		/// <summary>
		/// Chain an asynchronous data reader task with a translation to a list of objects.
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="reader">The data reader to read from.</param>
		/// <param name="recordReader">The reader to use to read the record.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <param name="firstRecordOnly">True to only read in the first record (for Single cases).</param>
		/// <returns>A task that returns the list of objects.</returns>
		internal static Task<IList<T>> ToListAsync<T>(this IDataReader reader, IRecordReader<T> recordReader, CancellationToken cancellationToken, bool firstRecordOnly)
		{
			return Task<IList<T>>.Factory.StartNew(() => reader.ToList<T>(recordReader), cancellationToken);
		}
#else
		/// <summary>
		/// Chain an asynchronous data reader task with a translation to a list of objects.
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="reader">The data reader to read from.</param>
		/// <param name="recordReader">The reader to use to read the record.</param>
		/// <param name="cancellationToken">The cancellationToken to use for the operation.</param>
		/// <param name="firstRecordOnly">True to only read in the first record (for Single cases).</param>
		/// <returns>A task that returns the list of objects.</returns>
		internal static async Task<IList<T>> ToListAsync<T>(this IDataReader reader, IRecordReader<T> recordReader, CancellationToken cancellationToken, bool firstRecordOnly)
		{
			cancellationToken.ThrowIfCancellationRequested();

			// only DbReader supports async read
			DbDataReader dbReader = reader as DbDataReader;
			if (dbReader == null)
				return reader.ToList<T>(recordReader);

			bool moreResults = false;
			IList<T> list = new List<T>();

			// if the reader is already closed, then return an empty list
			if (dbReader.IsClosed)
				return list;

			try
			{
				var mapper = recordReader.GetRecordReader(reader);

				// read in all of the records
				while (await dbReader.ReadAsync(cancellationToken).ConfigureAwait(false))
				{
					list.Add(mapper(dbReader));

					// if we only want the first record in the set, then skip the rest
					if (firstRecordOnly)
						break;

					// allow cancellation to occur within the list processing
					cancellationToken.ThrowIfCancellationRequested();
				}

				// move to the next result set - the token should already be here
				moreResults = await dbReader.NextResultAsync(cancellationToken).ConfigureAwait(false);

				return list;
			}
			finally
			{
				// clean up the reader unless there are more results
				if (!moreResults)
					reader.Dispose();
			}
		}
#endif

		/// <summary>
		/// Execute an asynchronous action, ensuring that the connection is auto-closed.
		/// </summary>
		/// <typeparam name="T">The return type of the task.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="parameters">The parameters for the call.</param>
		/// <param name="getCommand">An action to perform to get the command to execute.</param>
		/// <param name="callGetReader">True to automatically get the reader from the command.</param>
		/// <param name="translate">An action to perform to translate the reader into results.</param>
		/// <param name="closeConnection">True to force the connection to close after the operation completes.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task that returns the result of the command after closing the connection.</returns>
		private static Task<T> ExecuteAsyncAndAutoClose<T>(
			this IDbConnection connection,
			object parameters,
			Func<IDbConnection, IDbCommand> getCommand,
			bool callGetReader,
			Func<IDbCommand, IDataReader, Task<T>> translate,
			bool closeConnection,
			CancellationToken cancellationToken,
			object outputParameters)
		{
			return connection.ExecuteAsyncAndAutoClose<T>(
				parameters,
				getCommand,
				callGetReader,
				translate,
				closeConnection ? CommandBehavior.CloseConnection : CommandBehavior.Default,
				cancellationToken,
				outputParameters);
		}

		/// <summary>
		/// Execute an asynchronous action, ensuring that the connection is auto-closed.
		/// </summary>
		/// <typeparam name="T">The return type of the task.</typeparam>
		/// <param name="connection">The connection to use.</param>
		/// <param name="parameters">The parameters for the call.</param>
		/// <param name="getCommand">An action to perform to get the command to execute.</param>
		/// <param name="callGetReader">True to automatically call GetReader on the command.</param>
		/// <param name="translate">An action to perform to translate the reader into results.</param>
		/// <param name="commandBehavior">The CommandBehavior to use to execute the command.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <param name="outputParameters">An optional additional object to output parameters onto.</param>
		/// <returns>A task that returns the result of the command after closing the connection.</returns>
		private static Task<T> ExecuteAsyncAndAutoClose<T>(
			this IDbConnection connection,
			object parameters,
			Func<IDbConnection, IDbCommand> getCommand,
			bool callGetReader,
			Func<IDbCommand, IDataReader, Task<T>> translate,
			CommandBehavior commandBehavior,
			CancellationToken cancellationToken,
			object outputParameters)
		{
			bool closeConnection = commandBehavior.HasFlag(CommandBehavior.CloseConnection);
			IDbCommand command = null;
			IDataReader reader = null;

			return AutoOpenAsync(connection, cancellationToken)
				.ContinueWith(
					t =>
					{
						// this needs to run even if the open has been cancelled so we don't leak a connection
						closeConnection |= t.Result;

						// if the operation has been cancelled, throw after we know that the connection has been opened
						// but before taking the action
						cancellationToken.ThrowIfCancellationRequested();

						// get the command
						command = getCommand(connection);

						// if we have a command, execute it
						if (command != null && callGetReader)
							return command.GetReaderAsync(commandBehavior | CommandBehavior.SequentialAccess, cancellationToken);

						return Helpers.FromResult<IDataReader>(null);
					},
					TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						reader = t.Result;
						return translate(command, reader);
					},
					TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						if (!t.IsFaulted && !t.IsCanceled && command != null)
						{
							// make sure we go to the end so we can get the outputs
							if (reader != null && !reader.IsClosed)
								while (reader.NextResult());

							command.OutputParameters(parameters, outputParameters);
						}

						return t.Result;
					},
					TaskContinuationOptions.ExecuteSynchronously)
				.ContinueWith(
					t =>
					{
						if (reader != null)
							reader.Dispose();

						// close before accessing the result so we can guarantee that the connection doesn't leak
						if (closeConnection)
							connection.Close();

						return t.Result;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		private static Task<T> ExecuteAsyncAndAutoClose<T>(
			this IDbConnection connection,
			bool closeConnection,
			Func<IDbConnection, CancellationToken, Task<T>> action,
			CancellationToken cancellationToken)
		{
			return AutoOpenAsync(connection, cancellationToken)
				.ContinueWith(
					t =>
					{
						// this needs to run even if the open has been cancelled so we don't leak a connection
						closeConnection |= t.Result;

						// if the operation has been cancelled, throw after we know that the connection has been opened
						// but before taking the action
						cancellationToken.ThrowIfCancellationRequested();

						return action(connection, cancellationToken);
					},
					TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// close before accessing the result so we can guarantee that the connection doesn't leak
						if (closeConnection)
							connection.Close();

						return t.Result;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Detect if a connection needs to be automatically opened and closed.
		/// </summary>
		/// <param name="connection">The connection to test.</param>
		/// <param name="cancellationToken">The CancellationToken to use for the operation or null to not use cancellation.</param>
		/// <returns>
		/// A task representing the completion of the open operation
		/// and a flag indicating whether the connection should be closed at the end of the operation.
		/// </returns>
		private static Task<bool> AutoOpenAsync(IDbConnection connection, CancellationToken cancellationToken)
		{
			// if the connection is already open, then it doesn't need to be opened or closed.
			if (connection.State == ConnectionState.Open)
				return Helpers.FalseTask;

#if !NODBASYNC
			// open the connection and plan to close it
			DbConnection dbConnection = connection as DbConnection;
			if (dbConnection != null)
			{
				return dbConnection.OpenAsync(cancellationToken).ContinueWith(
					t =>
					{
						// call wait on the task to re-throw any connection errors
						// otherwise we just get a task cancelled error
						t.Wait();

						return dbConnection.State == ConnectionState.Open;
					},
					TaskContinuationOptions.ExecuteSynchronously);
			}
#endif

			// we don't have an asynchronous open method, so do it synchronously in a task
			return Task<bool>.Factory.StartNew(
				() =>
				{
					// synchronous open is not cancellable on its own
					connection.Open();

					// since we opened the connection, we should close it
					return true;
				},
				cancellationToken);
		}

		/// <summary>
		/// Lets us call QueryCoreAsync into a simple delegate for dynamic calls.
		/// </summary>
		/// <typeparam name="T">The type of object returned.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <param name="returns">The definition of the return structure.</param>
		/// <param name="commandBehavior">The commandBehavior to use.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <param name="outputParameters">Optional output parameters.</param>
		/// <returns>The result of the query.</returns>
		private static Task<T> QueryCoreAsyncUntyped<T>(
			this IDbCommand command,
			IQueryReader returns,
			CommandBehavior commandBehavior = CommandBehavior.Default,
			CancellationToken? cancellationToken = null,
			object outputParameters = null)
		{
			// this method lets us convert QueryCoreAsync to a delegate for dynamic calls
			return command.QueryAsync<T>((IQueryReader<T>)returns, commandBehavior, cancellationToken, outputParameters);
		}
		#endregion
	}
}
