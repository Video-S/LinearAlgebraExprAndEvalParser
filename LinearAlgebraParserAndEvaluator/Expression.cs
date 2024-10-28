namespace LinearAlgebraParserAndEvaluator;

using System;
using System.Collections.Generic;
using System.Data;
using static LangConfig.Operators;

/// <summary>
/// Abstract base class for expressions. Allows for storage of variables.
/// </summary>
public abstract class Expression
{
    protected static Dictionary<string, Value> _vars = new Dictionary<string, Value>();

    /// <summary>
    /// Evaluates the <see cref="Value"/>.
    /// </summary>
    /// <returns><see cref="Value"/></returns>
    public abstract Value Evaluate();

    /// <summary>
    /// Looks up a given variable name.
    /// </summary>
    /// <param name="var">Name of the variable.</param>
    /// <returns><see cref="Value"/></returns>
    public static Value LookUp(string var)
    {
        if (_vars.TryGetValue(var, out Value value))
            return value;
        else
            throw new InvalidOperationException($"Variable '{var}' is not defined.");
    }

    /// <summary>
    /// Stores a variable with a value.
    /// </summary>
    /// <param name="var">The variable name.</param>
    /// <param name="value">The value to store in the variable.</param>
    public static void Record(string var, Value value) => _vars[var] = value;
    public static void ClearRecord() => _vars.Clear();
}

/// <summary>
/// An <see cref="Expression"/> representing a Number.
/// </summary>
public class NumberExpression : Expression
{
    private Value _value;
    public NumberExpression(Value value) => _value = value;
    public override Value Evaluate() => _value;
    public override string ToString() => _value.ToString();
}

/// <summary>
/// An expression representing a Vec2.
/// </summary>
public class Vec2Expression : Expression
{
    private Value _value;
    public Vec2Expression(Value value) => _value = value;
    public override Value Evaluate() => _value;
    public override string ToString() => _value.ToString();
}

/// <summary>
/// An expression representing a variable.
/// </summary>
public class VariableExpression : Expression
{
    public string Name { get; private set; }
    public VariableExpression(string variableName) => Name = variableName;
    public override Value Evaluate() => LookUp(Name);
    public override string ToString() => Name;
}

/// <summary>
/// An expression representing an arithmetic operation.
/// Consists of an operator, a left hand side expression, and a right hand side expression.
/// Evaluation returns the resulting expression of the operation.
/// </summary>
public class OperationExpression : Expression
{
    private char _op;
    private Expression _lhs;
    private Expression _rhs;

    public OperationExpression(char op, Expression lhs, Expression rhs)
    {
        _op = op;
        _lhs = lhs;
        _rhs = rhs;
    }

    public override Value Evaluate()
    {
        Value lhs = _lhs.Evaluate();
        Value rhs = _rhs.Evaluate();
        Value result;

        if      (_op == Addition        ) result = lhs + rhs;
        else if (_op == Subtraction     ) result = lhs - rhs;
        else if (_op == Multiplication  ) result = lhs * rhs;
        else if (_op == Division        ) result = lhs / rhs;
        else throw new EvaluateException("Expression could not be evaluated.");

        return result;
    }

    public override string ToString() => $"{_lhs} {_op} {_rhs}";
}

/// <summary>
/// An expression representing an assignment of a Value to a Variable.
/// </summary>
public class AssignmentExpression : Expression
{
    private VariableExpression _variable;
    private readonly Expression _value;

    public AssignmentExpression(VariableExpression var, Expression value)
    {
        _variable = var;
        _value = value;
    }

    public override Value Evaluate()
    {
        Value value = _variable.Evaluate();
        Record(_variable.Name, value);
        return value;
    }

    public override string ToString() => $"{_variable} = {_value}";
}