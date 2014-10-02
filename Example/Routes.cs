using System;
using HttpServer;
using HttpServer.Middleware;

using System.Collections.Generic;
namespace Example
{
	public static class Routes
	{
	
		public static void QueryTest(HTTPRequest request, HTTPResponse response) {
			var query = request.Query();

			if (query != null) {
				var str = "<pre>" + Utils.DictinaryToString((IDictionary<string,object>)query) + "</pre>";
				response.Send(200, str);
			}
		}

		public static void JSONTest(HTTPRequest request, HTTPResponse response) {
			var json = request.GetJSON();
			response.SendFormat("This is json {0}", json); 
		}
	}
}

