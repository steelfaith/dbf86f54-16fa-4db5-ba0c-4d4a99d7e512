﻿using Insight.Database;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 0649

namespace Insight.Tests
{
	/// <summary>
	/// Test class to remove some repetition from test cases.
	/// </summary>
	public class ParentTestData
	{
		public TestData TestData;
		public int ParentX;

		public static readonly string Sql = "SELECT ParentX=2, X=5 ";

		public void Verify(bool withGraph = true)
		{
			Assert.AreEqual(2, ParentX);

			if (withGraph)
			{
				Assert.IsNotNull(TestData);
				Assert.AreEqual(5, TestData.X);
			}
			else
				Assert.IsNull(TestData);
		}

		public static void Verify(IEnumerable results, bool withGraph = true)
		{
			var list = results.OfType<ParentTestData>().ToList();

			Assert.IsNotNull(results);
			Assert.AreEqual(1, list.Count);

			list[0].Verify(withGraph);
		}
	}

	/// <summary>
	/// Test class to remove some repetition from test cases.
	/// </summary>
	public class TestData
	{
		public int X;
		public int Z;
	}

	/// <summary>
	/// Test class to remove some repetition from test cases.
	/// </summary>
	public class TestData2
	{
		public int Y;

		public static readonly string Sql = "SELECT Y=7 ";

		public static void Verify(IList<TestData2> results)
		{
			Assert.IsNotNull(results);
			Assert.AreEqual(1, results.Count);

			var data = results[0];
			Assert.AreEqual(7, data.Y);
		}
	}
}
