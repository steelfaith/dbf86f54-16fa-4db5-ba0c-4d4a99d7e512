﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Insight.Database.Structure;

namespace Insight.Database
{
	public static partial class Query
	{
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2> Then<T1, T2>(
			this IQueryReader<Results<T1>> previous,
			IRecordReader<T2> recordReader)
		{
			return new ResultsReader<T1, T2>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2> ThenChildren<T1, T2, TChild>(
			this ResultsReader<T1, T2> previous,
			RecordReader<TChild> recordReader,
			Action<T2, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T2, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2> ThenChildren<T1, T2, TChild, TId>(
			this ResultsReader<T1, T2> previous,
			RecordReader<TChild> recordReader,
			Func<T2, TId> id,
			Action<T2, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2> ThenChildren<T1, T2, TChild, TId>(
            this ResultsReader<T1, T2> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T2, TId> id = null,
            Action<T2, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T2, TChild, TId>(recordReader, new ChildMapper<T2, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3> Then<T1, T2, T3>(
			this IQueryReader<Results<T1, T2>> previous,
			IRecordReader<T3> recordReader)
		{
			return new ResultsReader<T1, T2, T3>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3> ThenChildren<T1, T2, T3, TChild>(
			this ResultsReader<T1, T2, T3> previous,
			RecordReader<TChild> recordReader,
			Action<T3, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T3, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3> ThenChildren<T1, T2, T3, TChild, TId>(
			this ResultsReader<T1, T2, T3> previous,
			RecordReader<TChild> recordReader,
			Func<T3, TId> id,
			Action<T3, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3> ThenChildren<T1, T2, T3, TChild, TId>(
            this ResultsReader<T1, T2, T3> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T3, TId> id = null,
            Action<T3, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T3, TChild, TId>(recordReader, new ChildMapper<T3, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4> Then<T1, T2, T3, T4>(
			this IQueryReader<Results<T1, T2, T3>> previous,
			IRecordReader<T4> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4> ThenChildren<T1, T2, T3, T4, TChild>(
			this ResultsReader<T1, T2, T3, T4> previous,
			RecordReader<TChild> recordReader,
			Action<T4, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T4, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4> ThenChildren<T1, T2, T3, T4, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4> previous,
			RecordReader<TChild> recordReader,
			Func<T4, TId> id,
			Action<T4, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4> ThenChildren<T1, T2, T3, T4, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T4, TId> id = null,
            Action<T4, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T4, TChild, TId>(recordReader, new ChildMapper<T4, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5> Then<T1, T2, T3, T4, T5>(
			this IQueryReader<Results<T1, T2, T3, T4>> previous,
			IRecordReader<T5> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5> ThenChildren<T1, T2, T3, T4, T5, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5> previous,
			RecordReader<TChild> recordReader,
			Action<T5, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T5, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5> ThenChildren<T1, T2, T3, T4, T5, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5> previous,
			RecordReader<TChild> recordReader,
			Func<T5, TId> id,
			Action<T5, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5> ThenChildren<T1, T2, T3, T4, T5, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T5, TId> id = null,
            Action<T5, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T5, TChild, TId>(recordReader, new ChildMapper<T5, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6> Then<T1, T2, T3, T4, T5, T6>(
			this IQueryReader<Results<T1, T2, T3, T4, T5>> previous,
			IRecordReader<T6> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6> ThenChildren<T1, T2, T3, T4, T5, T6, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6> previous,
			RecordReader<TChild> recordReader,
			Action<T6, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T6, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6> ThenChildren<T1, T2, T3, T4, T5, T6, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6> previous,
			RecordReader<TChild> recordReader,
			Func<T6, TId> id,
			Action<T6, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6> ThenChildren<T1, T2, T3, T4, T5, T6, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T6, TId> id = null,
            Action<T6, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T6, TChild, TId>(recordReader, new ChildMapper<T6, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7> Then<T1, T2, T3, T4, T5, T6, T7>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6>> previous,
			IRecordReader<T7> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7> ThenChildren<T1, T2, T3, T4, T5, T6, T7, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7> previous,
			RecordReader<TChild> recordReader,
			Action<T7, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T7, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7> ThenChildren<T1, T2, T3, T4, T5, T6, T7, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7> previous,
			RecordReader<TChild> recordReader,
			Func<T7, TId> id,
			Action<T7, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7> ThenChildren<T1, T2, T3, T4, T5, T6, T7, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T7, TId> id = null,
            Action<T7, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T7, TChild, TId>(recordReader, new ChildMapper<T7, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> Then<T1, T2, T3, T4, T5, T6, T7, T8>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7>> previous,
			IRecordReader<T8> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> previous,
			RecordReader<TChild> recordReader,
			Action<T8, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T8, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> previous,
			RecordReader<TChild> recordReader,
			Func<T8, TId> id,
			Action<T8, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T8, TId> id = null,
            Action<T8, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T8, TChild, TId>(recordReader, new ChildMapper<T8, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8>> previous,
			IRecordReader<T9> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> previous,
			RecordReader<TChild> recordReader,
			Action<T9, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T9, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> previous,
			RecordReader<TChild> recordReader,
			Func<T9, TId> id,
			Action<T9, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T9, TId> id = null,
            Action<T9, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T9, TChild, TId>(recordReader, new ChildMapper<T9, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>> previous,
			IRecordReader<T10> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> previous,
			RecordReader<TChild> recordReader,
			Action<T10, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T10, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> previous,
			RecordReader<TChild> recordReader,
			Func<T10, TId> id,
			Action<T10, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T10, TId> id = null,
            Action<T10, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T10, TChild, TId>(recordReader, new ChildMapper<T10, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> previous,
			IRecordReader<T11> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> previous,
			RecordReader<TChild> recordReader,
			Action<T11, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T11, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> previous,
			RecordReader<TChild> recordReader,
			Func<T11, TId> id,
			Action<T11, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T11, TId> id = null,
            Action<T11, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T11, TChild, TId>(recordReader, new ChildMapper<T11, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> previous,
			IRecordReader<T12> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> previous,
			RecordReader<TChild> recordReader,
			Action<T12, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T12, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> previous,
			RecordReader<TChild> recordReader,
			Func<T12, TId> id,
			Action<T12, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T12, TId> id = null,
            Action<T12, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T12, TChild, TId>(recordReader, new ChildMapper<T12, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> previous,
			IRecordReader<T13> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> previous,
			RecordReader<TChild> recordReader,
			Action<T13, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T13, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> previous,
			RecordReader<TChild> recordReader,
			Func<T13, TId> id,
			Action<T13, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T13, TId> id = null,
            Action<T13, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T13, TChild, TId>(recordReader, new ChildMapper<T13, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> previous,
			IRecordReader<T14> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> previous,
			RecordReader<TChild> recordReader,
			Action<T14, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T14, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> previous,
			RecordReader<TChild> recordReader,
			Func<T14, TId> id,
			Action<T14, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T14, TId> id = null,
            Action<T14, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T14, TChild, TId>(recordReader, new ChildMapper<T14, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="T15">The type of objects in the fifteenth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> previous,
			IRecordReader<T15> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="T15">The type of objects in the fifteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> previous,
			RecordReader<TChild> recordReader,
			Action<T15, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T15, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="T15">The type of objects in the fifteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> previous,
			RecordReader<TChild> recordReader,
			Func<T15, TId> id,
			Action<T15, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="T15">The type of objects in the fifteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T15, TId> id = null,
            Action<T15, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T15, TChild, TId>(recordReader, new ChildMapper<T15, TChild, TId>(id, into)));
        }

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="T15">The type of objects in the fifteenth set of results.</typeparam>
		/// <typeparam name="T16">The type of objects in the sixteenth set of results.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
			this IQueryReader<Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> previous,
			IRecordReader<T16> recordReader)
		{
			return new ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(previous, recordReader);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="T15">The type of objects in the fifteenth set of results.</typeparam>
		/// <typeparam name="T16">The type of objects in the sixteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TChild>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> previous,
			RecordReader<TChild> recordReader,
			Action<T16, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<T16, object>(), null, into);
		}

		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="T15">The type of objects in the fifteenth set of results.</typeparam>
		/// <typeparam name="T16">The type of objects in the sixteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
		public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TChild, TId>(
			this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> previous,
			RecordReader<TChild> recordReader,
			Func<T16, TId> id,
			Action<T16, List<TChild>> into = null)
		{
			if (previous == null) throw new ArgumentNullException("previous");
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			return previous.ThenChildren(recordReader.GroupByAuto<TChild, TId>(), id, into);
		}
		
		/// <summary>
		/// Extends the reader by reading another set of records.
		/// </summary>
		/// <typeparam name="T1">The type of objects in the first set of results.</typeparam>
		/// <typeparam name="T2">The type of objects in the second set of results.</typeparam>
		/// <typeparam name="T3">The type of objects in the third set of results.</typeparam>
		/// <typeparam name="T4">The type of objects in the fourth set of results.</typeparam>
		/// <typeparam name="T5">The type of objects in the fifth set of results.</typeparam>
		/// <typeparam name="T6">The type of objects in the sixth set of results.</typeparam>
		/// <typeparam name="T7">The type of objects in the seventh set of results.</typeparam>
		/// <typeparam name="T8">The type of objects in the eighth set of results.</typeparam>
		/// <typeparam name="T9">The type of objects in the nineth set of results.</typeparam>
		/// <typeparam name="T10">The type of objects in the tenth set of results.</typeparam>
		/// <typeparam name="T11">The type of objects in the eleventh set of results.</typeparam>
		/// <typeparam name="T12">The type of objects in the twelfth set of results.</typeparam>
		/// <typeparam name="T13">The type of objects in the thirteenth set of results.</typeparam>
		/// <typeparam name="T14">The type of objects in the fourteenth set of results.</typeparam>
		/// <typeparam name="T15">The type of objects in the fifteenth set of results.</typeparam>
		/// <typeparam name="T16">The type of objects in the sixteenth set of results.</typeparam>
		/// <typeparam name="TChild">The type of child record being read.</typeparam>
		/// <typeparam name="TId">The type of the ID of the parent record.</typeparam>
		/// <param name="previous">The previous reader.</param>
		/// <param name="recordReader">The mapping that defines the layout of the records in each row.</param>
		/// <param name="id">An optional function that extracts an ID from the object. Use when this row is a parent in a parent-child relationship.</param>
		/// <param name="into">A function that assigns the children to their parent.</param>
		/// <returns>A reader that reads a Results object with multiple results.</returns>
        public static ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ThenChildren<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TChild, TId>(
            this ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> previous,
            IChildRecordReader<TChild, TId> recordReader,
            Func<T16, TId> id = null,
            Action<T16, List<TChild>> into = null)
        {
            if (previous == null) throw new ArgumentNullException("previous");
            if (recordReader == null) throw new ArgumentNullException("recordReader");

            return previous.AddChild(new Children<T16, TChild, TId>(recordReader, new ChildMapper<T16, TChild, TId>(id, into)));
        }

	}
}
