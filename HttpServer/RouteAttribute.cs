using System;

namespace HttpServer
{
	[AttributeUsage(AttributeTargets.Method)]
	public class RouteAttribute : Attribute
	{
		public Methods Method { get; set; }
		public string Path { get; set; }

		public RouteAttribute  (string path)
		{
			this.Path = path;
			this.Method = Methods.Get;
		}
	}
}

