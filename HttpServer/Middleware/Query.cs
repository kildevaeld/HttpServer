using System;
using System.Collections.Generic;

namespace HttpServer.Middleware
{
	// Extension for HTTPREQuest
	static class QueryExtension {

		public static IReadOnlyDictionary<string,object> Query (this HTTPRequest request) {
			if (request.QueryString != null)
				return Utils.ParseQueryString (request.QueryString);

			return null;
		}

		public static object GetQuery(this HTTPRequest request, string prop) {
			if (request.QueryString == null)
				return null;

			var query = Utils.ParseQueryString (request.QueryString);
			if (query.ContainsKey (prop))
				return query [prop];
			return null;
		}

	}

	public class Query : IMiddelware
	{
		public Query ()
		{
		}

		public void Execute(HTTPRequest request, HTTPResponse response) {

		}
	}
}

