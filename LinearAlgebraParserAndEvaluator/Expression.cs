/// <summary>
/// Abstract base class for expressions. Allows for storage of variables.
/// </summary>
public abstract class Expression
{
    protected static Dictionary<string, Value> _vars = new();

    public Expression()
    {

    }

    /// <summary>
    /// Evaluates the <see cref="Value"/>.
    /// </summary>
    /// <returns>Resulting <see cref="Value"/> of the evaluation.</returns> 
    public abstract Value Evaluate();

    /// <summary>
    /// Looks up a given variable name.
    /// </summary>
    /// <param name="var">Name of the variable.</param>
    /// <returns><see cref="Value"/> stored within the variable.</returns>
    /// <exception cref="InvalidOperationException">Variable is not defined.</exception> 
    public static Value LookUp(string var)
    {
        if (_vars.TryGetValue(var, out Value value))
        {
            return value;
        }
        else
        {
           throw new InvalidOperationException($"Variable '{var}' is not defined."); 
        }
    }

    /// <summary>
    /// Stores a variable with a value.
    /// </summary>
    /// <param name="var">The variable name.</param>
    /// <param name="value">The value to store in the variable.</param>
    public static void Record(string var, Value value)
    {
        _vars[var] = value;
    }
}

/// <summary>
/// An <see cref="Expression"/> representing a Number.
/// </summary>
public class NumberExpression : Expression
{
    private Value _value;

    public NumberExpression(Value value)
    {
        _value = value;
    }

    public override Value Evaluate()
    {
        return _value;
    }
}

/// <summary>
/// An expression representing a Vec2.
/// </summary> 
public class Vec2Expression : Expression
{
    private Value _value;

    public Vec2Expression(Value value)
    {
        _value = value;
    }

    public override Value Evaluate()
    {
        return _value;
    }
}

/// <summary>
/// An expression representing a variable.
/// </summary>
public class VariableExpression : Expression
{
    public string Name { get; private set; }

    public VariableExpression(string variableName)
    {
        Name = variableName;
    }

    public override Value Evaluate()
    {
        return LookUp(Name);
    }
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
        Value lhsValue = _lhs.Evaluate();
        Value rhsValue = _rhs.Evaluate();
        ValueType lhsType = lhsValue.Type;
        ValueType rhsType = rhsValue.Type;

        if (lhsType == ValueType.Number && rhsType == ValueType.Number)
        {
            Number? lhs = lhsValue.NumberValue;
            Number? rhs = rhsValue.NumberValue;

            if (lhs != null && rhs != null)
            {
                Number? result = null;

                if (_op == '+') result = lhs + rhs;
                if (_op == '-') result = lhs - rhs;
                if (_op == '*') result = lhs * rhs;
                if (_op == '/') result = lhs / rhs;

                Value resultAsValue = new(result ?? throw new NullReferenceException());
                return resultAsValue;
            }
        }

        if (lhsType == ValueType.Vec2 && rhsType == ValueType.Vec2)
        {
            Vec2? lhs = lhsValue.Vec2Value;
            Vec2? rhs = rhsValue.Vec2Value;

            if (lhs != null && rhs != null)
            {
                Vec2? result = null;

                if (_op == '+') result = lhs + rhs;
                if (_op == '-') result = lhs - rhs;
                if (_op == '*') result = lhs * rhs;
                if (_op == '/') result = lhs / rhs;

                Value resultAsValue = new(result ?? throw new NullReferenceException());
                return resultAsValue;
            }
        }

        if (lhsType == ValueType.Vec2 && rhsType == ValueType.Number)
        {
            Vec2? lhs = lhsValue.Vec2Value;
            Number? rhs = rhsValue.NumberValue;

            if (lhs != null && rhs != null)
            {
                Vec2? result = null;

                if (_op == '+') result = lhs + rhs;
                if (_op == '-') result = lhs - rhs;
                if (_op == '*') result = lhs * rhs;
                if (_op == '/') result = lhs / rhs;

                Value resultAsValue = new(result ?? throw new NullReferenceException());
                return resultAsValue;
            }
        }

        // For now if all else fails, return fallback number
        Number terminal = new(-999f);
        return new Value(terminal);
    }
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

    public override string ToString() => _variable.Name;
}