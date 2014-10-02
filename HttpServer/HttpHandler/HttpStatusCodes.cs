using System;
using System.Collections.Generic;

namespace HttpServer
{
	public static class HttpStatusCodes
	{

		#region HttpCodes type list
		private static Dictionary<int, string> HttpCodes = new Dictionary<int, String>() {

			{100, "Continue"},
			{101, "Switching Protocols"},
			{102, "Processing"},
			{200, "OK"},
			{201, "Created"},
			{202, "Accepted"},
			{203, "Non Authoritative Information"},
			{204, "No Content"},
			{205, "Reset Content"},
			{206, "Partial Content"},
			{207, "Multi-Status"},
			{300, "Multiple Choices"},
			{301, "Moved Permanently"},
			{302, "Moved Temporarily"},
			{303, "See Other"},
			{304, "Not Modified"},
			{305, "Use Proxy"},
			{307, "Temporary Redirect"},
			{400, "Bad Request"},
			{401, "Unauthorized"},
			{402, "Payment Required"},
			{403, "Forbidden"},
			{404, "Not Found"},
			{405, "Method Not Allowed"},
			{406, "Not Acceptable"},
			{407, "Proxy Authentication Required"},
			{408, "Request Timeout"},
			{409, "Conflict"},
			{410, "Gone"},
			{411, "Length Required"},
			{412, "Precondition Failed"},
			{413, "Request Entity Too Large"},
			{414, "Request-URI Too Long"},
			{415, "Unsupported Media Type"},
			{416, "Requested Range Not Satisfiable"},
			{417, "Expectation Failed"},
			{419, "Insufficient Space on Resource"},
			{420, "Method Failure"},
			{422, "Unprocessable Entity"},
			{423, "Locked"},
			{500, "Server Error"},
			{501, "Not Implemented"},
			{502, "Bad Gateway"},
			{503, "Service Unavailable"},
			{504, "Gateway Timeout"},
			{505, "HTTP Version Not Supported"},
			{507, "Insufficient Storage"},
		};
		#endregion

		#region Get
		/// <summary>
		/// Returns the status code string for the requested status code. 
		/// </summary>
		/// <param name="statusCode"></param>
		/// <returns></returns>
		public static String Get(int statusCode)
		{
			return HttpCodes [statusCode];
		}
			
		#endregion Get

	}
}

