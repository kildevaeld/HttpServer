using System;
using System.Collections.Generic;

namespace SocketServer.Middlewares
{



	public delegate void MiddlewareHandler(HTTPRequest request, HTTPResponse response);
	public delegate void MiddlewareErrorHandler(HTTPRequest request, HTTPResponse response, HTTPException exception);

	public interface IMiddelware {
		void Execute(HTTPRequest request, HTTPResponse response);
	}

	/// <summary>
	///  A middleware is a method execute in sekvens every client request.
	///  
	/// </summary>
	public class Middleware
	{
		private IList<MiddlewareHandler> _handlers;
		private IList<MiddlewareErrorHandler> _errorHandlers;

		public Middleware ()
		{
			_handlers = new List<MiddlewareHandler> ();
			_errorHandlers = new List<MiddlewareErrorHandler> ();
		}

		public void Use(MiddlewareHandler handler) {
			_handlers.Add (handler);
		}

		public void Use(IMiddelware middleware) {
			_handlers.Add (middleware.Execute);
		}

		public void Use(MiddlewareErrorHandler handler) {
			_errorHandlers.Add (handler);
		}

		public void Run(HTTPRequest request, HTTPResponse response) {

			foreach (var handler in _handlers) {
				try {
					handler(request, response);
				} catch (Exception e) {

					if (!(e is HTTPException)) {
						e = new HTTPException (500, e.Message);
					}


					foreach (var h in _errorHandlers) {
						if (!response.IsFinished)

							h (request, response, (HTTPException)e);
					}

					if (!response.IsFinished)
						throw e;

				}
				if (response.IsFinished)
					break;
			}
		}
	}
}

