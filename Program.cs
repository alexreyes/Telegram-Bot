using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Practice
{
    class Program
    {
        static void Main(string[] args)
        {

            testApiAsync();
            Console.ReadLine();
        }
        static async void testApiAsync()
        {
            WeatherApiClient weatherClient = new WeatherApiClient();

            var Bot = new Telegram.Bot.TelegramBotClient("267703378:AAHaHqsitEahdo2j8F3F4uz0DGiu6aXQvL4");
            var me = await Bot.GetMeAsync();

            var message = await Bot.SendTextMessage("164850555", weatherClient.getTemp());
            Console.WriteLine("Message Sent " + me.FirstName);
            Console.WriteLine(weatherClient.getTemp());
        }
    }
}
