using System;
using System.Text;
using SocketServer.Handlers;
using System.Threading;
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

		public void Initialize (ISocketClient client)
		{

			ThreadPool.QueueUserWorkItem((object cli) => {
			//client.ReadAsync ().ContinueWith (x => {
				//string str = Encoding.UTF8.GetString (client.Buffer, 0, x.Result.Length);
				client = (ISocketClient)cli;
				int len = client.Read();

				if (len == 0) {
					client.Close();
					return;
				}

				string str = Encoding.UTF8.GetString (client.Buffer, 0, len);

				var res = new HTTPResponse (client);
				var req = new HTTPRequest ();

				try {
					req.ParseRequest(str);
				} catch (HTTPException e) {
					res.Send(e.StatusCode, e.Message);
				}

				try {
					this.Middleware.Run (req, res);
				} catch (HTTPException e) {
					res.Send(e.StatusCode, e.Message);
				} catch (Exception e) {
					res.Send (500);
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

