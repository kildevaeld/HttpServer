using System;
using System.Collections.Generic;

namespace SocketServer
{

	public delegate void MiddlewareHandler(HTTPRequest request, HTTPResponse response);

	public class Middleware
	{
		private IList<MiddlewareHandler> _handlers;

		public Middleware ()
		{
			_handlers = new List<MiddlewareHandler> ();
		}

		public void Use(MiddlewareHandler handler) {
			_handlers.Add (handler);
		}

		public void Run(HTTPRequest request, HTTPResponse response) {

			foreach (var handler in _handlers) {
				handler(request, response);
			}
		}
	}
}

