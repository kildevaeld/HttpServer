using System;
using System.Collections.Generic;

namespace HttpServer
{
	public class HTTPRequestData
	{

		private IDictionary<string,object> _data;


		public HTTPRequestData ()
		{
			_data = new Dictionary<string, object> ();
		}


		public object this[string name] {
			get { 
				return _data [name];
			}
			set { 
				_data [name] = value;
			}
		}
	}
}

