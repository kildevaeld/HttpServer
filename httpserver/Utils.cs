using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mime;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace SocketServer
{
	public static class Utils
	{
		public static readonly string UrlRegex = "(\\b(https?)://)?[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]\n";
		public static readonly string StatusLine = "^(GET|POST|PUT|DELETE|HEAD) (https?:\\/\\/[\\-A-Za-z0-9+&@#\\/%?=~_|!:,.;]*[\\-A-Za-z0-9+&@#\\/%=~_|‌​]|\\/.*) HTTP\\/(1\\.[01]).*";
		//private static readonly Regex _regex = new Regex(@"[?|&](\w+)=([^?|^&]+)");
		private static readonly Regex _regex = new Regex (@"([^?=&]+)(=([^&]*))?", RegexOptions.Multiline);

		public static IReadOnlyDictionary<string, object> ParseQueryString(string query)
		{
			var match = _regex.Match(query);
			var paramaters = new Dictionary<string, object>();
			var l = new Dictionary<string,IList<string>> ();
			while (match.Success)
			{
				var k = match.Groups[1].Value;
				var v = match.Groups [3].Value;
				// Value is a list
				if (k.EndsWith ("[]")) {
					var sk = k.TrimEnd ('[', ']');
					if (!l.ContainsKey (sk))
						l [sk] = new List<string> ();
					l [sk].Add (v);
				} else {
					paramaters.Add(k,v);
				}

				match = match.NextMatch();
			}
			foreach (var kv in l) {
				paramaters.Add (kv.Key, kv.Value.ToArray());
			}

			return 
				paramaters;
		}

		public static string DictinaryToString(IDictionary<string, object> dict) {
			string[] o = new string[dict.Count];

			var i = 0;
			foreach (var kv in dict) {
				if (kv.Value.GetType ().IsArray) {
					var e = (Array)kv.Value;

					string[] v = new string[e.Length];

					for (var z = 0; z < e.Length; z++) {
						v [z] = Convert.ToString(e.GetValue (z));
					} 

					o [i] = kv.Key + " : [" + String.Join (",", v) + ']'; 
				
				} else {
					o [i] = kv.Key + " : " + kv.Value;
				}
				i++;
			}
			return string.Join(",",o);

		}
	}


}

