using System;
using HttpServer;
//using HttpServer.Middleware;
using HttpServer.Extensions;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
			var json = request.GetBody();
			if (json == null)
				return;

			response.Headers["content-type"] = "application/json";

			if (json is JObject) {
				var o = (JObject)json;
				o.Add ("postProp", "This property is set from c#");
				response.Send(o.ToString()); 
			} else {
				var o = (IReadOnlyDictionary<string,object>)json;
				var d = new Dictionary<string,object> ();

				foreach (var kv in o) {
					d [kv.Key] = kv.Value;
				}

				d["postProp"] = "This property is set from c#";

				response.Send(JsonConvert.SerializeObject(d)); 
			}





		}
	}
}

