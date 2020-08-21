using System;
using Xunit;
using Subscription.Services;

namespace SubscriptionServiceTests
{
    public class SubscriptionServiceTests
    {
            private readonly SubscriptionService _subSer;

            public SubscriptionServiceTests()
            {
              string API_KEY = Environment.GetEnvironmentVariable("API_KEY");
                    _subSer = new SubscriptionService(API_KEY);
            }

        [Fact]
        public void FirstTest()
        {
          Assert.False(false, "Hey Kitty");
        }

        [Fact]
        public void CreateNewSubscriptionTest()
        {
          _subSer.Create("lala", "DKK", "Hello Kitty Subscription");
        }
    }
}
