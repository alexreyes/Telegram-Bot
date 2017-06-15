using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;


namespace Api_Practice
{
    class Program
    {
        private static Telegram.Bot.TelegramBotClient Bot = new Telegram.Bot.TelegramBotClient("267703378:AAHaHqsitEahdo2j8F3F4uz0DGiu6aXQvL4");

        static void Main(string[] args)
        {
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;
            Console.WriteLine("Started up the bot.");

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.Write("Received an error.");
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            InlineQueryResult[] results = {
                new InlineQueryResultLocation
                {
                    Id = "1",
                    Latitude = 40.7058316f, // displayed result
                    Longitude = -74.2581888f,
                    Title = "New York",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Latitude = 40.7058316f,
                        Longitude = -74.2581888f,
                    }
                },

                new InlineQueryResultLocation
                {
                    Id = "2",
                    Longitude = 52.507629f, // displayed result
                    Latitude = 13.1449577f,
                    Title = "Berlin",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Longitude = 52.507629f,
                        Latitude = 13.1449577f
                    }
                }
            };

            await Bot.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;

            if (message.Text.StartsWith("/inline")) // send inline keyboard
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                var keyboard = new InlineKeyboardMarkup(new[]
                {
                     new [] // first row
                    {
                        new InlineKeyboardButton("Example 1"),
                        new InlineKeyboardButton("Example 2"),
                        new InlineKeyboardButton("Example 3"),
                    },
                    new [] // last row
                    {
                        new InlineKeyboardButton("Row 2 example 1"),
                        new InlineKeyboardButton("Row 2 example 2"),
                    }
                });

                await Task.Delay(500); 

                await Bot.SendTextMessageAsync(message.Chat.Id, "Choose",
                    replyMarkup: keyboard);
            }
            if (message.Text.StartsWith("/keyboard")) // send custom keyboard
            {
                var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new [] // first row
                    {
                        new KeyboardButton("Example 1"),
                        new KeyboardButton("Example 2"),
                        new KeyboardButton("Example 3"),
                    },
                    new [] // last row
                    {
                        new KeyboardButton("Row 2 example 1"),
                        new KeyboardButton("Row 2 example 2"),
                    }
                });

                await Bot.SendTextMessageAsync(message.Chat.Id, "Test",
                    replyMarkup: keyboard);
            }
            if (message.Text.StartsWith("/photo")) // send a photo
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string file = @"C:\Users\alex.reyes\Desktop\favicon.jpg";

                var fileName = file.Split('\\').Last();

                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fts = new FileToSend(fileName, fileStream);

                    await Bot.SendPhotoAsync(message.Chat.Id, fts);
                }

            }
            if (message.Text.StartsWith("/request")) // request location or contact
            {
                var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("Location")
                    {
                        RequestLocation = true
                    },
                    new KeyboardButton("Contact")
                    {
                        RequestContact = true
                    },
                });

                try
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Who or Where are you?", replyMarkup: keyboard);
                }
                catch (Exception)
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Error: Due to Telegram restrictions, I can only send your location in a private chat.");
                }
            }

            if (message.Text.StartsWith("/weather")) {
                WeatherApiClient weather = new WeatherApiClient();

                string response = ("" + message.Text);
                int index = response.IndexOf("/weather");

                int responseLength = response.Length;
                if (responseLength > 8) // only happens if the user types in a custom zip code.
                {
                    string zipCode = response.Split(new string[] { "/weather " }, StringSplitOptions.None).Last();

                    await Bot.SendTextMessageAsync(message.Chat.Id, weather.getTemp(zipCode));
                }
                else
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id, weather.getTemp("17601"));

                }

            }
            if (message.Text.StartsWith("/help"))
            {
                var usage = @"Usage:
/inline   - send inline keyboard
/keyboard - send custom keyboard
/photo    - send a photo
/request  - request location or contact
/weather <zip code> - Returns the weather in the given zipcode. If no zipcode is given, it defaults to 17601.
";

                await Bot.SendTextMessageAsync(message.Chat.Id, usage,
                    replyMarkup: new ReplyKeyboardHide());
            }
            if (message.Text.StartsWith("/start"))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, "Already on. Make some commands with /help.");
            }

        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }
    }
}