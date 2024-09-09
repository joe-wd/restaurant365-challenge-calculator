using System.Data;
using System.Text.RegularExpressions;
using Contract;

namespace Calculator;

/// <summary>
/// ICalculator implementation that sums operands separated by "," or "\n"
/// </summary>
public class SumCalculator : ICalculator
{
    protected readonly string[] _delimiters =
    [
        @",",
        @"\n"
    ];

    protected readonly decimal _maxValue = 1000;
    protected const string _customDelimiterPrefix = @"//";
    protected const string _customDelimiterPattern = @"^//(.)\\n(.*)$";
    protected const string _customDelimiterPattern2 = @"^//\[(.+?)\]\\n(.*)$";


    public List<CalculatorOperator> SupportedOperators => [CalculatorOperator.Add];

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
        switch (op)
        {
            case CalculatorOperator.Add:
                return CalculateSum(expression);
            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Calculates the sum of the expression
    /// Negative numbers disallowed
    /// Non-numeric/missing/empty values are treated as 0.
    /// Numbers greater than 1000 are treated as 0.
    /// </summary>
    /// <param name="expression">Expression to calculate</param>
    /// <returns>The sum</returns>
    /// <exception cref="TooManyOperandsCalculatorException"></exception>
    protected virtual decimal CalculateSum(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return 0;
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
            return operands.Sum();
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
            // Expecting one custom delimiter any length, in square brackets
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
