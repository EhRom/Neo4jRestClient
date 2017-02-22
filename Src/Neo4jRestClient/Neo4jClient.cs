using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jRestClient
{
    /// <summary>
	/// Neo4j REST Client class. Embed the logic to call Neo4j with REST queries.
	/// </summary>
    public class Neo4jClient
    {
		/// <summary>
		/// Run a statetement collection.
		/// </summary>
		/// <param name="serverUri">URI of the Neo4j server.</param>
		/// <param name="username">Neo4j username.</param>
		/// <param name="password">Neo4j password.</param>
		/// <param name="statements">Collection of statetements to run.</param>
		/// <returns>The execution result of the statements.</returns>
		public ResultCollection Run(Uri serverUri, string username, string password, StatementCollection statements)
        {

            // TOOD gestion du contrôle des paramètres.
            return Run(serverUri, $"{username}:{password}", statements);
        }

		/// <summary>
		/// Run a statetement collection.
		/// </summary>
		/// <param name="serverUri">URI of the Neo4j server.</param>
		/// <param name="credentials">Neo4j credentials (format : {username}:{password}).</param>
		/// <param name="statements">Collection of statetements to run.</param>
		/// <returns>The execution result of the statements.</returns>
		public ResultCollection Run(Uri serverUri, string credentials, StatementCollection statements)
		{
			if (serverUri == null )
				throw new ArgumentNullException(nameof(serverUri));
			if (statements == null)
				throw new ArgumentNullException(nameof(statements));
			if (string.IsNullOrEmpty(credentials))
				throw new ArgumentNullException(nameof(credentials));

			try
			{
				ResultCollection result;
				HttpWebRequest targetRequest = WebRequest.Create(serverUri) as HttpWebRequest;
				targetRequest.Method = WebRequestMethods.Http.Post;
				targetRequest.ContentType = "application/json; charset=utf-8";
				targetRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(credentials));
				using (StreamWriter requestWriter = new StreamWriter(targetRequest.GetRequestStream()))
				{
					requestWriter.WriteAsync(JsonConvert.SerializeObject(statements));
				}
				using (HttpWebResponse response = targetRequest.GetResponse() as HttpWebResponse)
				using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
				{
					string responseJsonContent = reader.ReadToEnd();
					result = JsonConvert.DeserializeObject<ResultCollection>(responseJsonContent);
				}

				return result;
			}
			catch (Exception error)
			{
				throw new Exception($"Error while executing the statements. Neo4j Server Uri : {serverUri}", error);
			}
			
        }
		/// <summary>
		/// Run a statetement collection.
		/// </summary>
		/// <param name="serverUri">URI of the Neo4j server.</param>
		/// <param name="username">Neo4j username.</param>
		/// <param name="password">Neo4j password.</param>
		/// <param name="statements">Collection of statetements to run.</param>
		/// <returns>The execution result of the statements.</returns>
		public async Task<ResultCollection> RunAsync(Uri serverUri, string username, string password, StatementCollection statements)
        {
            return await RunAsync(serverUri, $"{username}:{password}", statements);
        }

		/// <summary>
		/// Run a statetement collection.
		/// </summary>
		/// <param name="serverUri">URI of the Neo4j server.</param>
		/// <param name="credentials">Neo4j credentials (format : {username}:{password}).</param>
		/// <param name="statements">Collection of statetements to run.</param>
		/// <returns>The execution result of the statements.</returns>
		public async Task<ResultCollection> RunAsync(Uri serverUri, string credentials, StatementCollection statements)
        {
			if (serverUri == null)
				throw new ArgumentNullException(nameof(serverUri));
			if (statements == null)
				throw new ArgumentNullException(nameof(statements));
			if (string.IsNullOrEmpty(credentials))
				throw new ArgumentNullException(nameof(credentials));


			ResultCollection result;
            HttpWebRequest targetRequest = WebRequest.Create(serverUri) as HttpWebRequest;
            targetRequest.Method = WebRequestMethods.Http.Post;
            targetRequest.ContentType = "application/json; charset=utf-8";
            targetRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(credentials));
            using (StreamWriter requestWriter = new StreamWriter(await targetRequest.GetRequestStreamAsync()))
            {
                await requestWriter.WriteAsync(JsonConvert.SerializeObject(statements));
            }
            using (HttpWebResponse response = await targetRequest.GetResponseAsync() as HttpWebResponse)
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string responseJsonContent = await reader.ReadToEndAsync();
                result = JsonConvert.DeserializeObject<ResultCollection>(responseJsonContent);
            }

            return result;
        }
    }
}
