using System;
using Xunit;
using System.Threading.Tasks;
using PingService;

namespace PingService.Tests
{
    public class PingServiceTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(true, "It's true digface");
        }

        [Fact]
        public async Task GetPongResponseMsg()
        {
            var result = await Ping.ShowPongMsg();

            Assert.Equal("Pong from QuickPay API V10, scope is anonymous", result);
        }
    }
}
