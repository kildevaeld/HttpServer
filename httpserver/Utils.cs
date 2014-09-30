using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mime;
using System.Collections.Generic;
namespace SocketServer
{
	public static class Utils
	{
		public static string UrlRegex = "(\\b(https?)://)?[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]\n";
		public static string StatusLine = "^(GET|POST|PUT|DELETE|HEAD) (https?:\\/\\/[\\-A-Za-z0-9+&@#\\/%?=~_|!:,.;]*[\\-A-Za-z0-9+&@#\\/%=~_|‌​]|\\/.*) HTTP\\/(1\\.[01]).*";
		private static readonly Regex _regex = new Regex(@"[?|&](\w+)=([^?|^&]+)");

		public static IReadOnlyDictionary<string, string> ParseQueryString(string query)
		{
			var match = _regex.Match(query);
			var paramaters = new Dictionary<string, string>();
			while (match.Success)
			{
				paramaters.Add(match.Groups[1].Value, match.Groups[2].Value);
				match = match.NextMatch();
			}
			return paramaters;
		}

		/*public static string PathToRegex(string path) {
			var str = "(\\\\.)|" +
				"([\\\\/.])?(?:\\\\:(\\\\w+)(?:\\\\(((?:\\\\\\\\.|[^)])*)\\\\))?|\\\\(((?:\\\\\\\\.|[^)])*)\\\\))([+*?])?" +
				"|([.+*?=^!:${}()[\\\\]|\\\\/])";


		}*/


	}
}

