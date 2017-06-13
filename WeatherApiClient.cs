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
        SQLiteConnection m_dbConnection;
        string combinedChoice = "";
        string zipCode = "";
        string choice = "";
        List<string> list = new List<string>();
        string jsonCurrentWeather = AccessWebPage.HttpGet("http://api.openweathermap.org/data/2.5/weather?q=" + "17601" + "&APPID=47992ad1b3261b707350bf13aac83023");
        WeatherData.CurrentRoot root = new WeatherData.CurrentRoot();
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(WeatherData.CurrentRoot));

        public void GetWeatherForecast()
        {

            dbSetup();

            try
            {
                zipCode = combinedChoice.Substring(combinedChoice.IndexOf(" "), combinedChoice.Length - 1);
            }
            catch (Exception)
            {

            }

            using (Stream s = GenerateStreamFromString(jsonCurrentWeather))
            {
                root = (WeatherData.CurrentRoot)ser.ReadObject(s);
            }

            if (zipCode.Equals(""))
            {
                Console.Write("Enter a zip code: ");
                zipCode = Console.ReadLine();
                Console.WriteLine();

            }

            else
            {
                choice = combinedChoice.Substring(0, combinedChoice.IndexOf(" "));
                zipCode = combinedChoice.Substring(combinedChoice.IndexOf(" "), combinedChoice.Length - 1);
                zipCode = Regex.Replace(zipCode, @"\s+", "");
            }

            if (choice.Equals(""))
            {
                Console.WriteLine("Do you want your temperature in Fahrenheit(F), Celcius(C), or Kelvin(K)");
                choice = Console.ReadLine().ToLower();
                Console.WriteLine("");

                string zipCodeInsertSql = "insert into personalInfo (zip, weatherFormat) values ('" + zipCode + "', '" + choice + "')";
                SQLiteCommand zipCodeCommand = new SQLiteCommand(zipCodeInsertSql, m_dbConnection);
                zipCodeCommand.ExecuteNonQuery();

            }

            // Show the weather on the console.
            foreach (string s in list)
            {
                doWeatherSearch();
            }
        }

        public void doWeatherSearch()
        {
            weatherFormatChoice(choice, root, true);

            ExtraStuff convertTime = new ExtraStuff();

            Console.WriteLine();
            Console.WriteLine("===========Random Stuff===========");
            Console.WriteLine("Sunrise: {0}. Sunset: {1}", convertTime.FromUnixTime(root.sys.sunrise), convertTime.FromUnixTime(root.sys.sunset));
            Console.WriteLine("Longitude: {0}. Latitude: {1}", root.coord.lon, root.coord.lat);
            Console.WriteLine("Type: {0}. ID: {1}. Message: {2}. Country: {3}.", root.sys.type, root.sys.id, root.sys.message, root.sys.country);
            Console.WriteLine();
            Console.WriteLine("===============Wind===============");
            Console.WriteLine("Speed: {0}. Gust: {1}. Wind Heading: {2}", root.wind.speed, root.wind.gust, root.wind.deg);
            Console.WriteLine();
            Console.WriteLine("==============Clouds==============");
            Console.WriteLine("Cloud Count: {0}", root.clouds.all);

        }
        public void dbSetup()
        {

            m_dbConnection = new SQLiteConnection("Data Source = MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

            string sql = "select * from personalInfo";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                combinedChoice = "" + reader["weatherFormat"] + " " + reader["zip"];
                list.Add(combinedChoice);
            }
        }

        public string getTemp() {
            WeatherData.CurrentRoot tempRoot = new WeatherData.CurrentRoot();

            using (Stream s = GenerateStreamFromString(jsonCurrentWeather))
            {
                tempRoot = (WeatherData.CurrentRoot)ser.ReadObject(s);
            }


            return "" + weatherFormatChoice("f", tempRoot, false);
        }
        public string weatherFormatChoice(string choice, WeatherData.CurrentRoot theRoot, Boolean consoleWeatherPart)
        {
            ExtraStuff convertTempTo = new ExtraStuff();

            // Fahrenheit 
            if (choice.Equals("f"))
            {
                double kelvin = theRoot.main.temp;

                double fahrenheit = convertTempTo.Fahrenheit(kelvin);

                string weatherOutsideF = ("Weather outside is: " + fahrenheit + "°F");

                if (consoleWeatherPart)
                {
                    Console.WriteLine(weatherOutsideF);
                    Console.WriteLine("Temperature min: {0} °F. Temperature max: {1} °F", convertTempTo.Fahrenheit(theRoot.main.temp_min), convertTempTo.Fahrenheit(theRoot.main.temp_max));
                }

                return weatherOutsideF;
            }

            // Celcius
            if (choice.Equals("c"))
            {
                double kelvin = theRoot.main.temp;

                double celcius = convertTempTo.Celcius(kelvin);

                string weatherOutsideC = ("Weather outside is: " + celcius);

                Console.WriteLine("Weather outside is: {0}°", celcius);
                Console.WriteLine("Temperature min: {0}°. Temperature max: {1}°", convertTempTo.Celcius(theRoot.main.temp_min), convertTempTo.Celcius(theRoot.main.temp_max));

                return weatherOutsideC;

            }
            // Kelvin
            if (choice.Equals("k"))
            {
                double kelvin = theRoot.main.temp;

                string weatherOutsideC = ("Weather outside is: { 0} °F" + kelvin);

                Console.WriteLine(weatherOutsideC);
                Console.WriteLine("Temperature min: {0}. Temperature max: {1}", theRoot.main.temp_min, theRoot.main.temp_max);

                return weatherOutsideC;

            }
            else
            {
                return "";
            }
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