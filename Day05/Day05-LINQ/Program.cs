// Day 05 - LINQ (Language Integrated Query)
// Topics: Query syntax, method syntax, filtering, projection, ordering, grouping,
//         aggregation, joins, set operations, deferred execution

using System;
using System.Collections.Generic;
using System.Linq;

namespace Day05_LINQ
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }

        public override string ToString() =>
            $"[{Id}] {Name} ({Category}) - {Price:C}, Stock: {Stock}";
    }

    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }

    public static class SampleData
    {
        public static List<Product> GetProducts() => new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 999.99m, Stock = 50, IsActive = true },
            new Product { Id = 2, Name = "Mouse", Category = "Electronics", Price = 29.99m, Stock = 200, IsActive = true },
            new Product { Id = 3, Name = "Keyboard", Category = "Electronics", Price = 79.99m, Stock = 150, IsActive = true },
            new Product { Id = 4, Name = "Monitor", Category = "Electronics", Price = 349.99m, Stock = 75, IsActive = true },
            new Product { Id = 5, Name = "Desk Chair", Category = "Furniture", Price = 299.99m, Stock = 30, IsActive = true },
            new Product { Id = 6, Name = "Standing Desk", Category = "Furniture", Price = 549.99m, Stock = 20, IsActive = true },
            new Product { Id = 7, Name = "Notebook", Category = "Stationery", Price = 4.99m, Stock = 500, IsActive = true },
            new Product { Id = 8, Name = "Pen Set", Category = "Stationery", Price = 9.99m, Stock = 300, IsActive = false },
            new Product { Id = 9, Name = "Webcam", Category = "Electronics", Price = 89.99m, Stock = 100, IsActive = true },
            new Product { Id = 10, Name = "USB Hub", Category = "Electronics", Price = 39.99m, Stock = 0, IsActive = true },
        };

        public static List<Customer> GetCustomers() => new List<Customer>
        {
            new Customer { Id = 1, Name = "Alice Johnson", City = "New York" },
            new Customer { Id = 2, Name = "Bob Smith", City = "Chicago" },
            new Customer { Id = 3, Name = "Carol Williams", City = "New York" },
            new Customer { Id = 4, Name = "Dave Brown", City = "Los Angeles" },
        };

        public static List<Order> GetOrders() => new List<Order>
        {
            new Order { Id = 1, ProductId = 1, CustomerId = 1, Quantity = 1, OrderDate = new DateTime(2024, 1, 15) },
            new Order { Id = 2, ProductId = 2, CustomerId = 1, Quantity = 2, OrderDate = new DateTime(2024, 1, 20) },
            new Order { Id = 3, ProductId = 3, CustomerId = 2, Quantity = 1, OrderDate = new DateTime(2024, 2, 5) },
            new Order { Id = 4, ProductId = 1, CustomerId = 3, Quantity = 1, OrderDate = new DateTime(2024, 2, 10) },
            new Order { Id = 5, ProductId = 5, CustomerId = 4, Quantity = 2, OrderDate = new DateTime(2024, 3, 1) },
            new Order { Id = 6, ProductId = 9, CustomerId = 2, Quantity = 1, OrderDate = new DateTime(2024, 3, 15) },
        };
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Day 05: LINQ ===\n");

            var products = SampleData.GetProducts();
            var customers = SampleData.GetCustomers();
            var orders = SampleData.GetOrders();

            DemonstrateFiltering(products);
            DemonstrateProjection(products);
            DemonstrateOrdering(products);
            DemonstrateGrouping(products);
            DemonstrateAggregation(products);
            DemonstrateJoins(products, orders, customers);
            DemonstrateSetOperations();
            DemonstrateDeferredExecution(products);
            DemonstrateQuerySyntax(products);
        }

        // WHERE - filtering
        static void DemonstrateFiltering(List<Product> products)
        {
            Console.WriteLine("--- Filtering (Where) ---");

            // Active electronics in stock
            var activeElectronics = products
                .Where(p => p.IsActive && p.Category == "Electronics" && p.Stock > 0)
                .ToList();

            Console.WriteLine("Active Electronics in stock:");
            activeElectronics.ForEach(p => Console.WriteLine($"  {p}"));

            // Price range filter
            var midRange = products.Where(p => p.Price >= 50 && p.Price <= 200);
            Console.WriteLine($"\nMid-range products (50-200): {midRange.Count()}");
            foreach (var p in midRange)
                Console.WriteLine($"  {p.Name}: {p.Price:C}");
            Console.WriteLine();
        }

        // SELECT - projection
        static void DemonstrateProjection(List<Product> products)
        {
            Console.WriteLine("--- Projection (Select) ---");

            // Project to anonymous type
            var summary = products
                .Where(p => p.IsActive)
                .Select(p => new
                {
                    p.Name,
                    p.Category,
                    p.Price,
                    StockValue = p.Price * p.Stock
                })
                .OrderByDescending(p => p.StockValue)
                .Take(5);

            Console.WriteLine("Top 5 by stock value:");
            foreach (var item in summary)
                Console.WriteLine($"  {item.Name} ({item.Category}): {item.Price:C} x {item.StockValue / item.Price:F0} = {item.StockValue:C}");

            // SelectMany - flatten
            var categories = new List<List<string>>
            {
                new List<string> { "Electronics", "Gadgets" },
                new List<string> { "Furniture", "Office" },
                new List<string> { "Stationery" }
            };
            var allCategories = categories.SelectMany(c => c).ToList();
            Console.WriteLine($"\nFlattened categories: {string.Join(", ", allCategories)}");
            Console.WriteLine();
        }

        // ORDERBY - ordering
        static void DemonstrateOrdering(List<Product> products)
        {
            Console.WriteLine("--- Ordering (OrderBy) ---");

            var ordered = products
                .OrderBy(p => p.Category)
                .ThenByDescending(p => p.Price)
                .Select(p => $"  {p.Category,-15} {p.Name,-20} {p.Price:C}");

            Console.WriteLine("Products ordered by category, then price desc:");
            foreach (var line in ordered)
                Console.WriteLine(line);
            Console.WriteLine();
        }

        // GROUPBY - grouping
        static void DemonstrateGrouping(List<Product> products)
        {
            Console.WriteLine("--- Grouping (GroupBy) ---");

            var byCategory = products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count(),
                    TotalValue = g.Sum(p => p.Price * p.Stock),
                    AveragePrice = g.Average(p => p.Price)
                })
                .OrderByDescending(g => g.TotalValue);

            Console.WriteLine("Category Summary:");
            foreach (var cat in byCategory)
            {
                Console.WriteLine($"  {cat.Category,-15} Count={cat.Count}, " +
                    $"Avg Price={cat.AveragePrice:C}, Total Value={cat.TotalValue:C}");
            }
            Console.WriteLine();
        }

        // Aggregation operations
        static void DemonstrateAggregation(List<Product> products)
        {
            Console.WriteLine("--- Aggregation ---");

            var active = products.Where(p => p.IsActive && p.Stock > 0);

            Console.WriteLine($"  Count (active, in stock): {active.Count()}");
            Console.WriteLine($"  Min price: {active.Min(p => p.Price):C}");
            Console.WriteLine($"  Max price: {active.Max(p => p.Price):C}");
            Console.WriteLine($"  Average price: {active.Average(p => p.Price):C}");
            Console.WriteLine($"  Total inventory value: {active.Sum(p => p.Price * p.Stock):C}");

            // Any and All
            Console.WriteLine($"  Any price > $500? {products.Any(p => p.Price > 500)}");
            Console.WriteLine($"  All active? {products.All(p => p.IsActive)}");
            Console.WriteLine($"  All active? (active only) {active.All(p => p.IsActive)}");

            // First, Last, Single
            var cheapest = products.OrderBy(p => p.Price).First();
            var mostExpensive = products.OrderBy(p => p.Price).Last();
            Console.WriteLine($"  Cheapest: {cheapest.Name} ({cheapest.Price:C})");
            Console.WriteLine($"  Most expensive: {mostExpensive.Name} ({mostExpensive.Price:C})");

            // FirstOrDefault with predicate
            var outOfStock = products.FirstOrDefault(p => p.Stock == 0);
            Console.WriteLine($"  First out of stock: {outOfStock?.Name ?? "None"}");

            // Aggregate (custom accumulator)
            string productList = products.Take(4).Aggregate("Products: ",
                (acc, p) => acc + p.Name + ", ",
                result => result.TrimEnd(' ', ','));
            Console.WriteLine($"  {productList}");
            Console.WriteLine();
        }

        // JOIN operations
        static void DemonstrateJoins(List<Product> products, List<Order> orders, List<Customer> customers)
        {
            Console.WriteLine("--- Joins ---");

            // Inner join
            var orderDetails = orders
                .Join(products,
                    o => o.ProductId,
                    p => p.Id,
                    (o, p) => new { o.Id, CustomerID = o.CustomerId, p.Name, p.Price, o.Quantity, o.OrderDate })
                .Join(customers,
                    od => od.CustomerID,
                    c => c.Id,
                    (od, c) => new
                    {
                        od.Id,
                        Customer = c.Name,
                        Product = od.Name,
                        od.Quantity,
                        Total = od.Price * od.Quantity,
                        od.OrderDate
                    });

            Console.WriteLine("Order Details:");
            foreach (var order in orderDetails.OrderBy(o => o.OrderDate))
            {
                Console.WriteLine($"  Order #{order.Id}: {order.Customer} bought {order.Quantity}x {order.Product} = {order.Total:C} on {order.OrderDate:d}");
            }

            // Left join (GroupJoin)
            var customersWithOrders = customers
                .GroupJoin(orders,
                    c => c.Id,
                    o => o.CustomerId,
                    (c, orderGroup) => new
                    {
                        Customer = c.Name,
                        OrderCount = orderGroup.Count(),
                        TotalSpent = orderGroup.Join(products, o => o.ProductId, p => p.Id,
                            (o, p) => o.Quantity * p.Price).Sum()
                    });

            Console.WriteLine("\nCustomer Order Summary (including those with no orders):");
            foreach (var c in customersWithOrders)
                Console.WriteLine($"  {c.Customer}: {c.OrderCount} orders, Total: {c.TotalSpent:C}");
            Console.WriteLine();
        }

        // Set operations
        static void DemonstrateSetOperations()
        {
            Console.WriteLine("--- Set Operations ---");

            var set1 = new[] { 1, 2, 3, 4, 5, 6 };
            var set2 = new[] { 4, 5, 6, 7, 8, 9 };

            Console.WriteLine($"Set1: [{string.Join(", ", set1)}]");
            Console.WriteLine($"Set2: [{string.Join(", ", set2)}]");
            Console.WriteLine($"Union: [{string.Join(", ", set1.Union(set2))}]");
            Console.WriteLine($"Intersect: [{string.Join(", ", set1.Intersect(set2))}]");
            Console.WriteLine($"Except (1 - 2): [{string.Join(", ", set1.Except(set2))}]");

            var withDups = new[] { 1, 2, 2, 3, 3, 3, 4 };
            Console.WriteLine($"Distinct: [{string.Join(", ", withDups.Distinct())}]");
            Console.WriteLine();
        }

        // Deferred execution
        static void DemonstrateDeferredExecution(List<Product> products)
        {
            Console.WriteLine("--- Deferred Execution ---");

            // Query is not executed until enumerated
            var query = products.Where(p =>
            {
                // This won't print until we enumerate
                return p.Price > 100;
            });

            Console.WriteLine("Query defined but not executed yet.");

            // Adding a product after query definition
            products.Add(new Product { Id = 99, Name = "Test Product", Category = "Test",
                Price = 200m, Stock = 1, IsActive = true });

            // Now execute - includes the newly added product
            int count = query.Count();
            Console.WriteLine($"After adding product, count of products over $100: {count}");

            // Remove the test product
            products.RemoveAll(p => p.Id == 99);

            // ToList/ToArray forces immediate execution
            var snapshot = products.Where(p => p.Price > 100).ToList();
            products.Add(new Product { Id = 98, Name = "Another Test", Category = "Test",
                Price = 200m, Stock = 1, IsActive = true });
            Console.WriteLine($"Snapshot count (won't include new product): {snapshot.Count}");
            products.RemoveAll(p => p.Id == 98);
            Console.WriteLine();
        }

        // Query syntax (alternative to method syntax)
        static void DemonstrateQuerySyntax(List<Product> products)
        {
            Console.WriteLine("--- Query Syntax ---");

            // LINQ query syntax (SQL-like)
            var query =
                from p in products
                where p.IsActive && p.Category == "Electronics"
                orderby p.Price descending
                select new { p.Name, p.Price };

            Console.WriteLine("Electronics (query syntax, ordered by price desc):");
            foreach (var item in query)
                Console.WriteLine($"  {item.Name}: {item.Price:C}");

            // Group query syntax
            var groupQuery =
                from p in products
                group p by p.Category into g
                orderby g.Key
                select new { Category = g.Key, Products = g.ToList() };

            Console.WriteLine("\nGrouped (query syntax):");
            foreach (var group in groupQuery)
            {
                Console.WriteLine($"  {group.Category}:");
                foreach (var p in group.Products)
                    Console.WriteLine($"    - {p.Name}: {p.Price:C}");
            }
        }
    }
}
