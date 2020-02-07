using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Classes
{
    public class City
    {
		public String name { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }

		public City(String _name, double _latitude, double _longitude)
		{
			this.name = _name;
			this.latitude = _latitude;
			this.longitude = _longitude;
		}
    }
}
