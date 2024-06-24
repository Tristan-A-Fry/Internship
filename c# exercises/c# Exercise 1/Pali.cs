using System;
using System.Collections.Generic;

public static class PalindromeGenerator
{
    public static List<int> Generate(int limit)
    {
        List<int> palindromicNumbers = new List<int>();

        for (int i = 0; i <= limit; i++)
        {
            if (IsPalindrome(i))
            {
                palindromicNumbers.Add(i);
            }
        }

        return palindromicNumbers;
    }

    private static bool IsPalindrome(int number)
    {
        string strNumber = number.ToString();
        int len = strNumber.Length;

        for (int i = 0; i < len / 2; i++)
        {
            if (strNumber[i] != strNumber[len - 1 - i])
            {
                return false;
            }
        }

        return true;
    }
}

public static class PalindromeGeneratorManager
{
    public static void Run()
    {
        int limit = 250000;
        List<int> palindromicNumbers = PalindromeGenerator.Generate(limit);

        Console.WriteLine($"Palindromic numbers up to {limit}:");
        foreach (int number in palindromicNumbers)
        {
            Console.Write(number + " ");
        }
        Console.WriteLine();
    }
}

