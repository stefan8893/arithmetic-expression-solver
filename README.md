# Arithmetic Expression Solver

A small .NET command-line application that evaluates arithmetic expressions.

This project is intended as a learning exercise to understand how expression parsers work. The focus is on parsing and evaluating arithmetic expressions in a small, understandable implementation before applying the same concepts to more advanced scenarios such as parsing filter predicates and translating them into C# lambda expressions.

## Usage

Run the application with an expression as its argument:

```sh
dotnet run --project ArithmeticExpressionSolver -- "2 + 3 * (4 - 1)"
```

The result is printed to standard output. Invalid expressions and division by
zero are reported on standard error, and the application exits with a non-zero
status code.

## Supported expressions

The parser supports:

- Addition (`+`) and subtraction (`-`)
- Multiplication (`*`) and division (`/`)
- Unary plus and minus
- Parentheses
- Decimal numbers using a period (`.`), independent of the system locale
- Whitespace between tokens

For example:

```sh
dotnet run --project ArithmeticExpressionSolver -- "-2 + 3.5 * (4 - 1)"
```
