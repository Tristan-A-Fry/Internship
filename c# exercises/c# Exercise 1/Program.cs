using System;

class Program
{
    static void Main()
    {
        string city = "Aberdeen";
        ReadOnlySpan<char> citySpan = city.AsSpan()[^5..];
        Console.WriteLine(citySpan.ToString());
        try
        {
            Console.Write("Enter the length of the Fibonacci sequence: ");
            if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
            {
                // Generate Fibonacci sequence 
                Console.WriteLine($"\nGenerating Fibonacci sequence of length {n}:");
                FibonacciManager.Run(n);

                // Generate Palindromic numbers 
                Console.WriteLine("\nGenerating Palindromic numbers up to 250,000:");
                PalindromeGeneratorManager.Run();

                // Find the 500th prime number
                int primeNumber = PrimeFinder.FindNthPrime(500);
                Console.WriteLine($"\nThe 500th prime number is: {primeNumber}");
                
                Console.WriteLine("\nSolving the maze:");
                MazeSolverManager.Run();

            }
            else
            {
                Console.WriteLine("Please enter a valid positive integer for the Fibonacci sequence length.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
