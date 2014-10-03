using System;
using HttpServer;
using Newtonsoft.Json;

namespace HttpServer.Extensions {

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

}
