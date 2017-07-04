using Newtonsoft.Json;
using System.Collections.Generic;

namespace Neo4jRestClient
{
    /// <summary>
    /// Result object. Used in ResultCollection when statements run correctly.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Collection of returned columns.
        /// </summary>
        [JsonProperty(PropertyName = "columns")]
        public List<string> Columns { get; set; }

        /// <summary>
        /// Collection of returned data.
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public List<Data> Data { get; set; }
    }
}
