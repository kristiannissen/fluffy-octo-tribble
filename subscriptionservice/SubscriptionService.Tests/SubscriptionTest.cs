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
                    _subSer = new SubscriptionService();
            }

        [Fact]
        public void FirstTest()
        {
          Assert.False(false, "Hey Kitty");
        }

        [Fact]
        public void CreateNewSubscriptionTest()
        {
          _subSer.create("lala", "DKK", "Hello Kitty Subscription");
        }
    }
}
