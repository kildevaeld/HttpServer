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

		internal HttpHandler Handler { get; private set; }

		private Server Server { get; set; }

		internal Router Router { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpServer.HttpServer"/> class.
		/// </summary>
		/// <param name="address">Address.</param>
		/// <param name="stype">Stype.</param>
		/// <param name="ptype">Ptype.</param>
		public HttpServer(AddressFamily address, SocketType stype, ProtocolType ptype) {
			this.Server = new Server (address, stype, ptype);
			Initialize ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpServer.HttpServer"/> class.
		/// </summary>
		public HttpServer () {
			this.Server = new Server ();
			Initialize ();
		}
			
		private void Initialize () {

			this.Router = new Router ();
			this.Server.Handler = this.Handler = new HttpHandler (this.Router);

		}


		#region Middlewares	
		/// <summary>
		/// Add the specified handler to the execution stack
		/// </summary>
		/// <param name="handler">Handler.</param>
		public void Use(MiddlewareHandler handler) {
			this.Router.Use (handler);
		}

		/// <summary>
		/// Use the specified middleware.
		/// </summary>
		/// <param name="middleware">Middleware.</param>
		public void Use(IMiddelwareHandler middleware) {
			this.Router.Use (middleware);
		}

		/// <summary>
		/// Use the specified handler.
		/// </summary>
		/// <param name="handler">Handler.</param>
		public void Use(MiddlewareErrorHandler handler) {
			this.Router.Use (handler);
		}
		#endregion

		#region Routing
		/// <summary>
		/// Get the specified path and handlers.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="handlers">Handlers.</param>
		public void Get(string path, params MiddlewareHandler[] handlers) {
			this.Router.Get (path, handlers);
		}

		/// <summary>
		/// Post the specified path and handlers.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="handlers">Handlers.</param>
		public void Post(string path, params MiddlewareHandler[] handlers) {
			this.Router.Post (path, handlers);
		}

		/// <summary>
		/// Put the specified path and handlers.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="handlers">Handlers.</param>
		public void Put(string path, params MiddlewareHandler[] handlers) {
			this.Router.Put (path, handlers);
		}

		/// <summary>
		/// Delete the specified path and handlers.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="handlers">Handlers.</param>
		public void Delete(string path, params MiddlewareHandler[] handlers) {
			this.Router.Delete (path, handlers);
		}

		/// <summary>
		/// Match the specified path, action and method.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="action">Action.</param>
		/// <param name="method">Method.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Match<T> (string path, string action, Methods method = Methods.Get) {
			this.Router.Match<T> (path, action, method);
		}
		#endregion

		/// <summary>
		/// Listen the specified port.
		/// </summary>
		/// <param name="port">Port.</param>
		public void Listen (int port) {

			this.Server.Listen (port);
		}

		/// <summary>
		/// Stop this instance.
		/// </summary>
		public void Stop () {
			this.Server.Stop ();
		}
	}
}

