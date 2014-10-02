using System;
using SocketServer;
using SocketServer.Middlewares;
using zlib;
namespace Middlewares
{
	public class Compression : IMiddelware
	{
		public Compression ()
		{
		}


		public void Execute(HTTPRequest request, HTTPResponse response) {
			
		}
	}
}

