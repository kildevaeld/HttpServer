using System;
using HttpServer;
using Newtonsoft.Json;

namespace HttpServer.Extensions {

	public static class MultipartExtension {
		public static T GetJSON<T>(this HTTPRequest request) {
			var contentType = request.Headers ["Content-Type"];
			if (contentType == null || contentType != MimeType.Get ("json") 
				|| string.IsNullOrWhiteSpace(request.Body))
				return default(T);

			return JsonConvert.DeserializeObject<T> (request.Body);
		}

		public static object GetJSON(this HTTPRequest request) {
			var contentType = request.Headers ["Content-Type"];
			if (contentType == null || contentType != MimeType.Get ("json") 
				|| string.IsNullOrWhiteSpace(request.Body))
				return null;

			return JsonConvert.DeserializeObject (request.Body);
		}

		public static object GetFormUrlEncoded(this HTTPRequest request) {
			var contentType = request.Headers ["Content-Type"];
			if (contentType == null || contentType != "application/x-www-form-urlencoded"
			    || string.IsNullOrWhiteSpace (request.Body))
				return null;

			return Utils.ParseQueryString (request.Body);
		
		}

		public static object GetBody(this HTTPRequest request) {

			var contentType = request.Headers ["Content-Type"];
			if (contentType == null || string.IsNullOrWhiteSpace (request.Body))
				return null;
			if (contentType == MimeType.Get ("json"))
				return request.GetJSON ();
			else if (contentType == "application/x-www-form-urlencoded")
				return request.GetFormUrlEncoded ();

			return null;
		}

	}

}
