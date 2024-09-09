using Contract;

namespace Calculator.Tests
{
    public class CalculatorTests
    {
        private readonly ICalculator2 _calculator;

        public CalculatorTests()
        {
            _calculator = new Calculator();
        }

        [Theory]
        // Stretch Requirement 1
        [InlineData(null, 0, "")]
        [InlineData(@"", 0, "")]
        [InlineData(@",,,", 0, "0+0+0+0")]
        [InlineData(@"1.1,1.1", 2.2, "1.1+1.1")]
        [InlineData(@"//[*][abc]\n2,2\n2*2abc2", 10, "2+2+2+2+2")]
        [InlineData(@"20", 20, "20")]
        [InlineData(@"5,tytyt", 5, "5+0")]
        [InlineData(@"2,,4,rrrr,1001,6", 12, "2+0+4+0+0+6")]
        public void CalcAddTest(string expression, decimal expectedResult, string expectedFormula)
        {
            var result = _calculator.CalculateWithFormula(expression, CalculatorOperator.Add);
            Assert.Equal(expectedResult, result.result);
            Assert.Equal(expectedFormula, result.formula);
        }

        [Theory]
        // Stretch Requirement 5 -- subtraction
        [InlineData(null, 0, "")]
        [InlineData(@"", 0, "")]
        [InlineData(@",,,", 0, "0-0-0-0")]
        [InlineData(@"1.1,1.1", 0, "1.1-1.1")]
        [InlineData(@"//[*][abc]\n2,2\n2*2abc2", -6, "2-2-2-2-2")]
        [InlineData(@"20", 20, "20")]
        [InlineData(@"5,tytyt", 5, "5-0")]
        [InlineData(@"2,,4,rrrr,1001,6", -8, "2-0-4-0-0-6")]
        public void CalcSubtractTest(string expression, decimal expectedResult, string expectedFormula)
        {
            var result = _calculator.CalculateWithFormula(expression, CalculatorOperator.Subtract);
            Assert.Equal(expectedResult, result.result);
            Assert.Equal(expectedFormula, result.formula);
        }

        [Theory]
        // Stretch Requirement 5 -- multiplication
        [InlineData(null, 0, "")]
        [InlineData(@"", 0, "")]
        [InlineData(@",,,", 0, "0*0*0*0")]
        [InlineData(@"1.1,1.1", 1.21, "1.1*1.1")]
        [InlineData(@"//[*][abc]\n2,2\n2*2abc2", 32, "2*2*2*2*2")]
        [InlineData(@"20", 20, "20")]
        [InlineData(@"5,tytyt", 0, "5*0")]
        [InlineData(@"2,,4,rrrr,1001,6", 0, "2*0*4*0*0*6")]
        public void CalcMultiplyTest(string expression, decimal expectedResult, string expectedFormula)
        {
            var result = _calculator.CalculateWithFormula(expression, CalculatorOperator.Multiply);
            Assert.Equal(expectedResult, result.result);
            Assert.Equal(expectedFormula, result.formula);
        }

        [Theory]
        // Stretch Requirement 5 -- division
        [InlineData(null, 0, "")]
        [InlineData(@"", 0, "")]
        [InlineData(@",,,", 0, "0/0/0/0")]
        [InlineData(@"1.1,1.1", 1, "1.1/1.1")]
        [InlineData(@"//[*][abc]\n2,2\n2*2abc2", .125, "2/2/2/2/2")]
        [InlineData(@"20", 20, "20")]
        [InlineData(@"0\n5,tytyt", 0, "0/5/0")]
        [InlineData(@"0,2,,4,rrrr,1001,6", 0, "0/2/0/4/0/0/6")]
        public void CalcDivideTest(string expression, decimal expectedResult, string expectedFormula)
        {
            var result = _calculator.CalculateWithFormula(expression, CalculatorOperator.Divide);
            Assert.Equal(expectedResult, result.result);
            Assert.Equal(expectedFormula, result.formula);
        }

        [Theory]
        // Stretch Requirement 5 - division exception
        [InlineData(@"1,,")]
        [InlineData(@"1,2,3,0,4")]
        [InlineData(@"1,2,3000,4")]
        public void DivideByZeroTest(string expression)
        {
            Assert.Throws<DivideByZeroException>(
                () => _calculator.CalculateWithFormula(expression, CalculatorOperator.Divide)
            );
        }

        [Theory]
        // Requirement 6
        // Console input is not escaped, so test strings should behave the same way.
        // In this case, the @"\t" is two characters, not a tab char, so cannot be used.
        // Delimiters must be one character
        [InlineData(@"//\t\n5\t5,,5,")]
        [InlineData(@"//**\n1,2,3")]
        [InlineData(@"//xyzzy\n1,2,3")]
        public void InvalidExpressionTest(string expression)
        {
            Assert.Throws<InvalidExpressionCalculatorException>(
                () => _calculator.CalculateWithFormula(expression, CalculatorOperator.Add)
            );
        }
    }
}