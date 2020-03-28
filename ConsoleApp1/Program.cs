using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ConsoleApp1.Classes;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {

		//convert degress to radians
		public static double toRadians(double angle)
		{
			return (Math.PI / 182) * angle;
		}

		//returns the distance in miles based on latitude and longitude of the visitor and city
		public static double GetDistance(City rest, double vLatitude, double vLongitude)
		{
			int R = 6371; // Radious of the earth
			Double lat1 = rest.latitude;
			Double lon1 = rest.longitude;
			Double lat2 = vLatitude;
			Double lon2 = vLongitude;
			Double latDistance = toRadians(lat2 - lat1);
			Double lonDistance = toRadians(lon2 - lon1);
			Double a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2) +
			Math.Cos(toRadians(lat1)) * Math.Cos(toRadians(lat2)) *
			Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
			Double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			Double distance = R * c;
			return distance * 0.6213;
		}

		//method takes in a json array and returns a sorted list of visitors in ascending order of distance.
		public static List<Tuple<string, string, string, string, double, double, double>> GetNearestVisitors(JArray json, City city)
		{

			var visitors = new List<Tuple<string, string, string, string, double, double, double>>();
			foreach (var item in json)
			{
				double lat = Convert.ToDouble(item["address"]["geo"]["lat"]);
				double lon = Convert.ToDouble(item["address"]["geo"]["lng"]);
				visitors.Add(new Tuple<string, string, string, string, double, double, double>(
					item["name"].ToString(),
					item["address"]["suite"].ToString() + "  " + item["address"]["street"].ToString() + " " + item["address"]["city"].ToString() + "  " + item["address"]["zipcode"].ToString(),
					item["company"]["name"].ToString(),
					item["phone"].ToString(),
					lat,
					lon,
					GetDistance(city, lat, lon)
					));
			}

			visitors.Sort((x, y) => x.Item7.CompareTo(y.Item7));
			return visitors;
		}

		//makes a get request to the argument url and returns the json as a string
		public static string Get(string uri)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		public static void WritetoFile(String path, List<Tuple<string, string, string, string, double, double, double>> visitors, string cityname)
		{
			var csv = new StringBuilder();
			csv.AppendLine("name, address, company name, phone,latitude, longitude, distance from " + cityname);

			//generate csv line by line
			foreach (var t in visitors)
			{
				Console.WriteLine("{0}, {1}, {2}, {3}", t.Item1, t.Item2, t.Item3, t.Item7);
				var newLine = string.Format("{0}, {1}, {2}, {3}, {4},{5}, {6}",
				t.Item1,
				t.Item2,
				t.Item3,
				t.Item4,
				t.Item5,
				t.Item6,
				t.Item7);
				csv.AppendLine(newLine);
			}
			//return the output as csv in the project folder itself.
			File.WriteAllText(path, csv.ToString());
		}

		static void Main(string[] args)
        {
			//arguments passed by user are city latitude, city longitude and city name
			double lat = Convert.ToDouble(args[0]);
			double lon = Convert.ToDouble(args[1]);
			string cityName = args[2];

			City city = new City(cityName, lat, lon);
			String json = Get("https://jsonplaceholder.typicode.com/users");
			String filePath = @"Visitors_"+ city.name+ ".csv";
			JArray obj = JsonConvert.DeserializeObject<JArray>(json);
			var output = GetNearestVisitors(obj, city);
			//var csv = new StringBuilder();
			//csv.AppendLine("id, name, address,phone,latitude, longitude, distance from " + city.name);

			WritetoFile(filePath, output, city.name);
			Console.ReadLine();


		}
    }
}
