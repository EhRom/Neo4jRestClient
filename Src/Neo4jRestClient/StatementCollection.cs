using Newtonsoft.Json;
using System.Collections.Generic;

namespace Neo4jRestClient
{
    /// <summary>
    /// Neo4j statement collection to execute.
    /// </summary>
    public class StatementCollection
    {
        /// <summary>
        /// Collection of the statements to execute.
        /// </summary>
        [JsonProperty(PropertyName = "statements")]
        public List<Statement> Statements { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatementCollection()
        {
            Statements = new List<Statement>();
        }
    }
}
