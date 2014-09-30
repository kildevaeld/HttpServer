using System;
using System.Collections.Generic;
namespace SocketServer
{

	public enum Methods {Get, Post, Put, Delete }

	public class Route () {
		public string Path;
		public Methods Method;
		public MiddlewareHandler Handler;
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


		public void Get(string path, MiddlewareHandler handler) {
			this.Route (Methods.Get, path, handler);
		}

		public void Execute(HTTPRequest request, HTTPResponse response) {
			foreach (var route in _stack) {
				if (!route.Match (request.Path))
					continue;
				route.Handler (request, response);
				if (response.IsFinished)
					break;
			}
		}

		public void Route(Methods method, string path, MiddlewareHandler handler) {
			_stack.Add (new Route { Method = method, Path = path, Handler = handler });
		}


	
	}
}

