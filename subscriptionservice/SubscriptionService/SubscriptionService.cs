using System;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;

namespace Subscription.Services
{
    public class SubscriptionService
    {
        private static readonly HttpClient client = new HttpClient();

        public SubscriptionService(string API_KEY)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept-Version", "v10");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string encoded = Base64Encode($":{API_KEY}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
            // Console.WriteLine(client.DefaultRequestHeaders.ToString());
        }

        public async Task<int> Create(string orderId, string currency, string description)
        {
            Subscription sub = new Subscription();
            sub.order_id = orderId;
            sub.currency = currency;
            sub.description = description;

            string json = JsonSerializer.Serialize(sub);
            StringContent sC = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://api.quickpay.net/subscriptions", sC);
                // Console.WriteLine(response);
                string responseBody = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                Subscription subscription = JsonSerializer.Deserialize<Subscription>(responseBody);

                return subscription.id;
            }
            catch (HttpRequestException e)
            {
              // FIXME: pass on
                Console.WriteLine(e.Message);
            }
            return 0;
        }

        public async Task<string> GetPaymentLinkUrl(string orderId, string currency, string description, int amount)
        {
          int subscriptionid = await Create(orderId, currency, description);
          // Console.WriteLine(subscriptionid);

          Subscription sub = new Subscription();
          sub.amount = amount;

          string json = JsonSerializer.Serialize(sub);
          StringContent sC = new StringContent(json, Encoding.UTF8, "application/json");

          try
          {
            HttpResponseMessage response = await client.PutAsync($"https://api.quickpay.net/subscriptions/{subscriptionid}/link", sC);
            string responseBody = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            // Console.WriteLine(responseBody);

            Subscription subscription = JsonSerializer.Deserialize<Subscription>(responseBody);

            return subscription.url;
          }
          catch(HttpRequestException e)
          {
            Console.WriteLine(e);
          }
          return "hello pussy";
        }

        private static string Base64Encode(string text)
        {
          byte[] bytes = Encoding.UTF8.GetBytes(text);
          return Convert.ToBase64String(bytes);
        }

        private class Subscription
        {
            public int id { get; set; }
            public string description { get; set; }
            public int amount { get; set; }
            public string currency { get; set; }
            public string order_id { get; set; }
            public string url { get; set; }
        }
    }
}
