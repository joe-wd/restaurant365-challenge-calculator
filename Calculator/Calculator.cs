using System.Data;
using System.Text.RegularExpressions;
using Contract;

namespace Calculator;

/// <summary>
/// ICalculator implementation that calculates operands separated by "," or "\n", or custom delimiters.
/// Supports add, subtract, multiply, divide.
/// Non-numeric values and numeric > 1000 are treated as 0.
/// Negative operands disallowed.
/// </summary>
public class Calculator : ICalculator2
{
    protected readonly string[] _delimiters =
    [
        @",",
        @"\n"
    ];

    protected readonly decimal _maxValue = 1000;
    protected const string _customDelimiterPrefix = @"//";
    protected const string _customDelimiterPattern = @"^//(.)\\n(.*)$";
    protected const string _customDelimiterPattern2 = @"^//(?:\[(.+?)\])+?\\n(.*)$";
    protected readonly Dictionary<CalculatorOperator, string> _operatorSymbols = new() {
       { CalculatorOperator.Add,  @"+" },
       { CalculatorOperator.Subtract,  @"-" },
       { CalculatorOperator.Multiply,  @"*" },
       { CalculatorOperator.Divide,  @"/" },
    };

    public virtual List<CalculatorOperator> SupportedOperators => 
    [ 
        CalculatorOperator.Add,
        CalculatorOperator.Subtract,
        CalculatorOperator.Multiply,
        CalculatorOperator.Divide
    ];

    /// <summary>
    /// Calculates the sum of values separated by "," or "\n"
    /// Negative numbers disallowed
    /// Non-numeric/missing/empty values are treated as 0.
    /// Numbers greater than 1000 are treated as 0.
    /// </summary>
    /// <param name="expression">Values separated by "," or "\n"</param>
    /// <returns>The sum of the values in the expression</returns>
    public virtual decimal Calculate(string expression, CalculatorOperator op = CalculatorOperator.Add)
    {
        return CalculateWithFormula(expression, op).result;
    }

