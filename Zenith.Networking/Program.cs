using Serilog;

namespace Zenith.Networking
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var server = new Server(new LoggerConfiguration().WriteTo.Console().CreateLogger());
			server.Start();

			Console.ReadLine();
		}
	}
}
