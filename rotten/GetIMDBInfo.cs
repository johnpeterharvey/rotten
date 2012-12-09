using System;
using System.Net;

namespace rotten
{
	public class GetIMDBInfo
	{
		private WebClient downloadClient;

		public GetIMDBInfo ()
		{
			downloadClient = new WebClient();
		}

		public String getData(String imdbNumber)
		{
			Uri rottenUri = new Uri("http://www.imdb.com/title/tt"+ imdbNumber);
			String returnedData = String.Empty;
			try {
				returnedData = downloadClient.DownloadString(rottenUri);
			} catch (Exception e) {
				Console.WriteLine(e);
				return null;
			}
			
			return returnedData;
		}
	}
}

