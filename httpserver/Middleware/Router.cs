using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SocketServer.Middlewares
{



	public class Route () {
		public string Path;
		public Methods Method;
		public MiddlewareHandler Handler;
		public IList<MiddlewareHandler> Middleware;
		public bool Match(string path) {
			return path == Path;
		}
	}

	public class Router : IMiddelware
	{

		protected IList<Route> _stack;

		public Router ()
		{
			_stack = new List<Route>();
		}


		public void Get(string path, params MiddlewareHandler[] handlers) {
			var list = handlers.ToList ();
			var handler = list.Last ();
			this.Route (Methods.Get, path, list.GetRange(1,list.Count - 1), handler);
		}

		public void Post(string path, params MiddlewareHandler[] handlers) {	
			var list = handlers.ToList ();
			var handler = list.Last ();
			this.Route (Methods.Post, path, list.GetRange(1,list.Count - 1), handler);
		}



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
			

		public void Execute(HTTPRequest request, HTTPResponse response) {

			foreach (var route in _stack) {
				if (!route.Match (request.Path) || route.Method != request.Method)
					continue;
					
				if (route.Middleware != null && route.Middleware.Count > 0) {
					foreach (var m in route.Middleware) {
						m (request, response);
						if (response.IsFinished)
							break;
					}

					if (response.IsFinished)
						break;
				}
					
				route.Handler (request, response);
				if (response.IsFinished)
					break;
			}
				
		}

		public void Route(Methods method, string path, IList<MiddlewareHandler>middleware, MiddlewareHandler handler) {
			var route = new Route { Method = method, Path = path, Handler = handler, Middleware = middleware };
			_stack.Add (route);
		}

	}
}

