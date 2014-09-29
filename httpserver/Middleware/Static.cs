using System;
using System.IO;
using System.Net.Mime;

namespace SocketServer
{
	public class Static : IMiddelware
	{

		public string _rootPath; 

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

		//private void 
			

	
	}
}

