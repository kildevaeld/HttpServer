using System;
using HttpServer;
using Newtonsoft.Json;

namespace HttpServer.Middleware {

	public static class MultipartExtension {
		public static T GetJSON<T>(this HTTPRequest request) {
			var contentType = request.Headers ["Content-Type"];
			if (contentType == null || contentType != MimeType.Get ("json"))
				return default(T);

			return JsonConvert.DeserializeObject<T> (request.Body);
		}

		public static object GetJSON(this HTTPRequest request) {
			var contentType = request.Headers ["Content-Type"];
			if (contentType == null || contentType != MimeType.Get ("json"))
				return null;

			return JsonConvert.DeserializeObject (request.Body);
		}

	}

	public class Json : IMiddelwareHandler {

		public void Execute(HTTPRequest request, HTTPResponse response) {
			var contentType = request.Headers ["Content-Type"];
			if (contentType == null || contentType != MimeType.Get("json"))
				return; // Nothing to parse.

			if (!string.IsNullOrWhiteSpace (request.Body)) {
				
			}

		}
	}

	public class FormData : IMiddelwareHandler {
			
		public void Execute(HTTPRequest request, HTTPResponse response) {
			/*var contentType = (string)request.Headers ["Content-Type"];
			if (!contentType.StartsWith ("multipart/form-data"))
				return;
			Console.WriteLine (request);*/
		}
	}

}
