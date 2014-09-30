using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Linq;
using System.Collections.Generic;
namespace SocketServer
{
	public class HttpRequestException : Exception {
		public int StatusCode { get; set; }
		public string StatusText { get; set; }
	}

	public class HTTPRequest
	{
		// Protocol 
		public Double Version { get; private set; }
		public string Protocol { get; private set; }
		public string Method { get; private set; }
		public string Path { get; private set; }

		public HeaderCollection Headers { get; private set; }
		public string UserAgent { get; private set; }

		public string Body { get; private set; }

		// TODO public string Accept { get; private set; } 
		public IReadOnlyDictionary<string,object> Query { get; private set; }
		public HTTPRequest () {
			this.Headers = new HeaderCollection ();
		}
			
		public void ParseRequest(string request) {

			// TODO: Get methods from another place
			// Maches: "Method filepath|uri protocol/version"
			var r = "^(GET|POST|PUT|DELETE|HEAD|PATH) " +
				"(https?:\\/\\/[\\-A-Za-z0-9+&@#\\/%?=~_|!:,.;]*[\\-A-Za-z0-9+&@#\\/%=~_|​]|\\/.*|)" +
				" (HTTP)\\/(1\\.[01]).*"; 

			var match = Regex.Match (request, r);
			if (!match.Success) {
				throw new HttpRequestException { StatusCode = 400 };
			}

			var split = Regex.Split (request, "\r\n\r\n");

			var i = split [0].IndexOf ("\r\n");

			// Parse headers
			string[] headers = Regex.Split(split [0].Substring (i, split [0].Length - i), "\r\n");
			foreach (var header in headers) {
				if (string.IsNullOrEmpty (header))
					continue;
				var index = header.IndexOf(":");
				var h = header.Substring (0, index);
				if (header.Length - index <= 0)
					continue;

				Headers [h] = header.Substring (index, header.Length - index);
			}

			var userAgent = Headers ["User-Agent"];
			if (userAgent != null) {
				this.UserAgent = userAgent;	
			}

			this.Method = match.Groups[1].Value;
			this.Body = split [1];

			this.Version = double.Parse(match.Groups [4].Value.Replace('.',','));
			this.Protocol = match.Groups [3].Value;
			this.Path = match.Groups [2].Value;

			var qi = this.Path.LastIndexOf ("?");
			if (qi > 0) {
				split = this.Path.Split('?');
				var q = Uri.UnescapeDataString ("?" + split [1]);
				this.Path = split [0];
				this.Query = Utils.ParseQueryString (q);
			}
		}

		public override string ToString ()
		{
			var q = Utils.DictinaryToString ((IDictionary<string,object>)this.Query);
			return string.Format ("[HTTPRequest: Headers={0}, Method={1}, Body={2}, Version={3}, Protocol={4}, Path={5}, Query={6}]", Headers, Method, Body, Version, Protocol, Path, q);
		}
	}
}

