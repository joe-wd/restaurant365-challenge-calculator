using Contract;
using Calculator;

/// <summary>
/// Simple addition calculator that calculates the result of comma separated values
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        try
        {
            ICalculator calculator = new SumCalculator();
            List<CalculatorOperator> supportedOperators = calculator.SupportedOperators;
            List<string> supportedOperatorNames = supportedOperators.Select(s => s.ToString()).ToList();

            // Expression user input
            Console.Write("Enter expression: ");
            string? expression = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(expression) || supportedOperators.Count == 0)
            {
                Console.WriteLine(0);
            }
            else 
            {
                expression = expression.Trim();

                // Operator user input if needed
                CalculatorOperator calculatorOperator = supportedOperators.First();
                if (supportedOperators.Count > 1)
                {
                    // Nice to have: accept operator symbol instead of name
                    Console.Write($"Enter operator ({string.Join(", ", supportedOperatorNames)}): ");
                    string? selectedOperator = Console.ReadLine();
                    if (!Enum.TryParse(selectedOperator, out calculatorOperator)) 
                    {
                        throw new UnsupportedOperatorCalculatorException(selectedOperator ?? "");
                    }
                }

                Console.WriteLine(calculator.Calculate(expression, calculatorOperator));
            }
        }
        catch (UnsupportedOperatorCalculatorException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Operator: {e.Operator}");
        }
        catch (TooManyOperandsCalculatorException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Maximum allowed: {e.MaxOperands}");
        }
        catch (NegativeValuesCalculatorException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Negative operands found: {string.Join(',', e.NegativeValues)}");
        }
        catch (Exception e) 
        {
            Console.WriteLine(e.Message);
        }
    }

}


