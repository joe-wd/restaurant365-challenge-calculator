namespace Contract;

public enum CalculatorOperator
{
    Add,
    Subtract,
    Multiply,
    Divide,
}

/// <summary>
/// Calculator interface
/// </summary>
public interface ICalculator
{
    /// <summary>
    /// Calculates the result of the expression based on the calculator rules
    /// </summary>
    /// <param name="expression">The expression to calculate</param>
    /// <param name="op">The operator to apply</param>
    /// <returns>The calculated result of the expression</returns>
    decimal Calculate(string expression, CalculatorOperator op = CalculatorOperator.Add);

    /// <summary>
    /// List of operators supported by the calculator implementation
    /// </summary>
    List<CalculatorOperator> SupportedOperators { get; }
}
