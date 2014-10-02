using System;
using HttpServer;
namespace Example
{
	public class MatchTest
	{
		public MatchTest ()
		{
		}


		public void Index(HTTPRequest request, HTTPResponse response) {
			response.Send ("Match test");
		}
	}
}

