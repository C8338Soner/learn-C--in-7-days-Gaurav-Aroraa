// Day 07 - More C# 7 Features
// Topics: Local functions, ref locals/returns, out variables (C# 7),
//         throw expressions, expression-bodied members, digit separators,
//         binary literals, default literal, delegates, events, lambda expressions

using System;
using System.Collections.Generic;
using System.Linq;

namespace Day07_MoreFeatures
{
    // ===== DELEGATES AND EVENTS =====

    public delegate int MathOperation(int a, int b);

    public class EventPublisher
    {
        // Standard event
        public event EventHandler<string> MessageReceived;

        // Custom delegate event
        public event Action<string, int> DataProcessed;

        public void PublishMessage(string message)
        {
            Console.WriteLine($"  [Publisher] Sending: {message}");
            MessageReceived?.Invoke(this, message);
        }

        public void ProcessData(string data, int value)
        {
            Console.WriteLine($"  [Publisher] Processing: {data}={value}");
            DataProcessed?.Invoke(data, value);
        }
    }

    // ===== EXPRESSION-BODIED MEMBERS (C# 6/7) =====

    public class Vector3D
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        // Expression-bodied constructor (C# 7)
        public Vector3D(double x, double y, double z) => (X, Y, Z) = (x, y, z);

        // Expression-bodied property
        public double Magnitude => Math.Sqrt(X * X + Y * Y + Z * Z);

        // Expression-bodied method
        public Vector3D Normalize() => new Vector3D(X / Magnitude, Y / Magnitude, Z / Magnitude);

