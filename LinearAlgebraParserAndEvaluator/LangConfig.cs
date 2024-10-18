using System.Globalization;
using System.Text;

public static class LangConfig
{
    public static class Settings
    {
        public static CultureInfo CultureInfo = CultureInfo.InvariantCulture; // disgusting post-modern globalism.
    }
    public static class ErrorHandling
    {
        private static string FormatSyntaxError(string line, char? received = null, string? message = null, char? expected = null)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(line))        sb.AppendLine($"Line:\t\t{line}");
            if (received.HasValue)                  sb.AppendLine($"Received:\t'{received}'");
            if (expected.HasValue)                  sb.AppendLine($"Expected:\t'{expected}'");
            if (!string.IsNullOrEmpty(message))     sb.AppendLine($"Message:\t{message}");
            
            return sb.ToString();
        }
        public static string CreateSyntaxError(string line, char received, string message)
        {
            return FormatSyntaxError(line, received, message);
        }

        public static string CreateSyntaxError(string line, char received, char expected, string message)
        {
            return FormatSyntaxError(line, received, message, expected);
        }
    }
    public static class Characters
    {
        public enum BracketType
        {
            Group,
            Vec2,
        }
        public class Bracket
        {
            public BracketType Type { get; }
            public char Open { get; }
            public char Close { get; }

            public Bracket(BracketType type, char open, char close)
            {
                Type = type;
                Open = open;
                Close = close;
            }
        }

        private static readonly HashSet<char> _chars = new HashSet<char>
        {
            'a','b','c','d','e','f','g','h','i','j','k','l','m',
            'n','o','p','q','r','s','t','u','v','w','x','y','z'
        };

        private static readonly Bracket[] _brackets =
        [
            new (BracketType.Group,     '(',    ')'     ),
            new (BracketType.Vec2,      '[',    ']'     ),
        ];
        public static Bracket GetBracket(BracketType type) => _brackets.First((Bracket b) => b.Type == type);
        public static bool Contains(char ch) => _chars.Contains(ch);
    }
    public static class Digits
    {
        private static readonly HashSet<char> _digits = ['0','1','2','3','4','5','6','7','8','9'];
        private static char _negativeSign = '-';
        private static char _decimalSign = '.';
        public static char NegativeSign => _negativeSign;
        public static char DecimalSign => _decimalSign;
        public static bool Contains(char ch) => _digits.Contains(ch);
    }
    public static class Operators
    {
        private enum OperatorType
        {
            Addition,
            Subtraction,
            Multiplication,
            Division,
            Assignment,
        }
        private static readonly Dictionary<OperatorType, char> _opSymbols = new()
        {
            { OperatorType.Addition,            '+' },
            { OperatorType.Subtraction,         '-' },
            { OperatorType.Multiplication,      '*' },
            { OperatorType.Division,            '/' },
            { OperatorType.Assignment,          '=' },
        };
        public static bool Contains(char ch) => _opSymbols.ContainsValue(ch);
        public static char Addition          => _opSymbols[OperatorType.Addition];
        public static char Subtraction       => _opSymbols[OperatorType.Subtraction];
        public static char Multiplication    => _opSymbols[OperatorType.Multiplication];
        public static char Division          => _opSymbols[OperatorType.Division];
        public static char Assignment        => _opSymbols[OperatorType.Assignment];
    }
}