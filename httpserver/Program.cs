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

			var server = new Server { Handler = handler };

			server.Listen (8080);

        }
    }
}
