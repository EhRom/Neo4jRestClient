using Newtonsoft.Json;
using System.Collections.Generic;

namespace Neo4jRestClient
{
    /// <summary>
	/// Neo4j statement to execute.
	/// </summary>
    public class Statement
    {
		/// <summary>
		/// Statement to execute
		/// </summary>
		[JsonProperty(PropertyName = "statement")]
		public string StatementContent { get; set; }

		/// <summary>
		/// Dictionary of each parameter used in the statement and the corresponding value.
		/// </summary>
		[JsonProperty(PropertyName = "parameters")]
		public Dictionary<string, object> Parameters { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
        public Statement()
        {
            Parameters = new Dictionary<string, object>();
        }
    }
}
