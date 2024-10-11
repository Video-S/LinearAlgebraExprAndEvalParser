using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;     // number parsing with decimals ('.', ',')
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;   // gets weird without this

        Console.WriteLine("Enter an expression to evaluate, or an empty line to quit.");

        while (true)
        {
            string? input = Console.ReadLine();

            if (input == null) break;
            if (input.Trim().Length == 0) break;

            Parser parser = new Parser(input);
            Expression? expression = null;
            
            try
            {
                expression = parser.Statement();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during interpretation: {ex.Message}");
            }

            if (expression != null)
            {
                try
                {
                    Value result = expression.Evaluate();
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during evaluation: {ex.Message}");
                }
            }
            else Console.WriteLine("Error");
        }
    }
}