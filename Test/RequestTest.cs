using NUnit.Framework;
using System;
using System.Net.Sockets;
using SocketServer;

using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
namespace Test
{
	[TestFixture (), RequiresThread]
	public class RequestTest
	{
		private const string CrLf = "\r\n";
		private HTTPRequest _request;



		[TestFixtureSetUp]
		public void Setup () {
			_request = new HTTPRequest ();
		}

		[Test]
		public void TestGet()
		{

			_request.ParseRequest ("GET /file.txt HTTP/1.0");
			Assert.AreEqual(_request.Path, "/file.txt");
			Assert.AreEqual (_request.Method, "GET");
			Assert.AreEqual(_request.Protocol, "HTTP");
			Assert.AreEqual (_request.Version, 1.0);

		}


		[Test]
		public void TestGetIllegalRequest()
		{
			var exception = Assert.Throws<HTTPException> (() => {
				_request.ParseRequest ("GET /file.txt HTTP 1.0");
			});

			Assert.AreEqual (exception.StatusCode, 400);
			Assert.AreEqual (exception.Message, HttpStatusCodes.Get (400));
		}

		[Test]
		public void TestGetIllegalMethodName()
		{
			var exception = Assert.Throws<HTTPException> (() => {
				_request.ParseRequest ("PLET /file.txt HTTP/1.0");
			});

			Assert.AreEqual (exception.StatusCode, 400);
			Assert.AreEqual (exception.Message, HttpStatusCodes.Get (400));

		}

		[Test]
		public void TestGetIllegalProtocol()
		{
			var exception = Assert.Throws<HTTPException> (() => {
				_request.ParseRequest ("GET /file.txt HTTP/1.2");
			});
			Assert.AreEqual (exception.StatusCode, 400);
			Assert.AreEqual (exception.Message, HttpStatusCodes.Get (400));

		}

		[Test]
		public void TestMethodNotImplemented()
		{
			//String line = GetFirstLine("POST /file.txt HTTP/1.0");
			//Assert.AreEqual("HTTP/1.0 200 xxx", line);
		}

		/// <summary>
		/// Private helper method
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		private static String GetFirstLine(String request)
		{
			TcpClient client = new TcpClient("127.0.0.1", 8080);

			NetworkStream networkStream = client.GetStream();

			StreamWriter toServer = new StreamWriter(networkStream, Encoding.UTF8);

			toServer.Write(request + CrLf);
			toServer.Write(CrLf);
			toServer.Flush();


			StreamReader fromServer = new StreamReader(networkStream);
			String firstline = fromServer.ReadLine();
			toServer.Close();
			fromServer.Close();
			client.Close();
			return firstline;


		}
	}
}

