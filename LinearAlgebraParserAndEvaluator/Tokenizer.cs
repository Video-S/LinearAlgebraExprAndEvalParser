/// <summary>
/// Lexer, breaks up input into Numbers, Vec2s or Variables.
/// </summary>
public class Tokenizer
{
    private string _data;
    private int _pos;
    private char _char => !AtEnd() ? _data[_pos] : '\0';
    private bool _letter => LangConfig.Characters.Contains(_char);
    private bool _number => LangConfig.Digits.Contains(_char);
    private bool _negativeSign => _char == LangConfig.Digits.NegativeSign;
    private bool _decimalSign => _char == LangConfig.Digits.DecimalSign;
    private bool _operator => LangConfig.Operators.Contains(_char);
    public Tokenizer(string data)
    {
        _data = new string(data.Where(ch => !char.IsWhiteSpace(ch)).ToArray());     // whitespaceisdead,andwehavekilledit.
        _pos = 0;                                                                   // whatwasmightiestofalltheworldpossessed, 
    }                                                                               // hasbledtodeathunderourknives.

    /// <summary>
    /// Tries to tokenize the current position as a <see cref="Number"/>
    /// </summary>
    /// <returns><see cref="Number"/> or null.</returns>
    public NumberExpression? Number()
    {
        if (AtEnd()) return null;

        if (_number || _negativeSign)
        {
            bool hasDecimal = false; 
            int start = _pos; 

            if (_char == '0' && Peek() == '0') return null; // check leading 0's
            if (_negativeSign) Step(); // TryParse can parse negative numbers

            while (!AtEnd() && (_number || _decimalSign))
            {
                if(_decimalSign) 
                {
                    if(!hasDecimal) hasDecimal = true; 
                    else return null; // 2nd decimal very illegal.
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
        }
        return null;
    }

    /// <summary>
    /// Tries to tokenize the current position as a <see cref="Vec2"/>
    /// </summary>
    /// <returns><see cref="Vec2"/> or null.</returns>
    public Vec2Expression? Vec2() 
    {
        if (AtEnd()) return null;
        
        if (_char == '[') Step(); 
        else return null;

        NumberExpression? x = null; 
        if (_number || _negativeSign) x = Number(); 
        if (x == null) return null;

        if (_char == ',') Step(); 
        else return null;

        NumberExpression? y = null;
        if (_number || _negativeSign) y = Number(); 
        if (y == null) return null;

        if (_char == ']') Step();
        else return null;

        float xValue = x.Evaluate().NumberValue?.Value ?? -999f;
        float yValue = y.Evaluate().NumberValue?.Value ?? -999f;

        Vec2 vec2 = new(xValue, yValue);
        Value vec2inValue = new(vec2);
        return new Vec2Expression(vec2inValue);
    }

    /// <summary>
    /// Tries to tokenize the current position as a variable.
    /// </summary>
    /// <returns><see cref="VariableExpression"/> or null.</returns>
    public VariableExpression? Variable()
    {
        int start = _pos; 

        while (!AtEnd() && _letter)
        {
            Step();  // !intuitive: ends a char after last letter; thus peeks.
        }   

        if (!_operator && !AtEnd()) return null; 

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
        if (AtEnd()) return false;
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
    private void Step() => _pos++;

    /// <summary>
    /// Peeks the next character without consumption. 
    /// </summary>
    /// <returns>Character or null.</returns>
    private char? Peek()
    {
        int peekPosition = _pos + 1;
        if (peekPosition >= 0 && peekPosition < _data.Length) return _data[peekPosition];
        else return null;
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
        if (mark >= 0 && mark <= _data.Length) _pos = mark;
        else throw new ArgumentOutOfRangeException(nameof(mark), "Mark position is out of range.");
    }
}