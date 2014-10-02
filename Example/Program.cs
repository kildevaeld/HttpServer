using System;
using HttpServer;
using HttpServer.Middleware;

using CommandLine;
using System.IO;
using System.Collections.Generic;

namespace Example
{
	// Extension for HTTPREQuest
	static class QueryExtension {

		// Jeg ved ikke om det er en fejl i mono eller, men jeg ka' ikke bruge extensions methods
		// defineret i HttpServer herfra... hmmm...
		public static IReadOnlyDictionary<string,object> Query (this HTTPRequest request) {
			if (request.QueryString != null)
				return Utils.ParseQueryString (request.QueryString);

			return null;
		}

		public static object GetQuery(this HTTPRequest request, string prop) {
			if (request.QueryString == null)
				return null;

			var query = Utils.ParseQueryString (request.QueryString);
			if (query.ContainsKey (prop))
				return query [prop];
			return null;
		}

	}

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

			var server = new HttpServer.HttpServer ();
				
			// FIXME: ...
			/*if (options.Verbose) {
				(HttpServer.Server.Log as HttpServer.Logger).IsDebugEnabled  = true;
				(HttpServer.Server.Log as HttpServer.Logger).IsErrorEnabled  = true;
				(HttpServer.Server.Log as HttpServer.Logger).IsFatalEnabled  = true;
				(HttpServer.Server.Log as HttpServer.Logger).IsWarnEnabled  = true;
				(HttpServer.Server.Log as HttpServer.Logger).IsInfoEnabled  = true;
			}*/


			if (options.AccessLog) {
				server.Use ((HttpServer.HTTPRequest request, HttpServer.HTTPResponse response) => {
					var date = DateTime.UtcNow;

					var str = String.Format("{0:MM/dd/yy H:mm:ss}: {1} {2} {3}",date, request.Method, request.Path, request.UserAgent);
					Console.WriteLine (str);
				});
			}



			if (options.Root != null) {
				server.Use (new Favicon { Path = Path.Combine(options.Root, "favicon.ico")});
				server.Use (new Static (options.Root));
				server.Use (new Html (options.Root));
				server.Use (new Json ());

			}

			// Alle request som ikke er blevet fanget eller behandlet
			// Ska' som default være html
			server.Use ((HTTPRequest request, HTTPResponse response) => {
				response.Headers["Content-Type"] = "text/html; charset=utf-8";
			});

			server.Get("/router", (HTTPRequest request, HTTPResponse response) => {
				response.Send("<p>This is created from the router middleware</p>");
			});

			// Dette link smider en Exception som bliver fanget af error handleren
			server.Get("/errorenos-link", (HTTPRequest request, HTTPResponse response) => {
				//throw new HTTPException(500);
				throw new HTTPException("A terrible error!");
			});
				

			server.Get ("/query-test", Routes.QueryTest);
			server.Post ("/post", Routes.JSONTest);

			server.Match<MatchTest> ("/match-test", "index" );



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
