using System;

namespace HttpServer
{
	public class HTTPException : Exception
	{
		public int StatusCode { get; set; }

		public HTTPException (int code, string message) : this(message) 
		{
			StatusCode = code;
		}

		public HTTPException (string message) : base(message) {
			StatusCode = 500;
		}
	}
}

