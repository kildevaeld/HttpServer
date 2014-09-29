using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SocketServer
{
	public class HTTPResponseEventArgs : EventArgs {
		public HTTPResponse Response;
	}

	public delegate void HTTPResponseEvent(object sender, HTTPResponseEventArgs args);

	public partial class HTTPResponse
	{

		private bool _headerSent = false;
		private ISocketClient _client;

		public bool IsFinished { get; private set; }

		// TODO: Create HeadersCollection class
		public HeaderCollection Headers { get; private set; }

		public int StatusCode { get; set; }

		public string Body { get; set; }

		// Events
		public event HTTPResponseEvent Finished;


		public HTTPResponse (ISocketClient client) {
			if (client == null)
				throw new ArgumentException ("Client is required!");

			_client = client;

			Headers = new HeaderCollection ();
			StatusCode = 200;
		}

		public void Set(string header, object value) {
			// TODO: Validate and sanitize
			this.Headers [header] = Convert.ToString (value);
		}

		#region send
		public int Send(string body) {
			return this.Send (200, body);
		}

		public int Send(int statusCode, string body = "") {
		
			this.StatusCode = statusCode;
			this.Body = body;

			var ret = this.Write (this.ToString ());

			this.End (); // Flag finished

			return ret;
		}

		public void SendFile(string path) {
			string ext;
			int index = path.LastIndexOf ('.');
			if (index > 0) {
				ext = path.Substring (index, path.Length - index);
			} else {
				ext = "html";
			}
			// Get content type of file from the extension
			var mime = MimeType.Get(ext);

			this.Set("Content-Type", mime);

			FileInfo info = new FileInfo (path);
			this.Set("Content-Length", info.Length);

			SendHeaders ();

			using (FileStream stream = new FileStream(path, FileMode.Open)) {
				using (NetworkStream ns = new NetworkStream (_client.Socket)) {
					stream.CopyTo (ns);
				}
			}

			this.End ();

		}

		public int Write(string message) {
			if (this.IsFinished)
				throw new Exception ("Socket already finished!");
			var b = Encoding.UTF8.GetBytes(message);
			return _client.Send (b, b.Length);
		}
		#endregion

		public void End() {
			this._headerSent = true;
			this.IsFinished = true;
			if (Finished != null) {
				Finished (this, new HTTPResponseEventArgs { Response = this } );
			}
		}

		public override string ToString ()
		{
			var str = String.Format ("HTTP/1.0 {0}\r\n{1}\r\n{2}", StatusCode, Headers,Body);
			return str;
		}
			
		private void SendHeaders() {
			if (this._headerSent) {
				throw new Exception ("Headers already sent");
			}
			this.Write (String.Format ("HTTP/1.0 {0}\r\n{1}\r\n", StatusCode, Headers));
			this._headerSent = true;
		}
	}
}

