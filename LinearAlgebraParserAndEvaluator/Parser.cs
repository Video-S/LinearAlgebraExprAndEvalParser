/// <summary>
/// Parses syntax.
/// </summary>
public class Parser
{
    private Tokenizer _t;
    private char _assignOp = LangConfig.Operators.Assignment;
    private char _additionOp = LangConfig.Operators.Addition;
    private char _subtractionOp = LangConfig.Operators.Subtraction;
    private char _multiplicationOp = LangConfig.Operators.Multiplication;
    private char _divisionOp = LangConfig.Operators.Division;


    public Parser(string line)
    {
        _t = new Tokenizer(line);
    }

    /// <summary>
    /// Parses the input to be a statement, meaning it is passed to <see cref="Assignment"/>, or to <see cref="Sum"/>.
    /// </summary>
    /// <returns><see cref="OperationExpression"/>, <see cref="AssigmentExpression"/> or null.</returns>
    public Expression? Statement()
    {
        Expression? exp = Assignment() ?? Sum();
        return exp;
    }

    /// <summary>
    /// Parses input to be an assignment, meaning that the right hand side is passed to <see cref="Tokenizer.Variable"/> and the left hand side to <see cref="Sum"/>
    /// </summary>
    /// <returns>An <see cref="AssignmentExpression"/> or null.</returns>
    public Expression? Assignment()
    {
        int mark = _t.Mark(); // if parsing fails, we can reset the tokenizer
        VariableExpression? lhs = null;
        Expression? rhs = null;

        lhs = _t.Variable();
        if (lhs != null && _t.Character(_assignOp))
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

    /// <summary>
    /// Parses the input to be a sum. Passes left and right hand side to <see cref="Product"/>
    /// </summary>
    /// <returns>An <see cref="OperationExpression"/> or null.</returns> 
    public Expression? Sum()
    {
        int mark = _t.Mark();
        Expression? lhs = Product();

        while (lhs != null)
        {
            if (_t.Character(_additionOp))
            {
                Expression? rhs = Product();
                if (rhs != null)
                {
                    lhs = new OperationExpression(_additionOp, lhs, rhs);
                }
                else
                {
                    _t.Reset(mark);
                    return null;
                }
            }
            else if (_t.Character(_subtractionOp))
            {
                Expression? rhs = Product();
                if(rhs != null)
                {
                    lhs = new OperationExpression(_subtractionOp, lhs, rhs);
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

    /// <summary>
    /// Parses the input to be a product. Passes left and right hand side to <see cref="Term"/>
    /// </summary>
    /// <returns><see cref="OperationExpression"/> or null.</returns>
    public Expression? Product()
    {
        int mark = _t.Mark();
        Expression? lhs = Term();
        
        while (lhs != null)
        {
            if (_t.Character(_multiplicationOp))
            {
                Expression? rhs = Term();
                if (rhs != null)
                {
                    lhs = new OperationExpression(_multiplicationOp, lhs, rhs);
                }
                else
                {
                    _t.Reset(mark);
                    return null;
                }
            }
            if (_t.Character(_divisionOp))
            {
                Expression? rhs = Term();
                if (rhs != null)
                {
                    lhs = new OperationExpression(_divisionOp, lhs, rhs);
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

    /// <summary>
    /// Parses the input to be a term; meaning it is a <see cref="Number"/>, <see cref="Vec2"/>, <see cref="Variable"/> or <see cref="Group"/>.
    /// </summary>
    /// <returns><see cref="NumberExpression"/>, <see cref="Vec2Expression"/>, <see cref="VariableExpression"/>, <see cref="OperationExpression"/> or null.</returns>
    public Expression? Term()
    {
        Expression? exp = _t.Number() ?? _t.Vec2() ?? _t.Variable() ?? Group();
        return exp;
    }

    /// <summary>
    /// Parses expression between parenthesis to be an <see cref="Sum"/>.
    /// </summary>
    /// <returns><see cref="OperationExpression"/> or null.</returns> 
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