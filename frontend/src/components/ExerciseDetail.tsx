import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Editor } from '@monaco-editor/react';
import * as monaco from 'monaco-editor';
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

  useEffect(() => {
    // Configure basic C# language features
    const configureCSharpLanguage = () => {
      // Set up C# language configuration for better syntax support
      monaco.languages.setLanguageConfiguration('csharp', {
        comments: {
          lineComment: '//',
          blockComment: ['/*', '*/']
        },
        brackets: [
          ['{', '}'],
          ['[', ']'],
          ['(', ')']
        ],
        autoClosingPairs: [
          { open: '{', close: '}' },
          { open: '[', close: ']' },
          { open: '(', close: ')' },
          { open: '"', close: '"' },
          { open: "'", close: "'" }
        ],
        surroundingPairs: [
          { open: '{', close: '}' },
          { open: '[', close: ']' },
          { open: '(', close: ')' },
          { open: '"', close: '"' },
          { open: "'", close: "'" }
        ]
      });
    };

    configureCSharpLanguage();
  }, []);

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
              quickSuggestions: {
                other: true,
                comments: true,
                strings: true
              },
              suggestOnTriggerCharacters: true,
              acceptSuggestionOnEnter: 'on',
              tabCompletion: 'on',
              wordBasedSuggestions: 'matchingDocuments',
              parameterHints: {
                enabled: true
              },
              hover: {
                enabled: true
              }
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
              <div>
              {/* Test Results Summary */}
              <div className={`output-panel ${result.submission.isCorrect ? 'success-output' : 'error-output'}`}>
                <strong>Test Results:</strong> {result.passedTests} / {result.totalTests} passed
                <br />
                <strong>Average Execution Time:</strong> {result.submission.executionTimeMs}ms
                {result.submission.performanceScore > 0 && (
                    <>
                    <br />
                    <strong>Performance Score:</strong> {result.submission.performanceScore.toFixed(1)}
                    </>
                  )}
                </div>

              {/* Individual Test Case Results */}
              {result.testCaseResults && result.testCaseResults.length > 0 && (
                <div style={{ marginTop: '15px' }}>
                  <h4>Test Cases</h4>
                  {result.testCaseResults.map((testResult, index) => (
                    <div 
                      key={testResult.id} 
                      className={`card`}
                      style={{ 
                        marginTop: '10px', 
                        padding: '10px',
                        border: testResult.passed ? '2px solid #4caf50' : '2px solid #f44336'
                      }}
                    >
                      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <strong>Test Case #{index + 1}</strong>
                        <span style={{ 
                          padding: '5px 10px', 
                          borderRadius: '4px',
                          backgroundColor: testResult.passed ? '#4caf50' : '#f44336',
                          color: 'white'
                        }}>
                          {testResult.passed ? '✓ PASSED' : '✗ FAILED'}
                        </span>
                      </div>
                      <div style={{ marginTop: '10px', fontSize: '14px' }}>
                        <strong>Execution Time:</strong> {testResult.executionTimeMs}ms
                        {testResult.timedOut && <span style={{ color: '#f44336' }}> (Timed Out)</span>}
                        <br />
                        <strong>Exit Code:</strong> {testResult.exitCode}
                      </div>
                      {!testResult.passed && (
                        <div style={{ marginTop: '10px' }}>
                          <div style={{ marginBottom: '5px' }}>
                            <strong>Expected Output:</strong>
                            <pre style={{ backgroundColor: '#f5f5f5', padding: '8px', borderRadius: '4px', margin: '5px 0', fontSize: '12px' }}>
                              {testResult.expectedOutput || '(empty)'}
                            </pre>
                          </div>
                          <div>
                            <strong>Actual Output:</strong>
                            <pre style={{ backgroundColor: '#f5f5f5', padding: '8px', borderRadius: '4px', margin: '5px 0', fontSize: '12px' }}>
                              {testResult.actualOutput || '(empty)'}
                            </pre>
                          </div>
                          {testResult.standardError && (
                            <div>
                              <strong>Error:</strong>
                              <pre style={{ backgroundColor: '#ffe6e6', padding: '8px', borderRadius: '4px', margin: '5px 0', fontSize: '12px' }}>
                                {testResult.standardError}
                              </pre>
                            </div>
                          )}
                  </div>
                )}
                    </div>
                  ))}
                </div>
              )}

              {/* Compilation Error if any */}
              {result.submission.compilationError && (
                <div className="output-panel error-output" style={{ marginTop: '15px' }}>
                  <strong>Compilation Error:</strong><br />
                  <pre style={{ whiteSpace: 'pre-wrap', fontSize: '12px' }}>{result.submission.compilationError}</pre>
              </div>
                )}
              </div>
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
              height="700px"
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
                quickSuggestions: {
                  other: true,
                  comments: true,
                  strings: true
                },
                suggestOnTriggerCharacters: true,
                acceptSuggestionOnEnter: 'on',
                tabCompletion: 'on',
                wordBasedSuggestions: 'matchingDocuments',
                parameterHints: {
                  enabled: true
                },
                hover: {
                  enabled: true
                }
              }}
            />
          </div>
        </div>
      )}
    </div>
  );
};

export default ExerciseDetail;


