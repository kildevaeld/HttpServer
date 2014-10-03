using System;
using HttpServer;
namespace Example
{
	public class MatchTest
	{
		public MatchTest ()
		{
		}

		[Route("/match-test")]
		public void Index(HTTPRequest request, HTTPResponse response) {
			response.Send ("Match test");
		}

		[Route("/match-test2", Method = Methods.Get)]
		public void Show(HTTPRequest request, HTTPResponse response) {
			response.Send ("Match test2");
		}

	}
}

