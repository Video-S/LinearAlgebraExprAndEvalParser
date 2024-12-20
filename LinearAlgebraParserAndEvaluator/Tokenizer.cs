namespace LinearAlgebraParserAndEvaluator;

using System;
using System.Data;
using System.Linq;
using static LangConfig;

#nullable enable

/// <summary>
/// Lexer, breaks up input into Numbers, Vec2s or Variables.
/// </summary>
internal class Tokenizer
{
    private string _data;
    private int _pos;
    private char _char
    {
        get
        {
            if (!AtEnd())
            {
                return _data[_pos];
            }
            else
            {
                string errorMessage = "Reached unexpected end of input";
                string syntaxError = ErrorHandling.CreateSyntaxError(_data, _data[_data.Length - 1], errorMessage);
                throw new SyntaxErrorException(syntaxError);
            }
        }
    }

    private bool _letter => Characters.Contains(_char);
    private bool _number => Digits.Contains(_char);
    private bool _operator => Operators.Contains(_char);
    private bool _negativeSign => _char == Digits.NegativeSign;
    private bool _decimalSign => _char == Digits.DecimalSign;

    public Tokenizer()
    {
        _data = String.Empty;
        _pos = 0;
    }

    /// <summary>
    /// Feed an expression to the tokenizer.
    /// </summary>
    /// <param name="expression">The expression to lex.</param>
    public void Feed(string expression)
    {
        _data = new string(expression.Where(ch => !char.IsWhiteSpace(ch)).ToArray());
        _pos = 0;
    }

    /// <summary>
    /// Walks through the input from the current position to try parse a <see cref="Number"/>
    /// </summary>
    /// <returns><see cref="Number"/> or null.</returns>
    /// <exception cref="SyntaxErrorException"></exception>
    /// <exception cref="FormatException"></exception>
    public NumberExpression? Number()
    {
        if (!AtEnd() && (_number || _negativeSign))
        {
            bool hasDecimal = false;
            int start = _pos;

            if (_negativeSign)
            {
                Step(); // TryParse can parse negative numbers
            }

            if (_char == '0' && Digits.Contains(Peek() ?? 'a')) // end of like after peek is great syntax
            {
                string errorMessage = "A Number cannot have leading 0's.";
                string syntaxError = ErrorHandling.CreateSyntaxError(_data, _char, errorMessage);
                throw new SyntaxErrorException(syntaxError);
            }

            while (!AtEnd() && (_number || _decimalSign))
            {
                if (_decimalSign)
                {
                    if (!Digits.Contains(Peek() ?? 'a')) // null peek is bad syntax, so is 'a'
                    {
                        string errorMessage = "Expected 0 to 9 [0-9] after decimal.";
                        string syntaxError = ErrorHandling.CreateSyntaxError(_data, _char, errorMessage);
                        throw new SyntaxErrorException(syntaxError);
                    }
                    if (!hasDecimal)
                    {
                        hasDecimal = true;
                    }
                    else
                    {
                        string errorMessage = "Number cannot contain a second decimal.";
                        string syntaxError = ErrorHandling.CreateSyntaxError(_data, _char, errorMessage);
                        throw new SyntaxErrorException(syntaxError);
                    }
                }
                Step();
            }

            string numberAsString = _data[start.._pos];
            if (float.TryParse(numberAsString, out float parsedFloat))
            {
                Number number = new(parsedFloat);
                Value numberInValue = new(number);
                return new NumberExpression(numberInValue);
            }
            else
            {
                throw new FormatException("Invalid number format.");
            }
        }
        else return null;
    }

