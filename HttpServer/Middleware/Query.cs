using System;
using System.Collections.Generic;

namespace SocketServer.Middlewares
{
	// Extension for HTTPREQuest
	static class QueryExtension {

		public static IReadOnlyDictionary<string,object> GetQuery (this HTTPRequest request) {
			if (request.QueryString != null)
				return Utils.ParseQueryString (request.QueryString);

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

