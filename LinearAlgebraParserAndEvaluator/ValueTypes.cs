/// <summary>
/// Contains recognized types: <see cref="Number"/>, <see cref="Vec2"/>.
/// </summary> 
public enum ValueType
{
    Number,
    Vec2,
}

/// <summary>
/// A wrapper that contains a value of either <see cref="Number"/> or <see cref="Vec2"/>.
/// <see cref="Value.Type"/> returns the value type contained in <see cref="Value"/>.
/// The value can then be retrieved with <see cref="NumberValue"/> or <see cref="Vec2Value"/>.
/// </summary>
public struct Value
{
    /// <summary>
    /// Type of the value contained in the wrapper.
    /// </summary> 
    /// <value><see cref="ValueType"/></value>
    public ValueType Type { get; }

    /// <summary>
    /// If <see cref="Type"/> is Number, returns Number; otherwise null.
    /// </summary>
    /// <value><see cref="Number"/> or <see cref="null"/></value>
    public readonly Number? NumberValue = null;

    /// <summary>
    /// If <see cref="Type"/> is Vec2, returns Vec2; otherwise null.
    /// </summary>
    /// <value><see cref="Vec2"/> or <see cref="null"/></value>
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

/// <summary>
/// Represents a number with decimals. Supports basic arithmetic with other numbers.
/// </summary> 
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
    /// <summary>
    /// A number value.
    /// </summary>
    /// <value><see cref="float"/></value>
    public float Value;
    public Number(float value)
    {
        Value = value;
    }
    public override readonly string ToString() => Value.ToString();
}

/// <summary>
/// Represents a vector value of two <see cref="Number"/> elements X and Y. Supports arithmetic functions with another <see cref="Vec2"/> or a <see cref="Number"/>.
/// </summary>
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
    /// <summary>
    /// X value.
    /// </summary>
    /// <value>float</value>
    public float X;

    /// <summary>
    /// Y value.
    /// </summary>
    /// <value>float</value>
    public float Y;
    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }
    public override readonly string ToString() => $"[{X}, {Y}]";
}