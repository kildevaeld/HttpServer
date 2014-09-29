using System;
using System.Text;
using SocketServer.Handlers;
namespace SocketServer
{
	public class SocketHTTPHandler : ISocketServerHandler 
	{
		public Middleware Middleware;

		public SocketHTTPHandler () 
		{
			Middleware = new Middleware ();
		}

		public void Initialize (ISocketClient client)
		{
			//int read = client.Read ();

			client.ReadAsync ().ContinueWith (x => {
				string str = Encoding.UTF8.GetString (client.Buffer, 0, x.Result.Length);

				HTTPRequest req = null;
				var res = new HTTPResponse (client);

				try {
					req = new HTTPRequest (str);
				} catch (HttpRequestException e) {
					res.Send(e.StatusCode, "Illegal request");
				}

				try {
					this.Middleware.Run (req, res);
				} catch (Exception e) {
					res.Send (500);
				}
				if (!res.IsFinished) {
					res.Send (404, "Not Found");
				}

				client.Close ();
				
			});


		}

		public void Use (MiddlewareHandler handler) {
			this.Middleware.Use (handler);
		}
	}
}

