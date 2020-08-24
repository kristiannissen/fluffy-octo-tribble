using System;
using Xunit;
using Subscription.Services;
using System.Threading.Tasks;
using System.Net.Http;

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
            int subscriptionId = await sut.Create("ladida5", "DKK", "Kitty porn subscription");
            Assert.True(subscriptionId > 0);
        }
    }
}
