using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
namespace HttpServer
{
	/// <summary>
	/// Header collection.
	/// </summary>
	public class HeaderCollection : IEnumerable
	{
		private Dictionary<string,string> _headers;

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpServer.HeaderCollection"/> class.
		/// </summary>
		public HeaderCollection ()
		{
			_headers = new Dictionary<string, string> ();
		}

		/// <summary>
		/// Gets or sets the <see cref="HttpServer.HeaderCollection"/> with the specified header.
		/// </summary>
		/// <param name="header">Header.</param>
		public string this[string header] {
			get { 
				var lk = Sanitize(header);
				if (_headers.ContainsKey (lk)) {
					return _headers [lk];
				}
				return null;
			}
			set { 
				_headers [Sanitize(header)] = value;
			}
		}


		private string Sanitize(string s) {
			if (s == null) return s;

			String[] words = s.Split('-');
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Length == 0) continue;

				Char firstChar = Char.ToUpper(words[i][0]); 
				String rest = "";
				if (words[i].Length > 1)
				{
					rest = words[i].Substring(1).ToLower();
				}
				words[i] = firstChar + rest;
			}
			return String.Join("-", words);
		} 

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="HttpServer.HeaderCollection"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="HttpServer.HeaderCollection"/>.</returns>
		public override string ToString ()
		{
			var h = _headers.Select(x => string.Format("{0}: {1}\r\n",x.Key,x.Value));
			var str = string.Join ("", h);
				return str;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator) GetEnumerator();
		}

		public IDictionary<string, string> GetEnumerator()
		{
			return _headers;
		}
	}
}

