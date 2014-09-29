﻿using System;
using System.Diagnostics;
namespace SocketServer
{
	public class Logger : IMiddelware
	{

		public Logger ()
		{

		}

		public void Execute(HTTPRequest request, HTTPResponse response) {
			var sw = new Stopwatch ();
			Console.WriteLine (request);
			response.Finished += (object sender, HTTPResponseEventArgs args) => {
				sw.Stop();
				var str = string.Format("{0} {1} ({2}ms)",request.Method, request.Path,sw.ElapsedMilliseconds);
				Console.Write(str);
			};
		}
	}
}
