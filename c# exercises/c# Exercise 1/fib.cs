using System;
using System.Collections.Generic;

public static class Fibonacci
{
    public static int RecursiveGenerate(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentException("Fibonacci sequence length must be positive.");
        }

        if (n == 1)
        {
            return 0;
        }

        if (n == 2)
        {
            return 1;
        }

        return RecursiveGenerate(n - 1) + RecursiveGenerate(n - 2);
    }
}

public static class FibonacciManager
{
    public static void Run(int length)
    {
        if (length <= 0)
        {
            Console.WriteLine("Please enter a valid positive integer for the Fibonacci sequence length.");
            return;
        }

        List<int> fibonacciSequence = new List<int>();

        for (int i = 1; i <= length; i++)
        {
            fibonacciSequence.Add(Fibonacci.RecursiveGenerate(i));
        }

        Console.WriteLine($"Fibonacci sequence of length {length}: {string.Join(", ", fibonacciSequence)}");
    }
}