public abstract class Expression
{
    protected static Dictionary<string, Value> _vars = new();

    public Expression()
    {

    }

    public abstract Value Evaluate();

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

    public static void Record(string var, Value value)
    {
        _vars[var] = value;
    }
}

public class NumberExpression : Expression
{
    public Value _value;

    public NumberExpression(Value value)
    {
        _value = value;
    }

    public override Value Evaluate()
    {
        return _value;
    }
}

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