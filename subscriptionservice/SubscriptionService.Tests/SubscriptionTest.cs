using System;
using Xunit;
using Subscription.Services;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

namespace SubscriptionServiceTests
{
    public class SubscriptionServiceTests
    {
        private SubscriptionService sut;

        public SubscriptionServiceTests()
        {
            string API_KEY = Environment.GetEnvironmentVariable("API_KEY");
            sut = new SubscriptionService(API_KEY);
        }

        [Fact]
        public void FirstTest()
        {
            Assert.False(false, "Hey Kitty");
        }

        [Fact]
        public async Task CreateNewSubscriptionTest()
        {
            int subscriptionId = await sut.Create(RandomString(10), "DKK", "Kitty porn subscription");
            Assert.True(subscriptionId > 0);
        }

        private static string RandomString(int length)
        {
          const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
          Random random = new Random();
          StringBuilder builder = new StringBuilder();
          for (int i = 0; i < length; i++) {
            var c = pool[random.Next(0, pool.Length)];
            builder.Append(c);
          }
          return builder.ToString();
        }
    }
}
