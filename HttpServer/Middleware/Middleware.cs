using System;
using System.Collections.Generic;

namespace HttpServer.Middleware
{



	public delegate void MiddlewareHandler(HTTPRequest request, HTTPResponse response);
	public delegate void MiddlewareErrorHandler(HTTPRequest request, HTTPResponse response, HTTPException exception);

	public interface IMiddelwareHandler {
		void Execute(HTTPRequest request, HTTPResponse response);
	}

	public interface IMiddleware {

		void Use (MiddlewareHandler handler);
		void Use (IMiddelwareHandler middleware);
		void Use (MiddlewareErrorHandler handler);
		void Run (HTTPRequest request, HTTPResponse response);
	}
	/// <summary>
	///  A middleware is a method executed in sekvens every client request.
	///  
	/// </summary>
	public class Middleware : IMiddleware
	{
		protected IList<MiddlewareHandler> _handlers;
		protected IList<MiddlewareErrorHandler> _errorHandlers;

		public Middleware ()
		{
			_handlers = new List<MiddlewareHandler> ();
			_errorHandlers = new List<MiddlewareErrorHandler> ();
		}

		public void Use(MiddlewareHandler handler) {
			_handlers.Add (handler);
		}

		public void Use(IMiddelwareHandler middleware) {
			_handlers.Add (middleware.Execute);
		}

		public void Use(MiddlewareErrorHandler handler) {
			_errorHandlers.Add (handler);
		}

		public virtual void Run(HTTPRequest request, HTTPResponse response) {

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