        // Operator overloading with expression body
        public static Vector3D operator +(Vector3D a, Vector3D b) =>
            new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector3D operator *(Vector3D v, double scalar) =>
            new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);

        public static double DotProduct(Vector3D a, Vector3D b) =>
            a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        // Expression-bodied ToString (C# 7)
        public override string ToString() => $"({X:F2}, {Y:F2}, {Z:F2})";

        // Throw expression (C# 7)
        public static Vector3D CreateNonZero(double x, double y, double z) =>
            (x == 0 && y == 0 && z == 0)
                ? throw new ArgumentException("Zero vector is not allowed")
                : new Vector3D(x, y, z);
    }

    // ===== LOCAL FUNCTIONS (C# 7) =====

    public static class LocalFunctionExamples
    {
        public static List<int> GetPrimesUpTo(int limit)
        {
            // Local function - only visible within GetPrimesUpTo
            bool IsPrime(int n)
            {
                if (n < 2) return false;
                if (n == 2) return true;
                if (n % 2 == 0) return false;
                for (int i = 3; i * i <= n; i += 2)
                    if (n % i == 0) return false;
                return true;
            }

            return Enumerable.Range(2, limit - 1).Where(IsPrime).ToList();
        }

        public static IEnumerable<int> GenerateFibonacci(int count)
        {
            // Local iterator function (C# 7)
            IEnumerable<int> GenerateSequence()
            {
                int a = 0, b = 1;
                for (int i = 0; i < count; i++)
                {
                    yield return a;
                    (a, b) = (b, a + b);
                }
            }

            if (count < 0) throw new ArgumentException("Count must be non-negative.");
            return GenerateSequence();
        }

        public static int BinarySearch(int[] arr, int target)
        {
            // Recursive local function
            int Search(int left, int right)
            {
                if (left > right) return -1;
                int mid = left + (right - left) / 2;
                if (arr[mid] == target) return mid;
                if (arr[mid] < target) return Search(mid + 1, right);
                return Search(left, mid - 1);
            }

            return Search(0, arr.Length - 1);
        }
    }

    // ===== REF LOCALS AND RETURNS (C# 7) =====

    public static class RefExamples
    {
        // ref return - returns a reference to an element
        public static ref int FindFirst(int[] arr, int value)
        {
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] == value)
                    return ref arr[i];

            throw new InvalidOperationException($"Value {value} not found in array.");
        }

        public static void Demonstrate()
        {
            Console.WriteLine("--- ref locals and returns (C# 7) ---");

            int[] data = { 10, 20, 30, 40, 50 };
            Console.WriteLine($"  Before: [{string.Join(", ", data)}]");

            // ref local - modifying through reference
            ref int element = ref LocalFunctions.GetRef(data, 2);
            element = 999;  // Modifies the original array
            Console.WriteLine($"  After modifying element at index 2: [{string.Join(", ", data)}]");

            // ref return
            ref int found = ref FindFirst(data, 40);
            found = 42;  // Modifies the original array
            Console.WriteLine($"  After modifying value 40->42: [{string.Join(", ", data)}]");
        }
    }

    public static class LocalFunctions
    {
        public static ref int GetRef(int[] arr, int index) => ref arr[index];
    }

    // ===== OUT VARIABLES (C# 7) =====

    public static class OutVariableExamples
    {
        public static void Demonstrate()
        {
            Console.WriteLine("--- Out Variables (C# 7) ---");

            // C# 7: declare out variable inline
            if (int.TryParse("42", out int parsedInt))
                Console.WriteLine($"  Parsed int: {parsedInt}");

            if (double.TryParse("3.14", out double parsedDouble))
                Console.WriteLine($"  Parsed double: {parsedDouble}");

            // Out with discard
            _ = int.TryParse("not a number", out _);

            // Multiple out parameters
            GetMinMax(new[] { 5, 2, 8, 1, 9, 3 }, out int min, out int max);
            Console.WriteLine($"  Min: {min}, Max: {max}");
        }

        static void GetMinMax(int[] arr, out int min, out int max)
        {
            min = arr[0]; max = arr[0];
            foreach (var v in arr)
            {
                if (v < min) min = v;
                if (v > max) max = v;
            }
        }
    }

    // ===== LAMBDAS AND FUNCTIONAL PATTERNS =====

    public static class LambdaExamples
    {
        public static void Demonstrate()
        {
            Console.WriteLine("--- Lambdas and Functional Patterns ---");

            // Action (no return value)
            Action<string> print = msg => Console.WriteLine($"  {msg}");
            print("Hello from Action lambda!");

            // Func (with return value)
            Func<int, int, int> add = (a, b) => a + b;
            Console.WriteLine($"  add(3, 5) = {add(3, 5)}");

            // Predicate
            Predicate<int> isEven = n => n % 2 == 0;
            Console.WriteLine($"  isEven(4) = {isEven(4)}, isEven(7) = {isEven(7)}");

            // Custom delegate
            MathOperation multiply = (a, b) => a * b;
            Console.WriteLine($"  multiply(6, 7) = {multiply(6, 7)}");

            // Delegate chaining (multicast)
            Action<string> logger = msg => Console.WriteLine($"  [LOG] {msg}");
            Action<string> auditor = msg => Console.WriteLine($"  [AUDIT] {msg}");
            Action<string> combined = logger + auditor;
            combined("User logged in");

            // Higher-order functions
            var numbers = Enumerable.Range(1, 10).ToList();
            Console.WriteLine($"  Numbers: [{string.Join(", ", numbers)}]");

            var evenSquares = numbers
                .Where(n => n % 2 == 0)
                .Select(n => n * n)
                .ToList();
            Console.WriteLine($"  Even squares: [{string.Join(", ", evenSquares)}]");

            // Closure
            int multiplier = 3;
            Func<int, int> tripler = x => x * multiplier;
            Console.WriteLine($"  tripler(5) = {tripler(5)} (captures multiplier={multiplier})");
            multiplier = 4;
            Console.WriteLine($"  tripler(5) after changing multiplier = {tripler(5)} (closure updates)");
        }
    }

    // ===== NUMERIC LITERALS (C# 7) =====

    public static class NumericLiteralExamples
    {
        public static void Demonstrate()
        {
            Console.WriteLine("--- Numeric Literals (C# 7) ---");

            // Digit separators (C# 7)
            int million = 1_000_000;
            double pi = 3.141_592_653_589;
            long bigNumber = 123_456_789_012L;

            Console.WriteLine($"  Million: {million:N0}");
            Console.WriteLine($"  Pi: {pi}");
            Console.WriteLine($"  Big number: {bigNumber:N0}");

            // Binary literals (C# 7)
            int flags = 0b1010_1010;
            int mask = 0b0000_1111;
            Console.WriteLine($"  Binary 0b10101010 = {flags} (decimal), 0x{flags:X2} (hex)");
            Console.WriteLine($"  flags & mask = {flags & mask} (0b{Convert.ToString(flags & mask, 2).PadLeft(8, '0')})");

            // Hex literals
            int hexValue = 0xFF_A0_3C;
            Console.WriteLine($"  Hex 0xFFA03C = {hexValue:N0} (decimal), rgb({hexValue >> 16},{(hexValue >> 8) & 0xFF},{hexValue & 0xFF})");
        }
    }

    // ===== THROW EXPRESSIONS (C# 7) =====

    public class Person
    {
        private string _name;

        // Throw expression in property setter (C# 7)
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value), "Name cannot be null.");
        }

        // Throw expression in null-coalescing
        public Person(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override string ToString() => $"Person: {_name}";
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Day 07: More C# 7 Features ===\n");

            // Delegates and Events
            Console.WriteLine("--- Delegates and Events ---");
            var publisher = new EventPublisher();
            publisher.MessageReceived += (sender, msg) => Console.WriteLine($"  [Handler1] Received: {msg}");
            publisher.MessageReceived += (sender, msg) => Console.WriteLine($"  [Handler2] Got: {msg.ToUpper()}");
            publisher.DataProcessed += (data, val) => Console.WriteLine($"  [DataHandler] {data}={val}");
            publisher.PublishMessage("Hello World");
            publisher.ProcessData("Temperature", 98);
            Console.WriteLine();

            // Expression-bodied members
            Console.WriteLine("--- Expression-Bodied Members & Vectors ---");
            var v1 = new Vector3D(1, 2, 3);
            var v2 = new Vector3D(4, 5, 6);
            Console.WriteLine($"  v1 = {v1}, magnitude = {v1.Magnitude:F2}");
            Console.WriteLine($"  v2 = {v2}");
            Console.WriteLine($"  v1 + v2 = {v1 + v2}");
            Console.WriteLine($"  v1 * 2 = {v1 * 2}");
            Console.WriteLine($"  dot product = {Vector3D.DotProduct(v1, v2):F2}");
            Console.WriteLine($"  normalized v1 = {v1.Normalize()}");

            try
            {
                var _ = Vector3D.CreateNonZero(0, 0, 0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  Throw expression caught: {ex.Message}");
            }
            Console.WriteLine();

            // Local functions
            Console.WriteLine("--- Local Functions ---");
            var primes = LocalFunctionExamples.GetPrimesUpTo(30);
            Console.WriteLine($"  Primes up to 30: [{string.Join(", ", primes)}]");

            var fib = LocalFunctionExamples.GenerateFibonacci(10);
            Console.WriteLine($"  Fibonacci(10): [{string.Join(", ", fib)}]");

            int[] sorted = { 1, 3, 5, 7, 9, 11, 13, 15 };
            int idx = LocalFunctionExamples.BinarySearch(sorted, 7);
            Console.WriteLine($"  BinarySearch for 7 in sorted array: index {idx}");
            Console.WriteLine();

            // ref locals and returns
            RefExamples.Demonstrate();
            Console.WriteLine();

            // out variables (C# 7)
            OutVariableExamples.Demonstrate();
            Console.WriteLine();

            // Lambdas
            LambdaExamples.Demonstrate();
            Console.WriteLine();

            // Numeric literals
            NumericLiteralExamples.Demonstrate();
            Console.WriteLine();

            // Throw expressions
            Console.WriteLine("--- Throw Expressions (C# 7) ---");
            var person = new Person("Alice");
            Console.WriteLine($"  Created: {person}");
            try
            {
                var badPerson = new Person(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  Null constructor arg caught: {ex.ParamName}");
            }
            try
            {
                person.Name = null;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  Null property set caught: {ex.ParamName}");
            }
            Console.WriteLine();

            Console.WriteLine("=== All 7 days completed! Happy C# coding! ===");
        }
    }
}
