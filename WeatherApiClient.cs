﻿using System;
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
        private string theZipCode = "";

        public string getTemp(string zipCode) {
            theZipCode = zipCode;

            string jsonCurrentWeather = AccessWebPage.HttpGet("http://api.openweathermap.org/data/2.5/weather?q=" + zipCode + "&APPID=47992ad1b3261b707350bf13aac83023");
            WeatherData.CurrentRoot root = new WeatherData.CurrentRoot();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(WeatherData.CurrentRoot));

            WeatherData.CurrentRoot tempRoot = new WeatherData.CurrentRoot();

            String returnValue = "";

            using (Stream s = GenerateStreamFromString(jsonCurrentWeather))
            {
                try
                {
                    tempRoot = (WeatherData.CurrentRoot)ser.ReadObject(s);
                    returnValue = "" + returnWeatherOutside(tempRoot);

                }
                catch (Exception)
                {
                    returnValue = "Error: ZipCode is not found. Try again";
                }
            }
            return returnValue;
        }

        public string getTemp() {

            return "" + getTemp("17601");
        }

        public string returnWeatherOutside(WeatherData.CurrentRoot theRoot)
        {
            ConvertTo convertTempTo = new ConvertTo();

            double kelvin = theRoot.main.temp;
            double fahrenheit = convertTempTo.Fahrenheit(kelvin);
            string weatherOutsideF = ("Weather in " + theZipCode+ " is: " + (int) fahrenheit + "°F");

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