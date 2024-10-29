namespace LinearAlgebraParserAndEvaluator;

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;

/// <summary>
/// Static class that contains configurable properties of the parser.
/// </summary>
public static class LangConfig
{
    public static class Settings
    {
        /// <summary>
        /// Changes how stuff like decimals are handled. Changing this breaks shit unless the decimal sign is changed as well.
        /// /// </summary>
        public static CultureInfo CultureInfo = CultureInfo.InvariantCulture;
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

        /// <summary>
        /// Creates a standardized error message.
        /// </summary>
        /// <param name="line">The line where the error occured.</param>
        /// <param name="received">The character that caused the error to occur.</param>
        /// <param name="message">Explain to the user what happened here.</param>
        /// <returns>Formatted error message.</returns>
        public static string CreateSyntaxError(string line, char received, string message)
        {
            return FormatSyntaxError(line, received, message);
        }

        /// <summary>
        /// Creates a standardized error message.
        /// </summary>
        /// <param name="line">The line where the error occured.</param>
        /// <param name="received">The character that caused the error to occur.</param>
        /// <param name="expected">The character the parser or tokenizer expected.</param>
        /// <param name="message">Explain to the user what happened here.</param>
        /// <returns>Formatted error message.</returns>
        public static string CreateSyntaxError(string line, char received, char expected, string message)
        {
            return FormatSyntaxError(line, received, message, expected);
        }
    }
    public static class Characters
    {
        /// <summary>Types of brackets.</summary>
        public enum BracketType
        {
            Group,
            Vec2,
        }
        /// <summary>Represents a type of bracket.</summary>
        public class Bracket
        {
            /// <summary>The type.</summary>
            /// <value><see cref="BracketType"/></value>
            public BracketType Type { get; }
            /// <summary>The opening bracket.</summary>
            /// <value><see cref="char"/></value>
            public char Open { get; }
            /// <summary>The closing bracket.</summary>
            /// <value><see cref="char"/></value>
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
            'n','o','p','q','r','s','t','u','v','w','x','y','z',
        };

        private static readonly Bracket[] _brackets = new Bracket[]
        {
            new Bracket(BracketType.Group,  '('  ,  ')'  ),
            new Bracket(BracketType.Vec2,   '['  ,  ']'  ),
        };
        /// <summary>
        /// Returns the opening and closing bracket of the specified type. See <see cref="BracketTypes"/> for avaible types.
        /// </summary>
        /// <param name="type"><see cref="BracketType"/></param>
        /// <returns><see cref="Bracket"/></returns>
        public static Bracket GetBracket(BracketType type) => _brackets.First((Bracket b) => b.Type == type);
        public static bool Contains(char ch) => _chars.Contains(ch);
    }
    public static class Digits
    {
        private static readonly HashSet<char> _digits = new HashSet<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
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
            { OperatorType.Assignment,          ':' },
        };
        public static bool Contains(char ch) => _opSymbols.ContainsValue(ch);
        public static char Addition          => _opSymbols[OperatorType.Addition];
        public static char Subtraction       => _opSymbols[OperatorType.Subtraction];
        public static char Multiplication    => _opSymbols[OperatorType.Multiplication];
        public static char Division          => _opSymbols[OperatorType.Division];
        public static char Assignment        => _opSymbols[OperatorType.Assignment];
    }
}