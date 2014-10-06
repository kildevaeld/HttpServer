using System;
using System.Collections.Generic;

namespace HttpServer.Extensions
{
	public static class Params
	{
		public static object Param(this HTTPRequest request, string param) {
			var p = (IDictionary<string, object>)request.Data ["params"];

			if (p == null || !p.ContainsKey(param))
				return null;

			return p [param];
		}

		public static T Param<T>(this HTTPRequest request, string param) {
			var p = (IDictionary<string, object>)request.Data ["params"];

			if (p == null || !p.ContainsKey(param))
				return default(T);

			return (T)p [param];
		}


	}
}

