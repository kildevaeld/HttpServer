using System;
using HttpServer;
using HttpServer.Middleware;
using zlib;
namespace Middlewares
{
	public class Compression : IMiddelwareHandler
	{
		public Compression ()
		{
		}


		public void Execute(HTTPRequest request, HTTPResponse response) {
			
		}
	}
}

