// Day 02 - Object-Oriented Programming (OOP) Concepts
// Topics: Classes, Objects, Inheritance, Interfaces, Polymorphism, Encapsulation, Abstraction

using System;
using System.Collections.Generic;

namespace Day02_OOPConcepts
{
    // ===== ENCAPSULATION =====
    // A class with private fields and public properties
    public class BankAccount
    {
        private decimal _balance;
        private readonly string _accountNumber;
        private List<string> _transactions;

        // Auto-implemented property
        public string OwnerName { get; set; }

        // Read-only property
        public string AccountNumber => _accountNumber;

        // Property with validation
        public decimal Balance
        {
            get => _balance;
            private set
            {
                if (value < 0)
                    throw new InvalidOperationException("Balance cannot be negative.");
                _balance = value;
            }
        }

        // Constructor
        public BankAccount(string ownerName, string accountNumber, decimal initialBalance = 0)
        {
            OwnerName = ownerName;
            _accountNumber = accountNumber;
            _balance = initialBalance;
            _transactions = new List<string>();
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit amount must be positive.");
            Balance += amount;
            _transactions.Add($"Deposit: +{amount:C}");
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.");
            if (amount > Balance) return false;
            Balance -= amount;
            _transactions.Add($"Withdrawal: -{amount:C}");
            return true;
        }

        public void PrintStatement()
        {
            Console.WriteLine($"\nAccount Statement for {OwnerName} ({_accountNumber})");
            foreach (var t in _transactions)
                Console.WriteLine($"  {t}");
            Console.WriteLine($"  Current Balance: {Balance:C}");
        }
    }

    // ===== INHERITANCE =====
    // Base class
    public abstract class Shape
    {
        public string Color { get; set; }

        protected Shape(string color = "White")
        {
            Color = color;
        }

        // Abstract method - must be implemented by derived classes
        public abstract double Area();

        // Virtual method - can be overridden
        public virtual double Perimeter() => 0;

        // Template method pattern
        public void Describe()
        {
            Console.WriteLine($"{GetType().Name}: Color={Color}, Area={Area():F2}, Perimeter={Perimeter():F2}");
        }
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }

        public Circle(double radius, string color = "Red") : base(color)
        {
            Radius = radius;
        }

        public override double Area() => Math.PI * Radius * Radius;
        public override double Perimeter() => 2 * Math.PI * Radius;
    }

    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height, string color = "Blue") : base(color)
        {
            Width = width;
            Height = height;
        }

        public override double Area() => Width * Height;
        public override double Perimeter() => 2 * (Width + Height);
    }

    public class Triangle : Shape
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public Triangle(double a, double b, double c, string color = "Green") : base(color)
        {
            A = a; B = b; C = c;
        }

        public override double Area()
        {
            double s = Perimeter() / 2;
            return Math.Sqrt(s * (s - A) * (s - B) * (s - C));
        }

        public override double Perimeter() => A + B + C;
    }

    // ===== INTERFACES =====
    public interface IAnimal
    {
        string Name { get; }
        string Sound { get; }
        void Speak();
    }

    public interface ITrainable
    {
        bool IsTrained { get; set; }
        void Train(string command);
    }

    public abstract class Animal : IAnimal
    {
        public string Name { get; }
        public abstract string Sound { get; }

        protected Animal(string name)
        {
            Name = name;
        }

        public virtual void Speak()
        {
            Console.WriteLine($"{Name} says: {Sound}");
        }
    }

    public class Dog : Animal, ITrainable
    {
        public bool IsTrained { get; set; }
        public string Breed { get; }
        public override string Sound => "Woof!";

        public Dog(string name, string breed) : base(name)
        {
            Breed = breed;
        }

        public void Train(string command)
        {
            IsTrained = true;
            Console.WriteLine($"{Name} learned: '{command}'");
        }

        public override void Speak()
        {
            base.Speak();
            if (IsTrained)
                Console.WriteLine($"  ({Name} is a trained {Breed})");
        }
    }

    public class Cat : Animal
    {
        public bool IsIndoor { get; }
        public override string Sound => "Meow!";

        public Cat(string name, bool isIndoor = true) : base(name)
        {
            IsIndoor = isIndoor;
        }
    }

    // ===== GENERICS =====
    public class GenericStack<T>
    {
        private List<T> _items = new List<T>();

        public int Count => _items.Count;
        public bool IsEmpty => _items.Count == 0;

        public void Push(T item)
        {
            _items.Add(item);
        }

        public T Pop()
        {
            if (IsEmpty) throw new InvalidOperationException("Stack is empty.");
            var item = _items[_items.Count - 1];
            _items.RemoveAt(_items.Count - 1);
            return item;
        }

        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Stack is empty.");
            return _items[_items.Count - 1];
        }
    }

    // ===== STATIC CLASSES AND MEMBERS =====
    public static class MathHelper
    {
        public static int Factorial(int n)
        {
            if (n < 0) throw new ArgumentException("n must be non-negative.");
            if (n <= 1) return 1;
            return n * Factorial(n - 1);
        }

        public static bool IsPrime(int n)
        {
            if (n < 2) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++)
                if (n % i == 0) return false;
            return true;
        }

        public static IEnumerable<int> Fibonacci(int count)
        {
            int a = 0, b = 1;
            for (int i = 0; i < count; i++)
            {
                yield return a;
                (a, b) = (b, a + b);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Day 02: OOP Concepts ===\n");

            // Encapsulation
            Console.WriteLine("--- Encapsulation: Bank Account ---");
            var account = new BankAccount("Alice Johnson", "ACC-001", 1000m);
            account.Deposit(500m);
            account.Withdraw(200m);
            account.Withdraw(2000m);  // Will fail silently
            account.PrintStatement();
            Console.WriteLine();

            // Inheritance and Polymorphism
            Console.WriteLine("--- Inheritance & Polymorphism: Shapes ---");
            var shapes = new List<Shape>
            {
                new Circle(5, "Red"),
                new Rectangle(4, 6, "Blue"),
                new Triangle(3, 4, 5, "Green")
            };
            foreach (var shape in shapes)
                shape.Describe();
            Console.WriteLine();

            // Interfaces
            Console.WriteLine("--- Interfaces: Animals ---");
            var dog = new Dog("Rex", "German Shepherd");
            var cat = new Cat("Whiskers");
            dog.Train("Sit");
            dog.Train("Fetch");
            dog.Speak();
            cat.Speak();
            Console.WriteLine();

            // Generics
            Console.WriteLine("--- Generics: Stack ---");
            var intStack = new GenericStack<int>();
            intStack.Push(1);
            intStack.Push(2);
            intStack.Push(3);
            Console.WriteLine($"Stack count: {intStack.Count}, Top: {intStack.Peek()}");
            Console.WriteLine($"Popped: {intStack.Pop()}, {intStack.Pop()}");
            Console.WriteLine($"Remaining count: {intStack.Count}");
            Console.WriteLine();

            // Static utility class
            Console.WriteLine("--- Static Utilities ---");
            Console.WriteLine($"Factorial(6) = {MathHelper.Factorial(6)}");
            Console.Write("Primes up to 20: ");
            for (int i = 2; i <= 20; i++)
                if (MathHelper.IsPrime(i)) Console.Write($"{i} ");
            Console.WriteLine();
            Console.Write("Fibonacci(8): ");
            foreach (var f in MathHelper.Fibonacci(8))
                Console.Write($"{f} ");
            Console.WriteLine();
        }
    }
}
