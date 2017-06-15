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
    class LocationClient
    {
        public string getLocation(string zipCode)
        {
            string jsonCurrentWeather = AccessWebPage.HttpGet("http://maps.googleapis.com/maps/api/geocode/json?address=" + zipCode + "&sensor=true");
            LocationData.RootObject root = new LocationData.RootObject();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(WeatherData.CurrentRoot));

            String returnValue = "";

            using (Stream s = GenerateStreamFromString(jsonCurrentWeather))
            {
                try
                {
                    root = (LocationData.RootObject)ser.ReadObject(s);
                    returnValue = "" + returnLocation(root);

                }
                catch (Exception)
                {
                    returnValue = "Error: ZipCode is not found. Try again";
                }
            }
            return returnValue;
        }
        public string returnLocation(LocationData.RootObject theRoot)
        {
            //string location = theRoot.results.
            return "";

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
