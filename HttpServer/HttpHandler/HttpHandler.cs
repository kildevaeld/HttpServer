using System;
using System.Text;
using SocketServer.Handlers;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SocketServer.Middlewares;
using System.Net.Sockets;
namespace SocketServer
{
	public class HttpHandler : ISocketServerHandler 
	{
		public Middleware Middleware;

		public HttpHandler () 
		{
			Middleware = new Middleware ();

			// Set threadpool
			int w;
			int c;
			ThreadPool.GetMinThreads(out w, out c);

			ThreadPool.SetMinThreads (20, c);
		}
			
		public void Initialize (ISocketClient client)
		{

			ThreadPool.QueueUserWorkItem((object cli) => {

				client = (ISocketClient)cli;

				// First off, validate the request
				var req = new HTTPRequest();
				var res = new HTTPResponse (client);

				using (var stream = new NetworkStream(client.Socket, false)) {
					req.ParseRequest(stream);
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

