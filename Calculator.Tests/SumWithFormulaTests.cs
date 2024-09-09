using Contract;

namespace Calculator.Tests
{
    public class SumWithFormulaTests
    {
        private readonly ICalculator2 _calculator;

        public SumWithFormulaTests()
        {
            _calculator = new SumCalculator();
        }

        [Theory]
        // Stretch Requirement 1
        [InlineData(null, 0, "")]
        [InlineData(@"", 0, "")]
        [InlineData(@",,,", 0, "0+0+0+0")]
        [InlineData(@"1.1,1.1", 2.2, "1.1+1.1")]
        [InlineData(@"20", 20, "20")]
        [InlineData(@"5,tytyt", 5, "5+0")]
        [InlineData(@"2,,4,rrrr,1001,6", 12, "2+0+4+0+0+6")]
        public void CalcTest(string expression, decimal expectedResult, string expectedFormula)
        {
            var result = _calculator.CalculateWithFormula(expression);
            Assert.Equal(expectedResult, result.result);
            Assert.Equal(expectedFormula, result.formula);
        }
    }
}