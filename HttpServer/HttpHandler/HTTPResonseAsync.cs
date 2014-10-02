using System;
using System.Threading.Tasks;
using System.Text;
namespace HttpServer
{
	public partial class HTTPResponse
	{

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

	}


}

