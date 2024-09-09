using Microsoft.Extensions.DependencyInjection;
using Contract;
using Calculator;

var services = new ServiceCollection();
services.AddSingleton<ICalculator2>(
    implementationFactory: static _ => new Calculator.Calculator()
);

services.AddSingleton<CalcService>();

var serviceProvider = services.BuildServiceProvider();

var calcService = serviceProvider.GetRequiredService<CalcService>();

calcService.Run();

