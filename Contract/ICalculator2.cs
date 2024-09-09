namespace Contract;

/// <summary>
/// Calculator interface
/// </summary>
public interface ICalculator2 : ICalculator
{
    /// <summary>
    /// Calculates the result of the expression based on the calculator rules,
    /// and returns a Tuple with the result and the formula used
    /// </summary>
    /// <param name="expression">The expression to calculate</param>
    /// <param name="op">The operator to apply</param>
    /// <returns>Tuple with the calculated result of the expression and the formula</returns>
    (decimal result, string formula) CalculateWithFormula(
        string expression, 
        CalculatorOperator op = CalculatorOperator.Add);
}
