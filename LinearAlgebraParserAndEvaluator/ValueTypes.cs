public enum ValueType
{
    Number,
    Scalar,
    Vec2,
}

public struct Value
{
    public ValueType Type { get; }
    public readonly Number? NumberValue = null;
    public readonly Vec2? Vec2Value = null;

    public Value(Number value)
    {
        Type = ValueType.Number;
        NumberValue = value;
    }

    public Value(Vec2 value)
    {
        Type = ValueType.Vec2;
        Vec2Value = value;
    }

    public override readonly string ToString()
    {
        if (Type == ValueType.Number) return NumberValue.ToString() ?? "Error";
        if (Type == ValueType.Vec2) return Vec2Value.ToString() ?? "Error";
        else return base.ToString() ?? "Error";
    }
}

public struct Number
{
    public static Number operator +(Number n1, Number n2) => new(n1.Value + n2.Value);
    public static Number operator -(Number n1, Number n2) => new(n1.Value - n2.Value);
    public static Number operator *(Number n1, Number n2) => new(n1.Value * n2.Value);
    public static Number operator /(Number n1, Number n2)
    {
        if (n2.Value == 0) throw new DivideByZeroException("Cannot divide by zero.");
        else return new(n1.Value / n2.Value);
    }
    public float Value;
    public Number(float value)
    {
        Value = value;
    }
    public override readonly string ToString() => Value.ToString();
}

public struct Vec2
{
    public static Vec2 operator +(Vec2 v1, Vec2 v2) => new(v1.X + v2.X, v1.Y + v2.Y);
    public static Vec2 operator +(Vec2 v, Number n) => new(v.X + n.Value, v.Y + n.Value);
    public static Vec2 operator -(Vec2 v1, Vec2 v2) => new(v1.X - v2.X, v1.Y - v2.Y);
    public static Vec2 operator -(Vec2 v, Number n) => new(v.X - n.Value, v.Y - n.Value);
    public static Vec2 operator *(Vec2 v1, Vec2 v2) => new(v1.X * v2.X, v1.Y * v2.Y);
    public static Vec2 operator *(Vec2 v, Number n) => new(v.X * n.Value, v.Y * n.Value);
    public static Vec2 operator /(Vec2 v1, Vec2 v2) 
    {
        if (v2.X == 0 || v2.Y == 0) throw new DivideByZeroException("Cannot divide by zero.");
        else return new(v1.X / v2.X, v1.Y / v2.Y);
    }
    public static Vec2 operator /(Vec2 v, Number n) 
    {
        if (n.Value == 0) throw new DivideByZeroException("Cannot divide by zero.");
        else return new(v.X / n.Value, v.Y / n.Value);
    }
    public float X;
    public float Y;
    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }
    public override readonly string ToString() => $"[{X}, {Y}]";
}