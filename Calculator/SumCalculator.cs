using System.Data;
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

    public List<CalculatorOperator> SupportedOperators => [CalculatorOperator.Add];

    /// <summary>
    /// Calculates the sum of values separated by "," or "\n"
    /// Non-numeric/missing/empty values are treated as 0.
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
        var operands = expression
            .Split(_delimiters, StringSplitOptions.None)
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
    /// </summary>
    /// <param name="operand">String operand</param>
    /// <returns>Decimal value of the provided string operand, or 0 if not a valid decimal number</returns>
    protected virtual decimal EvalOperand(string operand)
    {
        return Decimal.TryParse(operand, out decimal val) ? val : 0;
    }
}
