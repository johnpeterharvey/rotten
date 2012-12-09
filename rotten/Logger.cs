using System;
using System.IO;

namespace rotten
{
	public class Logger
	{
		StreamWriter writer;

		public Logger(String logfile)
		{
			this.writer = new StreamWriter(logfile);
		}

		public void log(String logLine)
		{
			this.writer.WriteLine(System.DateTime.Now + "\t" + logLine);
			Console.WriteLine(logLine);
		}

		public void closeLog()
		{
			this.writer.Close();
			Console.ReadLine();
		}

		public void flush()
		{
			this.writer.Flush();
		}
	}
}

