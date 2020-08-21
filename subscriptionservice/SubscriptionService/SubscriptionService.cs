using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;

namespace Subscription.Services
{
    public class SubscriptionService
    {
      private static readonly HttpClient client = new HttpClient();

      public SubscriptionService(string API_KEY)
      {
        client.DefaultRequestHeaders.Add("Accept-Version", "v10");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        // client.DefaultRequestHeaders.Add("Authorization", API_KEY);
        Console.WriteLine($"Shit Mulla, {API_KEY}");
      }

      public async void Create(string orderId, string currency, string description)
      {
        try {
          string response = await client.GetStringAsync("https://api.quickpay.net/ping");

        } catch(HttpRequestException e) {
          throw new Exception("Shit happens");
        }
      }

      public IEnumerable getAll()
      {
        throw new Exception("Method not implemented");
      }

      private class Subscription
      {
        public int id { get; set; }
        public String description { get; set; }
        public int amount { get; set; }
        public String currency { get; set; }
      }
    }
}
