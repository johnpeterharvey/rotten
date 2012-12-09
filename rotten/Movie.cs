using System;
using System.Collections.Generic;

namespace rotten
{
	public class Movie
	{
		public String ID { get; set; }
		public String Title { get; set; }
		public String Year { get; set; }
		public String Runtime { get; set; }
		public String Synopsis { get; set; }
		public Dictionary<String, String> Posters { get; set; }
		public Actor[] Abridged_Cast { get; set; }
		public Dictionary<String, String> Links { get; set; }
		public Dictionary<String, String> Alternate_IDs { get; set; }
	}
}

