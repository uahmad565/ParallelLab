using Microsoft.EntityFrameworkCore;
using ParallelLab.Core.Entities;
using ParallelLab.Infrastructure.Data;

namespace ParallelLab.API.Data;

public static class SeedData
{
    public static async Task SeedAsync(ParallelLabDbContext context)
    {
        // Seed demo users
        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new User
                {
                    Username = "user",
                    Email = "user@parallellab.com",
                    FullName = "Demo User",
                    PasswordHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes("password"))),
                    Role = UserRole.User,
                    IsActive = true
                },
                new User
                {
                    Username = "premium",
                    Email = "premium@parallellab.com",
                    FullName = "Premium User",
                    PasswordHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes("password"))),
                    Role = UserRole.PremiumUser,
                    IsActive = true
                },
                new User
                {
                    Username = "admin",
                    Email = "admin@parallellab.com",
                    FullName = "Admin User",
                    PasswordHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes("password"))),
                    Role = UserRole.Admin,
                    IsActive = true
                }
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }

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
                Title = "Sum of Even Numbers",
                Description = "Calculate the sum of even numbers in an array",
                ProblemStatement = @"Write a program that:
1. Reads numbers from input (space-separated)
2. Calculates the sum of even numbers only
3. Outputs the result

Example: Input '1 2 3 4 5 6' should output '12' (2+4+6=12)",
                StartingCode = @"using System;
using System.Linq;

class Solution 
{ 
    static void Main()
    { 
        // TODO: Read input, process even numbers, output sum
    } 
}",
                IdealSolution = @"using System;
using System.Linq;

