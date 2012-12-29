using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Threading;
using System.Collections;
using System.Security.Cryptography;

namespace rotten
{
	class MainClass
	{
		private static String baseDirectory = "//home//john//movieTest";
		private static String logFile = "//home//john//log.txt";

		private static GetInfo dataFetcher;
		private static DataParser dataParser;
		private static GetIMDBInfo imdbDataFetcher;
		private static IMDBDataParser imdbDataParser;
		private static OutputWriter outputWriter;
		
		private static Logger logger;

		private static ArrayList failedMovies = new ArrayList();

		public static void Main (string[] args)
		{			
			logger = new Logger(logFile);

			dataFetcher = new GetInfo();
			dataParser = new DataParser();
			imdbDataFetcher = new GetIMDBInfo();
			imdbDataParser = new IMDBDataParser();
			outputWriter = new OutputWriter();

			String[] movieDirectory = GetMovieNames();
			logger.log("Fetched [" + movieDirectory.Length.ToString() + "] movies");
			
			foreach (String moviePath in movieDirectory) {
				logger.flush();

				String movieName = moviePath.Replace(baseDirectory + "/", "");
				logger.log("Processing [" + movieName + "]");

				//Split out the directory name to get title and year
				DataFromFile dataFromFile = new DataFromFile(movieName);
				logger.log ("Split directory name to name [" + dataFromFile.getName() + "] and year [" + dataFromFile.getYear() + "]");

				//Grab rotten tomatoes data
				String rottenJson = dataFetcher.getData(movieName);
				rottenJson = rottenJson.Trim();
				MovieInfo rottenParsed = dataParser.getParsedData(rottenJson);

				if (rottenParsed == null) {
					failedMovies.Add(movieName);
					logger.log("Failed to get rottentomatoes data for movie [" + movieName + "]");
					continue;
				}

				processMovieInfo(rottenParsed.Movies[0], dataFromFile, moviePath, failedMovies);
			}

			logger.closeLog();
		}

		private static void processMovieInfo(Movie info, DataFromFile dataFromFile, String fullPath, ArrayList failedMovies)
		{
			//Check release year
			if (!dataFromFile.getYear().Equals(info.Year)) {
				failedMovies.Add(dataFromFile.getName());
				logger.log("Error - directory year " + dataFromFile.getYear() + " does not equal retrieved year " + info.Year);
				return;
			}
			
			//Grab full cast data
			String fullCastUrl = null;
			FullCast fullCast = new FullCast();
			info.Links.TryGetValue("cast", out fullCastUrl);
			
			if (fullCastUrl == null) {
				logger.log("Error - full cast url for " + dataFromFile.getName() + " was null");
			} else {
				String rottenCastJson = dataFetcher.getCastData(fullCastUrl);
				fullCast = dataParser.getCastParsedData(rottenCastJson);
			}
			
			//Get IMDB data for synopsis
			String imdbNumber = null;
			String imdbJson = null;
			info.Alternate_IDs.TryGetValue("imdb", out imdbNumber);
			
			if (imdbNumber == null) {
				logger.log("Error - IMDB number for " + dataFromFile.getName() + " was null");
			} else {
				imdbJson = imdbDataFetcher.getData(imdbNumber);
			}
			
			//Download the movie poster
			String posterUrl = null;
			info.Posters.TryGetValue("original", out posterUrl);
			if (posterUrl == null) {
				logger.log("Error - Poster url for movie " + dataFromFile.getName() + " was null");
			} else {
				saveCover(posterUrl, fullPath, failedMovies, dataFromFile.getName());
			}
			
			outputWriter.WriteOutput(fullPath + "//Info.xml", info, dataFromFile, fullCast);
		}

		//Get all subdirectories of the movie directory
		private static String[] GetMovieNames()
		{
			String[] movies;
			try {
				movies = Directory.GetDirectories(baseDirectory);
			} catch (IOException e) {
				movies = new String[]{};
				logger.log(e.ToString());
			}

			return movies;
		}

		private static void saveCover(String url, String destination, ArrayList failedMovies, String movieName)
		{
			WebClient client = new WebClient();
			String tempFile = System.IO.Path.GetTempFileName();
			logger.log(tempFile);
			client.DownloadFile(new Uri(url), tempFile);

			if (File.Exists(destination + "//Folder.jpg")) {
				SHA512 sha = SHA512.Create();
				Byte[] hash = sha.ComputeHash(new StreamReader(tempFile).BaseStream);

				Byte[] existingImageHash = sha.ComputeHash(new StreamReader(destination + "//Folder.jpg").BaseStream);

				Boolean hashMatch = false;
				
				hashMatch = existingImageHash.Equals(hash);
				
				if (!hashMatch) {
					logger.log("Existing file hash does not match downloaded for movie [" + movieName + "]");
					failedMovies.Add(movieName);
				} else {
					logger.log("Identical image for [" + movieName + "]");
				}
			} else {
				logger.log("No image yet, saving to [" + destination + "]");
				File.Copy(tempFile, destination + "//Folder.jpg");
			}
		}
	}
}