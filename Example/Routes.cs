using System;
using HttpServer;
using HttpServer.Middleware;

using System.Collections.Generic;

using Newtonsoft.Json.Linq;

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
			if (json == null)
				return;
			var o = (JObject)json;
			o.Add ("postProp", "This property is set from c#");

			response.Headers["content-type"] = "application/json";
			response.Send(json.ToString()); 

		}
	}
}

