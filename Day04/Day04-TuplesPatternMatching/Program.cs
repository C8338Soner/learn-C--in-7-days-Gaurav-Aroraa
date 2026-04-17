// Day 04 - Tuples and Pattern Matching (C# 7)
// Topics: Named tuples, tuple deconstruction, is-pattern, switch-pattern,
//         when clauses, positional patterns, property patterns

using System;
using System.Collections.Generic;

namespace Day04_TuplesPatternMatching
{
    // ===== TUPLES =====

    // Record-like class to use with tuples
    public class Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y) { X = x; Y = y; }

        // Deconstruct method enables tuple-style deconstruction
        public void Deconstruct(out double x, out double y)
        {
            x = X;
            y = Y;
        }

        public override string ToString() => $"({X}, {Y})";
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public int YearsOfService { get; set; }

        public Employee(int id, string name, string department, decimal salary, int years)
        {
            Id = id; Name = name; Department = department; Salary = salary; YearsOfService = years;
        }
    }

    public static class TupleExamples
    {
        // Method returning a named tuple (C# 7)
        public static (double Min, double Max, double Average) GetStatistics(IEnumerable<double> values)
        {
            double min = double.MaxValue, max = double.MinValue, sum = 0;
            int count = 0;

            foreach (var v in values)
            {
                if (v < min) min = v;
                if (v > max) max = v;
                sum += v;
                count++;
            }

            return (min, max, sum / count);
        }

        // Returning tuple with division result
        public static (int Quotient, int Remainder) DivMod(int dividend, int divisor)
        {
            return (dividend / divisor, dividend % divisor);
        }

        // Using tuples in sorting
        public static List<(string Name, decimal Salary, string Department)> GetTopEarners(
            List<Employee> employees, int top = 3)
        {
            var result = new List<(string, decimal, string)>();
            employees.Sort((a, b) => b.Salary.CompareTo(a.Salary));
            for (int i = 0; i < Math.Min(top, employees.Count); i++)
            {
                var e = employees[i];
                result.Add((e.Name, e.Salary, e.Department));
            }
            return result;
        }
    }

    // ===== PATTERN MATCHING =====

    public abstract class Shape { }
    public class Circle : Shape { public double Radius { get; set; } }
    public class Rectangle : Shape { public double Width { get; set; } public double Height { get; set; } }
    public class Triangle : Shape { public double Base { get; set; } public double Height { get; set; } }

    public static class PatternMatchingExamples
    {
        // is-pattern (C# 7)
        public static double CalculateArea(Shape shape)
        {
            if (shape is Circle c)
                return Math.PI * c.Radius * c.Radius;

            if (shape is Rectangle r)
                return r.Width * r.Height;

            if (shape is Triangle t)
                return 0.5 * t.Base * t.Height;

            return 0;
        }

        // switch with type patterns and when clauses (C# 7)
        public static string DescribeShape(Shape shape)
        {
            switch (shape)
            {
                case Circle c when c.Radius > 10:
                    return $"Large circle with radius {c.Radius}";
                case Circle c:
                    return $"Circle with radius {c.Radius}";
                case Rectangle r when r.Width == r.Height:
                    return $"Square with side {r.Width}";
                case Rectangle r:
                    return $"Rectangle {r.Width}x{r.Height}";
                case Triangle tri:
                    return $"Triangle with base {tri.Base} and height {tri.Height}";
                case null:
                    return "No shape (null)";
                default:
                    return $"Unknown shape: {shape.GetType().Name}";
            }
        }

        // Pattern matching with object types
        public static string Classify(object obj)
        {
            switch (obj)
            {
                case int n when n < 0:
                    return $"Negative integer: {n}";
                case int n when n == 0:
                    return "Zero";
                case int n:
                    return $"Positive integer: {n}";
                case double d:
                    return $"Double: {d:F2}";
                case string s when string.IsNullOrEmpty(s):
                    return "Empty string";
                case string s:
                    return $"String: '{s}' (length={s.Length})";
                case bool b:
                    return $"Boolean: {b}";
                case null:
                    return "Null value";
                default:
                    return $"Other type: {obj.GetType().Name}";
            }
        }

        // is-pattern for null checking and type checking
        public static void DemonstrateIsPattern()
        {
            Console.WriteLine("--- is-pattern ---");

            object[] values = { 42, -7, 0, 3.14, "Hello", "", true, null };
            foreach (var v in values)
                Console.WriteLine($"  classify({v ?? "null"}): {Classify(v)}");
        }

        // Pattern matching in expressions (switch expression - C# 8)
        public static string GetDayType(DayOfWeek day) => day switch
        {
            DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
            DayOfWeek.Monday => "Start of week",
            DayOfWeek.Friday => "End of week",
            _ => "Weekday"
        };
    }

    // ===== DECONSTRUCTION =====

    public static class DeconstructionExamples
    {
        public static void Demonstrate()
        {
            Console.WriteLine("--- Deconstruction ---");

            // Tuple deconstruction
            var (min, max, avg) = TupleExamples.GetStatistics(new[] { 1.0, 5.0, 3.0, 8.0, 2.0 });
            Console.WriteLine($"  Min={min}, Max={max}, Avg={avg:F2}");

            // Named tuple
            var stats = TupleExamples.GetStatistics(new[] { 10.0, 20.0, 30.0 });
            Console.WriteLine($"  Named - Min={stats.Min}, Max={stats.Max}, Avg={stats.Average:F2}");

            // DivMod
            var (quotient, remainder) = TupleExamples.DivMod(17, 5);
            Console.WriteLine($"  17 / 5 = {quotient} remainder {remainder}");

            // Custom Deconstruct
            var point = new Point(3.5, 7.2);
            var (x, y) = point;
            Console.WriteLine($"  Point: x={x}, y={y}");

            // Discard with _
            var (_, max2, _) = TupleExamples.GetStatistics(new[] { 5.0, 3.0, 9.0, 1.0 });
            Console.WriteLine($"  Only max needed: {max2}");

            // Tuple swap (no temp variable needed)
            int a = 10, b = 20;
            Console.WriteLine($"  Before swap: a={a}, b={b}");
            (a, b) = (b, a);
            Console.WriteLine($"  After swap: a={a}, b={b}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Day 04: Tuples and Pattern Matching ===\n");

            // Tuples
            Console.WriteLine("--- Named Tuples ---");
            var numbers = new[] { 4.0, 1.0, 7.0, 3.0, 9.0, 2.0 };
            var (min, max, avg) = TupleExamples.GetStatistics(numbers);
            Console.WriteLine($"  Numbers: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"  Min={min}, Max={max}, Avg={avg:F2}");
            Console.WriteLine();

            var employees = new List<Employee>
            {
                new Employee(1, "Alice", "Engineering", 95000m, 5),
                new Employee(2, "Bob", "Marketing", 75000m, 3),
                new Employee(3, "Carol", "Engineering", 110000m, 8),
                new Employee(4, "Dave", "HR", 65000m, 2),
                new Employee(5, "Eve", "Engineering", 120000m, 10)
            };

            Console.WriteLine("--- Top Earners (using tuples) ---");
            var topEarners = TupleExamples.GetTopEarners(employees);
            foreach (var (name, salary, dept) in topEarners)
                Console.WriteLine($"  {name} ({dept}): {salary:C}");
            Console.WriteLine();

            // Deconstruction
            DeconstructionExamples.Demonstrate();
            Console.WriteLine();

            // Pattern Matching
            Console.WriteLine("--- Pattern Matching: Shapes ---");
            var shapes = new Shape[]
            {
                new Circle { Radius = 5 },
                new Circle { Radius = 15 },
                new Rectangle { Width = 4, Height = 4 },
                new Rectangle { Width = 3, Height = 7 },
                new Triangle { Base = 6, Height = 4 }
            };

            foreach (var shape in shapes)
            {
                double area = PatternMatchingExamples.CalculateArea(shape);
                string description = PatternMatchingExamples.DescribeShape(shape);
                Console.WriteLine($"  {description}, Area={area:F2}");
            }
            Console.WriteLine();

            // is-pattern with objects
            PatternMatchingExamples.DemonstrateIsPattern();
            Console.WriteLine();

            // Switch expression (C# 8)
            Console.WriteLine("--- Switch Expression (C# 8) ---");
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                Console.WriteLine($"  {day}: {PatternMatchingExamples.GetDayType(day)}");
        }
    }
}
