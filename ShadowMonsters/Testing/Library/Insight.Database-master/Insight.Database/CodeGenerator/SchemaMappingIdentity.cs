﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Insight.Database.Structure;

namespace Insight.Database.CodeGenerator
{
	/// <summary>
	/// An identity for the schema of a reader being mapped to a graph type.
	/// This checks all of the column names and types, as well as the type of the graph.
	/// This lets us store schemas in a dictionary and get automatic efficient storage.
	/// </summary>
	class SchemaMappingIdentity : IEquatable<SchemaMappingIdentity>
	{
		#region Fields
		/// <summary>
		/// The identity of the schema that we are mapped to.
		/// </summary>
		private SchemaIdentity _schemaIdentity;

		/// <summary>
		/// The type of mapping operation.
		/// </summary>
		private SchemaMappingType _mappingType;

		/// <summary>
		/// The hash code of this identity (precalculated).
		/// </summary>
		private int _hashCode;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the SchemaMappingIdentity class.
		/// </summary>
		/// <param name="reader">The reader to construct from.</param>
		/// <param name="recordReader">The reader to use to read the objects from the stream.</param>
		/// <param name="mappingType">The type of mapping operation.</param>
		public SchemaMappingIdentity(IDataReader reader, IRecordReader recordReader, SchemaMappingType mappingType)
			: this(new SchemaIdentity(reader), recordReader, mappingType)
		{
		}

		/// <summary>
		/// Initializes a new instance of the SchemaMappingIdentity class.
		/// </summary>
		/// <param name="schemaIdentity">The identity of the schema to map to.</param>
		/// <param name="recordReader">The reader to use to read the objects from the stream.</param>
		/// <param name="mappingType">The type of mapping operation.</param>
		public SchemaMappingIdentity(SchemaIdentity schemaIdentity, IRecordReader recordReader, SchemaMappingType mappingType)
		{
			if (recordReader == null) throw new ArgumentNullException("recordReader");

			// save the values away for later
			RecordReader = recordReader;
			_mappingType = mappingType;
			_schemaIdentity = schemaIdentity;

			// we know that we are going to store this in a hashtable, so pre-calculate the hashcode
			unchecked
			{
				// base the hashcode on the mapping type, target graph, and schema contents
				_hashCode = (int)_mappingType;
				_hashCode *= 13;
				_hashCode += RecordReader.GetHashCode();
				_hashCode *= 13;
				_hashCode += schemaIdentity.GetHashCode();
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mapping used in this mapping identity.
		/// </summary>
		internal IRecordReader RecordReader { get; private set; }
		#endregion

		#region Equality Members
		/// <summary>
		/// Returns the hash code for the identity.
		/// </summary>
		/// <returns>The hash code for the identity.</returns>
		public override int GetHashCode()
		{
			return _hashCode;
		}

		/// <summary>
		/// Determines if this is equal to another object.
		/// </summary>
		/// <param name="obj">The object to test against.</param>
		/// <returns>True if the objects are equal.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as SchemaMappingIdentity);
		}

		/// <summary>
		/// Determines if this is equal to another object.
		/// </summary>
		/// <param name="other">The object to test against.</param>
		/// <returns>True if the objects are equal.</returns>
		public bool Equals(SchemaMappingIdentity other)
		{
			if (other == null)
				return false;

			if (_mappingType != other._mappingType)
				return false;

			if (!RecordReader.Equals(other.RecordReader))
				return false;

			if (!_schemaIdentity.Equals(other._schemaIdentity))
				return false;

			return true;
		}
		#endregion
	}
}
