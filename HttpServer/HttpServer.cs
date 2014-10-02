using System;
using System.Net.Sockets;
using HttpServer;
using HttpServer.Middleware;
using SocketServer;
using SocketServer.Handlers;
namespace HttpServer
{
	/// <summary>
	/// A simple facade class for Socketserver with a http handler.
	/// </summary>
	public class HttpServer  {

		public HttpHandler Handler { get; private set; }

		private Server Server { get; set; }

		public Router Router { get; internal set; }

		public HttpServer(AddressFamily address, SocketType stype, ProtocolType ptype) {
			this.Server = new Server (address, stype, ptype);
			Initialize ();
		}

		public HttpServer () {
			this.Server = new Server ();
			Initialize ();
		}

		private void Initialize () {
			// HACK:
			this.Router = new Router ();
			//this.Router.Use (this.Router);
			this.Server.Handler = this.Handler = new HttpHandler (this.Router);

		}


		#region Middlewares	
		public void Use(MiddlewareHandler handler) {

			this.Handler.Middleware.Use (handler);
		}

		public void Use(IMiddelwareHandler middleware) {
			this.Handler.Middleware.Use (middleware);
		}

		public void Use(MiddlewareErrorHandler handler) {
			this.Handler.Middleware.Use (handler);
		}
		#endregion

		#region Routing
		public void Get(string path, params MiddlewareHandler[] handlers) {
			this.Router.Get (path, handlers);
		}

		public void Post(string path, params MiddlewareHandler[] handlers) {
			this.Router.Post (path, handlers);
		}

		public void Put(string path, params MiddlewareErrorHandler[] handlers) {

		}

		public void Match<T> (string path, string action, Methods method = Methods.Get) {
			this.Router.Match<T> (path, action, method);
		}

		#endregion

		public void Listen (int port) {

			this.Server.Listen (port);
		}

		public void Stop () {
			this.Server.Stop ();
		}
	}
}

