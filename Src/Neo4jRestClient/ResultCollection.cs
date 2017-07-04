using Newtonsoft.Json;
using System.Collections.Generic;

namespace Neo4jRestClient
{
    /// <summary>
	/// Result collection : contains the execution result of each statement.
	/// </summary>
    public class ResultCollection
    {
        /// <summary>
        /// List of each correct statement.
        /// </summary>
        [JsonProperty(PropertyName = "results")]
        public List<Result> Results { get; set; }

        /// <summary>
        /// List of each statement in error.
        /// </summary>
        [JsonProperty(PropertyName = "errors")]
        public List<Error> Errors { get; set; }
    }
}
