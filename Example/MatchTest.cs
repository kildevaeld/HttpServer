using System;
using HttpServer;
using HttpServer.Extensions;
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

		[Route("/match/:id")]
		public void Params(HTTPRequest request, HTTPResponse response) {
			response.SendFormat ("TADA! {0}", request.Param<int>("id"));
		}
	}
}

