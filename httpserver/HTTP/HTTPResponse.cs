using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SocketServer
{

	//public delegate 

	public class HTTPResponse
	{
		public bool IsFinished { get; private set; }
		public IDictionary<string,string> Headers { get; private set; }
		public int StatusCode { get; set; }
		public string Body { get; set; }

		private ISocketClient _client;

		public HTTPResponse (ISocketClient client) {
			if (client == null)
				throw new ArgumentException ("Client is required!");

			_client = client;

			Headers = new Dictionary<string, string> ();
			StatusCode = 200;
		}


		public Task<int> SendAsync(string body) {
			return this.SendAsync (200, body);
		}

		public Task<int> SendAsync(int statusCode, string body) {
			this.StatusCode = statusCode;
			this.Body = body;

			var b = Encoding.UTF8.GetBytes(this.ToString ());
			return _client.SendAsync (b, b.Length).ContinueWith( x=> {
				this.IsFinished = true;
				return x.Result;
			});
		}

		public int Send(string body) {
			return this.Send (200, body);
		}

		public int Send(int statusCode, string body = "") {
			this.StatusCode = statusCode;
			this.Body = body;

			var b = Encoding.UTF8.GetBytes(this.ToString ());
			var ret = _client.Send (b, b.Length);
			this.IsFinished = true;
			return ret;
		}

		public override string ToString ()
		{
			var headers = this.Headers.Select (x => {
				return String.Format("{0} = {1}\r\n",x.Key, x.Value);
			});

			var str = String.Format ("HTTP/1.0 {0}\r\n{1}\r\n{2}", StatusCode, string.
				Join ("", headers),Body);

			return str;
		}
	}
}

