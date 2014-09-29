using System;
using System.Text.RegularExpressions;

namespace SocketServer
{
	public class HttpRequestException : Exception {

	}

	public class HTTPRequest
	{
		// TODO Refactor to collection class
		public string[] Headers { get; private set; }
		public string Method { get; private set; }
		public string Body { get; private set; }
		public Double Version { get; private set; }
		public string Protocol { get; private set; }
		public string Path { get; private set; }
		// TODO public string Accept { get; private set; } 

		public HTTPRequest (string request) {
			this.ParseRequest (request);
		}
			
		protected void ParseRequest(string request) {

			// TODO: Get methods from another place
			var r = "^(GET|POST|PUT|DELETE|HEAD) " +
				"(https?:\\/\\/[\\-A-Za-z0-9+&@#\\/%?=~_|!:,.;]*[\\-A-Za-z0-9+&@#\\/%=~_|‌​]|\\/.*|)" +
				" (HTTP)\\/(1\\.[01]).*"; 
			var match = Regex.Match (request, r);
			if (!match.Success) {
				// TODO: Handle error
				return;
			}

			var split = Regex.Split (request, "\r\n\r\n");

			var i = split [0].IndexOf ("\r\n");

			string[] headers = Regex.Split(split [0].Substring (i, split [0].Length - i), "\r\n");

			// Set properties
			this.Headers = headers;
			this.Method = match.Groups[1].Value;
			this.Body = split [1];

			this.Version = double.Parse(match.Groups [4].Value.Replace('.',','));
			this.Protocol = match.Groups [3].Value;
			this.Path = match.Groups [2].Value;

		}
	}
}

