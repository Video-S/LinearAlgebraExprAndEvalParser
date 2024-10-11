public class Parser
{
    private Tokenizer _t;

    public Parser(string line)
    {
        _t = new Tokenizer(line);
    }

    public Expression? Statement()
    {
        Expression? exp = Assignment() ?? Sum();
        return exp;
    }

    public Expression? Assignment()
    {
        int mark = _t.Mark(); // if parsing fails, we can reset the tokenizer
        VariableExpression? lhs = null;
        Expression? rhs = null;

        lhs = _t.Variable();
        if (lhs != null && _t.Character('='))
        {
            rhs = Sum();
            if (rhs != null && _t.AtEnd())
            {
                VariableExpression.Record(lhs.Name, rhs.Evaluate());
                return new AssignmentExpression(lhs, rhs); // success
            }
        }
        _t.Reset(mark); // failed: now reset
        return null;
    }

    public Expression? Sum()
    {
        int mark = _t.Mark();
        Expression? lhs = Product();

        while (lhs != null)
        {
            if (_t.Character('+'))
            {
                Expression? rhs = Product();
                if (rhs != null)
                {
                    lhs = new OperationExpression('+', lhs, rhs);
                }
                else
                {
                    _t.Reset(mark);
                    return null;
                }
            }
            else if (_t.Character('-'))
            {
                Expression? rhs = Product();
                if(rhs != null)
                {
                    lhs = new OperationExpression('-', lhs, rhs);
                }
                else
                {
                    _t.Reset(mark);
                    return null;
                }
            }
            else 
            {
                break;
            }
        }

        return lhs;
    }

    public Expression? Product()
    {
        int mark = _t.Mark();
        Expression? lhs = Term();
        
        while (lhs != null)
        {
            if (_t.Character('*'))
            {
                Expression? rhs = Term();
                if (rhs != null)
                {
                    lhs = new OperationExpression('*', lhs, rhs);
                }
                else
                {
                    _t.Reset(mark);
                    return null;
                }
            }
            if (_t.Character('/'))
            {
                Expression? rhs = Term();
                if (rhs != null)
                {
                    lhs = new OperationExpression('/', lhs, rhs);
                }
                else
                {
                    _t.Reset(mark);
                    return null;
                }
            }
            else
            {
                break;
            }
        }

        return lhs;
    }

    public Expression? Term()
    {
        Expression? exp = _t.Number() ?? _t.Vec2() ?? _t.Variable() ?? Group();
        return exp;
    }

    public Expression? Group()
    {
        int mark = _t.Mark();
        Expression? exp = null;

        if (_t.Character('('))
        {
            exp = Sum();
            if (exp != null && _t.Character(')'))
            {
                return exp;
            }
        }

        _t.Reset(mark);
        return null;
    }
}