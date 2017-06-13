using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Practice
{
    class ExtraStuff
    {
        public string FromUnixTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            string timeString = dtDateTime + "";
            string finalTimeString = timeString.Remove(0, timeString.IndexOf("2017") + 4);

            return finalTimeString;
        }

        public double Fahrenheit(double tempInCelcius)
        {
            return (Celcius(tempInCelcius)) * 9 / 5 + 32; ;
        }
        public double Celcius(double tempInKelvin)
        {
            return tempInKelvin - 273.15;
        }

        public void helpPage()
        {
            Console.WriteLine();
            Console.WriteLine("settings: edit default zip code, add more cities, and change units.");
            Console.WriteLine("help: get help.");
            Console.WriteLine("");
            Console.WriteLine("Created by Alex Reyes");
            Console.ReadLine();

        }
    }
}
