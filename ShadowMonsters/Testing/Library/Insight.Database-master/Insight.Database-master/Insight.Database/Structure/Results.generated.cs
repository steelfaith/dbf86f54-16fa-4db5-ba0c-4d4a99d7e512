﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Insight.Database.Structure;

namespace Insight.Database
{
	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2> : Results<T1>
	{
		/// <summary>
		/// Gets the second set of data returned from the database.
		/// </summary>
		public IList<T2> Set2 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3> : Results<T1, T2>
	{
		/// <summary>
		/// Gets the third set of data returned from the database.
		/// </summary>
		public IList<T3> Set3 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4> : Results<T1, T2, T3>
	{
		/// <summary>
		/// Gets the fourth set of data returned from the database.
		/// </summary>
		public IList<T4> Set4 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5> : Results<T1, T2, T3, T4>
	{
		/// <summary>
		/// Gets the fifth set of data returned from the database.
		/// </summary>
		public IList<T5> Set5 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6> : Results<T1, T2, T3, T4, T5>
	{
		/// <summary>
		/// Gets the sixth set of data returned from the database.
		/// </summary>
		public IList<T6> Set6 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7> : Results<T1, T2, T3, T4, T5, T6>
	{
		/// <summary>
		/// Gets the seventh set of data returned from the database.
		/// </summary>
		public IList<T7> Set7 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	/// <typeparam name="T8">The type of the data in the eighth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7, T8> : Results<T1, T2, T3, T4, T5, T6, T7>
	{
		/// <summary>
		/// Gets the eighth set of data returned from the database.
		/// </summary>
		public IList<T8> Set8 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	/// <typeparam name="T8">The type of the data in the eighth set of data.</typeparam>
	/// <typeparam name="T9">The type of the data in the nineth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7, T8, T9> : Results<T1, T2, T3, T4, T5, T6, T7, T8>
	{
		/// <summary>
		/// Gets the nineth set of data returned from the database.
		/// </summary>
		public IList<T9> Set9 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	/// <typeparam name="T8">The type of the data in the eighth set of data.</typeparam>
	/// <typeparam name="T9">The type of the data in the nineth set of data.</typeparam>
	/// <typeparam name="T10">The type of the data in the tenth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Results<T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{
		/// <summary>
		/// Gets the tenth set of data returned from the database.
		/// </summary>
		public IList<T10> Set10 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	/// <typeparam name="T8">The type of the data in the eighth set of data.</typeparam>
	/// <typeparam name="T9">The type of the data in the nineth set of data.</typeparam>
	/// <typeparam name="T10">The type of the data in the tenth set of data.</typeparam>
	/// <typeparam name="T11">The type of the data in the eleventh set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
	{
		/// <summary>
		/// Gets the eleventh set of data returned from the database.
		/// </summary>
		public IList<T11> Set11 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	/// <typeparam name="T8">The type of the data in the eighth set of data.</typeparam>
	/// <typeparam name="T9">The type of the data in the nineth set of data.</typeparam>
	/// <typeparam name="T10">The type of the data in the tenth set of data.</typeparam>
	/// <typeparam name="T11">The type of the data in the eleventh set of data.</typeparam>
	/// <typeparam name="T12">The type of the data in the twelfth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
	{
		/// <summary>
		/// Gets the twelfth set of data returned from the database.
		/// </summary>
		public IList<T12> Set12 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	/// <typeparam name="T8">The type of the data in the eighth set of data.</typeparam>
	/// <typeparam name="T9">The type of the data in the nineth set of data.</typeparam>
	/// <typeparam name="T10">The type of the data in the tenth set of data.</typeparam>
	/// <typeparam name="T11">The type of the data in the eleventh set of data.</typeparam>
	/// <typeparam name="T12">The type of the data in the twelfth set of data.</typeparam>
	/// <typeparam name="T13">The type of the data in the thirteenth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
	{
		/// <summary>
		/// Gets the thirteenth set of data returned from the database.
		/// </summary>
		public IList<T13> Set13 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	/// <typeparam name="T8">The type of the data in the eighth set of data.</typeparam>
	/// <typeparam name="T9">The type of the data in the nineth set of data.</typeparam>
	/// <typeparam name="T10">The type of the data in the tenth set of data.</typeparam>
	/// <typeparam name="T11">The type of the data in the eleventh set of data.</typeparam>
	/// <typeparam name="T12">The type of the data in the twelfth set of data.</typeparam>
	/// <typeparam name="T13">The type of the data in the thirteenth set of data.</typeparam>
	/// <typeparam name="T14">The type of the data in the fourteenth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
	{
		/// <summary>
		/// Gets the fourteenth set of data returned from the database.
		/// </summary>
		public IList<T14> Set14 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	/// <typeparam name="T8">The type of the data in the eighth set of data.</typeparam>
	/// <typeparam name="T9">The type of the data in the nineth set of data.</typeparam>
	/// <typeparam name="T10">The type of the data in the tenth set of data.</typeparam>
	/// <typeparam name="T11">The type of the data in the eleventh set of data.</typeparam>
	/// <typeparam name="T12">The type of the data in the twelfth set of data.</typeparam>
	/// <typeparam name="T13">The type of the data in the thirteenth set of data.</typeparam>
	/// <typeparam name="T14">The type of the data in the fourteenth set of data.</typeparam>
	/// <typeparam name="T15">The type of the data in the fifteenth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
	{
		/// <summary>
		/// Gets the fifteenth set of data returned from the database.
		/// </summary>
		public IList<T15> Set15 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.Default;
		}		  
	}

	/// <summary>
	/// Encapsulates multiple sets of data returned from the database.
	/// </summary>
	/// <typeparam name="T1">The type of the data in the first set of data.</typeparam>
	/// <typeparam name="T2">The type of the data in the second set of data.</typeparam>
	/// <typeparam name="T3">The type of the data in the third set of data.</typeparam>
	/// <typeparam name="T4">The type of the data in the fourth set of data.</typeparam>
	/// <typeparam name="T5">The type of the data in the fifth set of data.</typeparam>
	/// <typeparam name="T6">The type of the data in the sixth set of data.</typeparam>
	/// <typeparam name="T7">The type of the data in the seventh set of data.</typeparam>
	/// <typeparam name="T8">The type of the data in the eighth set of data.</typeparam>
	/// <typeparam name="T9">The type of the data in the nineth set of data.</typeparam>
	/// <typeparam name="T10">The type of the data in the tenth set of data.</typeparam>
	/// <typeparam name="T11">The type of the data in the eleventh set of data.</typeparam>
	/// <typeparam name="T12">The type of the data in the twelfth set of data.</typeparam>
	/// <typeparam name="T13">The type of the data in the thirteenth set of data.</typeparam>
	/// <typeparam name="T14">The type of the data in the fourteenth set of data.</typeparam>
	/// <typeparam name="T15">The type of the data in the fifteenth set of data.</typeparam>
	/// <typeparam name="T16">The type of the data in the sixteenth set of data.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance"), SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "The classes are related by implementing multiple generic signatures.")]
	public class Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : Results<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
	{
		/// <summary>
		/// Gets the sixteenth set of data returned from the database.
		/// </summary>
		public IList<T16> Set16 { get; internal set; }

		/// <inheritdoc/>
		public override void Read(IDbCommand command, IDataReader reader)
		{
			ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>.Default.Read(command, this, reader);
		}

		/// <inheritdoc/>
		public override Task ReadAsync(IDbCommand command, IDataReader reader, CancellationToken cancellationToken)
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>.Default.ReadAsync(command, this, reader, cancellationToken);
		}  

		/// <summary>
		/// Gets the default query reader for this class.
		/// </summary>
		/// <returns>A query reader that can read this class.</returns>
		/// <remarks>This is used by DynamicConnection</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static new IQueryReader GetReader()
		{
			return ResultsReader<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>.Default;
		}		  
	}

}