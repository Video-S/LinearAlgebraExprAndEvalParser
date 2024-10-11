public class Tokenizer
{
    private string _data;
    private int _pos;
    private char _char => !AtEnd() ? _data[_pos] : '\0';
    private static char[] _letters = ['a','b','c','d','e','f','g','h','i','j','k','l','m',
                                      'n','o','p','q','r','s','t','u','v','w','x','y','z']; // [a-z]+
    private bool _charIsLetter => _letters.Contains(_char);
    private static char[] _numbers = ['0','1','2','3','4','5','6','7','8','9']; // "0" | [1-9] [0-9]* ( "." [0-9]+ )?
    private bool _charIsNumber => _numbers.Contains(_char);
    private static char[] _operators => ['+', '-', '*', '/', '='];
    private bool _charIsOperator => _operators.Contains(_char);
    public Tokenizer(string data)
    {
        _data = new string(data.Where(ch => !char.IsWhiteSpace(ch)).ToArray());     // whitespace is dead, and we have killed it.
        _pos = 0;                                                                   // what was mightiest of all the world possessed, 
    }                                                                               // has bled to death under our knives.

    public NumberExpression? Number()
    {
        if (AtEnd()) return null;

        if (_charIsNumber || _char == '-') // expect: [0-9] or '-' (+= [0-9]+)
        {
            // expect: no number after 0 (0[0..9]+)
            if (_char == '0' && _numbers.Where(ch => ch == Peek()).Count() > 0) return null; //FIXME: yeah ugh.

            int start = _pos; 

            if(_char == '-') _pos++; // skip to allow negative numbers (-[0-9])

            bool hasDecimal = false; 
            while (!AtEnd() && (_charIsNumber || _char == '.')) // expect: [0-9]+ or '.'
            {
                if(_char == '.') 
                {
                    if(!hasDecimal) hasDecimal = true; 
                    else return null; // expect: no 2nd decimal (14.2.4)
                }
                _pos++;
            }

            string nString = _data[start.._pos];
            if (float.TryParse(nString, out float f))  // if number string parses then yay.
            {                                          // '-' does not parse because of this.
                Number n = new(f);
                Value nAsValue = new(n);
                return new NumberExpression(nAsValue);
            }
        }
        return null;
    }

    public Vec2Expression? Vec2() // expect: "[ne1,ne2]"
    {
        if (AtEnd()) return null;
        
        if (_char == '[') _pos++; // expect: '['
        else return null;

        NumberExpression? x = null; // expect: ne1
        if (_charIsNumber || _char == '-') x = Number(); 
        if (x == null) return null;

        if (_char == ',') _pos++; // expect: ','
        else return null;

        NumberExpression? y = null; // expect: ne2
        if (_charIsNumber || _char == '-') y = Number(); 
        if (y == null) return null;

        if (_char == ']') _pos++; // expect: ']'
        else return null;

        float xValue = x.Evaluate().NumberValue?.Value ?? -999f;
        float yValue = y.Evaluate().NumberValue?.Value ?? -999f;

        Vec2 vec2 = new(xValue, yValue);
        Value vec2AsValue = new(vec2);
        return new Vec2Expression(vec2AsValue); // all should be 👍
    }

    public VariableExpression? Variable() // expect "[a-z] '='"
    {
        int start = _pos; 

        while (!AtEnd() && _charIsLetter)  // expect: "[a-z]+"
        {
            _pos++;         // !intuitive: ends a char after last letter; thus peeks.
        }

        if (!_charIsOperator && !AtEnd()) return null; // expect operator 

        string var = _data[start.._pos]; 
        return new VariableExpression(var);
    }

    public bool Character(char expected)
    {
        if (AtEnd()) return false;

        if (_char == expected)
        {
            _pos++;
            return true;
        }

        return false;
    }
    private char? Peek()
    {
        int peekPosition = _pos + 1;
        if (peekPosition >= 0 && peekPosition < _data.Length) return _data[peekPosition];
        else return null;
    }
    public int Mark() => _pos;
    public bool AtEnd() => _pos >= _data.Length;
    public void Reset(int mark)
    {
        if (mark >= 0 && mark <= _data.Length) _pos = mark;
        else throw new ArgumentOutOfRangeException(nameof(mark), "Mark position is out of range.");
    }
}