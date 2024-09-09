using System.Data;
using Contract;

namespace Calculator;

/// <summary>
/// ICalculator implementation that sums comma-separated operands
/// </summary>
public class SumCalculator : ICalculator
{
    protected const string _delimiter = @",";

    public List<CalculatorOperator> SupportedOperators => [CalculatorOperator.Add];

    /// <summary>
    /// Calculates the sum of comma-separated values.
    /// Non-numeric/missing/empty values are treated as 0.
    /// </summary>
    /// <param name="expression">Comma-separated values</param>
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
        var operands = expression.Split(_delimiter);
        var numericOperands = operands.Select(o => EvalOperand(o));
        return numericOperands.Sum();
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
