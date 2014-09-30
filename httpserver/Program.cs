using System;
using SocketServer;
using SocketServer.Handlers;
using SocketServer.Middlewares;

namespace httpserver
{
    class Program
    {
        static void Main(string[] args)
        {
			var handler = new SocketHTTPHandler ();
			var router = new Router ();
			handler.Middleware.Use(new Html("/Users/rasmus/Sites/"));
			handler.Middleware.Use (new Logger());
			handler.Middleware.Use (new Static ("/Users/rasmus"));


			handler.Middleware.Use (router);

			router.Get("/test", delegate(HTTPRequest request, HTTPResponse response) {
				response.Send("Hello, World! - From " + request.Path);
			});

			var server = new Server { Handler = handler };

			server.Listen (8080);

        }
    }
}
