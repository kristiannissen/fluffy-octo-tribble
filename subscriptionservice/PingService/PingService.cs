using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace PingService
{
    public class Ping
    {
      private static readonly HttpClient _client = new HttpClient();

      public static async Task<string> ShowPongMsg()
      {
        _client.DefaultRequestHeaders.Add("Accept-Version", "v10");
        string response = await _client.GetStringAsync("https://api.quickpay.net/ping");

        Pong pong = JsonSerializer.Deserialize<Pong>(response);

        return pong.msg;
      }

      private class Pong
      {
        public string msg { get; set; }
      }
    }
}
