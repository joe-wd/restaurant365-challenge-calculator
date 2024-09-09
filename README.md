# Restaurant365 Challenge Calculator 


## Summary
Task: Create a calculator that only supports an Add operation given a single formatted string

Implemented 8 requirements and 4 of 5 stretch goals, each as separate commits.


## Code Structure

The solution contains the following projects:
  * Calc: Main console application
  * Contract: Library containing interface definitions and exceptions
  * Calculator: Library containing calculator implementations
  * Calculator.Tests: xUnit unit test suite

There are several ICalculator implementations in the Calculator library. I chose to keep several versions to demonstrate interface implementations and inheritance. 

Each requirement listed below identifies the relevant implementation and tests. 


## Instructions


To run unit tests:

```
dotnet build
dotnet test
```

To run the console application:

```
cd Calc
dotnet build
dotnet run
```


## Requirements

### 1. Support a maximum of 2 numbers using a comma delimiter. Throw an exception when more than 2 numbers are provided
  * examples: `20` will return `20`; `1,5000` will return `5001`; `4,-3` will return `1`
  * empty input or missing numbers should be converted to `0`
  * invalid numbers should be converted to `0` e.g. `5,tytyt` will return `5`

##### Implementation Notes: 
  * Interface: Contract/ICalculator.cs
  * Exceptions: Contract/Exceptions.cs
  * Implementation: Calculator/SumTwoOperandsCalculator.cs
  * Tests: Calculator.Tests/SumTwoOperandsTests.cs
  * Console App: Calc/Program.cs

##### Additional notes: 
  * Interface includes subtraction, multiplication, and division. However, those operators are not implemented until later stretch goals.
  * Various unit tests demonstrate calculations and TooManyOperandsCalculatorException

### 2. Remove the maximum constraint for numbers e.g. `1,2,3,4,5,6,7,8,9,10,11,12` will return `78`

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Unchanged
  * Implementation: Calculator/SumCalculator.cs added as base implementation, and modified SumTwoOperandsCalculator.cs to maintain old behavior
  * Tests: Calculator.Tests/SumTests.cs added for new tests
  * Console App: Calc/Program.cs updated in a second commit

### 3. Support a newline character as an alternative delimiter e.g. `1\n2,3` will return `6` 

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Unchanged
  * Implementation: Calculator/SumCalculator.cs now includes the new delimiter. Old implementation (SumTwoOperandsTests.cs) overrides to maintain its behavior.
  * Tests: Calculator.Tests/SumTests.cs includes additional unit tests
  * Console App: Unchanged

### 4. Deny negative numbers by throwing an exception that includes all of the negative numbers provided

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Added "NegativeValuesCalculatorException" class
  * Implementation: Calculator/SumCalculator.cs now checks for negative values, and throws exception
  * Tests: Calculator.Tests/SumTests.cs includes additional unit tests
  * Calc/Program.cs updated to handle new exception

### 5. Make any value greater than 1000 an invalid number e.g. `2,1001,6` will return `8`

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Unchanged
  * Implementation: Calculator/SumCalculator.cs now treats values > 1000 as 0. SumTwoOperandsTests.cs overrides this to maintain its old behavior.
  * Tests: Calculator.Tests/SumTests.cs includes additional unit tests
  * Console App: Unchanged

### 6. Support 1 custom delimiter of a single character using the format: `//{delimiter}\n{numbers}`
  * examples: `//#\n2#5` will return `7`; `//,\n2,ff,100` will return `102` 
  * all previous formats should also be supported

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Added "InvalidExpressionCalculatorException" class
  * Implementation: Calculator/SumCalculator.cs now uses RegEx to test for possible custom delimiters.
  * Tests: Calculator.Tests/SumTests.cs includes additional unit tests
  * Calc/Program.cs updated to handle exception

### 7. Support 1 custom delimiter of any length using the format: `//[{delimiter}]\n{numbers}`
  * example: `//[***]\n11***22***33` will return `66`
  * all previous formats should also be supported

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Unchanged
  * Implementation: Calculator/SumCalculator.cs additional RegEx for new delimiter pattern
  * Tests: Calculator.Tests/SumTests.cs includes additional unit tests
  * Calc/Program.cs Unchanged

### 8. Support multiple delimiters of any length using the format: `//[{delimiter1}][{delimiter2}]...\n{numbers}`
  * example: `//[*][!!][r9r]\n11r9r22*hh*33!!44` will return `110`
  * all previous formats should also be supported

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Unchanged
  * Implementation: Calculator/SumCalculator.cs updated RegEx to allow multiple custom delimiters
  * Tests: Calculator.Tests/SumTests.cs includes additional unit tests
  * Calc/Program.cs Unchanged


## Stretch goals

### 1. Display the formula used to calculate the result e.g. `2,,4,rrrr,1001,6` will return `2+0+4+0+0+6 = 12`

##### Implementation Notes: 
  * Interface: Added ICalculator2.cs as new interface to support formula without breaking old implementations
  * Exceptions: Unchanged
  * Implementation: Calculator/SumCalculator.cs now implements ICalculator2 and returns Tuple with the result and formula
  * Tests: Calculator.Tests/SumTests.cs includes additional unit tests
  * Calc/Program.cs Unchanged

### 2. Allow the application to process entered entries until Ctrl+C is used

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Unchanged
  * Implementation: Unchanged
  * Tests: Unchanged
  * Calc/Program.cs now adds event handler for ConsoleCancelEventHandler, and executes calculator in while(true) loop

### 3. Allow the acceptance of arguments to define...
  * alternate delimiter in step #3 
  * toggle whether to deny negative numbers in step #4
  * upper bound in step #5

This stretch goal is not implemented. There are various libraries for parsing arguments, so I didn't feel a need to do it here.

### 4. Use DI

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Unchanged
  * Implementation: Unchanged
  * Tests: Unchanged
  * Calc/Program.cs creates ServiceCollection and adds SumCalculator as the implementation to use for the ICalculator2 interface. CalcService now contains the main body of code, and ICalculator2 is injected 


### 5. Support subtraction, multiplication, and division operations

##### Implementation Notes: 
  * Interface: Unchanged
  * Exceptions: Unchanged
  * Implementation: Calculator/Calculator.cs added as a more generic ICalculator2 implementation supporting Add, Subtract, Multiply, Divide. SumCalculator now inherits Calculator, while imposing restriction to use Add operator only.
  * Tests: Calculator.Tests/CalculatorTests.cs added for testing new operators
  * Calc/Program.cs now adds Calculator (instead of SumCalculator) as the ICalculator2 implementation to use. CalcService.cs now prompts user to enter an operator after entering the expression to calculate.



