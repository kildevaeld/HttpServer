using System;
using System.Text;

using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using HttpServer.Middleware;
using System.Net.Sockets;

using SocketServer;
using SocketServer.Handlers;

namespace HttpServer
{
	public class HttpHandler : ISocketServerHandler 
	{
		public IMiddleware Middleware;

		public HttpHandler () : this(new Middleware.Middleware ())
		{


		}

		public HttpHandler (IMiddleware middleware)  
		{
			Middleware = middleware;

			// Set threadpool
			int w;
			int c;
			//ThreadPool.GetMinThreads(out w, out c);

			//ThreadPool.SetMinThreads (20, c);
		}
			
		public void Initialize (ISocketClient client)
		{

			ThreadPool.QueueUserWorkItem((object cli) => {

				client = (ISocketClient)cli;

				// First off, validate the request
				var req = new HTTPRequest();
				var res = new HTTPResponse (client);

				using (var stream = new NetworkStream(client.Socket, false)) {
					try {
						req.ParseRequest(stream);
					} catch (HTTPException e) {
						res.Send(e.StatusCode, e.Message);
					}
				}
			
				// Run middleware
				try {
					this.Middleware.Run (req, res);
				} catch (HTTPException e) {
					res.Send(e.StatusCode, e.Message);
				} catch (SocketException e) {

				}


				if (!res.IsFinished) {
					res.Send (404);
				}

				client.Close ();
				
			},client);


		}

		public void Use (MiddlewareHandler handler) {
			this.Middleware.Use (handler);
		}
	}
}

