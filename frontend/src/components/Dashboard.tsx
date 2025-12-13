import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { exerciseApi, submissionApi } from '../services/api';
import { Exercise, ExerciseSubmission, DifficultyLevel, ExerciseCategory } from '../types';
import '../styles/Dashboard.css';

const Dashboard: React.FC = () => {
  const { user } = useAuth();
  const [recentSubmissions, setRecentSubmissions] = useState<ExerciseSubmission[]>([]);
  const [exercises, setExercises] = useState<Exercise[]>([]);
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    totalSolved: 0,
    totalAttempts: 0,
    averageScore: 0,
    currentStreak: 0
  });

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      
      // Load exercises
      const exercisesData = await exerciseApi.getAll();
      setExercises(exercisesData);

      // Load user submissions
      if (user) {
        const submissionsData = await submissionApi.getByUser(user.id.toString());
        setRecentSubmissions(submissionsData.slice(0, 5)); // Last 5 submissions

        // Calculate stats
        const solved = new Set(submissionsData.filter(s => s.isCorrect).map(s => s.exerciseId)).size;
        const avgScore = submissionsData.length > 0 
          ? submissionsData.reduce((sum, s) => sum + s.performanceScore, 0) / submissionsData.length 
          : 0;

        setStats({
          totalSolved: solved,
          totalAttempts: submissionsData.length,
          averageScore: avgScore,
          currentStreak: calculateStreak(submissionsData)
        });
      }
    } catch (error) {
      console.error('Error loading dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  const calculateStreak = (submissions: ExerciseSubmission[]): number => {
    // Simple streak calculation - consecutive days with submissions
    if (submissions.length === 0) return 0;
    
    const sortedSubmissions = submissions
      .map(s => new Date(s.submittedAt).toDateString())
      .filter((date, index, self) => self.indexOf(date) === index)
      .sort((a, b) => new Date(b).getTime() - new Date(a).getTime());

    let streak = 1;
    const today = new Date().toDateString();
    
    if (sortedSubmissions[0] !== today && 
        sortedSubmissions[0] !== new Date(Date.now() - 86400000).toDateString()) {
      return 0; // Streak broken
    }

    for (let i = 1; i < sortedSubmissions.length; i++) {
      const diff = new Date(sortedSubmissions[i - 1]).getTime() - new Date(sortedSubmissions[i]).getTime();
      if (diff === 86400000) { // Exactly 1 day
        streak++;
      } else {
        break;
      }
    }

    return streak;
  };

  const getCategoryProgress = () => {
    const categoryStats = Object.values(ExerciseCategory).map(category => {
      const categoryExercises = exercises.filter(e => 
        Object.values(ExerciseCategory)[e.category] === category
      );
      const solved = recentSubmissions.filter(s => 
        s.isCorrect && categoryExercises.some(e => e.id === s.exerciseId)
      ).length;
      
      return {
        name: category,
        total: categoryExercises.length,
        solved: solved,
        percentage: categoryExercises.length > 0 ? (solved / categoryExercises.length) * 100 : 0
      };
    });

    return categoryStats.filter(cs => cs.total > 0);
  };

  const getDifficultyProgress = () => {
    const difficultyStats = Object.values(DifficultyLevel).map(difficulty => {
      const difficultyExercises = exercises.filter(e => 
        Object.values(DifficultyLevel)[e.difficulty] === difficulty
      );
      const solved = new Set(
        recentSubmissions
          .filter(s => s.isCorrect && difficultyExercises.some(e => e.id === s.exerciseId))
          .map(s => s.exerciseId)
      ).size;
      
      return {
        name: difficulty,
        total: difficultyExercises.length,
        solved: solved,
        percentage: difficultyExercises.length > 0 ? (solved / difficultyExercises.length) * 100 : 0
      };
    });

    return difficultyStats.filter(ds => ds.total > 0);
  };

  if (loading) {
    return (
      <div className="loading">
        <div className="spinner"></div>
      </div>
    );
  }

  const categoryProgress = getCategoryProgress();
  const difficultyProgress = getDifficultyProgress();

  return (
    <div className="dashboard">
      {/* Welcome Header */}
      <div className="dashboard-header">
        <div>
          <h1>Welcome back, {user?.fullName}!</h1>
          <p>Track your progress and continue your parallel programming journey</p>
        </div>
        <Link to="/exercises" className="btn-primary-dashboard">
          Browse Exercises
        </Link>
      </div>

      {/* Stats Cards */}
      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-icon">✓</div>
          <div className="stat-content">
            <div className="stat-value">{stats.totalSolved}</div>
            <div className="stat-label">Problems Solved</div>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon">📝</div>
          <div className="stat-content">
            <div className="stat-value">{stats.totalAttempts}</div>
            <div className="stat-label">Total Submissions</div>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon">⭐</div>
          <div className="stat-content">
            <div className="stat-value">{stats.averageScore.toFixed(1)}</div>
            <div className="stat-label">Average Score</div>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon">🔥</div>
          <div className="stat-content">
            <div className="stat-value">{stats.currentStreak}</div>
            <div className="stat-label">Day Streak</div>
          </div>
        </div>
      </div>

      {/* Main Content Grid */}
      <div className="dashboard-grid">
        {/* Left Column */}
        <div className="dashboard-main">
          {/* Progress by Category */}
          <div className="dashboard-card">
            <h3>Progress by Category</h3>
            <div className="progress-list">
              {categoryProgress.map(cat => (
                <div key={cat.name} className="progress-item">
                  <div className="progress-header">
                    <span className="progress-name">{cat.name}</span>
                    <span className="progress-stats">{cat.solved} / {cat.total}</span>
                  </div>
                  <div className="progress-bar">
                    <div 
                      className="progress-fill" 
                      style={{ width: `${cat.percentage}%` }}
                    ></div>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Recent Submissions */}
          <div className="dashboard-card">
            <div className="card-header-flex">
              <h3>Recent Submissions</h3>
              <Link to="/submissions" className="view-all-link">View All →</Link>
            </div>
            
            {recentSubmissions.length > 0 ? (
              <div className="submissions-list">
                {recentSubmissions.map(submission => {
                  const exercise = exercises.find(e => e.id === submission.exerciseId);
                  return (
                    <div key={submission.id} className="submission-item">
                      <div className="submission-main">
                        <div className={`submission-status ${submission.isCorrect ? 'success' : 'failed'}`}>
                          {submission.isCorrect ? '✓' : '✗'}
                        </div>
                        <div className="submission-info">
                          <div className="submission-title">{exercise?.title || 'Unknown Exercise'}</div>
                          <div className="submission-meta">
                            {new Date(submission.submittedAt).toLocaleDateString()} • 
                            {submission.executionTimeMs}ms • 
                            Score: {submission.performanceScore.toFixed(1)}
                          </div>
                        </div>
                      </div>
                      <Link to={`/exercise/${submission.exerciseId}`} className="submission-action">
                        View →
                      </Link>
                    </div>
                  );
                })}
              </div>
            ) : (
              <div className="empty-state">
                <p>No submissions yet</p>
                <Link to="/exercises" className="btn-secondary-dashboard">
                  Start Practicing
                </Link>
              </div>
            )}
          </div>
        </div>

        {/* Right Sidebar */}
        <div className="dashboard-sidebar">
          {/* Skill Level */}
          <div className="dashboard-card">
            <h3>Difficulty Progress</h3>
            <div className="difficulty-progress">
              {difficultyProgress.map(diff => (
                <div key={diff.name} className="difficulty-item">
                  <div className="difficulty-header">
                    <span className={`difficulty-badge difficulty-${diff.name.toLowerCase()}`}>
                      {diff.name}
                    </span>
                    <span className="difficulty-count">{diff.solved}/{diff.total}</span>
                  </div>
                  <div className="progress-bar-small">
                    <div 
                      className={`progress-fill-${diff.name.toLowerCase()}`}
                      style={{ width: `${diff.percentage}%` }}
                    ></div>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Quick Actions */}
          <div className="dashboard-card">
            <h3>Quick Actions</h3>
            <div className="quick-actions">
              <Link to="/exercises" className="quick-action-btn">
                <div className="quick-action-icon">🎯</div>
                <div className="quick-action-text">
                  <div className="quick-action-title">Practice</div>
                  <div className="quick-action-desc">Solve new problems</div>
                </div>
              </Link>
              
              <Link to="/submissions" className="quick-action-btn">
                <div className="quick-action-icon">📊</div>
                <div className="quick-action-text">
                  <div className="quick-action-title">My Submissions</div>
                  <div className="quick-action-desc">Review your work</div>
                </div>
              </Link>

              {user?.role === 'User' && (
                <div className="quick-action-btn upgrade-card">
                  <div className="quick-action-icon">⭐</div>
                  <div className="quick-action-text">
                    <div className="quick-action-title">Upgrade to Premium</div>
                    <div className="quick-action-desc">Access all exercises</div>
                  </div>
                </div>
              )}
            </div>
          </div>

          {/* Account Info */}
          <div className="dashboard-card account-info">
            <h3>Account</h3>
            <div className="account-details">
              <div className="account-row">
                <span className="account-label">Status</span>
                <span className={`account-value role-${user?.role.toLowerCase()}`}>
                  {user?.role === 'User' ? 'Free' : user?.role}
                </span>
              </div>
              <div className="account-row">
                <span className="account-label">Member Since</span>
                <span className="account-value">
                  {new Date().toLocaleDateString('en-US', { month: 'short', year: 'numeric' })}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;

