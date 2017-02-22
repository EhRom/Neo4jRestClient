using Newtonsoft.Json;
using System.Collections.Generic;

namespace Neo4jRestClient
{
	/// <summary>
	/// Data retrieved by a statement.
	/// </summary>
    public class Data
    {
		/// <summary>
		/// Rows retrieved by the statement.
		/// </summary>
		[JsonProperty(PropertyName = "row")]
		public List<Dictionary<string, object>> Rows { get; set; }

		/// <summary>
		/// Metadate retrieved by the statement.
		/// </summary>
		[JsonProperty(PropertyName = "meta")]
		public List<Dictionary<string, object>> Meta { get; set; }
    }
}
