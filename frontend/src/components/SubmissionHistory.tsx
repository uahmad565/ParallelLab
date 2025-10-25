import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { ExerciseSubmission, PerformanceLevel } from '../types';
import { submissionApi } from '../services/api';

const SubmissionHistory: React.FC = () => {
  const [submissions, setSubmissions] = useState<ExerciseSubmission[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadSubmissions();
  }, []);

  const loadSubmissions = async () => {
    try {
      setLoading(true);
      const data = await submissionApi.getByUser('user123'); // In a real app, this would come from authentication
      setSubmissions(data);
    } catch (err) {
      setError('Failed to load submissions');
      console.error('Error loading submissions:', err);
    } finally {
      setLoading(false);
    }
  };

  const getPerformanceClass = (score: number): string => {
    if (score >= 90) return 'performance-excellent';
    if (score >= 75) return 'performance-good';
    if (score >= 60) return 'performance-average';
    if (score >= 40) return 'performance-below-average';
    return 'performance-poor';
  };

  const getPerformanceLevel = (score: number): PerformanceLevel => {
    if (score >= 90) return PerformanceLevel.Excellent;
    if (score >= 75) return PerformanceLevel.Good;
    if (score >= 60) return PerformanceLevel.Average;
    if (score >= 40) return PerformanceLevel.BelowAverage;
    return PerformanceLevel.Poor;
  };

  const formatDate = (dateString: string): string => {
    return new Date(dateString).toLocaleString();
  };

  if (loading) {
    return (
      <div className="loading">
        <div className="spinner"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="card">
        <h2>Error</h2>
        <p>{error}</p>
        <button className="btn btn-primary" onClick={loadSubmissions}>
          Retry
        </button>
      </div>
    );
  }

  return (
    <div>
      <div className="card">
        <h1>My Submissions</h1>
        <p>Track your progress and performance across all exercises.</p>
      </div>

      {submissions.length === 0 ? (
        <div className="card">
          <h3>No submissions yet</h3>
          <p>Start solving exercises to see your submissions here.</p>
          <Link to="/" className="btn btn-primary">
            Browse Exercises
          </Link>
        </div>
      ) : (
        <div className="grid">
          {submissions.map(submission => (
            <div key={submission.id} className="card">
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '15px' }}>
                <div>
                  <h3 style={{ margin: 0 }}>
                    {submission.exercise?.title || `Exercise ${submission.exerciseId}`}
                  </h3>
                  <p style={{ color: '#666', margin: '5px 0 0 0' }}>
                    Submitted: {formatDate(submission.submittedAt)}
                  </p>
                </div>
                <div className={`performance-score ${getPerformanceClass(submission.performanceScore)}`} style={{ padding: '10px', minWidth: '100px' }}>
                  <div style={{ fontSize: '14px' }}>Score</div>
                  <div style={{ fontSize: '24px' }}>{submission.performanceScore.toFixed(1)}</div>
                  <div style={{ fontSize: '12px' }}>{getPerformanceLevel(submission.performanceScore)}</div>
                </div>
              </div>

              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '15px', marginBottom: '15px' }}>
                <div>
                  <strong>Execution Time:</strong> {submission.executionTimeMs}ms
                </div>
                <div>
                  <strong>Status:</strong> 
                  <span style={{ color: submission.isCorrect ? '#28a745' : '#dc3545', marginLeft: '5px' }}>
                    {submission.isCorrect ? '✓ Correct' : '✗ Incorrect'}
                  </span>
                </div>
              </div>

              {submission.compilationError && (
                <div className="output-panel error-output" style={{ marginBottom: '10px' }}>
                  <strong>Compilation Error:</strong><br />
                  {submission.compilationError}
                </div>
              )}

              {submission.runtimeError && (
                <div className="output-panel error-output" style={{ marginBottom: '10px' }}>
                  <strong>Runtime Error:</strong><br />
                  {submission.runtimeError}
                </div>
              )}

              {submission.output && (
                <div className="output-panel" style={{ marginBottom: '10px' }}>
                  <strong>Output:</strong><br />
                  {submission.output}
                </div>
              )}

              <div style={{ display: 'flex', gap: '10px' }}>
                <Link 
                  to={`/exercise/${submission.exerciseId}`} 
                  className="btn btn-primary"
                  style={{ flex: 1 }}
                >
                  View Exercise
                </Link>
                <button 
                  className="btn btn-success"
                  onClick={() => {
                    // In a real app, this would open a modal or navigate to a code viewer
                    alert('Code viewer would open here');
                  }}
                >
                  View Code
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default SubmissionHistory;


