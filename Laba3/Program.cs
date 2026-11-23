using ConsoleApp.ConsoleObject.Table;
using ConsoleApp.ConsoleObject.Table.TableRows;
using Laba2.ConsoleObject.Table.TableRows;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Program
{
    static ITableRow Row(params object[] cells)
    {
        return new TableRow(new List<object>(cells));
    }

    static bool IsPrimeBasic(long n)
    {
        if (n < 2)
        {
            return false;
        }

        for (long i = 2; i < n; i++)
        {
            if (n % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    static bool IsPrimeOdd(long n)
    {
        if (n < 2)
        {
            return false;
        }

        if (n == 2)
        {
            return true;
        }

        if (n % 2 == 0)
        {
            return false;
        }

        for (long i = 3; i < n; i += 2)
        {
            if (n % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    static bool IsPrimeSqrt(long n)
    {
        if (n < 2)
        {
            return false;
        }

        long limit = (long)Math.Sqrt(n);

        for (long i = 2; i <= limit; i++)
        {
            if (n % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    static bool IsPrimeSqrtOdd(long n)
    {
        if (n < 2)
        {
            return false;
        }

        if (n == 2)
        {
            return true;
        }

        if (n % 2 == 0)
        {
            return false;
        }

        long limit = (long)Math.Sqrt(n);

        for (long i = 3; i <= limit; i += 2)
        {
            if (n % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    static long NextPrime(long n)
    {
        while (!IsPrimeSqrtOdd(n))
        {
            n++;
        }

        return n;
    }

    static long GcdArray(long m, long n)
    {
        List<long> divisors = new List<long>();
        long min = Math.Min(m, n);

        for (long i = 1; i <= min; i++)
        {
            if (m % i == 0 && n % i == 0)
            {
                divisors.Add(i);
            }
        }

        long max = divisors[0];

        foreach (long d in divisors)
        {
            if (d > max)
            {
                max = d;
            }
        }

        return max;
    }

    static long GcdUp(long m, long n)
    {
        long nod = 1;
        long min = Math.Min(m, n);

        for (long i = 2; i <= min; i++)
        {
            if (m % i == 0 && n % i == 0)
            {
                nod = i;
            }
        }

        return nod;
    }

    static long GcdDown(long m, long n)
    {
        for (long i = Math.Min(m, n); i >= 2; i--)
        {
            if (m % i == 0 && n % i == 0)
            {
                return i;
            }
        }

        return 1;
    }

    static long GcdEuclid(long a, long b)
    {
        while (b != 0)
        {
            long t = a % b;
            a = b;
            b = t;
        }

        return a;
    }

    static long LcmArray(long m, long n)
    {
        List<long> multiples = new List<long>();
        long max = Math.Max(m, n);
        long product = m * n;

        for (long i = max; i <= product; i++)
        {
            if (i % m == 0 && i % n == 0)
            {
                multiples.Add(i);
            }
        }

        long min = multiples[0];

        foreach (long x in multiples)
        {
            if (x < min)
            {
                min = x;
            }
        }

        return min;
    }

    static long LcmBrute(long m, long n)
    {
        long product = m * n;

        for (long i = Math.Max(m, n); i <= product; i++)
        {
            if (i % m == 0 && i % n == 0)
            {
                return i;
            }
        }

        return product;
    }

    static long LcmEuclid(long m, long n)
    {
        return m / GcdEuclid(m, n) * n;
    }

    static string Measure(Func<long, long, object> func, long a, long b, int trials)
    {
        Stopwatch sw = Stopwatch.StartNew();

        for (int t = 0; t < trials; t++)
        {
            func(a, b);
        }

        sw.Stop();

        double ms = sw.Elapsed.TotalMilliseconds / trials;

        return ms.ToString("F4");
    }

    static string MeasureLimited(Func<long, long, object> func, long a, long b, long size, long maxSize, int trials)
    {
        if (size > maxSize)
        {
            return "-";
        }

        return Measure(func, a, b, trials);
    }

    static void Task1()
    {
        Console.WriteLine("\nЗадание 2 (проверка числа на простоту, время в мс, число простое)");
        var rows = new List<ITableRow>();
        rows.Add(Row("N", "Перебор от 2 до N", "Только нечётные до N", "Перебор до sqrt(N)", "Нечётные до sqrt(N)"));

        long[] sizes = { 10_000, 100_000, 1_000_000, 10_000_000, 100_000_000, 1_000_000_000, 100_000_000_000 };
        long slowLimit = 100_000_000;

        foreach (long size in sizes)
        {
            long p = NextPrime(size);
            int trials = size <= 1_000_000 ? 100 : 3;

            rows.Add(Row(
                p,
                MeasureLimited((a, b) => IsPrimeBasic(a), p, 0, p, slowLimit, trials),
                MeasureLimited((a, b) => IsPrimeOdd(a), p, 0, p, slowLimit, trials),
                MeasureLimited((a, b) => IsPrimeSqrt(a), p, 0, p, long.MaxValue, trials),
                MeasureLimited((a, b) => IsPrimeSqrtOdd(a), p, 0, p, long.MaxValue, trials)));
        }

        new Table("task1", rows).Print();
    }

    static void Task2()
    {
        Console.WriteLine("\nЗадание 3 (НОД, время в мс, M = N + 1, НОД = 1)");
        var rows = new List<ITableRow>();
        rows.Add(Row("N", "Массив делителей", "Перебор от 1 до min", "Перебор от min до 1", "Алгоритм Евклида"));

        long[] sizes = { 10_000, 100_000, 1_000_000, 10_000_000, 100_000_000 };

        foreach (long size in sizes)
        {
            long m = size + 1;
            long n = size;
            int trials = size <= 1_000_000 ? 20 : 3;

            rows.Add(Row(
                size,
                Measure((a, b) => GcdArray(a, b), m, n, trials),
                Measure((a, b) => GcdUp(a, b), m, n, trials),
                Measure((a, b) => GcdDown(a, b), m, n, trials),
                Measure((a, b) => GcdEuclid(a, b), m, n, 100_000)));
        }

        new Table("task2", rows).Print();
    }

    static void Task3()
    {
        Console.WriteLine("\nДоп. задание 1 (НОК, время в мс, M = N + 1, НОД = 1)");
        var rows = new List<ITableRow>();
        rows.Add(Row("N", "Массив кратных", "Перебор от max до M*N", "Через НОД (Евклид)"));

        long[] sizes = { 100, 300, 1_000, 3_000, 10_000 };

        foreach (long size in sizes)
        {
            long m = size + 1;
            long n = size;
            int trials = size <= 1_000 ? 20 : 3;

            rows.Add(Row(
                size,
                Measure((a, b) => LcmArray(a, b), m, n, trials),
                Measure((a, b) => LcmBrute(a, b), m, n, trials),
                Measure((a, b) => LcmEuclid(a, b), m, n, 100_000)));
        }

        new Table("task3", rows).Print();
    }

    static void Task4()
    {
        Console.WriteLine("\nДоп. задание 2 (сложность встроенной сортировки, время в мс)");
        var rows = new List<ITableRow>();
        rows.Add(Row("N", "Время сортировки", "N * log2(N)", "Время / (N * log2(N)), нс"));

        Random rng = new Random(12345);
        int[] sizes = { 10_000, 50_000, 100_000, 500_000, 1_000_000, 5_000_000, 10_000_000 };

        foreach (int n in sizes)
        {
            int trials = n <= 1_000_000 ? 5 : 2;
            double totalMs = 0;

            for (int t = 0; t < trials; t++)
            {
                int[] data = new int[n];

                for (int i = 0; i < n; i++)
                {
                    data[i] = rng.Next();
                }

                Stopwatch sw = Stopwatch.StartNew();
                Array.Sort(data);
                sw.Stop();

                totalMs += sw.Elapsed.TotalMilliseconds;
            }

            double ms = totalMs / trials;
            double nLogN = n * Math.Log2(n);
            double ratio = ms * 1_000_000 / nLogN;

            rows.Add(Row(n, ms.ToString("F4"), Math.Round(nLogN), ratio.ToString("F4")));
        }

        new Table("task4", rows).Print();
    }

    public static void Main(string[] args)
    {
        Task1();
        Task2();
        Task3();
        Task4();
    }
}