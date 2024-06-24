using System;

public class PrimeFinder
{
    public static int FindNthPrime(int n)
    {
        if (n <= 0)
            throw new ArgumentException("N must be a positive integer.");

        // Using the Sieve of Eratosthenes to find primes up to an estimate
        int limit = EstimatePrimeLimitForNth(n);
        bool[] isPrime = SieveOfEratosthenes(limit);

        // Collecting the nth prime number
        int primeCount = 0;
        for (int num = 2; num < isPrime.Length; num++)
        {
            if (isPrime[num])
            {
                primeCount++;
                if (primeCount == n)
                    return num;
            }
        }

        throw new ApplicationException($"Failed to find the {n}th prime number.");
    }

    private static int EstimatePrimeLimitForNth(int n)
    {
        // A rough estimation of the nth prime based on the prime number theorem
        // This is just an approximation, can be adjusted based on actual needs
        return (int)(n * Math.Log(n) * 1.15); // Adjusted with a factor for safety margin
    }

    private static bool[] SieveOfEratosthenes(int limit)
    {
        bool[] isPrime = new bool[limit + 1];
        for (int i = 2; i <= limit; i++)
        {
            isPrime[i] = true;
        }

        for (int p = 2; p * p <= limit; p++)
        {
            if (isPrime[p])
            {
                for (int i = p * p; i <= limit; i += p)
                {
                    isPrime[i] = false;
                }
            }
        }

        return isPrime;
    }
}
