using System.Globalization;

public static class LangConfig
{
    public static class Settings
    {
        public static CultureInfo CultureInfo = CultureInfo.InvariantCulture;
    }
    public static class ErrorHandling
    {
        public static string CreateSyntaxError(string line, char received, string message)
        {
            return $"Line:\t\t{line}\nReceived:\t'{received}'\nMessage:\t{message}";
        }
        public static string CreateSyntaxError(string line, char received, char expected, string message)
        {
            return $"Line:\t\t{line}\nReceived:\t'{received}'\nExpected:\t'{expected}'\nMessage:\t{message}";
        }
    }
    public static class Characters
    {
        private static readonly HashSet<char> _chars = ['a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                        'n','o','p','q','r','s','t','u','v','w','x','y','z']; 
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