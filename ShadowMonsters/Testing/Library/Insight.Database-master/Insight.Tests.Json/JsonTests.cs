﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Insight.Database;
using Insight.Database.Json;

namespace Insight.Tests.Json
{
	[TestFixture]
    public class JsonTests : BaseTest
    {
		public class JsonClass
		{
			[Column(SerializationMode = SerializationMode.Json)]
			public JsonSubClass SubClass;
		}

		public class JsonSubClass
		{
			public string Foo;
			public int Bar;
		}

		[Test]
		public void TestJsonNetOverride()
		{
			JsonNetObjectSerializer.Initialize();

			using (var connection = ConnectionWithTransaction())
			{
				connection.ExecuteSql("CREATE PROC ClassAsJsonNetParameter @SubClass [varchar](max) AS SELECT SubClass=@SubClass");

				var input = new JsonClass();
				input.SubClass = new JsonSubClass() { Foo = "foo", Bar = 5 };

				Assert.AreEqual("{\"Foo\":\"foo\",\"Bar\":5}", connection.Single<string>("ClassAsJsonNetParameter", input));

				var result = connection.Query<JsonClass, JsonSubClass>("ClassAsJsonNetParameter", input).First();
				Assert.IsNotNull(result.SubClass);
				Assert.AreEqual(input.SubClass.Foo, result.SubClass.Foo);
				Assert.AreEqual(input.SubClass.Bar, result.SubClass.Bar);
			}
		}

		[Test]
		public void TestNullValue()
		{
			using (var connection = ConnectionWithTransaction())
			{
				var result = connection.QuerySql<JsonClass, JsonSubClass>("SELECT SubClass=CONVERT (varchar(MAX),NULL)").First();
				Assert.IsNull(result.SubClass);
			}
		}
    }
}
