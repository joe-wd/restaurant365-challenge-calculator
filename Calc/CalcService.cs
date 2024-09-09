using System.Text;
using Contract;

public class CalcService(ICalculator2 calculator) 
{
    public void Run()
    {
        // Build the prompt for operator selection
        List<CalculatorOperator> supportedOperators = calculator.SupportedOperators;
        List<string> supportedOperatorNames = supportedOperators.Select(s => s.ToString()).ToList();
        StringBuilder sb = new();
        for (int i = 0; i < supportedOperatorNames.Count; i++)
        {
            sb.AppendFormat($"{i}={supportedOperatorNames[i]}");
            if (i < supportedOperatorNames.Count - 1)
            {
                sb.Append(", ");
            }
        }

        while (true)
        {
            try
            {

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
                    CalculatorOperator calculatorOperator;
                    if (supportedOperators.Count == 1)
                    {
                        calculatorOperator = supportedOperators.First();
                    }
                    else
                    {
                        // Nice to have: accept operator symbol instead of name
                        Console.Write($"Enter operator ({sb.ToString()}): ");
                        string selectedOperator = Console.ReadLine() ?? "";
                        int selectedOperatorVal;
                        
                        if (int.TryParse(selectedOperator, out selectedOperatorVal) && 
                            Enum.IsDefined(typeof(CalculatorOperator), selectedOperatorVal))
                        {
                            calculatorOperator = (CalculatorOperator) (int.Parse(selectedOperator));
                        }
                        else
                        {
                            throw new UnsupportedOperatorCalculatorException(selectedOperator);
                        }
                    }

                    var result = calculator.CalculateWithFormula(expression, calculatorOperator);
                    Console.WriteLine($"{result.formula} = {result.result}");
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
            catch (InvalidExpressionCalculatorException e)
            {
                Console.WriteLine($"{e.Message}: {e.Expression}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}