using System;
using System.IO;
using System.Text.RegularExpressions;

namespace HttpServer.Middleware
{
	public class Favicon : IMiddelwareHandler
	{

		public string Path { get; set; }


		public void Execute(HTTPRequest request, HTTPResponse response) {
			if (!File.Exists (Path)) {
				return;
			}

			if (Regex.IsMatch (request.Path, "\\/favicon\\.ico$", RegexOptions.Compiled)) {
				response.Headers ["Content-Type"] = "image/x-icon";
				response.SendFile (Path);
			}

		}
	}
}

