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
        [InlineData(@"1,abc", 1)]
        [InlineData(@"x,y", 0)]
        [InlineData(@"1.1,1.1", 2.2)]
        [InlineData(@"0.333333333", 0.333333333)]
        [InlineData(@"0.333333333,0.666666666", 0.999999999)]
        [InlineData(@"20", 20)]
        [InlineData(@"5,tytyt", 5)]

        // Requirement 2
        [InlineData(@",,", 0)]
        [InlineData(@",,,", 0)]
        [InlineData(@",,,,,,,,", 0)]
        [InlineData(@"1,2,3", 6)]
        [InlineData(@"a,b,c", 0)]
        [InlineData(@"*,+,1", 1)]
        [InlineData(@"1.1,2.2,3.3", 6.6)]
        [InlineData(@"1,2,3,4,5,6,7,8,9,10,11,12", 78)]

        // Requirement 3
        [InlineData(@"1\n2,3", 6)]
        [InlineData(@"1\n2.2\n\nabc\n", 3.2)]
        [InlineData(@"1.1\n2.2\n3.3,4.4,,,", 11)]

        // Requirement 5
        [InlineData("1,5000", 1)]
        [InlineData("2,1001,6", 8)]
        [InlineData(",,\n,,9999,,\n,,", 0)]

        // Requirement 6
        [InlineData(@"//#\n2#5", 7)]
        [InlineData(@"//,\n2,ff,100", 102)]
        [InlineData(@"//x\n1,2,3,,,x94x,", 100)]

        // Requirement 7
        [InlineData(@"//[***]\n11***22***33", 66)]
        [InlineData(@"//[#]\n5#10#15,20", 50)]

        // Requirement 8
        [InlineData(@"//[*][!!][r9r]\n11r9r22*hh*33!!44", 110)]

        public void CalcTest(string expression, decimal expectedResult)
        {
            decimal result = _calculator.Calculate(expression);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        // Requirement 4
        [InlineData(@"-1,-1")]
        [InlineData(@"2000,-1000")]
        [InlineData(@"-0.5,-0.5")]
        [InlineData(@"4,-3")]
        [InlineData(@"-1,2.5,1")]
        [InlineData(@"1,1,-1")]
        [InlineData(@"-7,-8,-9,10,,,")]
        public void NegativeOperandsTest(string expression)
        {
            Assert.Throws<NegativeValuesCalculatorException>(
                () => _calculator.Calculate((expression))
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
                () => _calculator.Calculate(expression)
            );
        }
    }
}