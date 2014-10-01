using System;
using SocketServer;

namespace SocketServer.Middlewares.Multipart {

	public class Json : IMiddelware {

		public void Execute(HTTPRequest request, HTTPResponse response) {
			var contentType = request.Headers ["Content-Type"];
			if (contentType == null || contentType != MimeType.Get("json"))
				return; // Nothing to parse.


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
