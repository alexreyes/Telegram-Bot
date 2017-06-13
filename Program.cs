using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Practice
{
    class Program
    {
        string chatId = "164850555";
        static void Main(string[] args)
        {

            testApiAsync();
            Console.ReadLine();
    
        }
        static async void testApiAsync()
        {
            var Bot = new Telegram.Bot.TelegramBotClient("267703378:AAHaHqsitEahdo2j8F3F4uz0DGiu6aXQvL4");
            var me = await Bot.GetMeAsync();

            //var t = await Bot.SendTextMessage("164850555", "");
            Console.WriteLine("Hello my name is " + me.FirstName);
            Console.ReadLine();
        }
    }
}
