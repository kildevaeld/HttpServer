using System;
using CommandLine;
using CommandLine.Text;

using Newtonsoft.Json;
using System.IO;
using SocketServer;
namespace Example
{
	public class OptionMiddleware {
		public string Name { get; set; }

	}


	public class Options
	{
		[Option('c', "config")]
		[JsonIgnore]
		public string ConfigFile { get; set; }

		[Option('p', "port", DefaultValue = 8080)]
		public int Port { get; set; }

		[Option('v',"verbose", DefaultValue = false)]
		public bool Verbose { get; set; }

		[Option('r', "root")]
		public string Root { get; set; }

		[Option('l', "log", DefaultValue = false)]
		public bool AccessLog { get; set; }
		[HelpOption]
		public string GetUsage() {
			return HelpText.AutoBuild(this,
				(HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}

		public static Options LoadFromFile(string path) {
			var str = File.ReadAllText (path);
			var options = JsonConvert.DeserializeObject<Options> (str);
			return options;
		}

		public Options ()
		{
		}


	}
}

