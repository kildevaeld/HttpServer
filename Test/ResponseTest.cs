using System;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Moq;

using SocketServer;
using HttpServer;

namespace Test
{
	[TestFixture]
	public class ResponseTest
	{
		public ResponseTest ()
		{
		}

		[Test]
		public void SimpleSendTest () {
			var mock = new Mock<SocketServer.ISocketClient> ();

			var response = new HTTPResponse (mock.Object);

			response.Send ("Hello, World");

			var b = GetBytes ("HTTP/1.0 200\r\nContent-Length: 12\r\n\r\nHello, World");

			mock.Verify (c => c.Send ( It.Is<byte[]>( by => by.SequenceEqual(b)), 48));
		}

		[Test]
		public void ContentTypeTest () {
			var mock = new Mock<SocketServer.ISocketClient> ();

			var response = new HTTPResponse (mock.Object);

			response.Headers ["Content-Type"] = "text/html";
			response.Send ("<p>Hello, World</p>");

			var b = GetBytes ("HTTP/1.0 200\r\nContent-Type: text/html\r\n" +
				"Content-Length: 19\r\n\r\n<p>Hello, World</p>");

			mock.Verify (c => c.Send ( It.Is<byte[]>( by => by.SequenceEqual(b)), 80));
		}

		[Test]
		public void SendFormatTest () {
			var mock = new Mock<SocketServer.ISocketClient> ();

			var response = new HTTPResponse (mock.Object);

			response.Headers ["Content-Type"] = "text/html";
			response.SendFormat("<p>Hello, {0}</p>", "Bjarne");

			var b = GetBytes ("HTTP/1.0 200\r\nContent-Type: text/html\r\n" +
				"Content-Length: 20\r\n\r\n<p>Hello, Bjarne</p>");

			mock.Verify (c => c.Send ( It.Is<byte[]>( by => by.SequenceEqual(b)), 81));
		}


		private string GetString(byte[] s) {
			return Encoding.UTF8.GetString(s);
		}

		private byte[] GetBytes(string str) {
			return Encoding.UTF8.GetBytes (str);
		}
	}
}

