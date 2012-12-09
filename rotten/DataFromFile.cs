using System;
using System.IO;

namespace rotten
{
	public class DataFromFile
	{
		private String nm;
		private String yr;

		public DataFromFile (String directoryName)
		{
			String[] split = directoryName.Split(new char[] {'(', ')'});

			this.nm = split[0].TrimEnd(new char[] {' '});
			this.yr = split[1];
		}

		public String getName()
		{
			return this.nm;
		}

		public String getYear()
		{
			return this.yr;
		}
	}
}

