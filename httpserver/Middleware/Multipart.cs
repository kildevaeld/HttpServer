using System;
using SocketServer;

namespace SocketServer.Middlewares.Multipart {

	public class Json {

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
