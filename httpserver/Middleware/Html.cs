using System;
using System.IO;

namespace SocketServer.Middlewares
{
	public class Html : IMiddelware
	{
		private string _rootPath;
		private string _default = "index";
		private string _defaultExt = "html";

		// TODO: Initial check if rootPath exists
		public Html (string root, string defaults = "index", string defaultExt = "html") {
			_default = defaults;
			_defaultExt = defaultExt;
			_rootPath = root;
		}

		public void Execute(HTTPRequest request, HTTPResponse response) { 
			var path = request.Path;

			if (path.StartsWith ("/"))
				path = path.Trim ('/');



			var fullPath = Path.Combine (_rootPath,path);

			if (File.Exists (fullPath)) {
				response.SendFile (fullPath);
				return;
		
			// If it's a directory add default file and extension
			} else if (Directory.Exists (fullPath)) {
				fullPath = Path.Combine (fullPath, _default + "." + _defaultExt);
			} else if (path.LastIndexOf (".") == -1) {
				fullPath +=  "." + _defaultExt;
			}

			if (File.Exists(fullPath)) {
				response.SendFile (fullPath);
			}
		}
	}


}

