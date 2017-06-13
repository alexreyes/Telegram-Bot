using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace Api_Practice
{ 
    class Database
    {
        SQLiteConnection m_dbConnection;

        public Database()
        {
            createNewDatabase();
            connectToDatabase();
            createTable();
            fillTable();
            //deleteRow();
            printTableResults();
        }

        void createNewDatabase()
        {
            SQLiteConnection.CreateFile("MyDatabase.sqlite");
        }

        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source = MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }

        void createTable()
        {
            string sql = "create table if not exists personalInfo (zip VARCHAR(20), weatherFormat VARCHAR(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }
        void fillTable()
        {
            string sql = "insert into personalInfo (zip, weatherFormat) values ('17601', 'f')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

        }

        void deleteRow()
        {
            string sql = "delete from personalInfo where zip = " + " " + "17601";
            //string sql = "drop table personalInfo";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }
        // Writes the highscores to the console sorted on score in descending order.
        void printTableResults()
        {
            m_dbConnection = new SQLiteConnection("Data Source = MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select * from personalInfo";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            string zipCode = "";
            string weatherFormat = "";

            while (reader.Read())
            {
                zipCode = "" + reader["zip"];
                weatherFormat = "" + reader["weatherFormat"];

                if (weatherFormat.Equals("f"))
                {
                    weatherFormat = "Fahrenheit";
                }
                if (weatherFormat.Equals("c"))
                {
                    weatherFormat = "Celcius";
                }
                if (weatherFormat.Equals("k"))
                {
                    weatherFormat = "Kelvin";
                }
                Console.WriteLine("Zip Code: " + zipCode + "\tWeather Format: " + weatherFormat);
            }


            Console.WriteLine();
        }
    }
}
