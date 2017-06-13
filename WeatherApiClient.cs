using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Data.SQLite;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace Api_Practice
{
    class WeatherApiClient
    {

        public string getTemp() {

            string jsonCurrentWeather = AccessWebPage.HttpGet("http://api.openweathermap.org/data/2.5/weather?q=" + "17601" + "&APPID=47992ad1b3261b707350bf13aac83023");
            WeatherData.CurrentRoot root = new WeatherData.CurrentRoot();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(WeatherData.CurrentRoot));

            WeatherData.CurrentRoot tempRoot = new WeatherData.CurrentRoot();

            using (Stream s = GenerateStreamFromString(jsonCurrentWeather))
            {
                tempRoot = (WeatherData.CurrentRoot)ser.ReadObject(s);
            }
            return "" + returnWeatherOutside(tempRoot);
        }

        public string returnWeatherOutside(WeatherData.CurrentRoot theRoot)
        {
            ConvertTo convertTempTo = new ConvertTo();

            double kelvin = theRoot.main.temp;
            double fahrenheit = convertTempTo.Fahrenheit(kelvin);
            string weatherOutsideF = ("Weather outside is: " + fahrenheit + "°F");

            return weatherOutsideF;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}