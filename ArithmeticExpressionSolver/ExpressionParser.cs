using System.Globalization;

namespace ArithmeticExpressionSolver;

public sealed class ExpressionParser
{
    private readonly string _expression;
    private int _position;

    private ExpressionParser(string expression)
    {
        _expression = expression;
    }

    private bool IsAtEnd => _position >= _expression.Length;

    private char CurrentCharacter => _expression[_position];

    public static double Evaluate(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var parser = new ExpressionParser(expression);
        var result = parser.ParseExpression();
        parser.SkipWhitespaces();

        return !parser.IsAtEnd
            ? throw parser.Error($"Unexpected character '{parser.CurrentCharacter}'.")
            : result;
    }

    private double ParseExpression()
    {
        var result = ParseTerm();

        while (true)
        {
            SkipWhitespaces();

            if (Match('+'))
                result += ParseTerm();
            else if (Match('-'))
                result -= ParseTerm();
            else
                return result;
        }
    }

    private double ParseTerm()
    {
        var result = ParseUnary();

        while (true)
        {
            SkipWhitespaces();

            if (Match('*'))
            {
                result *= ParseUnary();
            }
            else if (Match('/'))
            {
                var divisor = ParseUnary();
                if (divisor == 0)
                    throw new DivideByZeroException("Division by zero.");

                result /= divisor;
            }
            else
            {
                return result;
            }
        }
    }

    private double ParseUnary()
    {
        SkipWhitespaces();

        if (Match('+'))
            return ParseUnary();

        if (Match('-'))
            return -ParseUnary();

        return ParsePrimary();
    }

    private double ParsePrimary()
    {
        SkipWhitespaces();

        if (!Match('('))
            return ParseNumber();

        var result = ParseExpression();

        SkipWhitespaces();

        return Match(')')
            ? result
            : throw Error("Expected ')'.");
    }

    private double ParseNumber()
    {
        SkipWhitespaces();

        var start = _position;

        var hasIntegerPartDigits = ParseIntegerPart();
        var hasDecimalPlacesDigits = ParseDecimalPlaces();
        var hasDigits = hasIntegerPartDigits || hasDecimalPlacesDigits;

        if (!hasDigits)
            throw Error("Expected a number or parenthesized expression.");

        var numberText = _expression[start.._position];

        var successfullyParsed = double.TryParse(numberText,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out var number);

        return successfullyParsed
            ? number
            : throw Error($"Invalid number '{numberText}'.");

        bool ParseDecimalPlaces()
        {
            var hasDecimalPlaceDigits = false;

            if (IsAtEnd || CurrentCharacter != '.')
                return hasDecimalPlaceDigits;

            _position++;

            while (!IsAtEnd && char.IsDigit(CurrentCharacter))
            {
                hasDecimalPlaceDigits = true;
                _position++;
            }

            return hasDecimalPlaceDigits;
        }

        bool ParseIntegerPart()
        {
            var hasIntegerPartDigits = false;
            while (!IsAtEnd && char.IsDigit(CurrentCharacter))
            {
                _position++;
                hasIntegerPartDigits = true;
            }

            return hasIntegerPartDigits;
        }
    }

    private bool Match(char expected)
    {
        if (IsAtEnd || CurrentCharacter != expected)
            return false;

        _position++;
        return true;
    }

    private void SkipWhitespaces()
    {
        while (!IsAtEnd && char.IsWhiteSpace(CurrentCharacter))
            _position++;
    }

    private FormatException Error(string message)
    {
        return new FormatException($"{message} (at position {_position}).");
    }
}