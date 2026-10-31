using System.CommandLine;
using ArithmeticExpressionSolver;

var expressionArgument = new Argument<string>("expression")
{
    Description = "The mathematical expression to evaluate."
};

var rootCommand = new RootCommand("Evaluate a mathematical expression.");
rootCommand.Arguments.Add(expressionArgument);
rootCommand.SetAction(parseResult =>
    MathApplication.Run(parseResult.GetValue(expressionArgument)!));

return rootCommand.Parse(args).Invoke();
