using System;
using System.Text;
using System.Text.RegularExpressions;

namespace httpserver
{
	public class Utils
	{
		public static string UrlRegex = "(\\b(https?)://)?[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]\n";
		/*public static bool IsURL (string url) {
			//return Regex.
				IsMatch ("(\\b(https?)://)?[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]\n");
		}*/
		public Utils ()
		{
		}
	}
}

