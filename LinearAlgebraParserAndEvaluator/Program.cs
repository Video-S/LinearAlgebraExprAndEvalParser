using LinearAlgebraParserAndEvaluator;
class Program
{
    static void Main(string[] args)
    {
        Parser parser = new Parser();
        Console.WriteLine("Enter an expression to evaluate, or an empty line to quit.");

        while (true)
        {
            string? input = Console.ReadLine();

            if (input == null) break;
            if (input.Trim().Length == 0) break;

            Expression? expression = null;

            try
            {
                expression = parser.Parse(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Expression returned null.");
            }
        }
    }
}