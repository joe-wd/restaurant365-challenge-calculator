using Contract;

namespace Calculator.Tests
{
    public class SumTests
    {
        private readonly ICalculator _calculator;

        public SumTests()
        {
            _calculator = new SumCalculator();
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

        // Requirement 2
        [InlineData(@",,", 0)]
        [InlineData(@",,,", 0)]
        [InlineData(@",,,,,,,,", 0)]
        [InlineData(@"1,2,3", 6)]
        [InlineData(@"a,b,c", 0)]
        [InlineData(@"*,+,1", 1)]
        [InlineData(@"1.1,2.2,3.3", 6.6)]
        [InlineData(@"-1,2.5,1", 2.5)]
        [InlineData(@"1,2,3,4,5,6,7,8,9,10,11,12", 78)]

        // Requirement 3
        [InlineData(@"1\n2,3", 6)]
        [InlineData(@"1\n2.2\n\nabc\n", 3.2)]
        [InlineData(@"1.1\n2.2\n3.3,4.4,,,", 11)]
        
        public void CalcTest(string expression, decimal expectedResult)
        {
            decimal result = _calculator.Calculate(expression);
            Assert.Equal(expectedResult, result);
        }
    }
}