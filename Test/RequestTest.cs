﻿using NUnit.Framework;
using System;
using System.Net.Sockets;
using HttpServer;

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

			_request.ParseRequest (StreamFromString("GET /file.txt HTTP/1.0"));
			Assert.AreEqual(_request.Path, "/file.txt");
			Assert.AreEqual (_request.Method, Methods.Get);
			Assert.AreEqual(_request.Protocol, "HTTP");
			Assert.AreEqual (_request.Version, 1.0);

		}


		[Test]
		public void TestGetIllegalRequest()
		{
			var exception = Assert.Throws<HTTPException> (() => {
				_request.ParseRequest (StreamFromString("GET /file.txt HTTP 1.0"));
			});

			Assert.AreEqual (exception.StatusCode, 400);
			Assert.AreEqual (exception.Message, HttpStatusCodes.Get (400));
		}

		[Test]
		public void TestGetIllegalMethodName()
		{
			var exception = Assert.Throws<HTTPException> (() => {
				_request.ParseRequest (StreamFromString("PLET /file.txt HTTP/1.0"));
			});

			Assert.AreEqual (exception.StatusCode, 400);
			Assert.AreEqual (exception.Message, HttpStatusCodes.Get (400));

		}

		[Test]
		public void TestGetIllegalProtocol()
		{
			var exception = Assert.Throws<HTTPException> (() => {
				_request.ParseRequest (StreamFromString("GET /file.txt HTTP/1.2"));
			});
			Assert.AreEqual (exception.StatusCode, 400);
			Assert.AreEqual (exception.Message, HttpStatusCodes.Get (400));

		}

		[Test]
		public void TestPost()
		{
			_request.ParseRequest (StreamFromString("POST /file.txt HTTP/1.0"));
			Assert.AreEqual(_request.Path, "/file.txt");
			Assert.AreEqual (_request.Method, Methods.Post);
			Assert.AreEqual(_request.Protocol, "HTTP");
			Assert.AreEqual (_request.Version, 1.0);
		}

		[Test]
		public void TestHeaders() {
			_request.ParseRequest(StreamFromString("POST / HTTP/1.0\r\nContent-Type: " +
				"application/json\r\nContent-Length: 100\r\n\r\n"));

			Assert.AreEqual(_request.Headers["Content-Type"],"application/json");
			Assert.AreEqual(_request.Headers["Content-Length"], "100");
		}

		[Test]
		public void TestIllegalBody() {
		
			var exception = Assert.Throws<HTTPException> (() => {
				_request.ParseRequest(StreamFromString("POST / HTTP/1.0\r\n\r\n" +
					"Hello, World!"));
			});
				
			Assert.AreEqual (exception.StatusCode, 400);
			Assert.AreEqual (exception.Message, HttpStatusCodes.Get (400));
		}

		[Test]
		public void TestFull() {
			_request.ParseRequest(StreamFromString("POST / HTTP/1.0\r\nContent-Type: application/json" +
				"\r\nContent-Length: 13\r\n\r\nHello, World!"));

			Assert.AreEqual(_request.Headers["Content-Type"],"application/json");
			Assert.AreEqual(_request.Headers["Content-Length"], "13");
			Assert.AreEqual ("Hello, World!", _request.Body);
		}

		[Test]
		public void TestFullWithCRLFInBody() {
			_request.ParseRequest(StreamFromString("POST / HTTP/1.0\r\nContent-Type: text/plain" +
				"\r\nContent-Length: 23\r\n\r\nHello, World!\r\n\r\nOr not"));

			Assert.AreEqual(_request.Headers["Content-Type"],"text/plain");
			Assert.AreEqual(_request.Headers["Content-Length"], "23");
			Assert.AreEqual ("Hello, World!\r\n\r\nOr not", _request.Body);
		}

		private Stream StreamFromString(string str) {
			return new MemoryStream (Encoding.ASCII.GetBytes (str));
		}
	}
}

