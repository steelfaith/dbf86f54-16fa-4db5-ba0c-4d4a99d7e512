﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Database
{
	/// <summary>
	/// Wraps an IDbConnection with a retry strategy to handle transient exceptions with retry logic.
	/// </summary>
	[SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "The implementation of IDbConnection is generated code")]
	[SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "This class only implements certain members")]
	[SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Documenting the implementation of IDbConnection would be redundant without adding additional information.")]
	public class DbConnectionWrapper : DbConnection, IDbTransaction
	{
		#region Private Members
		/// <summary>
		/// Gets or sets the inner connection to use to execute the database commands.
		/// </summary>
		public DbConnection InnerConnection { get; protected internal set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the DbConnectionWrapper class.
		/// </summary>
		/// <param name="innerConnection">The inner connection to wrap.</param>
		public DbConnectionWrapper(IDbConnection innerConnection)
		{
			if (innerConnection == null) throw new ArgumentNullException("innerConnection");

			DbConnection dbConnection = innerConnection as DbConnection;

			// currently we only support wrapping DbConnection because we need the async support, etc.
			if (dbConnection == null)
				throw new ArgumentException("innerConnection must be derived from DbConnection", "innerConnection");

			InnerConnection = dbConnection;
		}

		/// <summary>
		/// Initializes a new instance of the DbConnectionWrapper class.
		/// </summary>
		protected internal DbConnectionWrapper()
		{
		}
		#endregion

		#region Core Implementation Methods
		/// <summary>
		/// Opens the database connection with retry.
		/// </summary>
		public override void Open()
		{
			InnerConnection.Open();
		}

		/// <summary>
		/// Creates a DbCommand for calls to the database.
		/// </summary>
		/// <returns>A ReliableCommand.</returns>
		protected override DbCommand CreateDbCommand()
		{
			DbCommand command = InnerConnection.CreateCommand();
			if (InnerTransaction != null)
				command.Transaction = InnerTransaction;
			return command;
		}

		/// <summary>
		/// Returns the connection as a DbConnectionWrapper, wrapping it in a new wrapper if necessary.
		/// </summary>
		/// <param name="connection">The connection to wrap.</param>
		/// <returns>The connection, possibly wrapped in a DbConnection wrapper.</returns>
		internal static DbConnectionWrapper Wrap(IDbConnection connection)
		{
			return (connection as DbConnectionWrapper) ?? new DbConnectionWrapper(connection);
		}
		#endregion

		#region IDbConnection Implementation
		/// <inheritdoc/>
		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
		{
			return InnerConnection.BeginTransaction(isolationLevel);
		}

		/// <inheritdoc/>
		protected override bool CanRaiseEvents
		{
			get
			{
				return false;
			}
		}

		/// <inheritdoc/>
		public override void ChangeDatabase(string databaseName)
		{
			InnerConnection.ChangeDatabase(databaseName);
		}

		/// <inheritdoc/>
		public override void Close()
		{
			InnerConnection.Close();
		}

		/// <inheritdoc/>
		public override string ConnectionString
		{
			get
			{
				return InnerConnection.ConnectionString;
			}

			set
			{
				InnerConnection.ConnectionString = value;
			}
		}

		/// <inheritdoc/>
		public override string ServerVersion
		{
			get { return InnerConnection.ServerVersion; }
		}

		/// <inheritdoc/>
		public override string DataSource
		{
			get { return InnerConnection.DataSource; }
		}

		/// <inheritdoc/>
		public override int ConnectionTimeout
		{
			get { return InnerConnection.ConnectionTimeout; }
		}

		/// <inheritdoc/>
		public override string Database
		{
			get { return InnerConnection.Database; }
		}

		/// <inheritdoc/>
		public override ConnectionState State
		{
			get { return InnerConnection.State; }
		}

		/// <inheritdoc/>
		public override DataTable GetSchema()
		{
			return InnerConnection.GetSchema();
		}

		/// <inheritdoc/>
		public override DataTable GetSchema(string collectionName)
		{
			return InnerConnection.GetSchema(collectionName);
		}

		/// <inheritdoc/>
		public override DataTable GetSchema(string collectionName, string[] restrictionValues)
		{
			return InnerConnection.GetSchema(collectionName, restrictionValues);
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (InnerTransaction != null)
					{
						InnerTransaction.Dispose();
						InnerTransaction = null;
					}

					if (InnerConnection != null)
					{
						InnerConnection.Close();
						InnerConnection = null;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		/// <summary>
		/// Gets the DbProviderFactory that can be used to create this connection.
		/// </summary>
		protected override DbProviderFactory DbProviderFactory
		{
			get
			{
#if !NODBASYNC && !MONO
				// get the provider for the connection
				var innerProviderFactory = DbProviderFactories.GetFactory(InnerConnection);
#else
				var innerProviderFactory = InnerConnection.GetType().GetProperty("ProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(InnerConnection, null) as DbProviderFactory;
#endif

				// give up a wrapped provider
				var type = typeof(DbConnectionWrapperProviderFactory<>).MakeGenericType(innerProviderFactory.GetType());
				return (DbProviderFactory)type.GetField("Instance").GetValue(null);
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the inner auto transaction for the connection.
		/// </summary>
		public DbTransaction InnerTransaction { get; private set; }
		#endregion

		#region IDbTransaction Members
		/// <summary>
		/// Gets the connection associated with this transaction.
		/// </summary>
		public IDbConnection Connection
		{
			get { return this; }
		}

		/// <summary>
		/// Gets the isolation level for the transaction.
		/// </summary>
		public IsolationLevel IsolationLevel
		{
			get
			{
				if (InnerTransaction == null)
					throw new InvalidOperationException("A transaction has not been created for this connection");

				return InnerTransaction.IsolationLevel;
			}
		}

		/// <summary>
		/// Commits the open transaction.
		/// </summary>
		public void Commit()
		{
			if (InnerTransaction == null)
				throw new InvalidOperationException("A transaction has not been created for this connection");

			InnerTransaction.Commit();
		}

		/// <summary>
		/// Rolls back the open transaction.
		/// </summary>
		public void Rollback()
		{
			if (InnerTransaction == null)
				throw new InvalidOperationException("A transaction has not been created for this connection");

			InnerTransaction.Rollback();
		}

		/// <summary>
		/// Begins an automatic transaction that ends when the connection is disposed.
		/// </summary>
		/// <param name="isolationLevel">The isolationLevel for the transaction.</param>
		/// <returns>This connection.</returns>
		public DbConnectionWrapper BeginAutoTransaction(IsolationLevel isolationLevel = System.Data.IsolationLevel.Unspecified)
		{
			InnerTransaction = BeginTransaction(isolationLevel);

			return this;
		}
		#endregion

#if NODBASYNC
		/// <summary>
		/// Opens a connection asynchronously.
		/// </summary>
		/// <returns>A task representing when the connection has opened.</returns>
		public Task OpenAsync()
		{
			return Task.Factory.StartNew(() => Open());
		}
#endif
	}
}