    /// <summary>
    /// Walks from the current position to try parse a <see cref="Vec2"/>
    /// </summary>
    /// <returns><see cref="Vec2"/> or null.</returns>
    /// <exception cref="SyntaxErrorException"></exception>
    public Vec2Expression? Vec2()
    {
        if (AtEnd() || _char != '[')
        {
            return null;
        }
        else
        {
            Step();
        }

        Expression? x = null;
        if (_number || _negativeSign)
        {
            x = Number();
        }
        else if (_letter)
        {
            x = Variable();
        }

        if (x == null)
        {
            string errorMessage = "Expected a Number or Variable X in declaration of Vec2.";
            string syntaxError = ErrorHandling.CreateSyntaxError(_data, _char, errorMessage);
            throw new SyntaxErrorException(syntaxError);
        }

        if (_char == ',')
        {
            Step();
        }
        else
        {
            string errorMessage = "Expected a separator ',' and two Numbers in declaration of Vec2.";
            string syntaxError = ErrorHandling.CreateSyntaxError(_data, _char, ',', errorMessage);
            throw new SyntaxErrorException(syntaxError);
        }

        Expression? y = null;
        if (_number || _negativeSign)
        {
            y = Number();
        }
        else if (_letter)
        {
            y = Variable();
        }

        if (y == null)
        {
            string errorMessage = "Expected a Number or Variable Y in declaration of Vec2.";
            string syntaxError = ErrorHandling.CreateSyntaxError(_data, _char, errorMessage);
            throw new SyntaxErrorException(syntaxError);
        }

        if (_char == ']')
        {
            Step();
        }
        else
        {
            string errorMessage = "Expected a closure ']' in declaration of Vec2.";
            string syntaxError = ErrorHandling.CreateSyntaxError(_data, _char, ']', errorMessage);
            throw new SyntaxErrorException(syntaxError);
        }


        if (x.Evaluate().Type != ValueType.Number)
        {
            string errorMessage = "X is not of type number.";
            string syntaxError = ErrorHandling.CreateSyntaxError(_data, errorMessage);
            throw new SyntaxErrorException(syntaxError);
        }
        else if (y.Evaluate().Type != ValueType.Number)
        {
            string errorMessage = "Y is not of type number.";
            string syntaxError = ErrorHandling.CreateSyntaxError(_data, errorMessage);
            throw new SyntaxErrorException(syntaxError);
        }

        float xValue = x.Evaluate().NumberValue.Value;
        float yValue = y.Evaluate().NumberValue.Value;
        Vec2 vec2 = new(xValue, yValue);
        Value vec2inValue = new(vec2);
        return new Vec2Expression(vec2inValue);
    }

    /// <summary>
    /// Walks the input from the current position to try parse a <see cref="VariableExpression"/>
    /// </summary>
    /// <returns><see cref="VariableExpression"/> or null.</returns>
    /// <exception cref="SyntaxErrorException"></exception>
    public VariableExpression? Variable()
    {
        if(AtEnd() || !_letter)
        {
            return null;
        }

        int start = _pos;

        while (!AtEnd() && _letter)
        {
            Step();  // !intuitive: ends a char after last letter; thus peeks.
        }

        char groupClosure = Characters.GetBracket(Characters.BracketType.Group).Close;
        char vecClosure = Characters.GetBracket(Characters.BracketType.Vec2).Close;
        if (!AtEnd() && !_operator && !(_char == groupClosure) && !(_char == vecClosure) && !(_char == ','))
        {
            string errorMessage = "Unexpected end of variable. A variable can only contain lowercase characters a to z [a-z].";
            string syntaxError = ErrorHandling.CreateSyntaxError(_data, _char, errorMessage);
            throw new SyntaxErrorException(syntaxError);
        }

        string var = _data[start.._pos];
        return new VariableExpression(var);
    }

    /// <summary>
    /// Checks if the character is the expected character, and then consumes it.
    /// </summary>
    /// <param name="expected">The expected character.</param>
    /// <returns>True if expected, false otherwise.</returns>
    public bool Character(char expected)
    {
        if (AtEnd())
        {
            return false;
        }
        if (_char == expected)
        {
            Step();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Step to the next character, consuming the current.
    /// </summary>
    private void Step()
    {
        if (!AtEnd())
        {
            _pos++;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(_pos), "Input reached unexpected end.");
        }
    }

    /// <summary>
    /// Peeks the next character without consumption.
    /// </summary>
    /// <returns>Character or null.</returns>
    private char? Peek()
    {
        int peekPosition = _pos + 1;
        if (peekPosition >= 0 && peekPosition < _data.Length)
        {
            return _data[peekPosition];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Get the current position of the Tokenizer. The mark can later be used in Reset. Use before parsing in case of failure.
    /// </summary>
    /// <returns>int</returns>
    public int Mark() => _pos;

    /// <summary>
    /// Has the tokenizer reached the end of the input?
    /// </summary>
    /// <returns>True if at end; otherwise false.</returns>
    public bool AtEnd() => _pos >= _data.Length;

    /// <summary>
    /// Resets the tokenizer to a given mark.
    /// </summary>
    /// <param name="mark">The position to reset to.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Reset(int mark)
    {
        if (mark >= 0 && mark <= _data.Length)
        {
            _pos = mark;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(mark), "Mark position is out of range.");
        }
    }
    /// <summary>
    /// Current line of tokenizer.
    /// </summary>
    public string Data => _data;

    /// <summary>
    /// Current character of <see cref="Data"/>. Useful for error messaging.
    /// </summary>
    public char CurrentCharacter => _char;
}