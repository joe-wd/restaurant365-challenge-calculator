using System.Data;
using Contract;

namespace Calculator;

/// <summary>
/// ICalculator implementation that sums up to two comma-separated operands
/// </summary>
public class SumTwoOperandsCalculator : ICalculator
{
    private const string _delimiter = @",";
    
    private const int _maxOperands = 2;

    public List<CalculatorOperator> SupportedOperators => [CalculatorOperator.Add];

    /// <summary>
    /// Calculates the sum of two comma-separated values.
    /// Non-numeric/missing/empty values are treated as 0.
    /// </summary>
    /// <param name="expression">Up to two comma-separated values</param>
    /// <returns>The sum of the two values in the expression</returns>
    /// <exception cref="TooManyOperandsCalculatorException"></exception>
    public decimal Calculate(string expression, CalculatorOperator op = CalculatorOperator.Add)
    {
        switch (op) {
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
    private decimal CalculateSum(string expression) {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return 0;
        }
        var operands = expression.Split(_delimiter);
        if (operands.Length > _maxOperands)
        {
            throw new TooManyOperandsCalculatorException(_maxOperands);
        }
        var numericOperands = operands.Select(o => EvalOperand(o));
        return numericOperands.Sum();
    }

    /// <summary>
    /// Converts the provided string to a decimal value.
    /// </summary>
    /// <param name="operand">String operand</param>
    /// <returns>Decimal value of the provided string operand, or 0 if not a valid decimal number</returns>
    private decimal EvalOperand(string operand)
    {
        return Decimal.TryParse(operand, out decimal val) ? val : 0;
    }
}
