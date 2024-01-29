using ErrorService.Core;
using ErrorService.Core.Domains;
using SmsSender;
using System;
using System.Collections.Generic;

namespace ErrorServiceSMS.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Usługa start");
            var smsText = new GenrateHtmlSms();
            var clientNumber = "";
            string token = "";
            SMS sms = new SMS(token);

            var errors =  new List<Error>
            {
                new Error{Id = 1, Message = "Błąd test1", Date = DateTime.Now },
                new Error{Id = 2, Message = "Błąd test2", Date = DateTime.Now },
            };

            sms.Send(clientNumber, smsText.GenrateError(errors, 1));
            Console.WriteLine("Send error");
            Console.ReadLine();
        }
    }
}
