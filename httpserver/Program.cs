using System;
using SocketServer;
using SocketServer.Handlers;


namespace httpserver
{
    class Program
    {
        static void Main(string[] args)
        {
			var handler = new SocketHTTPHandler ();

			handler.Middleware.Use (new Logger());
			handler.Middleware.Use (new Static ("/Users/rasmus"));

			var router = new Router ();
			handler.Middleware.Use (router);

			router.Get("/test",delegate(HTTPRequest request, HTTPResponse response) {
				response.Send("Hello, World! - From " + request.Path);
			});

			var server = new Server { Handler = handler };

			server.Listen (8080);

        }
    }
}
