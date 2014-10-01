using System;
using SocketServer;
using SocketServer.Handlers;
using SocketServer.Middlewares;
using SocketServer.Middlewares.Multipart;
namespace httpserver
{
    class Program
    {
        static void Main(string[] args)
        {
			var handler = new SocketHTTPHandler ();
			var router = new Router ();

			handler.Middleware.Use (new Json ());
			handler.Middleware.Use(new Html("/Users/rasmus/Sites/"));
			handler.Middleware.Use (new SocketServer.Middlewares.Logger());
			handler.Middleware.Use (new Static ("/Users/rasmus"));
			handler.Middleware.Use (new FormData ());
			handler.Middleware.Use (router);

			router.Get("/test", delegate(HTTPRequest request, HTTPResponse response) {
				var q = request.GetQuery();
				response.Write("Hello, World! - From " + request.Path);
				
				//response.Write("Query: " + q["family"]);
				response.End();
			});

			router.Post ("/test", delegate(HTTPRequest request, HTTPResponse response) {
				response.Send("Post test");
			});

			var server = new Server { Handler = handler };

			server.Listen (8080);

        }
    }
}
