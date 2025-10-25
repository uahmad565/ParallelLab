# ParallelLab - Parallel Programming Learning Platform

A comprehensive web application for learning parallel programming in .NET. Users can complete coding exercises and compare their performance with ideal solutions.

## Features

- **Interactive Code Editor**: Monaco Editor with C# syntax highlighting
- **Real-time Code Execution**: Safe sandboxed execution of user code
- **Performance Analysis**: Compare execution times with ideal solutions
- **Multiple Exercise Categories**: Threads, Tasks, LINQ, ParallelFor, Concurrent Collections, Async/Await, PLINQ
- **Difficulty Levels**: Beginner to Expert
- **Submission History**: Track progress and performance over time
- **Solution Comparison**: View ideal solutions after completing exercises

## Technology Stack

### Backend
- .NET 8 Web API
- Entity Framework Core with SQL Server
- Roslyn for code compilation and execution
- Clean Architecture with separate layers

### Frontend
- React 18 with TypeScript
- Monaco Editor for code editing
- Axios for API communication
- React Router for navigation

## Project Structure

```
ParallelLab/
├── src/
│   ├── ParallelLab.Core/           # Domain entities and interfaces
│   ├── ParallelLab.Infrastructure/ # Data access and external services
│   └── ParallelLab.API/           # Web API controllers and configuration
├── frontend/                      # React frontend application
└── README.md
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js 16+ and npm
- SQL Server (LocalDB is fine for development)
- Visual Studio 2022 or VS Code

### Backend Setup

1. Navigate to the solution directory:
   ```bash
   cd ParallelLab
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Update the connection string in `src/ParallelLab.API/appsettings.json` if needed:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ParallelLabDb;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

4. Run the API:
   ```bash
   dotnet run --project src/ParallelLab.API
   ```

   The API will be available at `https://localhost:7000` (HTTPS) or `http://localhost:5000` (HTTP).

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

   The frontend will be available at `http://localhost:3000`.

## Sample Exercises

The application comes with 5 sample exercises covering different aspects of parallel programming:

1. **Parallel Array Processing with Threads** (Intermediate)
   - Process large arrays using multiple threads
   - Learn thread synchronization and work distribution

2. **Async File Processing with Tasks** (Intermediate)
   - Process multiple files concurrently using Task.Run
   - Master async/await patterns

3. **Parallel LINQ Data Processing** (Advanced)
   - Use PLINQ for efficient data processing
   - Learn parallel aggregation and filtering

4. **Parallel For Loop with Custom Partitioning** (Advanced)
   - Optimize 2D array processing with Parallel.For
   - Implement custom partitioning strategies

5. **Concurrent Collection Operations** (Expert)
   - Implement producer-consumer patterns
   - Use ConcurrentBag and thread-safe operations

## How It Works

1. **Exercise Selection**: Browse exercises by category and difficulty
2. **Code Implementation**: Write your solution in the Monaco editor
3. **Code Execution**: Submit your code for safe execution
4. **Performance Analysis**: Compare your solution's performance with the ideal solution
5. **Learning**: View the ideal solution and get recommendations for improvement

## Security Features

- **Code Sandboxing**: User code runs in a controlled environment
- **Input Validation**: Dangerous operations are blocked
- **Timeout Protection**: Code execution has time limits
- **Resource Limits**: Memory and CPU usage are controlled

## API Endpoints

### Exercises
- `GET /api/exercises` - Get all exercises
- `GET /api/exercises/{id}` - Get exercise by ID
- `GET /api/exercises/category/{category}` - Get exercises by category
- `GET /api/exercises/difficulty/{difficulty}` - Get exercises by difficulty

### Submissions
- `POST /api/submissions/submit` - Submit code for execution
- `GET /api/submissions/exercise/{exerciseId}` - Get submissions for an exercise
- `GET /api/submissions/user/{userId}` - Get user's submissions

## Performance Scoring

The system uses a sophisticated scoring algorithm:

- **Excellent (90-100)**: Performance within 1.5x of ideal solution
- **Good (75-89)**: Performance within 2x of ideal solution
- **Average (60-74)**: Performance within 3x of ideal solution
- **Below Average (40-59)**: Performance within 5x of ideal solution
- **Poor (0-39)**: Performance worse than 5x ideal solution

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Future Enhancements

- User authentication and profiles
- More exercise categories (GPU programming, distributed computing)
- Code review and peer feedback
- Progress tracking and achievements
- Mobile-responsive design improvements
- Integration with external compilers and testing frameworks


