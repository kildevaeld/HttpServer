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
			int read = client.Read ();

			string str = Encoding.UTF8.GetString (client.Buffer, 0, read);

			var req = new HTTPRequest (str);
			var res = new HTTPResponse (client);


			try {
				this.Middleware.Run (null, res);
			} catch (Exception e) {
				res.Send (500);
			}
			if (!res.IsFinished) {
				res.Send (404, "Not Found");
			}

			client.Close ();

		}

		public void Use (MiddlewareHandler handler) {
			this.Middleware.Use (handler);
		}
	}
}

