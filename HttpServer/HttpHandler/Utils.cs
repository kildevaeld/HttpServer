using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mime;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace HttpServer
{
	public static class Utils
	{
		public static readonly string UrlRegex = "(\\b(https?)://)?[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]\n";
		public static readonly string StatusLine = @"^(GET|POST|PUT|DELETE|HEAD) (https?:\/\/[\-A-Za-z0-9+&@#\/%?=~_|!:,.;]*[\-A-Za-z0-9+&@#\/%=~_|​]|\/.*) (HTTP)?\/(1\.[01]).*";

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

		public static string DictinaryToString(IDictionary<string, object> dict, bool pretty = true) {
			string[] o = new string[dict.Count];

			var i = 0;
			foreach (var kv in dict) {
				if (kv.Value.GetType ().IsArray) {
					var e = (Array)kv.Value;

					string[] v = new string[e.Length];

					for (var z = 0; z < e.Length; z++) {
						v [z] = Convert.ToString(e.GetValue (z));
					} 

					o [i] = kv.Key + " : [\n\t" + String.Join (",\n\t", v) + "\n]"; 
				
				} else {
					o [i] = kv.Key + " : " + kv.Value;
				}
				i++;
			}
			return string.Join(",\n",o);

		}

		// Shamelessly borrowed from expressjs@3
		public static Regex PathToRegex(string path, ref List<string>outKeys) {
			var keys = new List<object> ();
			var p = Regex.Replace(path,"\\/\\(", "(?:/");
			p = Regex.Replace (p, "(\\/)?(\\.)?:(\\w+)(?:(\\(.*?\\)))?(\\?)?(\\*)?", delegate(Match match) {

				string slash = match.Groups[1].Value, format = match.Groups[2].Value, key = match.Groups[3].Value,
				capture = match.Groups[4].Value, optional = match.Groups[5].Value, star = match.Groups[6].Value;
				//keys.Add(new { Name = key, Optional = string.IsNullOrEmpty(optional) ? false : true });
				keys.Add(key);

				var opt = string.IsNullOrEmpty(optional) ? false : true ;
				return "" 
					+ (opt ? "" : slash) 
					+ "(?:" 
					+ (opt ? slash : ""
					)
					+ (string.IsNullOrEmpty(format) ? "" : format)
					+ (!string.IsNullOrEmpty(capture) ? capture : "([^/]+?)") + ')'
					+ (opt ? optional : "")
					+ (!string.IsNullOrEmpty(star) ? "(/*)?" : "");

			});
			foreach (var k in keys)
				outKeys.Add ((string)k);
			p = Regex.Replace (p, "([\\/.])","\\$1",RegexOptions.IgnoreCase);
			p = Regex.Replace (p, "\\*", "(.*)");
			return new Regex ("^" + p + "$");
		}
			
	}


}

