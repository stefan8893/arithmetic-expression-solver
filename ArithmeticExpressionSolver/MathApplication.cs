using System.Globalization;

namespace ArithmeticExpressionSolver;

public static class MathApplication
{
    public static int Run(string expression)
    {
        try
        {
            var result = ExpressionParser.Evaluate(expression);
            Console.WriteLine(result.ToString(CultureInfo.InvariantCulture));
            return 0;
        }
        catch (FormatException exception)
        {
            Console.Error.WriteLine($"Invalid expression: {exception.Message}");
            return 1;
        }
        catch (DivideByZeroException exception)
        {
            Console.Error.WriteLine($"Cannot evaluate expression: {exception.Message}");
            return 1;
        }
    }
}
