using System.Data;
using System.Globalization;
using static LangConfig;

/// <summary>
/// Parses a given valid statement into valid expressions.
/// Uses a <c><see cref="Tokenizer"/></c> to tokenize the input.
/// </summary>
public class Parser
{
    private Tokenizer _t;
    private char _opAssignment = Operators.Assignment;
    private char _opAddition = Operators.Addition;
    private char _opSubtraction = Operators.Subtraction;
    private char _opMultiplication = Operators.Multiplication;
    private char _opDivision = Operators.Division;

    public Parser(string line)
    {
        _t = new Tokenizer(line);

        CultureInfo.DefaultThreadCurrentCulture = Settings.CultureInfo;     // number parsing with decimals
        CultureInfo.DefaultThreadCurrentUICulture = Settings.CultureInfo;   // gets weird without this
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
        if (lhs != null && _t.Character(_opAssignment))
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
            if (_t.Character(_opAddition))
            {
                Expression? rhs = Product();
                if (rhs != null)
                {
                    lhs = new OperationExpression(_opAddition, lhs, rhs);
                }
                else
                {
                    _t.Reset(mark);
                    return null;
                }
            }
            else if (_t.Character(_opSubtraction))
            {
                Expression? rhs = Product();
                if(rhs != null)
                {
                    lhs = new OperationExpression(_opSubtraction, lhs, rhs);
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
            if (_t.Character(_opMultiplication))
            {
                Expression? rhs = Term();
                if (rhs != null)
                {
                    lhs = new OperationExpression(_opMultiplication, lhs, rhs);
                }
                else
                {
                    _t.Reset(mark);
                    return null;
                }
            }
            if (_t.Character(_opDivision))
            {
                Expression? rhs = Term();
                if (rhs != null)
                {
                    lhs = new OperationExpression(_opDivision, lhs, rhs);
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
            else
            {
                string errorMessage = "Group could not be closed. Did you forget a ')'?";
                string syntaxError = ErrorHandling.CreateSyntaxError(_t.Data, _t.CurrentCharacter, ')', errorMessage);
                throw new SyntaxErrorException(syntaxError);
            }
        }

        _t.Reset(mark);
        return null;
    }
}