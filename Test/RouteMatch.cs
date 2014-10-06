using NUnit.Framework;
using System;
using System.Collections.Generic;
using HttpServer;

namespace Test
{
	[TestFixture]
	public class RouteMatch
	{
		public RouteMatch ()
		{
		}

		[Test]
		public void ParametizedTest () {
			var keys = new List<string> ();
			var reg = Utils.PathToRegex ("/api/:id", ref keys);

			Console.WriteLine (reg);
			Assert.AreEqual (reg.ToString (), "^\\/api\\/(?:([^\\/]+?))$");
			Assert.AreEqual (keys.Count, 1);
			Assert.AreEqual (keys [0], "id");
			Assert.AreEqual(reg.IsMatch("/api/12121"), true);

		}

		[Test]
		public void ParametizedMultiTest () {
			var keys = new List<string> ();
			var reg = Utils.PathToRegex ("/api/:id/:profile", ref keys);

			Console.WriteLine (reg);
			Assert.AreEqual (reg.ToString (), "^\\/api\\/(?:([^\\/]+?))\\/(?:([^\\/]+?))$");
			Assert.AreEqual (keys.Count, 2);
			Assert.AreEqual (keys [0], "id");
			Assert.AreEqual (keys [1], "profile");
			Assert.AreEqual(reg.IsMatch("/api/12121/noget-andet"), true);

		}
	}


}

