using System;
using System.IO;
using System.Net.Mime;

namespace HttpServer.Middleware
{
	public class Static : IMiddelware
	{

		public string _rootPath; 

		// TODO: Initial check if rootPath exists
		public Static (string root)
		{
			_rootPath = root;
		}

		public void Execute(HTTPRequest request, HTTPResponse response) {

			var path = request.Path;

			if (path.StartsWith ("/"))
				path = path.TrimStart ('/');

			var fullPath = Path.Combine (_rootPath,path);

			if (!File.Exists(fullPath)) {
				// Let other middleware execute;
				return;
			}

			response.SendFile (fullPath);


		}
	}
}

