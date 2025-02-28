using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RubbenHedgeStrategy
{
    class SendMessage
    {
        private static readonly string token = "7796410603:AAHOMdIfyUOuXLSOcnf39Te0P-wUq_y1Eoc";

        private static readonly string channel = "-1002419241659";

        public static async Task SendMessageAsync(string message)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={channel}&text={Uri.EscapeDataString(message)}";
                var response = await httpClient.GetAsync(url);
            }
        }
    }
}
