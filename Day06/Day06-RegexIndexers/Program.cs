// Day 06 - Regular Expressions and Indexers
// Topics: Regex basics, groups, named groups, lookahead/lookbehind,
//         Replace, Split, Indexers (standard, multi-dimensional, named)

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day06_RegexIndexers
{
    // ===== INDEXERS =====

    // Simple 1D indexer - a typed collection with bounds checking
    public class SafeArray<T>
    {
        private readonly T[] _data;

        public int Length => _data.Length;

        public SafeArray(int size)
        {
            _data = new T[size];
        }

        // Standard indexer
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _data.Length)
                    throw new IndexOutOfRangeException($"Index {index} is out of range [0, {_data.Length - 1}]");
                return _data[index];
            }
            set
            {
                if (index < 0 || index >= _data.Length)
                    throw new IndexOutOfRangeException($"Index {index} is out of range [0, {_data.Length - 1}]");
                _data[index] = value;
            }
        }

        // Range-based indexer (C# 8)
        public T[] this[int start, int end]
        {
            get
            {
                int len = end - start;
                T[] result = new T[len];
                Array.Copy(_data, start, result, 0, len);
                return result;
            }
        }
    }

    // 2D matrix with indexer
    public class Matrix
    {
        private readonly double[,] _data;

        public int Rows { get; }
        public int Columns { get; }

        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _data = new double[rows, columns];
        }

        // Multi-dimensional indexer
        public double this[int row, int col]
        {
            get
            {
                ValidateBounds(row, col);
                return _data[row, col];
            }
            set
            {
                ValidateBounds(row, col);
                _data[row, col] = value;
            }
        }

        private void ValidateBounds(int row, int col)
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Columns)
                throw new IndexOutOfRangeException($"({row},{col}) out of bounds ({Rows}x{Columns})");
        }

        public void Print()
        {
            for (int r = 0; r < Rows; r++)
            {
                Console.Write("  | ");
                for (int c = 0; c < Columns; c++)
                    Console.Write($"{_data[r, c],8:F2} ");
                Console.WriteLine("|");
            }
        }
    }

    // String-keyed indexer (dictionary-like)
    public class Configuration
    {
        private readonly Dictionary<string, string> _settings;

        public Configuration()
        {
            _settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        // String indexer
        public string this[string key]
        {
            get => _settings.TryGetValue(key, out string val) ? val : null;
            set => _settings[key] = value;
        }

        public bool Contains(string key) => _settings.ContainsKey(key);

        public void Print()
        {
            foreach (var (k, v) in _settings)
                Console.WriteLine($"  {k} = {v}");
        }
    }

    // ===== REGEX =====

    public static class RegexExamples
    {
        // Basic matching
        public static void DemonstrateBasicMatching()
        {
            Console.WriteLine("--- Basic Regex Matching ---");

            string text = "The quick brown fox jumps over the lazy dog. FOX!";

            // Simple pattern
            bool hasWord = Regex.IsMatch(text, @"\bfox\b", RegexOptions.IgnoreCase);
            Console.WriteLine($"  Contains 'fox' (case-insensitive): {hasWord}");

            // Find all matches
            MatchCollection matches = Regex.Matches(text, @"\b\w{4}\b");
            Console.Write("  4-letter words: ");
            foreach (Match m in matches)
                Console.Write($"{m.Value} ");
            Console.WriteLine();

            // Word count
            int wordCount = Regex.Matches(text, @"\b\w+\b").Count;
            Console.WriteLine($"  Word count: {wordCount}");
        }

        // Groups and captures
        public static void DemonstrateGroups()
        {
            Console.WriteLine("\n--- Groups and Captures ---");

            // Phone number parsing
            string[] phones = { "(123) 456-7890", "800-555-1234", "123.456.7890", "invalid" };
            string phonePattern = @"^[(\s]*(\d{3})[)\s.-]*(\d{3})[\s.-](\d{4})$";

            foreach (string phone in phones)
            {
                Match m = Regex.Match(phone, phonePattern);
                if (m.Success)
                    Console.WriteLine($"  '{phone}' -> Area: {m.Groups[1].Value}, Exchange: {m.Groups[2].Value}, Number: {m.Groups[3].Value}");
                else
                    Console.WriteLine($"  '{phone}' -> Invalid phone number");
            }

            // Named groups for date parsing
            Console.WriteLine();
            string[] dates = { "2024-01-15", "15/01/2024", "January 15, 2024" };
            string isoDate = @"(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})";

            foreach (string date in dates)
            {
                Match m = Regex.Match(date, isoDate);
                if (m.Success)
                    Console.WriteLine($"  ISO Date: {date} -> Year={m.Groups["year"].Value}, Month={m.Groups["month"].Value}, Day={m.Groups["day"].Value}");
                else
                    Console.WriteLine($"  Not ISO date: {date}");
            }
        }

        // Email validation
        public static void DemonstrateEmailValidation()
        {
            Console.WriteLine("\n--- Email Validation ---");

            // Compiled regex for performance
            var emailRegex = new Regex(
                @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            string[] emails = {
                "user@example.com",
                "john.doe+tag@company.org",
                "invalid@",
                "@nodomain.com",
                "no-at-sign",
                "user@domain.c",  // TLD too short
                "valid.user@sub.domain.co.uk"
            };

            foreach (string email in emails)
            {
                bool valid = emailRegex.IsMatch(email);
                Console.WriteLine($"  {email,-35} -> {(valid ? "Valid" : "Invalid")}");
            }
        }

        // Replace and Transform
        public static void DemonstrateReplace()
        {
            Console.WriteLine("\n--- Replace and Transform ---");

            // Simple replace
            string text = "The price is $10.00 and $25.99 for items.";
            string masked = Regex.Replace(text, @"\$[\d.]+", "[PRICE]");
            Console.WriteLine($"  Masked: {masked}");

            // Replace with MatchEvaluator
            string template = "Hello, {Name}! Your balance is {Balance:C}.";
            var data = new Dictionary<string, string>
            {
                ["Name"] = "Alice",
                ["Balance"] = "1234.56"
            };

            string result = Regex.Replace(template, @"\{(\w+)(?::.*?)?\}",
                m =>
                {
                    string key = m.Groups[1].Value;
                    return data.TryGetValue(key, out string val) ? val : m.Value;
                });
            Console.WriteLine($"  Template result: {result}");

            // CamelCase to snake_case
            string camelCase = "myVariableName isAwesome camelCaseToSnakeCase";
            string snakeCase = Regex.Replace(camelCase, @"(?<=[a-z])([A-Z])", "_$1").ToLower();
            Console.WriteLine($"  CamelCase: {camelCase}");
            Console.WriteLine($"  snake_case: {snakeCase}");

            // Normalize whitespace
            string messy = "This   has   extra    spaces\t\tand\ttabs";
            string clean = Regex.Replace(messy.Trim(), @"\s+", " ");
            Console.WriteLine($"  Cleaned: '{clean}'");
        }

        // Split
        public static void DemonstrateSplit()
        {
            Console.WriteLine("\n--- Split ---");

            // Split by multiple delimiters
            string csv = "one,two;three|four  five";
            string[] parts = Regex.Split(csv, @"[,;|\s]+");
            Console.WriteLine($"  Split '{csv}':");
            Console.Write("    Parts: ");
            foreach (string p in parts)
                Console.Write($"'{p}' ");
            Console.WriteLine();

            // Split keeping delimiters (using capturing group)
            string expr = "10+20-5*3";
            string[] tokens = Regex.Split(expr, @"([+\-*/])");
            Console.Write($"  Tokenize '{expr}': ");
            foreach (string t in tokens)
                Console.Write($"'{t}' ");
            Console.WriteLine();
        }

        // Lookahead and lookbehind
        public static void DemonstrateLookaround()
        {
            Console.WriteLine("\n--- Lookahead / Lookbehind ---");

            // Positive lookahead: match word followed by a digit
            string text = "item1 item2 value3 name product4";
            var matches = Regex.Matches(text, @"\b\w+(?=\d)\b");
            Console.Write("  Words followed by digit: ");
            foreach (Match m in matches)
                Console.Write($"'{m.Value}' ");
            Console.WriteLine();

            // Positive lookbehind: number preceded by $
            string prices = "Item costs $29.99, discount $5.00, total $24.99";
            var priceMatches = Regex.Matches(prices, @"(?<=\$)\d+\.\d{2}");
            Console.Write("  Dollar amounts: ");
            foreach (Match m in priceMatches)
                Console.Write($"{m.Value} ");
            Console.WriteLine();

            // Negative lookahead: log without ERROR
            string[] logs = { "INFO: all good", "ERROR: something failed", "WARN: low memory", "DEBUG: tracing" };
            Console.WriteLine("  Non-error log lines:");
            foreach (string log in logs)
                if (Regex.IsMatch(log, @"^(?!ERROR)"))
                    Console.WriteLine($"    {log}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Day 06: Regular Expressions and Indexers ===\n");

            // Indexers
            Console.WriteLine("--- 1D SafeArray Indexer ---");
            var arr = new SafeArray<string>(5);
            arr[0] = "Apple"; arr[1] = "Banana"; arr[2] = "Cherry";
            arr[3] = "Date"; arr[4] = "Elderberry";
            Console.Write("  Elements: ");
            for (int i = 0; i < arr.Length; i++)
                Console.Write($"{arr[i]} ");
            Console.WriteLine();
            var slice = arr[1, 4];
            Console.Write("  Slice [1,4]: ");
            foreach (var s in slice) Console.Write($"{s} ");
            Console.WriteLine();

            try
            {
                var _ = arr[10];
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"  Caught: {ex.Message}");
            }
            Console.WriteLine();

            Console.WriteLine("--- 2D Matrix Indexer ---");
            var matrix = new Matrix(3, 3);
            // Fill with some values
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    matrix[r, c] = (r + 1) * 10 + (c + 1);
            matrix.Print();
            Console.WriteLine($"  matrix[1,2] = {matrix[1, 2]}");
            Console.WriteLine();

            Console.WriteLine("--- String-Key Configuration Indexer ---");
            var config = new Configuration();
            config["Database"] = "Server=localhost;Database=mydb";
            config["MaxConnections"] = "100";
            config["Timeout"] = "30";
            config["Debug"] = "false";
            config.Print();
            Console.WriteLine($"  database (case-insensitive): {config["database"]}");
            Console.WriteLine($"  Missing key: {config["Missing"] ?? "(null)"}");
            Console.WriteLine();

            // Regex examples
            RegexExamples.DemonstrateBasicMatching();
            RegexExamples.DemonstrateGroups();
            RegexExamples.DemonstrateEmailValidation();
            RegexExamples.DemonstrateReplace();
            RegexExamples.DemonstrateSplit();
            RegexExamples.DemonstrateLookaround();
        }
    }
}
