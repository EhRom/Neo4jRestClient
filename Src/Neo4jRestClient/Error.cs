using Newtonsoft.Json;

namespace Neo4jRestClient
{
	/// <summary>
	/// Error object. Used in ResultCollection when one or more statement throws errors.
	/// </summary>
	public class Error
	{
		/// <summary>
		/// Code of the error.
		/// </summary>
		[JsonProperty(PropertyName = "code")]
		public string Code { get; set; }

		/// <summary>
		/// Description of the error.
		/// </summary>
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }
	}
}
