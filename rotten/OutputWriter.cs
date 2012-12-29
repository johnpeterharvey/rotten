using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace rotten
{
	public class OutputWriter
	{
		public OutputWriter ()
		{
		}
		
		public void WriteOutput(String outputFile, Movie info, DataFromFile parsedDataFromFile, FullCast cast)
		{
			XmlDocument document = new XmlDocument();
			
			XmlNode rootNode = document.CreateElement("movie");
			document.AppendChild(rootNode);
			
			XmlNode movieName = document.CreateElement("name");
			movieName.InnerText = parsedDataFromFile.getName();
			rootNode.AppendChild(movieName);
			
			XmlNode movieYear = document.CreateElement ("year");
			movieYear.InnerText = parsedDataFromFile.getYear();
			rootNode.AppendChild(movieYear);
			
			XmlNode movieRuntime = document.CreateElement("runtime");
			movieRuntime.InnerText = info.Runtime;
			rootNode.AppendChild(movieRuntime);
			
			XmlNode movieSynopsis = document.CreateElement("synopsis");
			movieSynopsis.InnerText = info.Synopsis;
			rootNode.AppendChild(movieSynopsis);
			
			XmlNode movieCast = document.CreateElement("cast");
			rootNode.AppendChild(movieCast);
			Actor[] actors = cast.Cast;
			foreach (Actor a in actors) {
				XmlNode movieCastActor = document.CreateElement("actor");
				movieCast.AppendChild(movieCastActor);
				
				
				XmlNode movieCastName = document.CreateElement("name");
				movieCastName.InnerText = a.Name;
				movieCastActor.AppendChild(movieCastName);
				
				String[] characters = a.Characters;
				foreach (String s in characters) {
					XmlNode movieCastCharacter = document.CreateElement("character");
					movieCastCharacter.InnerText = s;
					movieCastActor.AppendChild(movieCastCharacter);
				}
			}
			
			document.Save(outputFile);
		}
	}
}

