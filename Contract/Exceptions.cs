namespace Contract;

public class CalculatorException(string message) : Exception(message)
{
}

public class UnsupportedOperatorCalculatorException(string op) : CalculatorException(_message)
{
    static readonly string _message = "Unsupported operator";

    public string Operator { get; private set; } = op;
}

public class TooManyOperandsCalculatorException(int maxOperands) : CalculatorException(_message)
{
    static readonly string _message = "Expression contains too many operands";

    public int MaxOperands { get; private set; } = maxOperands;
}

public class NegativeValuesCalculatorException(decimal[] negativeValues) : CalculatorException(_message)
{
    static readonly string _message = "Expression cannot contain negative operands";

    public decimal[] NegativeValues { get; private set; } = negativeValues;
}

public class InvalidExpressionCalculatorException(string expression) : CalculatorException(_message)
{
    static readonly string _message = "Invalid calculator expression";

    public string Expression { get; private set; } = expression;
}
