using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SocketServer
{

	public enum Methods {Get, Post, Put, Delete }

	// TODO: Should be able to store abitrary data on request to propagate down stack.
	public class HTTPRequest
	{
		#region Properties
		// Protocol 
		/// <summary>
		/// HTTP version
		/// </summary>
		/// <value>The version.</value>
		public Double Version { get; private set; }

		/// <summary>
		/// HTTP protocol
		/// </summary>
		/// <value>The protocol.</value>
		public string Protocol { get; private set; }

		/// <summary>
		/// HTTP Method
		/// </summary>
		/// <value>The method.</value>
		public Methods Method { get; private set; }

		/// <summary>
		/// The request path
		/// </summary>
		/// <value>The path.</value>
		public string Path { get; private set; }

		/// <summary>
		/// Parsed header collection
		/// </summary>
		/// <value>The headers.</value>
		public HeaderCollection Headers { get; private set; }

		/// <summary>
		/// The client's user agent.
		/// </summary>
		/// <value>The user agent.</value>
		public string UserAgent { get; private set; }

		/// <summary>
		/// Gets the body.
		/// </summary>
		/// <value>The body.</value>
		public string Body { get; private set; }
		#endregion

		// TODO public string Accept { get; private set; } 
		public string Query { get; private set; }

		public HTTPRequest () {
			this.Headers = new HeaderCollection ();
			this.Query = null;
		}

		/// <summary>
		/// Parses the request string
		/// </summary>
		/// <param name="request">Request.</param>
		// TODO: Should handle streams
		public void ParseRequest(string request) {
		

			ParseStatusLine (request);

			var split = Regex.Split (request, "\r\n\r\n");
		
			var index = split [0].IndexOf ("\r\n");
			// Is it fullform
			if (index > 0) {
				// Parse headers
				string[] headers = Regex.Split(split [0].Substring (index, split [0].Length - index), "\r\n");
				this.ParseHeaders (headers);
			}
				
			// Is there a body
			if (split.Count() == 2)
				this.Body = split [1];
			 
			if (this.Path.LastIndexOf ("?") > 0) {

				split = this.Path.Split('?');
				var q = Uri.UnescapeDataString ("?" + split [1]);

				this.Path = split [0];
				this.Query = q;
			}
		}


		protected void ParseStatusLine (string request) {
			var match = Regex.Match (request, Utils.StatusLine);

			if (!match.Success) {
				throw new HTTPException (400, HttpStatusCodes.Get (400));
			}

			Methods type = Methods.Get;
			switch (match.Groups [1].Value) {
			case "GET":
				type = Methods.Get;
				break;
			case "POST":
				type = Methods.Post;
				break;
			case "PUT":
				type = Methods.Put;
				break;
			case "DELETE":
				type = Methods.Delete;
				break;
			}

			this.Method = type;

			this.Version = double.Parse(match.Groups [4].Value.Replace('.',','));
			this.Protocol = match.Groups [3].Value;
			this.Path = match.Groups [2].Value;


		}

		protected void ParseHeaders (string[] headers) {

			foreach (var header in headers) {
				if (string.IsNullOrEmpty (header))
					continue;
				var index = header.IndexOf(":");
				var h = header.Substring (0, index);
				if (header.Length - index <= 0)
					continue;

				Headers [h] = header.Substring (index, header.Length - index)
					.TrimStart(':',' ');
			}

			var userAgent = Headers ["User-Agent"];
			if (userAgent != null) {
				this.UserAgent = userAgent;	
			}

		}
	
		public override string ToString ()
		{
			var q = this.Query;
			return string.Format ("[HTTPRequest: Headers={0}, Method={1}, Body={2}, Version={3}, Protocol={4}, Path={5}, Query={6}]", Headers, Method, Body, Version, Protocol, Path, q);
		}
	}
}

