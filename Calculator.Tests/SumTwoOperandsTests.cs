using Contract;

namespace Calculator.Tests
{
    public class SumTwoOperandsTests
    {
        private readonly ICalculator _calculator;

        public SumTwoOperandsTests()
        {
            _calculator = new SumTwoOperandsCalculator();
        }

        [Fact]
        public void SanityTest()
        {
            Assert.True(true);
        }

        [Theory]
        // Requirement 1
        [InlineData(null, 0)]
        [InlineData(@"", 0)]
        [InlineData(@"a*b*c", 0)]
        [InlineData(@",", 0)]
        [InlineData(@"0", 0)]
        [InlineData(@"10", 10)]
        [InlineData(@"0,0", 0)]
        [InlineData(@"101,99", 200)]
        [InlineData(@"-1,-1", -2)]
        [InlineData(@"2000,-1000", 1000)]
        [InlineData(@"1,abc", 1)]
        [InlineData(@"x,y", 0)]
        [InlineData(@"1.1,1.1", 2.2)]
        [InlineData(@"-0.5,-0.5", -1)]
        [InlineData(@"0.333333333", 0.333333333)]
        [InlineData(@"0.333333333,0.666666666", 0.999999999)]
        [InlineData(@"20", 20)]
        [InlineData(@"1,5000", 5001)]
        [InlineData(@"4,-3", 1)]
        [InlineData(@"5,tytyt", 5)]
        public void CalcTest(string expression, decimal expectedResult)
        {
            decimal result = _calculator.Calculate(expression);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        // Requirement 1
        [InlineData(@",,")]
        [InlineData(@",,,")]
        [InlineData(@"1,2,3")]
        [InlineData(@"a,b,c")]
        [InlineData(@"*,+,1")]
        [InlineData(@"1.1,2.2,3.3")]
        [InlineData(@"1,2,3,4,5,6")]
        public void TooManyOperandsTest(string expression)
        {
            Assert.Throws<TooManyOperandsCalculatorException>(
                () => _calculator.Calculate(expression)
            );
        }
    }
}