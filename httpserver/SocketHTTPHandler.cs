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
	public class SocketHTTPHandler : ISocketServerHandler 
	{
		public Middleware Middleware;

		public SocketHTTPHandler () 
		{
			Middleware = new Middleware ();

			// Set threadpool
			int w;
			int c;
			ThreadPool.GetMinThreads(out w, out c);

			ThreadPool.SetMinThreads (20, c);
		}

		// TODO: Does not handle request larger than 1024 bytes. Implement Reader as a streamer
		public void Initialize (ISocketClient client)
		{

			ThreadPool.QueueUserWorkItem((object cli) => {

				client = (ISocketClient)cli;

				var req = new HTTPRequest();
				var res = new HTTPResponse (client);

				using (var stream = new NetworkStream(client.Socket, false)) {
					req.ParseRequest(stream);
				}


				//string str = Encoding.UTF8.GetString(client.Buffer,0, len);



				/*try {
					req.ParseRequest(str);
				} catch (HTTPException e) {
					res.Send(e.StatusCode, e.Message);
				}*/


				try {
					this.Middleware.Run (req, res);
				} catch (HTTPException e) {
					res.Send(e.StatusCode, e.Message);
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