class Solution 
{ 
    static void Main()
    { 
        var input = Console.ReadLine();
        var numbers = input.Split(' ').Select(int.Parse);
        var sum = numbers.Where(n => n % 2 == 0).Sum();
        Console.WriteLine(sum);
    } 
}",
                TestData = "",
                Category = ExerciseCategory.LINQ,
                Difficulty = DifficultyLevel.Beginner,
                ExpectedExecutionTimeMs = 50,
                MaxExecutionTimeMs = 1000,
                TestCases = new List<TestCase>
                {
                    new TestCase
                    {
                        Input = "1 2 3 4 5 6",
                        ExpectedOutput = "12",
                        TimeoutMs = 1000,
                        IdealExecutionTimeMs = 50,
                        IsHidden = false,
                        Order = 1
                    },
                    new TestCase
                    {
                        Input = "10 20 30 40",
                        ExpectedOutput = "100",
                        TimeoutMs = 1000,
                        IdealExecutionTimeMs = 50,
                        IsHidden = false,
                        Order = 2
                    },
                    new TestCase
                    {
                        Input = "1 3 5 7 9",
                        ExpectedOutput = "0",
                        TimeoutMs = 1000,
                        IdealExecutionTimeMs = 50,
                        IsHidden = true,
                        Order = 3
                    }
                }
            },
            
            new Exercise
            {
                Title = "Parallel Array Sum with Tasks",
                Description = "Calculate sum of numbers using parallel tasks",
                ProblemStatement = @"Write a program that:
1. Reads numbers from input (space-separated)
2. Uses Task.Run to calculate sum in parallel chunks
3. Outputs the total sum

Use at least 2 parallel tasks to process the array.",
                StartingCode = @"using System;
using System.Linq;
using System.Threading.Tasks;

class Solution 
{ 
    static void Main()
    { 
        var input = Console.ReadLine();
        var numbers = input.Split(' ').Select(int.Parse).ToArray();
        
        // TODO: Use Task.Run to calculate sum in parallel
        // Split array into chunks and process each chunk in parallel
        
        Console.WriteLine(0); // Replace with actual sum
    } 
}",
                IdealSolution = @"using System;
using System.Linq;
using System.Threading.Tasks;

class Solution 
{ 
    static void Main()
    { 
        var input = Console.ReadLine();
        var numbers = input.Split(' ').Select(int.Parse).ToArray();
        
        int mid = numbers.Length / 2;
        
        var task1 = Task.Run(() => numbers.Take(mid).Sum());
        var task2 = Task.Run(() => numbers.Skip(mid).Sum());
        
        Task.WaitAll(task1, task2);
        var sum = task1.Result + task2.Result;
        
        Console.WriteLine(sum);
    } 
}",
                TestData = "",
                Category = ExerciseCategory.Tasks,
                Difficulty = DifficultyLevel.Intermediate,
                ExpectedExecutionTimeMs = 100,
                MaxExecutionTimeMs = 2000,
                TestCases = new List<TestCase>
                {
                    new TestCase
                    {
                        Input = "1 2 3 4 5",
                        ExpectedOutput = "15",
                        TimeoutMs = 2000,
                        IdealExecutionTimeMs = 100,
                        IsHidden = false,
                        Order = 1
                    },
                    new TestCase
                    {
                        Input = "10 20 30 40 50",
                        ExpectedOutput = "150",
                        TimeoutMs = 2000,
                        IdealExecutionTimeMs = 100,
                        IsHidden = false,
                        Order = 2
                    },
                    new TestCase
                    {
                        Input = "100 200 300",
                        ExpectedOutput = "600",
                        TimeoutMs = 2000,
                        IdealExecutionTimeMs = 100,
                        IsHidden = true,
                        Order = 3
                    }
                }
            },
            
            new Exercise
            {
                Title = "Count Words Using PLINQ",
                Description = "Count word occurrences using parallel LINQ",
                ProblemStatement = @"Write a program that:
1. Reads a sentence from input
2. Counts how many words are in the sentence
3. Outputs the word count

Use LINQ/PLINQ for processing.",
                StartingCode = @"using System;
using System.Linq;

class Solution 
{ 
    static void Main()
    { 
        var input = Console.ReadLine();
        
        // TODO: Count words and output the result
        
        Console.WriteLine(0); // Replace with actual count
    } 
}",
                IdealSolution = @"using System;
using System.Linq;

class Solution 
{ 
    static void Main()
    { 
        var input = Console.ReadLine();
        var words = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        var count = words.AsParallel().Count();
        Console.WriteLine(count);
    } 
}",
                TestData = "",
                Category = ExerciseCategory.PLINQ,
                Difficulty = DifficultyLevel.Beginner,
                ExpectedExecutionTimeMs = 50,
                MaxExecutionTimeMs = 1000,
                TestCases = new List<TestCase>
                {
                    new TestCase
                    {
                        Input = "Hello World",
                        ExpectedOutput = "2",
                        TimeoutMs = 1000,
                        IdealExecutionTimeMs = 50,
                        IsHidden = false,
                        Order = 1
                    },
                    new TestCase
                    {
                        Input = "The quick brown fox jumps",
                        ExpectedOutput = "5",
                        TimeoutMs = 1000,
                        IdealExecutionTimeMs = 50,
                        IsHidden = false,
                        Order = 2
                    },
                    new TestCase
                    {
                        Input = "One two three four five six seven",
                        ExpectedOutput = "7",
                        TimeoutMs = 1000,
                        IdealExecutionTimeMs = 50,
                        IsHidden = true,
                        Order = 3
                    }
                }
            },
            
            new Exercise
            {
                Title = "Parallel.For Square Numbers",
                Description = "Use Parallel.For to calculate squares of numbers",
                ProblemStatement = @"Write a program that:
1. Reads numbers from input (space-separated)
2. Uses Parallel.For to calculate the square of each number
3. Outputs the sum of all squared values

Use Parallel.For for the computation.",
                StartingCode = @"using System;
using System.Linq;
using System.Threading.Tasks;

class Solution 
{ 
    static void Main()
    { 
        var input = Console.ReadLine();
        var numbers = input.Split(' ').Select(int.Parse).ToArray();
        
        // TODO: Use Parallel.For to calculate sum of squares
        
        Console.WriteLine(0); // Replace with actual sum
    } 
}",
                IdealSolution = @"using System;
using System.Linq;
using System.Threading.Tasks;

class Solution 
{ 
    static void Main()
    { 
        var input = Console.ReadLine();
        var numbers = input.Split(' ').Select(int.Parse).ToArray();
        
        long sum = 0;
        object lockObj = new object();
        
        Parallel.For(0, numbers.Length, i =>
        {
            long square = (long)numbers[i] * numbers[i];
            lock (lockObj)
            {
                sum += square;
            }
        });
        
        Console.WriteLine(sum);
    } 
}",
                TestData = "",
                Category = ExerciseCategory.ParallelFor,
                Difficulty = DifficultyLevel.Intermediate,
                ExpectedExecutionTimeMs = 80,
                MaxExecutionTimeMs = 2000,
                TestCases = new List<TestCase>
                {
                    new TestCase
                    {
                        Input = "1 2 3",
                        ExpectedOutput = "14",
                        TimeoutMs = 2000,
                        IdealExecutionTimeMs = 80,
                        IsHidden = false,
                        Order = 1
                    },
                    new TestCase
                    {
                        Input = "2 4 6",
                        ExpectedOutput = "56",
                        TimeoutMs = 2000,
                        IdealExecutionTimeMs = 80,
                        IsHidden = false,
                        Order = 2
                    },
                    new TestCase
                    {
                        Input = "5 10 15",
                        ExpectedOutput = "350",
                        TimeoutMs = 2000,
                        IdealExecutionTimeMs = 80,
                        IsHidden = true,
                        Order = 3
                    }
                }
            },
            
            new Exercise
            {
                Title = "Fibonacci with Multiple Threads",
                Description = "Calculate multiple Fibonacci numbers using threads",
                ProblemStatement = @"Write a program that:
1. Reads a number N from input
2. Calculates the Nth Fibonacci number
3. Outputs the result

You can use iterative or recursive approach.",
                StartingCode = @"using System;

class Solution 
{ 
    static void Main()
    { 
        var n = int.Parse(Console.ReadLine());
        
        // TODO: Calculate Nth Fibonacci number
        
        Console.WriteLine(0); // Replace with actual Fibonacci result
    } 
}",
                IdealSolution = @"using System;

class Solution 
{ 
    static void Main()
    { 
        var n = int.Parse(Console.ReadLine());
        var result = Fibonacci(n);
        Console.WriteLine(result);
    }
    
    static long Fibonacci(int n)
    {
        if (n <= 1) return n;
        
        long a = 0, b = 1;
        for (int i = 2; i <= n; i++)
        {
            long temp = a + b;
            a = b;
            b = temp;
        }
        return b;
    }
}",
                TestData = "",
                Category = ExerciseCategory.Threads,
                Difficulty = DifficultyLevel.Intermediate,
                ExpectedExecutionTimeMs = 100,
                MaxExecutionTimeMs = 2000,
                TestCases = new List<TestCase>
                {
                    new TestCase
                    {
                        Input = "5",
                        ExpectedOutput = "5",
                        TimeoutMs = 2000,
                        IdealExecutionTimeMs = 100,
                        IsHidden = false,
                        Order = 1
                    },
                    new TestCase
                    {
                        Input = "10",
                        ExpectedOutput = "55",
                        TimeoutMs = 2000,
                        IdealExecutionTimeMs = 100,
                        IsHidden = false,
                        Order = 2
                    },
                    new TestCase
                    {
                        Input = "15",
                        ExpectedOutput = "610",
                        TimeoutMs = 2000,
                        IdealExecutionTimeMs = 100,
                        IsHidden = true,
                        Order = 3
                    }
                }
            }
        };

        context.Exercises.AddRange(exercises);
        await context.SaveChangesAsync();
    }
}


