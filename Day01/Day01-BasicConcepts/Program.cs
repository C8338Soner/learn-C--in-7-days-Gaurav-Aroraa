// Day 01 - Basic C# Concepts
// Topics: Variables, Data Types, Operators, Control Flow, Methods

using System;
using System.Text;

namespace Day01_BasicConcepts
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Day 01: Basic C# Concepts ===\n");

            DemonstrateVariablesAndTypes();
            DemonstrateOperators();
            DemonstrateControlFlow();
            DemonstrateMethods();
            DemonstrateStringOperations();
        }

        // 1. Variables and Data Types
        static void DemonstrateVariablesAndTypes()
        {
            Console.WriteLine("--- Variables and Data Types ---");

            // Value types
            int intValue = 42;
            double doubleValue = 3.14159;
            float floatValue = 2.71f;
            decimal moneyValue = 19.99m;
            bool isTrue = true;
            char charValue = 'C';

            // var keyword - type inference
            var inferredInt = 100;
            var inferredString = "Hello, C#!";

            // const
            const double PI = 3.14159265;

            // String and verbatim string
            string greeting = "Hello, World!";
            string path = @"C:\Users\Documents\file.txt";

            // Nullable types
            int? nullableInt = null;
            double? nullableDouble = 3.14;

            Console.WriteLine($"int: {intValue}, double: {doubleValue}, float: {floatValue}");
            Console.WriteLine($"decimal: {moneyValue}, bool: {isTrue}, char: {charValue}");
            Console.WriteLine($"var int: {inferredInt}, var string: {inferredString}");
            Console.WriteLine($"const PI: {PI}");
            Console.WriteLine($"String: {greeting}");
            Console.WriteLine($"Verbatim string: {path}");
            Console.WriteLine($"Nullable int: {nullableInt}, nullable double: {nullableDouble}");
            Console.WriteLine($"HasValue: {nullableDouble.HasValue}, Value: {nullableDouble.Value}");
            Console.WriteLine();

            // Type conversions
            int x = 10;
            double y = x;           // implicit conversion
            int z = (int)3.99;      // explicit cast
            string numStr = "42";
            int parsed = int.Parse(numStr);
            bool tryParsed = int.TryParse("not a number", out int result);

            Console.WriteLine($"Implicit: {y}, Explicit cast: {z}, Parsed: {parsed}");
            Console.WriteLine($"TryParse success: {tryParsed}, result: {result}");
            Console.WriteLine();
        }

        // 2. Operators
        static void DemonstrateOperators()
        {
            Console.WriteLine("--- Operators ---");

            int a = 10, b = 3;

            // Arithmetic
            Console.WriteLine($"Arithmetic: {a}+{b}={a + b}, {a}-{b}={a - b}, {a}*{b}={a * b}");
            Console.WriteLine($"Division: {a}/{b}={a / b}, Modulo: {a}%{b}={a % b}");

            // Comparison
            Console.WriteLine($"Comparison: {a}>{b}={a > b}, {a}<{b}={a < b}, {a}=={b}={a == b}");

            // Logical
            bool p = true, q = false;
            Console.WriteLine($"Logical: {p}&&{q}={p && q}, {p}||{q}={p || q}, !{p}={!p}");

            // Bitwise - using decimal representation for readability
            int m = 10, n = 12;   // 0b1010=10, 0b1100=12
            Console.WriteLine($"Bitwise AND: {m}&{n}={(m & n)}, OR: {m}|{n}={(m | n)}, XOR: {m}^{n}={(m ^ n)}");

            // Ternary
            int max = a > b ? a : b;
            Console.WriteLine($"Ternary max({a},{b}) = {max}");

            // Null-coalescing
            string nullStr = null;
            string coalescedResult = nullStr ?? "default value";
            Console.WriteLine($"Null-coalescing: {coalescedResult}");

            // Null-conditional
            string maybeNull = null;
            int? length = maybeNull?.Length;
            Console.WriteLine($"Null-conditional length: {length}");
            Console.WriteLine();
        }

        // 3. Control Flow
        static void DemonstrateControlFlow()
        {
            Console.WriteLine("--- Control Flow ---");

            // if-else
            int score = 85;
            if (score >= 90) Console.WriteLine("Grade: A");
            else if (score >= 80) Console.WriteLine($"Score {score}: Grade B");
            else if (score >= 70) Console.WriteLine("Grade: C");
            else Console.WriteLine("Grade: F");

            // switch
            string day = "Monday";
            switch (day)
            {
                case "Monday":
                case "Tuesday":
                case "Wednesday":
                case "Thursday":
                case "Friday":
                    Console.WriteLine($"{day} is a weekday");
                    break;
                case "Saturday":
                case "Sunday":
                    Console.WriteLine($"{day} is a weekend");
                    break;
                default:
                    Console.WriteLine("Unknown day");
                    break;
            }

            // for loop
            Console.Write("For loop: ");
            for (int i = 1; i <= 5; i++)
                Console.Write($"{i} ");
            Console.WriteLine();

            // foreach loop
            string[] fruits = { "Apple", "Banana", "Cherry", "Date" };
            Console.Write("foreach: ");
            foreach (string fruit in fruits)
                Console.Write($"{fruit} ");
            Console.WriteLine();

            // while loop
            int count = 0;
            Console.Write("while: ");
            while (count < 5)
            {
                Console.Write($"{count} ");
                count++;
            }
            Console.WriteLine();

            // do-while
            int num = 0;
            Console.Write("do-while: ");
            do
            {
                Console.Write($"{num} ");
                num++;
            } while (num < 5);
            Console.WriteLine();

            // break and continue
            Console.Write("break/continue: ");
            for (int i = 0; i < 10; i++)
            {
                if (i == 3) continue;
                if (i == 7) break;
                Console.Write($"{i} ");
            }
            Console.WriteLine("\n");
        }

        // 4. Methods
        static void DemonstrateMethods()
        {
            Console.WriteLine("--- Methods ---");

            // Regular method call
            int sum = Add(5, 10);
            Console.WriteLine($"Add(5, 10) = {sum}");

            // Method with default parameter
            Greet("Alice");
            Greet("Bob", "Good evening");

            // Method with out parameter
            if (TryDivide(10, 3, out double quotient))
                Console.WriteLine($"10 / 3 = {quotient:F4}");

            // Method with params
            int total = Sum(1, 2, 3, 4, 5);
            Console.WriteLine($"Sum(1,2,3,4,5) = {total}");

            // Method overloading
            Console.WriteLine($"Multiply(3, 4) = {Multiply(3, 4)}");
            Console.WriteLine($"Multiply(3.5, 4.2) = {Multiply(3.5, 4.2):F2}");

            // Expression-bodied method (C# 6+)
            Console.WriteLine($"Square(7) = {Square(7)}");
            Console.WriteLine();
        }

        static int Add(int a, int b) => a + b;

        static int Square(int n) => n * n;

        static void Greet(string name, string message = "Hello")
        {
            Console.WriteLine($"{message}, {name}!");
        }

        static bool TryDivide(double a, double b, out double result)
        {
            if (b == 0) { result = 0; return false; }
            result = a / b;
            return true;
        }

        static int Sum(params int[] numbers)
        {
            int total = 0;
            foreach (var n in numbers) total += n;
            return total;
        }

        static int Multiply(int a, int b) => a * b;
        static double Multiply(double a, double b) => a * b;

        // 5. String Operations
        static void DemonstrateStringOperations()
        {
            Console.WriteLine("--- String Operations ---");

            string str = "  Hello, C# World!  ";

            Console.WriteLine($"Original: '{str}'");
            Console.WriteLine($"Trimmed: '{str.Trim()}'");
            Console.WriteLine($"Upper: '{str.Trim().ToUpper()}'");
            Console.WriteLine($"Lower: '{str.Trim().ToLower()}'");
            Console.WriteLine($"Length: {str.Length}");
            Console.WriteLine($"Contains 'C#': {str.Contains("C#")}");
            Console.WriteLine($"Replace: '{str.Replace("World", "Universe")}'");
            Console.WriteLine($"Substring(7,2): '{str.Trim().Substring(7, 2)}'");

            // String interpolation
            string name = "Developer";
            int year = 2024;
            Console.WriteLine($"Welcome, {name}! Year: {year}. Pi: {Math.PI:F3}");

            // String formatting
            Console.WriteLine(string.Format("Formatted: {0:C}", 1234.56));

            // String join and split
            string[] words = { "C#", "is", "awesome" };
            string joined = string.Join(" ", words);
            Console.WriteLine($"Joined: {joined}");

            string[] splitResult = joined.Split(' ');
            Console.WriteLine($"Split count: {splitResult.Length}");

            // StringBuilder for efficient concatenation
            var sb = new StringBuilder();
            for (int i = 1; i <= 5; i++)
                sb.Append($"Item{i} ");
            Console.WriteLine($"StringBuilder: {sb}");
        }
    }
}
