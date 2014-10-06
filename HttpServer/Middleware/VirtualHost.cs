using System;

namespace HttpServer.Middleware
{
	public class VirtualHost : IMiddelwareHandler
	{

		private string _hostName;
		private MiddlewareHandler _handler;

		public VirtualHost (string hostName, MiddlewareHandler handler)
		{
			this._hostName = hostName;
			_handler = handler;
		}

		public VirtualHost (string hostName, IMiddelwareHandler handler) : this(hostName,handler.Execute) {

		} 

		public VirtualHost (string hostName, HttpServer handler) : this(hostName,handler.Router) {
		
		}


		public void Execute(HTTPRequest request, HTTPResponse response) {

			if (request.Headers["Host"] == null) {
				return;
			}

			var host = request.Headers ["Host"];

			host = host.Split (':')[0];

			if (host != _hostName) {
				int index = host.IndexOf (".");
				if (index == -1)
					return;
				if ("*" + host.Substring(index) != _hostName)
					return;
			}

			_handler (request, response);

		}
	}
}

