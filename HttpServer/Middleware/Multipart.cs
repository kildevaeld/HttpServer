using System;
using SocketServer;
using Newtonsoft.Json;

namespace SocketServer.Middlewares.Multipart {

	public static class MultipartExtension {
		public static T GetJSON<T>(this HTTPRequest request) {
			return JsonConvert.DeserializeObject<T> (request.Body);
		}

		public static object GetJSON(this HTTPRequest request) {
			return JsonConvert.DeserializeObject (request.Body);
		}

	}

	public class Json : IMiddelware {

		public void Execute(HTTPRequest request, HTTPResponse response) {
			var contentType = request.Headers ["Content-Type"];
			if (contentType == null || contentType != MimeType.Get("json"))
				return; // Nothing to parse.

			if (!string.IsNullOrWhiteSpace (request.Body)) {
				
			}

		}
	}

	public class FormData : IMiddelware {
			
		public void Execute(HTTPRequest request, HTTPResponse response) {
			/*var contentType = (string)request.Headers ["Content-Type"];
			if (!contentType.StartsWith ("multipart/form-data"))
				return;
			Console.WriteLine (request);*/
		}
	}

}
