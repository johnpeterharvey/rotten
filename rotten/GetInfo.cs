using System;
using System.Net;
using System.Linq;
using System.Xml.Linq;

namespace rotten
{
	public class GetInfo
	{
		private WebClient downloadClient;
		private String apikey;
		
		public GetInfo ()
		{
			downloadClient = new WebClient();
			apikey = getApiKey();
		}

		public String getData(String movieName)
		{
			Uri rottenUri = new Uri("http://api.rottentomatoes.com/api/public/v1.0/movies.json?apikey=" + apikey + "&q=" + movieName + "&page_limit=5");
			String returnedData = String.Empty;
			try {
				returnedData = downloadClient.DownloadString(rottenUri);
			} catch (Exception e) {
				Console.WriteLine(e);
				return null;
			}

			return returnedData;
		}

		public String getCastData(String castURL)
		{
			Uri rottenUri = new Uri(castURL + "?apikey=" + apikey);
			String returnedData = String.Empty;
			try {
				returnedData = downloadClient.DownloadString(rottenUri);
			} catch (Exception e) {
				Console.WriteLine(e);
				return null;
			}
			
			return returnedData;
		}
		
		private String getApiKey()
		{
			XDocument doc = XDocument.Load ("RottenTomatoesApiKey.config");

			var data = from item in doc.Descendants ("configuration")
						select new {
							apikey = item.Element ("apikey").Value
			};

			String key = "";
			
			foreach (var val in data)
			{
				key = val.apikey;
			}
			return key;
		}
	}
}

