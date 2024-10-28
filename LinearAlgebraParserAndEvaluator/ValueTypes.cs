namespace LinearAlgebraParserAndEvaluator;

using System;

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
    private readonly Number _numberValue;
    private readonly Vec2 _vec2Value;
    private readonly ValueType _type;
    /// <summary>The type of the contained Value.</summary>
    public ValueType Type => _type;

    /// <summary>
    /// Check <see cref="Type"/> first to see if the value contained is a Number.
    /// </summary>
    /// <value><see cref="Number"/></value>
    public Number NumberValue
    {
        get
        {
            if (_type != ValueType.Number)
                throw new InvalidOperationException("Value is not of a type Number.");
            return _numberValue;
        }
    }

    /// <summary>
    /// Check <see cref="Type"/> first to see if the value contained is a Vec2.
    /// </summary>
    /// <value><see cref="Vec2"/></value>
    public Vec2 Vec2Value
    {
        get
        {
            if (_type != ValueType.Vec2)
                throw new InvalidOperationException("Value is not of a type Vec2.");
            return _vec2Value;
        }
    }

    /// <summary>
    /// Create a new Value of the type Number.
    /// </summary>
    /// <param name="value">The Number to wrap as a Value.</param>
    public Value(Number value)
    {
        _type = ValueType.Number;
        _numberValue = value;
        _vec2Value = default;
    }

    /// <summary>
    /// Create a new Value of the type Vec2.
    /// </summary>
    /// <param name="value">The Vec2 to wrap as a Value.</param>
    public Value(Vec2 value)
    {
        _type = ValueType.Vec2;
        _numberValue = default;
        _vec2Value = value;
    }

    public override readonly string ToString()
    {
        if (_type == ValueType.Number)
            return _numberValue.ToString();

        else if (_type == ValueType.Vec2)
            return _vec2Value.ToString();

        else
            throw new InvalidOperationException("Value is empty.");
    }

    public static Value operator +(Value lhs, Value rhs)
    {
        if (lhs.Type == ValueType.Vec2 && rhs.Type == ValueType.Vec2)
            return new(lhs.Vec2Value + rhs.Vec2Value);

        if (lhs.Type == ValueType.Vec2 && rhs.Type == ValueType.Number)
            return new(lhs.Vec2Value + rhs.NumberValue);

        if (lhs.Type == ValueType.Number && rhs.Type == ValueType.Number)
            return new(lhs.NumberValue + rhs.NumberValue);

        if (lhs.Type == ValueType.Number && rhs.Type == ValueType.Vec2)
            return new(rhs.Vec2Value + lhs.NumberValue);

        else throw new InvalidOperationException($"Addition is not available for type {lhs.Type} and {rhs.Type}");
    }

    public static Value operator -(Value lhs, Value rhs)
    {
        if (lhs.Type == ValueType.Vec2 && rhs.Type == ValueType.Vec2)
            return new(lhs.Vec2Value - rhs.Vec2Value);

        if (lhs.Type == ValueType.Vec2 && rhs.Type == ValueType.Number)
            return new(lhs.Vec2Value - rhs.NumberValue);

        if (lhs.Type == ValueType.Number && rhs.Type == ValueType.Number)
            return new(lhs.NumberValue - rhs.NumberValue);

        if (lhs.Type == ValueType.Number && rhs.Type == ValueType.Vec2)
            return new(rhs.Vec2Value - lhs.NumberValue);

        else throw new InvalidOperationException($"Subtraction is not available for type {lhs.Type} and {rhs.Type}");
    }

    public static Value operator *(Value lhs, Value rhs)
    {
        if (lhs.Type == ValueType.Vec2 && rhs.Type == ValueType.Vec2)
            return new(lhs.Vec2Value * rhs.Vec2Value);

        if (lhs.Type == ValueType.Vec2 && rhs.Type == ValueType.Number)
            return new(lhs.Vec2Value * rhs.NumberValue);

        if (lhs.Type == ValueType.Number && rhs.Type == ValueType.Number)
            return new(lhs.NumberValue * rhs.NumberValue);

        if (lhs.Type == ValueType.Number && rhs.Type == ValueType.Vec2)
            return new(rhs.Vec2Value * lhs.NumberValue);

        else throw new InvalidOperationException($"Multiplication is not available for type {lhs.Type} and {rhs.Type}");
    }

    public static Value operator /(Value lhs, Value rhs)
    {
        if (lhs.Type == ValueType.Vec2 && rhs.Type == ValueType.Vec2)
            return new(lhs.Vec2Value / rhs.Vec2Value);

        if (lhs.Type == ValueType.Vec2 && rhs.Type == ValueType.Number)
            return new(lhs.Vec2Value / rhs.NumberValue);

        if (lhs.Type == ValueType.Number && rhs.Type == ValueType.Number)
            return new(lhs.NumberValue / rhs.NumberValue);

        if (lhs.Type == ValueType.Number && rhs.Type == ValueType.Vec2)
            return new(rhs.Vec2Value / lhs.NumberValue);

        else throw new InvalidOperationException($"Division is not available for type {lhs.Type} and {rhs.Type}");
    }
}

/// <summary>
/// Represents a number with decimals. Supports basic arithmetic with other numbers.
/// </summary>
public struct Number
{
    public float Value;
    public Number(float value)
    {
        Value = value;
    }

    public static Number operator +(Number n1, Number n2) => new(n1.Value + n2.Value);
    public static Number operator -(Number n1, Number n2) => new(n1.Value - n2.Value);
    public static Number operator *(Number n1, Number n2) => new(n1.Value * n2.Value);
    public static Number operator /(Number n1, Number n2)
    {
        if (n2.Value == 0)
            throw new DivideByZeroException("Cannot divide by zero.");
        else
            return new(n1.Value / n2.Value);
    }
    public override readonly string ToString() => Value.ToString();
}

/// <summary>
/// Represents a vector value of two <see cref="Number"/> elements X and Y.
/// Supports arithmetic functions with another <see cref="Vec2"/> or a <see cref="Number"/>.
/// </summary>
public struct Vec2
{
    public float X;
    public float Y;
    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Vec2 operator +(Vec2 v1, Vec2 v2) => new(v1.X + v2.X     ,  v1.Y + v2.Y   );
    public static Vec2 operator +(Vec2 v, Number n) => new(v.X  + n.Value  ,  v.Y  + n.Value);
    public static Vec2 operator -(Vec2 v1, Vec2 v2) => new(v1.X - v2.X     ,  v1.Y - v2.Y   );
    public static Vec2 operator -(Vec2 v, Number n) => new(v.X  - n.Value  ,  v.Y  - n.Value);
    public static Vec2 operator *(Vec2 v1, Vec2 v2) => new(v1.X * v2.X     ,  v1.Y * v2.Y   );
    public static Vec2 operator *(Vec2 v, Number n) => new(v.X  * n.Value  ,  v.Y  * n.Value);
    public static Vec2 operator /(Vec2 v1, Vec2 v2)
    {
        if (v2.X == 0 || v2.Y == 0)
            throw new DivideByZeroException("Cannot divide by zero.");
        else
            return new(v1.X / v2.X, v1.Y / v2.Y);
    }
    public static Vec2 operator /(Vec2 v, Number n)
    {
        if (n.Value == 0)
            throw new DivideByZeroException("Cannot divide by zero.");
        else
            return new(v.X / n.Value, v.Y / n.Value);
    }
    public override readonly string ToString() => $"[{X}, {Y}]";
}