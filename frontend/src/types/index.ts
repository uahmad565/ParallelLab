export interface Exercise {
  id: number;
  title: string;
  description: string;
  problemStatement: string;
  startingCode: string;
  idealSolution: string;
  testData: string;
  category: number; // API returns numbers, not enum strings
  difficulty: number; // API returns numbers, not enum strings
  expectedExecutionTimeMs: number;
  maxExecutionTimeMs: number;
  createdAt: string;
  updatedAt?: string;
  isActive: boolean;
}

export interface ExerciseSubmission {
  id: number;
  exerciseId: number;
  userCode: string;
  compilationError?: string;
  runtimeError?: string;
  executionTimeMs: number;
  isCorrect: boolean;
  output?: string;
  performanceScore: number;
  submittedAt: string;
  userId?: string;
  exercise?: Exercise;
}

export interface PerformanceComparison {
  id: number;
  submissionId: number;
  userExecutionTimeMs: number;
  idealExecutionTimeMs: number;
  performanceRatio: number;
  analysis: string;
  comparedAt: string;
}

export interface CodeExecutionResult {
  isSuccess: boolean;
  output?: string;
  error?: string;
  executionTimeMs: number;
  isCorrect: boolean;
  compilationError?: string;
  runtimeError?: string;
}

export interface PerformanceAnalysisResult {
  performanceScore: number;
  analysis: string;
  recommendations: string[];
  level: PerformanceLevel | number;
}

export interface CodeSubmissionRequest {
  exerciseId: number;
  userCode: string;
  userId?: string;
}

export interface TestCaseResult {
  id: number;
  submissionId: number;
  testCaseId: number;
  passed: boolean;
  actualOutput: string;
  expectedOutput: string;
  executionTimeMs: number;
  timedOut: boolean;
  exitCode: number;
  standardError?: string;
  executedAt: string;
}

export interface CodeSubmissionResponse {
  submission: ExerciseSubmission;
  testCaseResults: TestCaseResult[];
  totalTests: number;
  passedTests: number;
  // Legacy fields for backward compatibility
  executionResult?: CodeExecutionResult;
  performanceAnalysis?: PerformanceAnalysisResult;
}

export enum ExerciseCategory {
  Threads = 'Threads',
  Tasks = 'Tasks',
  LINQ = 'LINQ',
  ParallelFor = 'ParallelFor',
  ConcurrentCollections = 'ConcurrentCollections',
  AsyncAwait = 'AsyncAwait',
  PLINQ = 'PLINQ'
}

export enum DifficultyLevel {
  Beginner = 'Beginner',
  Intermediate = 'Intermediate',
  Advanced = 'Advanced',
  Expert = 'Expert'
}

export enum PerformanceLevel {
  Poor = 'Poor',
  BelowAverage = 'BelowAverage',
  Average = 'Average',
  Good = 'Good',
  Excellent = 'Excellent'
}


