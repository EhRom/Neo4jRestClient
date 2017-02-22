using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Neo4jRestClient;

namespace Tests.Neo4jRestClient
{
	[TestClass]
	public class Neo4jClientTests
	{
		private const string serviceUri = "http://localhost:7474/db/data/transaction/commit";
		private const string userName = "neo4j";
		private const string password = "p@ssw0rd";

		[TestMethod]
		public void RunTest()
		{
			DateTime now = DateTime.Now;
			var expectedResultCollection = BuildExpectedResultCollection(now);
			var acltualstatementCollection = BuildActualStatementColection(now);

			// Test the Run method.
			Neo4jClient client = new Neo4jClient();
			var actualResultCollection = client.Run(new Uri(serviceUri), userName, password, acltualstatementCollection);

			CompareResults(actualResultCollection, expectedResultCollection);
		}

		[TestMethod]
		public async Task RunAsyncTest()
		{
			DateTime now = DateTime.Now;
			var expectedResultCollection = BuildExpectedResultCollection(now);
			var acltualstatementCollection = BuildActualStatementColection(now);

			// Test the RunAsync method.
			Neo4jClient client = new Neo4jClient();
			var actualResultCollection = await client.RunAsync(new Uri(serviceUri), userName, password, acltualstatementCollection);

			CompareResults(actualResultCollection, expectedResultCollection);
		}


		private StatementCollection BuildActualStatementColection(DateTime actualDate)
		{
			return new StatementCollection
			{
				Statements = new List<Statement>
				{
					new Statement
					{
						StatementContent = "MATCH (e:UnitTest) DETACH DELETE e",
					},
					new Statement
					{
						StatementContent = "MERGE (e:UnitTest {Id: {Id}, Name: {Name}, LastUpdateDate: {LastUpdateDate}}) RETURN e",
						Parameters = new System.Collections.Generic.Dictionary<string, object>
						{
							{ "Id", 1 },
							{ "Name", "Test" },
							{ "LastUpdateDate", actualDate },
						},
					},
					new Statement
					{
						StatementContent = "MATCH (e:UnitTest) RETURN e LIMIT 25",
					},
					new Statement
					{
						StatementContent = "MATCH (e:UnitTest) aha RETURN e.aha LIMIT 25",
					}
				}
			};
		}

		private ResultCollection BuildExpectedResultCollection(DateTime expectedDate)
		{
			return new ResultCollection
			{
				Results = new List<Result>
				{
					new Result
					{
						Columns = new List<string>(),
						Data = new List<Data>(),
					},
					new Result
					{
						Columns = new List<string>
						{
							"e",
						},
						Data = new List<Data>
						{
							new Data
							{
								Rows = new List<Dictionary<string, object>>
								{
									new Dictionary<string, object>
									{
										{ "Id", 1 },
										{ "Name", "Test" },
										{ "LastUpdateDate", expectedDate }
									}
								},
								Meta = new List<Dictionary<string, object>>
								{
									new Dictionary<string, object>
									{
										{ "id", 10 },
										{ "type", "node" },
										{ "deleted", false },
									}
								}
							}
						}
					},
					new Result
					{
						Columns = new List<string>
						{
							"e",
						},
						Data = new List<Data>
						{
							new Data
							{
								 Rows = new List<Dictionary<string, object>>
								{
									new Dictionary<string, object>
									{
										{ "Id", 1 },
										{ "Name", "Test" },
										{ "LastUpdateDate", expectedDate }
									}
								},
								Meta = new List<Dictionary<string, object>>
								{
									new Dictionary<string, object>
									{
										{ "id", 10 },
										{ "type", "node" },
										{ "deleted", false },
									}
								}
							}
						}
					},
				},
				Errors = new List<Error>
				{
					new Error
					{
						Code = "Neo.ClientError.Statement.SyntaxError",
						Message = @"Invalid input 'a': expected whitespace, comment, a relationship pattern, ',', USING, WHERE, LOAD CSV, START, MATCH, UNWIND, MERGE, CREATE, SET, DELETE, REMOVE, FOREACH, WITH, CALL, RETURN, UNION, ';' or end of input (line 1, column 20 (offset: 19))
""MATCH (e:UnitTest) aha RETURN e.aha LIMIT 25""
                    ^",
					}
				}
			};
		}

		private void CompareResults(ResultCollection actualResultCollection, ResultCollection expectedResultCollection)
		{
			// Get the id of the processed element.
			if (actualResultCollection?.Results?.Count > 1 &&
				actualResultCollection.Results[1].Data?.Count > 0 &&
				actualResultCollection.Results[1].Data[0].Meta?.Count > 0 &&
				actualResultCollection.Results[1].Data[0].Meta[0]?.ContainsKey("id") == true)
			{
				expectedResultCollection.Results[1].Data[0].Meta[0]["id"] = actualResultCollection.Results[1].Data[0].Meta[0]["id"];
				expectedResultCollection.Results[2].Data[0].Meta[0]["id"] = actualResultCollection.Results[1].Data[0].Meta[0]["id"];
			}

			Assert.IsNotNull(actualResultCollection, "A result collection container is expected.");
			Assert.IsNotNull(actualResultCollection.Results, "A result collection (at least empty) is expected.");
			Assert.IsNotNull(actualResultCollection.Errors, "An error collection (at least empty) is expected.");

			Assert.AreEqual(expectedResultCollection.Results.Count, actualResultCollection.Results.Count, $"The result count ({actualResultCollection.Results.Count}) does not match the expected result count ({expectedResultCollection.Results.Count}).");
			Assert.AreEqual(expectedResultCollection.Errors.Count, actualResultCollection.Errors.Count, $"The result count ({actualResultCollection.Errors.Count}) does not match the expected result count ({expectedResultCollection.Errors.Count}).");
			
			JToken actual = JToken.FromObject(actualResultCollection);
			JToken expected = JToken.FromObject(expectedResultCollection);
			bool match = JToken.DeepEquals(expected, actual);

			Assert.IsTrue(match, $"Result does not match. Result : {actual}. Expected : {expected}");
		}
	}
}
