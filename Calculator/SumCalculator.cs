using System.Data;
using System.Text.RegularExpressions;
using Contract;

namespace Calculator;

/// <summary>
/// ICalculator implementation that sums operands separated by "," or "\n"
/// </summary>
public class SumCalculator : Calculator
{
    protected const string _operatorSymbol = @"+";
    override public List<CalculatorOperator> SupportedOperators => [CalculatorOperator.Add];

    /// <summary>
    /// Calculates the sum of values separated by "," or "\n"
    /// Negative numbers disallowed
    /// Non-numeric/missing/empty values are treated as 0.
    /// Numbers greater than 1000 are treated as 0.
    /// </summary>
    /// <param name="expression">Values separated by "," or "\n"</param>
    /// <returns>The sum of the values in the expression</returns>
    override public decimal Calculate(string expression, CalculatorOperator op = CalculatorOperator.Add)
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
    /// Calculates the result of the expression based on the calculator rules,
    /// and returns a Tuple with the result and the formula used
    /// </summary>
    /// <param name="expression">The expression to calculate</param>
    /// <param name="op">The operator to apply</param>
    /// <returns>Tuple with the calculated result of the expression and the formula</returns>
    override public (decimal result, string formula) CalculateWithFormula(
        string expression, 
        CalculatorOperator op = CalculatorOperator.Add)
    {
        switch (op)
        {
            case CalculatorOperator.Add:
                return base.CalculateWithFormula(expression, op);
            default:
                throw new NotImplementedException();
        }
    }

    protected virtual decimal CalculateSum(string expression)
    {
        return CalculateWithFormula(expression, CalculatorOperator.Add).result;
    }
}
