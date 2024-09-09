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

