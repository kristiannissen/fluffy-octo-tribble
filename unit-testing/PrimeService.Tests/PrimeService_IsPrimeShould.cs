using System;
using Xunit;
using Prime.Services;

namespace Prime.UnitTests.Services
{
    public class PrimeService_IsPrimeShould
    {
            private readonly PrimeService _primeService;
            public PrimeService_IsPrimeShould()
            {
                    _primeService = new PrimeService();
            }
        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0)]
        public void IsPrime_ValueLessThan2_ReturnFalse(int value)
        {
          var result = _primeService.IsPrime(value);
          Assert.False(result, $"Hey Fucker {value} should not be a prime");
        }
    }
}
