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

        public string continueUrl { get; set; }
        public string cancelUrl { get; set; }
        public string callbackUrl { get; set; }

        public SubscriptionService(string API_KEY)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept-Version", "v10");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string encoded = Base64Encode($":{API_KEY}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
            // Set default for URLs
            continueUrl = "";
            cancelUrl = "";
            callbackUrl = "";
        }

        public async Task<int> Create(string orderId, string currency, string description)
        {
            Subscription sub = new Subscription();
            sub.order_id = orderId;
            sub.currency = currency;
            sub.description = description;

            string json = JsonSerializer.Serialize(sub);
            StringContent sC = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.quickpay.net/subscriptions", sC);

            try
            {
                response.EnsureSuccessStatusCode();

                if (response.Content is Object)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    sub = JsonSerializer.Deserialize<Subscription>(responseBody);
                }
            }
            finally
            {
                response.Dispose();
            }
            return sub.id;
        }

        public async Task<string> GetPaymentLinkUrl(string orderId, string currency, string description, int amount)
        {
            int subscriptionid = await Create(orderId, currency, description);

            Subscription sub = new Subscription();
            sub.amount = amount;
            sub.continue_url = continueUrl;
            sub.cancel_url = cancelUrl;
            sub.callback_url = callbackUrl;

            string json = JsonSerializer.Serialize(sub);
            StringContent sC = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"https://api.quickpay.net/subscriptions/{subscriptionid}/link", sC);

            try
            {
                response.EnsureSuccessStatusCode();
                if (response.Content is Object)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    sub = JsonSerializer.Deserialize<Subscription>(responseBody);
                }
            }
            finally
            {
                response.Dispose();
            }
            return sub.url;
        }

        public async Task<string> CreateRecurring(int subscriptionid, int amount, string orderid, bool autoCapture)
        {
            Subscription sub = new Subscription();
            sub.amount = amount;
            sub.order_id = orderid;
            sub.auto_capture = autoCapture;

            string json = JsonSerializer.Serialize(sub);
            StringContent sC = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"https://api.quickpay.net/subscriptions/{subscriptionid}/recurring", sC);

            try
            {
                response.EnsureSuccessStatusCode();
                if (response.Content is Object)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    sub = JsonSerializer.Deserialize<Subscription>(responseBody);
                }
            }
            finally
            {
                response.Dispose();
            }
            return sub.state;
        }

        public async Task<string> CheckState(int subscriptionid)
        {
            Subscription sub = new Subscription();
            HttpResponseMessage response = await client.GetAsync($"https://api.quickpay.net/subscriptions/{subscriptionid}");

            try
            {
                response.EnsureSuccessStatusCode();
                if (response.Content is Object)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    sub = JsonSerializer.Deserialize<Subscription>(responseBody);
                }
            }
            finally
            {
                response.Dispose();
            }
            return sub.state;
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
            public string state { get; set; }
            public bool auto_capture { get; set; }
            public string continue_url { get; set; }
            public string cancel_url { get; set; }
            public string callback_url { get; set; }
        }
    }
}
