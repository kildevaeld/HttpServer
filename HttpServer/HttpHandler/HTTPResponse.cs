using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace HttpServer
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
			this.Body = null;
		}

		/// <summary>
		/// Set response headers
		/// </summary>
		/// <param name="header">Header.</param>
		/// <param name="value">Value.</param>
		public void Set(string header, object value) {
			// TODO: Validate and sanitize
			this.Headers [header] = Convert.ToString (value);
		}

		#region send
		/// <summary>
		/// Send message to client with a default statuscode of 200 and end the response
		/// </summary>
		/// <param name="body">Body.</param>
		public int Send(string body) {
			return this.Send (200, body);
		}

		public int SendFormat(string body, params object[] args) {
			return this.Send (200, string.Format (body, args));
		}

		/// <summary>
		/// Send message to client with optional body and end the response
		/// </summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="body">Body.</param>
		public int Send(int statusCode, string body = null) {
		
			this.StatusCode = statusCode;

			if (body != null)
				this.Body = body;

			if (this.Body == null) {
				this.Body = HttpStatusCodes.Get (this.StatusCode);
			}

			var ret = this.Write (this.ToString ());

			this.End (); // Flag finished

			return ret;
		}

		/// <summary>
		/// Sends the file and end the response
		/// </summary>
		/// <param name="path">Path.</param>
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

			using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				using (NetworkStream ns = _client.GetStream (false)) {
					if (ns.CanWrite) {
						try {
							stream.CopyTo (ns);
						} catch {
						
						}
					}
				}
			
			}

			this.End ();

		}

		/// <summary>
		/// Write the specified message, but don't end the response.
		/// </summary>
		/// <param name="message">Message.</param>
		public int Write(string message) {
			// If the finished flag is up, the reponse and underlying socket is closed
			if (this.IsFinished)
				throw new Exception ("Socket already finished!");

			var b = Encoding.UTF8.GetBytes(message);
			return _client.Send (b, b.Length);
		}
		#endregion

		/// <summary>
		/// End the response. This should render the response unuseable for any more sends/writes
		/// </summary>
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

