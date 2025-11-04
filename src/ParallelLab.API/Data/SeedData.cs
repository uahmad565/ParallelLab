using Microsoft.EntityFrameworkCore;
using ParallelLab.Core.Entities;
using ParallelLab.Infrastructure.Data;

namespace ParallelLab.API.Data;

public static class SeedData
{
    public static async Task SeedAsync(ParallelLabDbContext context)
    {
        if (await context.Exercises.AnyAsync())
            return;

        var exercises = new List<Exercise>
        {
            new Exercise
            {
                Title = "Hello World Console App",
                Description = "Create a simple console application that reads input and outputs it",
                ProblemStatement = @"Create a C# console application that:
1. Reads a line from Console input
2. Outputs that line to Console

This is a basic test to verify your code execution environment works correctly.",
                StartingCode = @"using System;

class Solution 
{ 
    static void Main()
    { 
        // TODO: Read from Console and write to Console
    } 
}",
                IdealSolution = @"using System;

class Solution 
{ 
    static void Main()
    { 
        Console.WriteLine(Console.ReadLine()); 
    } 
}",
                TestData = "",
                Category = ExerciseCategory.Threads,
                Difficulty = DifficultyLevel.Beginner,
                ExpectedExecutionTimeMs = 50,
                MaxExecutionTimeMs = 1000,
                TestCases = new List<TestCase>
                {
                    new TestCase
                    {
                        Input = "Hello World",
                        ExpectedOutput = "Hello World",
                        TimeoutMs = 1000,
                        IdealExecutionTimeMs = 50,
                        IsHidden = false,
                        Order = 1
                    },
                    new TestCase
                    {
                        Input = "Test 123",
                        ExpectedOutput = "Test 123",
                        TimeoutMs = 1000,
                        IdealExecutionTimeMs = 50,
                        IsHidden = false,
                        Order = 2
                    },
                    new TestCase
                    {
                        Input = "Parallel Programming",
                        ExpectedOutput = "Parallel Programming",
                        TimeoutMs = 1000,
                        IdealExecutionTimeMs = 50,
                        IsHidden = true,
                        Order = 3
                    }
                }
            },
            new Exercise
            {
                Title = "Parallel Array Processing with Threads",
                Description = "Process a large array using multiple threads to improve performance",
                ProblemStatement = @"You are given a large array of integers. Your task is to calculate the sum of squares of all even numbers in the array using multiple threads.

Requirements:
- Use Thread class to create multiple worker threads
- Divide the array work among threads
- Each thread should process a portion of the array
- Combine results from all threads
- Return the total sum

The array will contain 1,000,000 integers, and you should use 4 threads for processing.",
                StartingCode = @"// Complete this method to process the array using multiple threads
public static long CalculateSumOfSquaresOfEvens(int[] numbers)
{
    // TODO: Implement parallel processing using Threads
    // Divide the work among 4 threads
    // Each thread should process numbers.Length / 4 elements
    // Combine results from all threads
    
    return 0; // Replace with actual implementation
}

// Test data will be provided as: int[] numbers = {1, 2, 3, 4, 5, ...};
// Call: var result = CalculateSumOfSquaresOfEvens(numbers);
//       output.AppendLine($""Result: {{result}}"");",
                IdealSolution = @"public static long CalculateSumOfSquaresOfEvens(int[] numbers)
{
    const int threadCount = 4;
    var results = new long[threadCount];
    var threads = new Thread[threadCount];
    
    int chunkSize = numbers.Length / threadCount;
    
    for (int i = 0; i < threadCount; i++)
    {
        int startIndex = i * chunkSize;
        int endIndex = (i == threadCount - 1) ? numbers.Length : (i + 1) * chunkSize;
        int threadIndex = i;
        
        threads[i] = new Thread(() =>
        {
            long sum = 0;
            for (int j = startIndex; j < endIndex; j++)
            {
                if (numbers[j] % 2 == 0)
                {
                    sum += (long)numbers[j] * numbers[j];
                }
            }
            results[threadIndex] = sum;
        });
        
        threads[i].Start();
    }
    
    foreach (var thread in threads)
    {
        thread.Join();
    }
    
    return results.Sum();
}

// Test execution
var numbers = new int[] {{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}};
var result = CalculateSumOfSquaresOfEvens(numbers);
output.AppendLine($""Result: {{result}}"");",
                TestData = "GenerateArray(1000000)",
                Category = ExerciseCategory.Threads,
                Difficulty = DifficultyLevel.Intermediate,
                ExpectedExecutionTimeMs = 150,
                MaxExecutionTimeMs = 5000,
                IsActive = true
            },
            
            new Exercise
            {
                Title = "Async File Processing with Tasks",
                Description = "Process multiple files asynchronously using Task.Run and async/await",
                ProblemStatement = @"You need to process multiple text files concurrently. For each file, count the number of lines and words.

Requirements:
- Use Task.Run to process files in parallel
- Use async/await pattern
- Process all files concurrently
- Return a dictionary with filename as key and (lineCount, wordCount) as value
- Handle potential file I/O exceptions

You will be given an array of file paths to process.",
                StartingCode = @"// Complete this method to process files asynchronously
public static async Task<Dictionary<string, (int lines, int words)>> ProcessFilesAsync(string[] filePaths)
{
    // TODO: Implement async file processing using Task.Run
    // Process all files concurrently
    // Return dictionary with results
    
    return new Dictionary<string, (int, int)>();
}

// Test data will be provided as: string[] filePaths = {""file1.txt"", ""file2.txt"", ...};
// Call: await ProcessFilesAsync(filePaths);",
                IdealSolution = @"public static async Task<Dictionary<string, (int lines, int words)>> ProcessFilesAsync(string[] filePaths)
{
    var tasks = filePaths.Select(async filePath =>
    {
        return await Task.Run(async () =>
        {
            try
            {
                var content = await File.ReadAllTextAsync(filePath);
                var lines = content.Split('\n').Length;
                var words = content.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                return new { FilePath = filePath, Lines = lines, Words = words };
            }
            catch (Exception)
            {
                return new { FilePath = filePath, Lines = 0, Words = 0 };
            }
        });
    });
    
    var results = await Task.WhenAll(tasks);
    
    return results.ToDictionary(r => r.FilePath, r => (r.Lines, r.Words));
}",
                TestData = "CreateTestFiles(10)",
                Category = ExerciseCategory.Tasks,
                Difficulty = DifficultyLevel.Intermediate,
                ExpectedExecutionTimeMs = 200,
                MaxExecutionTimeMs = 10000,
                IsActive = true
            },
            
            new Exercise
            {
                Title = "Parallel LINQ Data Processing",
                Description = "Use PLINQ to process a large dataset efficiently",
                ProblemStatement = @"You have a collection of Person objects with Name, Age, and Salary properties. Process this data using PLINQ to:

1. Find the average salary by age group (20-30, 31-40, 41-50, 51+)
2. Get the top 10 highest earners
3. Count people in each age group

Requirements:
- Use AsParallel() for parallel processing
- Use appropriate PLINQ operations (GroupBy, OrderBy, etc.)
- Return results in a structured format
- Handle potential null values",
                StartingCode = @"public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
}

// Complete this method using PLINQ
public static object ProcessPersonData(List<Person> people)
{
    // TODO: Implement PLINQ processing
    // 1. Average salary by age group
    // 2. Top 10 highest earners
    // 3. Count by age group
    
    return new { };
}

// Test data will be provided as: List<Person> people = GeneratePeople(100000);
// Call: ProcessPersonData(people);",
                IdealSolution = @"public static object ProcessPersonData(List<Person> people)
{
    var result = people.AsParallel()
        .Where(p => p != null)
        .GroupBy(p => p.Age switch
        {
            >= 20 and <= 30 => ""20-30"",
            >= 31 and <= 40 => ""31-40"",
            >= 41 and <= 50 => ""41-50"",
            _ => ""51+""
        })
        .Select(g => new
        {
            AgeGroup = g.Key,
            AverageSalary = g.Average(p => p.Salary),
            Count = g.Count()
        })
        .ToList();
    
    var topEarners = people.AsParallel()
        .Where(p => p != null)
        .OrderByDescending(p => p.Salary)
        .Take(10)
        .Select(p => new { p.Name, p.Salary })
        .ToList();
    
    return new
    {
        AverageSalaryByAgeGroup = result,
        TopEarners = topEarners
    };
}",
                TestData = "GeneratePeople(100000)",
                Category = ExerciseCategory.LINQ,
                Difficulty = DifficultyLevel.Advanced,
                ExpectedExecutionTimeMs = 100,
                MaxExecutionTimeMs = 3000,
                IsActive = true
            },
            
            new Exercise
            {
                Title = "Parallel For Loop with Custom Partitioning",
                Description = "Use Parallel.For with custom partitioning for optimal performance",
                ProblemStatement = @"You need to process a 2D array where each cell requires intensive computation. Use Parallel.For with custom partitioning to optimize performance.

Requirements:
- Use Parallel.For for the outer loop
- Implement custom partitioning strategy
- Each iteration should process a row of the 2D array
- Calculate the sum of squares for each row
- Return the total sum

The array will be 1000x1000 integers.",
                StartingCode = @"// Complete this method using Parallel.For with custom partitioning
public static long Process2DArray(int[,] matrix)
{
    // TODO: Implement Parallel.For with custom partitioning
    // Process each row in parallel
    // Calculate sum of squares for each row
    // Return total sum
    
    return 0;
}

// Test data will be provided as: int[,] matrix = GenerateMatrix(1000, 1000);
// Call: Process2DArray(matrix);",
                IdealSolution = @"public static long Process2DArray(int[,] matrix)
{
    int rows = matrix.GetLength(0);
    long totalSum = 0;
    object lockObject = new object();
    
    Parallel.For(0, rows, new ParallelOptions
    {
        MaxDegreeOfParallelism = Environment.ProcessorCount
    }, row =>
    {
        long rowSum = 0;
        for (int col = 0; col < matrix.GetLength(1); col++)
        {
            rowSum += (long)matrix[row, col] * matrix[row, col];
        }
        
        lock (lockObject)
        {
            totalSum += rowSum;
        }
    });
    
    return totalSum;
}",
                TestData = "GenerateMatrix(1000, 1000)",
                Category = ExerciseCategory.ParallelFor,
                Difficulty = DifficultyLevel.Advanced,
                ExpectedExecutionTimeMs = 80,
                MaxExecutionTimeMs = 2000,
                IsActive = true
            },
            
            new Exercise
            {
                Title = "Concurrent Collection Operations",
                Description = "Use ConcurrentBag and other concurrent collections for thread-safe operations",
                ProblemStatement = @"Implement a producer-consumer pattern using concurrent collections. Multiple producer threads will generate data, and consumer threads will process it.

Requirements:
- Use ConcurrentBag for thread-safe data storage
- Create multiple producer and consumer threads
- Producers add random numbers to the collection
- Consumers process numbers (calculate square root)
- Use appropriate synchronization mechanisms
- Return the count of processed items

Generate 10,000 numbers with 4 producer threads and 2 consumer threads.",
                StartingCode = @"// Complete this implementation using concurrent collections
public static int ProcessConcurrentData()
{
    // TODO: Implement producer-consumer pattern
    // Use ConcurrentBag for thread-safe operations
    // Multiple producers and consumers
    // Return count of processed items
    
    return 0;
}

// Call: ProcessConcurrentData();",
                IdealSolution = @"public static int ProcessConcurrentData()
{
    var dataBag = new ConcurrentBag<int>();
    var processedCount = 0;
    var lockObject = new object();
    
    var producerTasks = Enumerable.Range(0, 4).Select(_ => Task.Run(() =>
    {
        var random = new Random();
        for (int i = 0; i < 2500; i++) // 4 producers * 2500 = 10000
        {
            dataBag.Add(random.Next(1, 1000));
        }
    })).ToArray();
    
    var consumerTasks = Enumerable.Range(0, 2).Select(_ => Task.Run(() =>
    {
        while (dataBag.TryTake(out int value))
        {
            // Process the value (calculate square root)
            Math.Sqrt(value);
            
            lock (lockObject)
            {
                processedCount++;
            }
        }
    })).ToArray();
    
    Task.WaitAll(producerTasks);
    Task.WaitAll(consumerTasks);
    
    return processedCount;
}",
                TestData = "No external data needed",
                Category = ExerciseCategory.ConcurrentCollections,
                Difficulty = DifficultyLevel.Expert,
                ExpectedExecutionTimeMs = 120,
                MaxExecutionTimeMs = 5000,
                IsActive = true
            }
        };

        context.Exercises.AddRange(exercises);
        await context.SaveChangesAsync();
    }
}


