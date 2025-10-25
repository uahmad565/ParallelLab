import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Editor } from '@monaco-editor/react';
import { Exercise, CodeSubmissionRequest, CodeSubmissionResponse, PerformanceLevel } from '../types';
import { exerciseApi, submissionApi } from '../services/api';

const ExerciseDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [exercise, setExercise] = useState<Exercise | null>(null);
  const [userCode, setUserCode] = useState<string>('');
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [result, setResult] = useState<CodeSubmissionResponse | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [showIdealSolution, setShowIdealSolution] = useState(false);
  const [isDarkTheme, setIsDarkTheme] = useState(false);

  useEffect(() => {
    if (id) {
      loadExercise(parseInt(id));
    }
  }, [id]);

  const loadExercise = async (exerciseId: number) => {
    try {
      setLoading(true);
      const data = await exerciseApi.getById(exerciseId);
      setExercise(data);
      setUserCode(data.startingCode);
    } catch (err) {
      setError('Failed to load exercise');
      console.error('Error loading exercise:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async () => {
    if (!exercise) return;

    try {
      setSubmitting(true);
      setError(null);
      
      const request: CodeSubmissionRequest = {
        exerciseId: exercise.id,
        userCode: userCode,
        userId: 'user123' // In a real app, this would come from authentication
      };

      const response = await submissionApi.submit(request);
      setResult(response);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to submit code');
      console.error('Error submitting code:', err);
    } finally {
      setSubmitting(false);
    }
  };

  const getPerformanceClass = (level: PerformanceLevel | number): string => {
    // Handle both string enum values and numeric values from API
    let levelStr: string;
    if (typeof level === 'number') {
      // Convert numeric enum to string
      levelStr = Object.values(PerformanceLevel)[level] || 'Average';
    } else {
      levelStr = level;
    }
    return `performance-${levelStr.toLowerCase().replace(/([A-Z])/g, '-$1').toLowerCase()}`;
  };

  const getPerformanceLevelDisplay = (level: PerformanceLevel | number): string => {
    // Handle both string enum values and numeric values from API
    if (typeof level === 'number') {
      // Convert numeric enum to string
      return Object.values(PerformanceLevel)[level] || 'Average';
    }
    return level;
  };

  const resetCode = () => {
    if (exercise) {
      setUserCode(exercise.startingCode);
      setResult(null);
      setError(null);
    }
  };

  if (loading) {
    return (
      <div className="loading">
        <div className="spinner"></div>
      </div>
    );
  }

  if (error && !exercise) {
    return (
      <div className="card">
        <h2>Error</h2>
        <p>{error}</p>
        <button className="btn btn-primary" onClick={() => navigate('/')}>
          Back to Exercises
        </button>
      </div>
    );
  }

  if (!exercise) {
    return (
      <div className="card">
        <h2>Exercise not found</h2>
        <button className="btn btn-primary" onClick={() => navigate('/')}>
          Back to Exercises
        </button>
      </div>
    );
  }

  return (
    <div>
      <div className="card">
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '20px' }}>
          <div>
            <h1>{exercise.title}</h1>
            <p style={{ color: '#666', fontSize: '16px' }}>{exercise.description}</p>
          </div>
          <button className="btn btn-primary" onClick={() => navigate('/')}>
            ← Back to Exercises
          </button>
        </div>

        <div className="card" style={{ backgroundColor: '#f8f9fa' }}>
          <h3>Problem Statement</h3>
          <div style={{ whiteSpace: 'pre-wrap', lineHeight: '1.6' }}>
            {exercise.problemStatement}
          </div>
        </div>

      </div>

      <div className="card">
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
          <h3>Your Code</h3>
          <button 
            className="btn btn-secondary" 
            onClick={() => setIsDarkTheme(!isDarkTheme)}
            style={{ fontSize: '14px', padding: '8px 16px' }}
          >
            {isDarkTheme ? '☀️ Light' : '🌙 Dark'}
          </button>
        </div>
        <div className="code-editor">
          <Editor
            height="600px"
            defaultLanguage="csharp"
            value={userCode}
            onChange={(value) => setUserCode(value || '')}
            theme={isDarkTheme ? 'vs-dark' : 'vs-light'}
            options={{
              minimap: { enabled: false },
              fontSize: 14,
              lineNumbers: 'on',
              roundedSelection: false,
              scrollBeyondLastLine: false,
              automaticLayout: true,
            }}
          />
        </div>
        <div style={{ display: 'flex', gap: '10px', marginTop: '15px' }}>
          <button className="btn btn-primary" onClick={handleSubmit} disabled={submitting}>
            {submitting ? 'Running...' : 'Run Code'}
          </button>
          <button className="btn btn-success" onClick={resetCode}>
            Reset Code
          </button>
          <button 
            className="btn btn-danger" 
            onClick={() => setShowIdealSolution(!showIdealSolution)}
          >
            {showIdealSolution ? 'Hide' : 'Show'} Ideal Solution
          </button>
        </div>
      </div>

      <div className="card">
        <h3>Output & Results</h3>
        {result ? (
          <div>
            {result.executionResult.isSuccess ? (
              <div>
                <div className={`output-panel success-output`}>
                  <strong>Execution Time:</strong> {result.executionResult.executionTimeMs}ms
                  {result.executionResult.output && (
                    <>
                      <br /><br />
                      <strong>Output:</strong><br />
                      {result.executionResult.output}
                    </>
                  )}
                </div>

                {result.performanceAnalysis && (
                  <div className={`performance-score ${getPerformanceClass(result.performanceAnalysis.level)}`}>
                    <div>Performance Score</div>
                    <div style={{ fontSize: '36px' }}>{result.performanceAnalysis.performanceScore.toFixed(1)}</div>
                    <div style={{ fontSize: '14px' }}>{getPerformanceLevelDisplay(result.performanceAnalysis.level)}</div>
                  </div>
                )}

                <div className="card" style={{ marginTop: '15px' }}>
                  <h4>Analysis</h4>
                  <p>{result.performanceAnalysis?.analysis}</p>
                  
                  {result.performanceAnalysis?.recommendations && result.performanceAnalysis.recommendations.length > 0 && (
                    <div>
                      <h5>Recommendations:</h5>
                      <ul>
                        {result.performanceAnalysis.recommendations.map((rec, index) => (
                          <li key={index}>{rec}</li>
                        ))}
                      </ul>
                    </div>
                  )}
                </div>
              </div>
            ) : (
              <div className={`output-panel error-output`}>
                <strong>Error:</strong><br />
                {result.executionResult.compilationError && (
                  <>
                    <strong>Compilation Error:</strong><br />
                    {result.executionResult.compilationError}<br /><br />
                  </>
                )}
                {result.executionResult.runtimeError && (
                  <>
                    <strong>Runtime Error:</strong><br />
                    {result.executionResult.runtimeError}
                  </>
                )}
              </div>
            )}
          </div>
        ) : error ? (
          <div className={`output-panel error-output`}>
            <strong>Error:</strong><br />
            {error}
          </div>
        ) : (
          <div className="output-panel">
            Click "Run Code" to execute your solution and see the results here.
          </div>
        )}
      </div>

      {showIdealSolution && (
        <div className="card">
          <h3>Ideal Solution</h3>
          <div className="code-editor">
            <Editor
              height="300px"
              defaultLanguage="csharp"
              value={exercise.idealSolution}
              theme={isDarkTheme ? 'vs-dark' : 'vs-light'}
              options={{
                readOnly: true,
                minimap: { enabled: false },
                fontSize: 14,
                lineNumbers: 'on',
                roundedSelection: false,
                scrollBeyondLastLine: false,
                automaticLayout: true,
              }}
            />
          </div>
        </div>
      )}
    </div>
  );
};

export default ExerciseDetail;


