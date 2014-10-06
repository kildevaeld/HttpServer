using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HttpServer.Middleware;

namespace HttpServer
{
	/// <summary>
	/// Represent a route
	/// </summary>
	public class Route : IMiddelwareHandler {

		/// <summary>
		/// The regexp.
		/// </summary>
		public Regex Regexp;

		/// <summary>
		/// The route path
		/// </summary>
		public string Path;

		/// <summary>
		/// The http method
		/// </summary>
		public Methods Method;

		/// <summary>
		/// The handler.
		/// </summary>
		public MiddlewareHandler Handler;

		/// <summary>
		/// The middleware.
		/// </summary>
		public IList<MiddlewareHandler> Middleware;

		public String[] Keys;

		/// <summary>
		/// Match the specified path.
		/// </summary>
		/// <param name="path">Path.</param>
		public bool Match(string path) {
			return this.Regexp.IsMatch (path);
		}

		/// <summary>
		/// Execute the specified request and response.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="response">Response.</param>
		public void Execute(HTTPRequest request, HTTPResponse response) {
			if (!Match (request.Path) || this.Method != request.Method)
				return;

			// Get parameters
			if (this.Keys.Length > 0) {
				var match = this.Regexp.Match (request.Path);

				var dict = new Dictionary<string,object> ();
				for (var i = 0; i < this.Keys.Length; i++) {
					var key = this.Keys [i];
					dict [key] = match.Groups [i + 1].Value;
				}

				request.Data ["params"] = dict;
			}

			if (Middleware != null && Middleware.Count > 0) {
				foreach (var m in Middleware) {
					m (request, response);
					if (response.IsFinished)
						return;
				}

				if (response.IsFinished)
					return;
			}

			Handler (request, response);
			if (response.IsFinished)
				return;
		}
	}
}

