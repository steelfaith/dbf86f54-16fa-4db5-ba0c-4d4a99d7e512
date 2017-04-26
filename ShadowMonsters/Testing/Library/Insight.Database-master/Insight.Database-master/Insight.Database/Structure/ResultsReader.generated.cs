﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.Database.Structure
{

	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1> : IQueryReader<Results<T1>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1> Default = new ResultsReader<T1>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T1> _listReader;
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			OneToOne<T1>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IRecordReader<T1> recordReader)
		{
			_listReader = new ListReader<T1>(recordReader);
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class from a ListReader.
		/// </summary>
		/// <param name="reader">The ListReader to convert to a ResultsReader.</param>
		public ResultsReader(ListReader<T1> reader)
		{
			_listReader = reader;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			results.SaveCommandForOutputs(command);

			// read the objects from the reader
			results.Set1 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1>> ReadAsync(IDbCommand command, Results<T1> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			results.SaveCommandForOutputs(command);

			return _listReader.ReadAsync(command, reader, cancellationToken)
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set1 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1> AddChild(Children<T1> child)
		{
			var clone = (ResultsReader<T1>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2> : IQueryReader<Results<T1, T2>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2> Default = new ResultsReader<T1, T2>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T2> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1>.Default,
			OneToOne<T2>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1>> previous,
			IRecordReader<T2> recordReader)
		{
			_previous = (ResultsReader<T1>)previous;
			_listReader = new ListReader<T2>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set2 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2>> ReadAsync(IDbCommand command, Results<T1, T2> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set2 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2> AddChild(Children<T2> child)
		{
			var clone = (ResultsReader<T1, T2>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3> : IQueryReader<Results<T1, T2, T3>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3> Default = new ResultsReader<T1, T2, T3>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T3> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2>.Default,
			OneToOne<T3>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2>> previous,
			IRecordReader<T3> recordReader)
		{
			_previous = (ResultsReader<T1, T2>)previous;
			_listReader = new ListReader<T3>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set3 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3>> ReadAsync(IDbCommand command, Results<T1, T2, T3> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set3 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3> AddChild(Children<T3> child)
		{
			var clone = (ResultsReader<T1, T2, T3>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4> : IQueryReader<Results<T1, T2, T3, T4>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4> Default = new ResultsReader<T1, T2, T3, T4>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T4> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3>.Default,
			OneToOne<T4>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3>> previous,
			IRecordReader<T4> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3>)previous;
			_listReader = new ListReader<T4>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set4 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set4 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4> AddChild(Children<T4> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5> : IQueryReader<Results<T1, T2, T3, T4, T5>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5> Default = new ResultsReader<T1, T2, T3, T4, T5>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T5> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4>.Default,
			OneToOne<T5>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4>> previous,
			IRecordReader<T5> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4>)previous;
			_listReader = new ListReader<T5>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set5 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set5 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5> AddChild(Children<T5> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6> : IQueryReader<Results<T1, T2, T3, T4, T5, T6>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6> Default = new ResultsReader<T1, T2, T3, T4, T5, T6>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T6> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5>.Default,
			OneToOne<T6>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5>> previous,
			IRecordReader<T6> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5>)previous;
			_listReader = new ListReader<T6>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set6 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set6 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6> AddChild(Children<T6> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T7> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6>.Default,
			OneToOne<T7>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6>> previous,
			IRecordReader<T7> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6>)previous;
			_listReader = new ListReader<T7>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set7 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set7 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7> AddChild(Children<T7> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T8> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6, T7> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6, T7>.Default,
			OneToOne<T8>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7>> previous,
			IRecordReader<T8> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6, T7>)previous;
			_listReader = new ListReader<T8>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7, T8>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7, T8> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7, T8>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set8 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set8 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> AddChild(Children<T8> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T9> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8>.Default,
			OneToOne<T9>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8>> previous,
			IRecordReader<T9> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8>)previous;
			_listReader = new ListReader<T9>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7, T8, T9> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set9 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set9 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> AddChild(Children<T9> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T10> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Default,
			OneToOne<T10>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>> previous,
			IRecordReader<T10> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9>)previous;
			_listReader = new ListReader<T10>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set10 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set10 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AddChild(Children<T10> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T11> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Default,
			OneToOne<T11>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> previous,
			IRecordReader<T11> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)previous;
			_listReader = new ListReader<T11>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set11 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set11 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AddChild(Children<T11> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T12> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Default,
			OneToOne<T12>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> previous,
			IRecordReader<T12> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)previous;
			_listReader = new ListReader<T12>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set12 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set12 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AddChild(Children<T12> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T13> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Default,
			OneToOne<T13>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> previous,
			IRecordReader<T13> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)previous;
			_listReader = new ListReader<T13>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set13 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set13 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> AddChild(Children<T13> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T14> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Default,
			OneToOne<T14>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> previous,
			IRecordReader<T14> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)previous;
			_listReader = new ListReader<T14>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set14 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set14 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> AddChild(Children<T14> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T15> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Default,
			OneToOne<T15>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> previous,
			IRecordReader<T15> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)previous;
			_listReader = new ListReader<T15>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set15 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set15 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> AddChild(Children<T15> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}


	/// <summary>
	/// Reads a Results structure from a data reader stream.
	/// </summary>
	public class ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
	{
		#region Fields
		/// <summary>
		/// The default reader for this type of result.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Default = new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>();

		/// <summary>
		/// The list reader used to read a set of records.
		/// </summary>
		private ListReader<T16> _listReader;
		#endregion

		/// <summary>
		/// Gets the previous reader for reading the results.
		/// </summary>
		/// <returns>The previous reader.</returns>
		private ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> _previous;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		public ResultsReader() : this(
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Default,
			OneToOne<T16>.Records)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResultsReader class.
		/// </summary>
		/// <param name="previous">The reader for the previous data set in the stream.</param>
		/// <param name="recordReader">The mapping that can read a single record from the stream.</param>
		public ResultsReader(
			IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> previous,
			IRecordReader<T16> recordReader)
		{
			_previous = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)previous;
			_listReader = new ListReader<T16>(recordReader);
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets the type of objects returned by this reader.
		/// </summary>
		/// <returns>The type of objects returned by this reader.</returns>
		public Type ReturnType { get { return typeof(Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>); } }

		/// <summary>
		/// Reads the results from the data stream.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Read(IDbCommand command, IDataReader reader)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>();
			Read(command, results, reader);

			return results;
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			var results = new Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>();
			return ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => (Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Reads the results from the data stream into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <returns>The results.</returns>
		public void Read(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> results, IDataReader reader)
		{
			if (results == null) throw new ArgumentNullException("results");

			_previous.Read(command, results, reader);

			// read the objects from the reader
			results.Set16 = _listReader.Read(command, reader);
		}

		/// <summary>
		/// Reads the results from the data stream asynchronously into a specific object.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="results">The results to read into.</param>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
		/// <returns>The results.</returns>
		public Task<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> ReadAsync(IDbCommand command, Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> results, IDataReader reader, CancellationToken cancellationToken)
		{
			if (results == null) throw new ArgumentNullException("results");

			return _previous.ReadAsync(command, results, reader, cancellationToken)
				.ContinueWith(t => { t.Wait(); return _listReader.ReadAsync(command, reader, cancellationToken); }, TaskContinuationOptions.ExecuteSynchronously)
				.Unwrap()
				.ContinueWith(
					t =>
					{
						// read the objects from the reader
						results.Set16 = t.Result;
						return results;
					},
					TaskContinuationOptions.ExecuteSynchronously);
		}

		/// <summary>
		/// Adds a child reader to this reader.
		/// </summary>
		/// <param name="child">The child reader to add.</param>
		internal ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> AddChild(Children<T16> child)
		{
			var clone = (ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>)MemberwiseClone();
			clone._listReader = _listReader.AddChild(child);
			return clone;
		}
		#endregion
	}

}
