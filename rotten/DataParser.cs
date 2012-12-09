using System;
using Newtonsoft.Json;

namespace rotten
{
	public class DataParser
	{
		public MovieInfo getParsedData(String json)
		{
			if (String.IsNullOrEmpty(json)) {
				Console.WriteLine("Error processing json [" + json + "]");
				return null;
			}

			return JsonConvert.DeserializeObject<MovieInfo>(json);
		}

		public FullCast getCastParsedData(String json)
		{
			if (String.IsNullOrEmpty(json)) {
				Console.WriteLine("Error processing json [" + json + "]");
				return null;
			}
			
			return JsonConvert.DeserializeObject<FullCast>(json);
		}
	}
}

