using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HttpServer.Middleware
{
	/// <summary>
	/// Represent a route
	/// </summary>
	public class Route : IMiddelwareHandler {
		/// <summary>
		/// The route path
		/// </summary>
		public string Path;
		/// <summary>
		/// The http method
		/// </summary>
		public Methods Method;
		/// <summary>
		/// The handler.
		/// </summary>
		public MiddlewareHandler Handler;
		/// <summary>
		/// The middleware.
		/// </summary>
		public IList<MiddlewareHandler> Middleware;
		/// <summary>
		/// Match the specified path.
		/// </summary>
		/// <param name="path">Path.</param>
		public bool Match(string path) {
			return path == Path;
		}

		/// <summary>
		/// Execute the specified request and response.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="response">Response.</param>
		public void Execute(HTTPRequest request, HTTPResponse response) {
			if (!Match (request.Path) || this.Method != request.Method)
				return;
			if (Middleware != null && Middleware.Count > 0) {
				foreach (var m in Middleware) {
					m (request, response);
					if (response.IsFinished)
						return;
				}

				if (response.IsFinished)
					return;
			}

			Handler (request, response);
			if (response.IsFinished)
				return;
		}
	}

	/// <summary>
	/// Simple routing middleware.
	/// Can be used as a regular middlewarehandler or as middleware
	/// </summary>
	// TODO: Implement parametized routes: /api/:id
	public class Router : Middleware, IMiddelwareHandler
	{
	
		/// <summary>
		/// HTTP Get request
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="handlers">Handlers.</param>
		public void Get(string path, params MiddlewareHandler[] handlers) {
			var list = handlers.ToList ();
			var handler = list.Last ();
			this.Route (Methods.Get, path, list.GetRange(1,list.Count - 1), handler);
		}

		/// <summary>
		/// HTTP Post request
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="handlers">Handlers.</param>
		public void Post(string path, params MiddlewareHandler[] handlers) {	
			var list = handlers.ToList ();
			var handler = list.Last ();
			this.Route (Methods.Post, path, list.GetRange(1,list.Count - 1), handler);
		}

		/// <summary>
		/// HTTP Put request
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="handlers">Handlers.</param>
		public void Put(string path, params MiddlewareHandler[] handlers) {	
			var list = handlers.ToList ();
			var handler = list.Last ();
			this.Route (Methods.Put, path, list.GetRange(1,list.Count - 1), handler);
		}

		/// <summary>
		/// HTTP Delete request
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="handlers">One or more handler</param>
		public void Delete(string path, params MiddlewareHandler[] handlers) {	
			var list = handlers.ToList ();
			var handler = list.Last ();
			this.Route (Methods.Delete, path, list.GetRange(1,list.Count - 1), handler);
		}

		/// <summary>
		/// Match a route to a method on a class (T)
		/// The mehod should take a HTTPRequest and a HTTPResonse object
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="action">The method on the object</param>
		/// <param name="method">The HTTP Method eg. GET</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Match<T>(string path, string action, Methods method) {
			var type = typeof(T);

			MethodInfo cMethod = null;
			foreach (var m in type.GetMethods()) {
				if (m.Name.ToLower () == action.ToLower ()) {
					cMethod = m;
					break;
				}
			}
			if (cMethod== null) {
				throw new Exception ("Method not found");
			}
				
			this.Route (method, path, default(IList<MiddlewareHandler>), (HTTPRequest request, HTTPResponse response) => {
				var c = Activator.CreateInstance(type);
				cMethod.Invoke(c, new object[] {request, response});
			});
		}

		public void Match<T> () {
			var methods = typeof(T).GetMethods ();

			foreach (var m in methods) {
				var attr = m.GetCustomAttribute<RouteAttribute> ();
				if (attr == null)
					continue;
				this.Match<T> (attr.Path, m.Name, attr.Method);

			}
		}

		/// <summary>
		/// Execute the specified request and response.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="response">Response.</param>
		public void Execute(HTTPRequest request, HTTPResponse response) {
			this.Run (request, response);
		}

		internal void Route(Methods method, string path, IList<MiddlewareHandler>middleware, MiddlewareHandler handler) {
			var route = new Route { Method = method, Path = path, Handler = handler, Middleware = middleware };
			this.Use (route);
		}
			
	}
}