    /// <summary>
    /// Calculates the result of the expression based on the calculator rules,
    /// and returns a Tuple with the result and the formula used
    /// </summary>
    /// <param name="expression">The expression to calculate</param>
    /// <param name="op">The operator to apply</param>
    /// <returns>Tuple with the calculated result of the expression and the formula</returns>
    public virtual (decimal result, string formula) CalculateWithFormula(
        string expression,
        CalculatorOperator op = CalculatorOperator.Add)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return (0, "");
        }

        // parse the input to get any custom delimiters and the expression to calculate
        var parseResult = ParseExpression(expression);
        var operands = parseResult.expression
            .Split(parseResult.delimiters, StringSplitOptions.None)
            .Select(o => EvalOperand(o));

        var negativeOperands = operands.Where(o => o < 0);
        if (negativeOperands.Any())
        {
            throw new NegativeValuesCalculatorException(negativeOperands.ToArray());
        }
        else
        {
            switch (op)
            {
                case CalculatorOperator.Add:
                    return CalculateSumWithFormula(operands.ToArray());
                case CalculatorOperator.Subtract:
                    return CalculateDifferenceWithFormula(operands.ToArray());
                case CalculatorOperator.Multiply:
                    return CalculateProductWithFormula(operands.ToArray());
                case CalculatorOperator.Divide:
                    return CalculateQuotientWithFormula(operands.ToArray());
                default:
                    throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// Calculates the sum of the expression
    /// Negative numbers disallowed
    /// Non-numeric/missing/empty values are treated as 0.
    /// Numbers greater than 1000 are treated as 0.
    /// </summary>
    /// <param name="operands">Delimiters to use</param>
    /// <param name="expression">Expression to calculate</param>
    /// <returns>The sum</returns>
    /// <exception cref="TooManyOperandsCalculatorException"></exception>
    protected virtual (decimal result, string formula) CalculateSumWithFormula(decimal[] operands)
    {
        var formula = string.Join(_operatorSymbols[CalculatorOperator.Add], operands);
        return (operands.Sum(), formula);
    }

    /// <summary>
    /// Calculates the difference of the operands in expression
    /// Negative numbers disallowed
    /// Non-numeric/missing/empty values are treated as 0.
    /// Numbers greater than 1000 are treated as 0.
    /// </summary>
    /// <param name="operands">Delimiters to use</param>
    /// <param name="expression">Expression to calculate</param>
    /// <returns>The difference</returns>
    /// <exception cref="TooManyOperandsCalculatorException"></exception>
    protected virtual (decimal result, string formula) CalculateDifferenceWithFormula(decimal[] operands)
    {
        var formula = string.Join(_operatorSymbols[CalculatorOperator.Subtract], operands);
        // Start with a null value for the aggregator, because the first value
        // should not be subtracted from anything (it should return self)
        decimal? diff = null;
        var finalDiff = operands.Aggregate(diff, (diff, operand) =>
            diff == null ? operand : diff - operand);

        return (finalDiff ?? 0, formula);
    }

    /// <summary>
    /// Calculates the product of the operands in expression
    /// Negative numbers disallowed
    /// Non-numeric/missing/empty values are treated as 0.
    /// Numbers greater than 1000 are treated as 0.
    /// </summary>
    /// <param name="operands">Delimiters to use</param>
    /// <param name="expression">Expression to calculate</param>
    /// <returns>The product</returns>
    /// <exception cref="TooManyOperandsCalculatorException"></exception>
    protected virtual (decimal result, string formula) CalculateProductWithFormula(decimal[] operands)
    {
        var formula = string.Join(_operatorSymbols[CalculatorOperator.Multiply], operands);
        if (operands.Any(o => o == 0)) 
        {
            return (0, formula);
        }
        else 
        {
            decimal product = 1;
            var finalProduct = operands.Aggregate(product, (product, operand) => product * operand);
            return (finalProduct, formula);
        }
    }

    /// <summary>
    /// Calculates the quotient of the operands in expression
    /// Negative numbers disallowed
    /// Non-numeric/missing/empty values are treated as 0.
    /// Numbers greater than 1000 are treated as 0.
    /// </summary>
    /// <param name="operands">Delimiters to use</param>
    /// <param name="expression">Expression to calculate</param>
    /// <returns>The quotient</returns>
    /// <exception cref="DivideByZeroException"></exception>
    protected virtual (decimal result, string formula) CalculateQuotientWithFormula(decimal[] operands)
    {
        var formula = string.Join(_operatorSymbols[CalculatorOperator.Divide], operands);
        // 0 for the first operand is ok, but result will always be 0.
        if (operands.Count() > 0 && operands.First() == 0)
        {
            return (0, formula);
        }
        else if (operands.Contains(0))
        {
            // Could just let the division operation throw this, but nice to be explicit
            throw new DivideByZeroException();
        }
        else
        {
            // Start with a null value for the aggregator, because the first value
            // should not be divide by anything (it should return self)
            decimal? quotient = null;
            var finalQuotient = operands.Aggregate(quotient, (quotient, operand) =>
                quotient == null ? operand : quotient / operand);
            return (finalQuotient ?? 0m, formula);
        }
    }

    /// <summary>
    /// Converts the provided string to a decimal value.
    /// Any values over _maxValue (1000) are treated as 0.
    /// </summary>
    /// <param name="operand">String operand</param>
    /// <returns>Decimal value of the provided string operand, or 0 if not a valid decimal number</returns>
    protected virtual decimal EvalOperand(string operand)
    {
        return Decimal.TryParse(operand, out decimal val) && val <= _maxValue ? val : 0;
    }

    /// <summary>
    /// Parses the provided expression to determine whether there are any custom delimiters
    /// </summary>
    /// <param name="expression">expression possibly with custom delimiters</param>
    /// <returns>A tuple with the delimiters and expression to compute</returns>
    /// <exception cref="InvalidExpressionCalculatorException"></exception>
    protected (string[] delimiters, string expression) ParseExpression(string expression)
    {
        // Any custom delimiter?
        if (!expression.StartsWith(_customDelimiterPrefix))
        {
            // No, just use the standard delimiters and return the whole expression provided
            return (_delimiters, expression);
        }
        else if (Regex.IsMatch(expression, _customDelimiterPattern, RegexOptions.None, TimeSpan.FromSeconds(5)))
        {
            // Expecting one single-char custom delimiter
            return ParseMatch(expression, _customDelimiterPattern);
        }
        else if (Regex.IsMatch(expression, _customDelimiterPattern2, RegexOptions.None, TimeSpan.FromSeconds(5)))
        {
            // Expecting one or more custom delimiters of any length, each in square brackets
            return ParseMatch(expression, _customDelimiterPattern2);
        }

        throw new InvalidExpressionCalculatorException(expression);
    }

    private (string[] delimiters, string expression) ParseMatch(string expression, string pattern)
    {
        var matches = Regex.Matches(
                expression,
                pattern,
                RegexOptions.None,
                TimeSpan.FromSeconds(5));

        // Regex must have at least 1 match with 3 groups: 
        // 1) the whole input, 
        // 2) the delimiters, 
        // 3) the expression
        if (matches.Count == 1 && matches.First().Groups.Count == 3)
        {
            var match = matches.First();
            // Extract the delimiters from group 1
            List<string> delimiters = matches
                .First()
                .Groups[1]
                .Captures
                .Select(c => c.Value)
                .ToList<string>();

            var remainingExpression = matches.First().Groups[2].Captures[0].Value;

            // Include the standard delimiters in the return value; they still apply
            return (delimiters.Concat(_delimiters).ToArray(), remainingExpression);
        }

        throw new InvalidExpressionCalculatorException(expression);
    }
}
