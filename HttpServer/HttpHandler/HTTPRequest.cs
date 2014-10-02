using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HttpServer
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


		// TODO public string Accept { get; private set; } 
		public string QueryString { get; private set; }
		#endregion

		public HTTPRequest () {
			this.Headers = new HeaderCollection ();
		}



		#region Parsing
		public void ParseRequest(Stream stream) {

			int pos = 0;
			string header = "";
			using (var reader = new StreamReader (stream, Encoding.ASCII)) {
				while (true) { 
					var line = reader.ReadLine();
					if (line == "")
						break;
					if (line == null)
						continue;
					switch (pos) {
					case 0:
						this.ParseStatusLine (line);
						pos++;
						break;
					case 1:
						header = header + line + "\r\n";
						break;
					}

					if (reader.EndOfStream)
						break;
				}

				if (header.Length > 0) {
					this.ParseHeaders (header);

					// Only parse body, if there's a content-length
					// TODO: Body should be parsed as raw bytes, as it could be binary data. 
					// 		 multipart/form-data file upload cannot be done without.
					//		 Cannot just use the underlying stream (network),
					//       because the streamreader has read ahead.
					var cl = this.Headers ["Content-Length"];
					if (cl != null) {
						var buffer = new char[Convert.ToInt32 (cl)];
						var len = reader.Read (buffer, 0, buffer.Length);

						this.Body = new String (buffer);
					}
				} else if (!reader.EndOfStream) {
					throw new HTTPException (400, HttpStatusCodes.Get (400));
				}


			}


		}

		// TODO: Implement this without regex.
		internal int ParseStatusLine (string request) {

			var match = Regex.Match (request, Utils.StatusLine,RegexOptions.Compiled);

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

			// Check for query parameters
			if (this.Path.LastIndexOf ("?") > 0) {

				var split = this.Path.Split('?');
				var q = Uri.UnescapeDataString ("?" + split [1]);

				this.Path = split [0];
				this.QueryString = q;
			}

			return match.Groups [0].Value.Length;
		}
		// TODO: Clean parseHeaders method
		protected int ParseHeaders (string headers) {
			var sb = new StringBuilder ();

			int len = 0;
			for (var i = 0; i < headers.Length; i++, len++) {
				if (headers [i] == '\r' && headers [i + 1] == '\n') {
					var h = sb.ToString ();
					var k = h.Substring (0, h.IndexOf (":")).TrimStart('\n');
					Headers [k] = h.Substring (k.Length + 1, h.Length - k.Length - 1).Trim(' ',':');
					sb.Clear ();

				} else {
					sb.Append (headers [i]);
				}

			}

			var userAgent = Headers ["User-Agent"];
			if (userAgent != null) {
				this.UserAgent = userAgent;	
			}

			return len;

		}

		#endregion
	
		public override string ToString ()
		{
			return string.Format ("[HTTPRequest: Headers={0}, Method={1}, Body={2}, Version={3}, Protocol={4}, Path={5}, Query={6}]", Headers, Method, Body, Version, Protocol, Path, QueryString);
		}
	}
}

