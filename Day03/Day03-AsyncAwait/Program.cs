// Day 03 - Async/Await and Async Main (C# 7.1+)
// Topics: async/await, Task, Task<T>, async Main, exception handling in async code,
//         parallel tasks, cancellation, progress reporting

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Day03_AsyncAwait
{
    // Simulates a data service with async operations
    public class DataService
    {
        // Async method returning Task<T>
        public async Task<List<string>> GetProductsAsync(CancellationToken ct = default)
        {
            Console.WriteLine("  [DataService] Fetching products...");
            await Task.Delay(500, ct);  // Simulates network I/O
            return new List<string> { "Laptop", "Mouse", "Keyboard", "Monitor", "Headset" };
        }

        // Async method returning Task (no return value)
        public async Task SaveOrderAsync(string product, int quantity, CancellationToken ct = default)
        {
            Console.WriteLine($"  [DataService] Saving order: {quantity}x {product}...");
            await Task.Delay(300, ct);
            Console.WriteLine($"  [DataService] Order saved: {quantity}x {product}");
        }

        // Async method that may throw
        public async Task<decimal> GetPriceAsync(string product)
        {
            await Task.Delay(200);
            var prices = new Dictionary<string, decimal>
            {
                ["Laptop"] = 999.99m,
                ["Mouse"] = 29.99m,
                ["Keyboard"] = 79.99m,
                ["Monitor"] = 349.99m,
                ["Headset"] = 149.99m
            };

            if (!prices.TryGetValue(product, out decimal price))
                throw new KeyNotFoundException($"Price not found for: {product}");

            return price;
        }
    }

    // Demonstrates async patterns
    public class OrderProcessor
    {
        private readonly DataService _service;

        public OrderProcessor(DataService service)
        {
            _service = service;
        }

        // Sequential async calls
        public async Task ProcessOrderSequentialAsync()
        {
            Console.WriteLine("\n--- Sequential Async Calls ---");
            var sw = System.Diagnostics.Stopwatch.StartNew();

            var products = await _service.GetProductsAsync();
            Console.WriteLine($"  Products: {string.Join(", ", products)}");

            // Process one by one (sequential)
            foreach (var product in products.Take(3))
            {
                decimal price = await _service.GetPriceAsync(product);
                Console.WriteLine($"  {product}: {price:C}");
            }

            sw.Stop();
            Console.WriteLine($"  Sequential time: {sw.ElapsedMilliseconds}ms");
        }

        // Parallel async calls using Task.WhenAll
        public async Task ProcessOrderParallelAsync()
        {
            Console.WriteLine("\n--- Parallel Async Calls (Task.WhenAll) ---");
            var sw = System.Diagnostics.Stopwatch.StartNew();

            var products = await _service.GetProductsAsync();

            // Start all price fetches simultaneously
            var priceTasks = new Dictionary<string, Task<decimal>>();
            foreach (var product in products)
                priceTasks[product] = _service.GetPriceAsync(product);

            // Wait for all to complete
            await Task.WhenAll(priceTasks.Values);

            decimal total = 0;
            foreach (var (product, task) in priceTasks)
            {
                Console.WriteLine($"  {product}: {task.Result:C}");
                total += task.Result;
            }
            Console.WriteLine($"  Total: {total:C}");

            sw.Stop();
            Console.WriteLine($"  Parallel time: {sw.ElapsedMilliseconds}ms");
        }

        // Task.WhenAny - first to complete wins
        public async Task DemonstrateWhenAnyAsync()
        {
            Console.WriteLine("\n--- Task.WhenAny (First to Complete) ---");

            var tasks = new List<Task<decimal>>
            {
                Task.Run(async () => { await Task.Delay(300); return 1.0m; }),
                Task.Run(async () => { await Task.Delay(100); return 2.0m; }),
                Task.Run(async () => { await Task.Delay(200); return 3.0m; }),
            };

            var firstTask = await Task.WhenAny(tasks);
            Console.WriteLine($"  First completed with value: {firstTask.Result}");
        }

        // Async error handling
        public async Task DemonstrateExceptionHandlingAsync()
        {
            Console.WriteLine("\n--- Async Exception Handling ---");

            // Single exception
            try
            {
                decimal price = await _service.GetPriceAsync("UnknownProduct");
                Console.WriteLine($"  Price: {price}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"  Caught: {ex.Message}");
            }

            // AggregateException from Task.WhenAll
            var tasks = new[]
            {
                _service.GetPriceAsync("Laptop"),
                _service.GetPriceAsync("BadProduct1"),
                _service.GetPriceAsync("BadProduct2"),
            };

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                // All exceptions are captured in the tasks
                foreach (var task in tasks)
                {
                    if (task.IsFaulted)
                        Console.WriteLine($"  Task failed: {task.Exception?.InnerException?.Message}");
                }
            }
        }

        // Cancellation token example
        public async Task DemonstrateCancellationAsync()
        {
            Console.WriteLine("\n--- Cancellation Token ---");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(400));

            try
            {
                // This will be cancelled after 400ms
                for (int i = 0; i < 5; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"  Processing item {i + 1}...");
                    await Task.Delay(150, cts.Token);
                }
                Console.WriteLine("  Completed without cancellation.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  Operation was cancelled!");
            }
        }
    }

    class Program
    {
        // C# 7.1+ feature: async Main
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Day 03: Async/Await and Async Main ===\n");

            var service = new DataService();
            var processor = new OrderProcessor(service);

            // Basic async/await
            Console.WriteLine("--- Basic Async/Await ---");
            var products = await service.GetProductsAsync();
            Console.WriteLine($"  Fetched {products.Count} products: {string.Join(", ", products)}");

            await service.SaveOrderAsync("Laptop", 2);

            // Sequential vs Parallel
            await processor.ProcessOrderSequentialAsync();
            await processor.ProcessOrderParallelAsync();

            // WhenAny
            await processor.DemonstrateWhenAnyAsync();

            // Exception handling
            await processor.DemonstrateExceptionHandlingAsync();

            // Cancellation
            await processor.DemonstrateCancellationAsync();

            // ConfigureAwait
            Console.WriteLine("\n--- ConfigureAwait ---");
            await DemonstrateConfigureAwait();

            Console.WriteLine("\n=== Async Main completed! ===");
        }

        static async Task DemonstrateConfigureAwait()
        {
            // ConfigureAwait(false) avoids capturing synchronization context
            // Useful in library code for performance
            await Task.Delay(100).ConfigureAwait(false);
            Console.WriteLine("  ConfigureAwait(false) used - good practice in library code.");

            // ValueTask for hot paths that often complete synchronously
            var result = await GetCachedValueAsync("key");
            Console.WriteLine($"  ValueTask result: {result}");
        }

        static ValueTask<string> GetCachedValueAsync(string key)
        {
            // If the value is cached, return synchronously (no allocation)
            if (key == "key")
                return new ValueTask<string>("Cached Value");

            // Otherwise, do async work
            return new ValueTask<string>(FetchValueAsync(key));
        }

        static async Task<string> FetchValueAsync(string key)
        {
            await Task.Delay(100);
            return $"Fetched: {key}";
        }
    }

    // Extension method for LINQ-like Take on List
    public static class ListExtensions
    {
        public static IEnumerable<T> Take<T>(this List<T> list, int count)
        {
            for (int i = 0; i < Math.Min(count, list.Count); i++)
                yield return list[i];
        }
    }
}
