using System;
using Http;
using SocketServer;
using SocketServer.Middlewares;
using CommandLine;
using System.IO;


namespace Example
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var options = new Options ();

			if (CommandLine.Parser.Default.ParseArguments (args, options)) {

				if (options.ConfigFile != null) {
					try {
						options = Options.LoadFromFile (options.ConfigFile);
					} catch (Exception e) {
						Console.WriteLine ("Config file not found!" + e);
						return;
					}
				}


			} else {
				Console.WriteLine ("Could not parse commandline options");
				return;
			}

			var server = new HttpServer ();
				
			// FIXME: ...
			if (options.Verbose) {
				(SocketServer.Server.Log as SocketServer.Logger).IsDebugEnabled  = true;
				(SocketServer.Server.Log as SocketServer.Logger).IsErrorEnabled  = true;
				(SocketServer.Server.Log as SocketServer.Logger).IsFatalEnabled  = true;
				(SocketServer.Server.Log as SocketServer.Logger).IsWarnEnabled  = true;
				(SocketServer.Server.Log as SocketServer.Logger).IsInfoEnabled  = true;
			}


			if (options.AccessLog) {
				server.Use ((SocketServer.HTTPRequest request, SocketServer.HTTPResponse response) => {
					var date = DateTime.UtcNow;

					var str = String.Format("{0:MM/dd/yy H:mm:ss}: {1} {2} {3}",date, request.Method, request.Path, request.UserAgent);
					Console.WriteLine (str);
				});
			}



			if (options.Root != null) {
				server.Use (new Favicon { Path = Path.Combine(options.Root, "favicon.ico")});
				server.Use (new Static (options.Root));
				server.Use (new Html (options.Root));

			}

			// Alle request som ikke er blevet fanget eller behandlet
			// Ska' som default være html
			server.Use ((HTTPRequest request, HTTPResponse response) => {
				response.Headers["Content-Type"] = "text/html";
			});

			server.Get("/router", (HTTPRequest request, HTTPResponse response) => {
				response.Send("<p>This is created from the router middleware</p>");
			});

			// Dette link smider en Exception som bliver fanget af error handleren
			server.Get("/errorenos-link", (HTTPRequest request, HTTPResponse response) => {
				//throw new HTTPException(500);
				throw new Exception("A terrible error!");
			});

			// Handle not found!
			// Dette er den sidste i rækken er middlewares. Hvis requestet ikke er blevet fanget eller
			// fullendt endnu, så findes ruten ikke.
			server.Use ((HTTPRequest request, HTTPResponse response) => {
				response.Send("<h4>The request route could not be found!</h4>");
			});

			// Custom error handler
			server.Use ((HTTPRequest request, HTTPResponse response, HTTPException exception) => {
				response.Headers["Content-Type"] = "text/html";
				response.SendFormat("<h3>Ups! An Error Happened! " +
					":</h3><p>Code : {0}, Message: {1}</p>",exception.StatusCode, exception.Message);
			});


			server.Listen (options.Port);
		}
	}
}
