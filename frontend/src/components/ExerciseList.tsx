import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Exercise, ExerciseCategory, DifficultyLevel } from '../types';
import { exerciseApi } from '../services/api';
import { useAuth } from '../contexts/AuthContext';

const ExerciseList: React.FC = () => {
  const [exercises, setExercises] = useState<Exercise[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedCategory, setSelectedCategory] = useState<string>('all');
  const [selectedDifficulty, setSelectedDifficulty] = useState<string>('all');
  const { user, canAccessExercise } = useAuth();

  useEffect(() => {
    loadExercises();
  }, []);

  const loadExercises = async () => {
    try {
      setLoading(true);
      const data = await exerciseApi.getAll();
      setExercises(data);
    } catch (err) {
      setError('Failed to load exercises');
      console.error('Error loading exercises:', err);
    } finally {
      setLoading(false);
    }
  };

  const getDifficultyClass = (difficulty: number): string => {
    const difficultyStr = Object.values(DifficultyLevel)[difficulty];
    return `difficulty-${difficultyStr ? difficultyStr.toLowerCase() : 'unknown'}`;
  };

  const getCategoryClass = (category: number): string => {
    const categoryStr = Object.values(ExerciseCategory)[category];
    return categoryStr ? categoryStr.toLowerCase() : 'unknown';
  };

  const filteredExercises = exercises.filter(exercise => {
    const categoryMatch = selectedCategory === 'all' || Object.values(ExerciseCategory)[exercise.category] === selectedCategory;
    const difficultyMatch = selectedDifficulty === 'all' || Object.values(DifficultyLevel)[exercise.difficulty] === selectedDifficulty;
    return categoryMatch && difficultyMatch;
  });

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
        <button className="btn btn-primary" onClick={loadExercises}>
          Retry
        </button>
      </div>
    );
  }

  return (
    <div>
      <div className="card">
        <h1>Parallel Programming Exercises</h1>
        <p>Master parallel programming in .NET with hands-on exercises. Complete the code and compare your performance with ideal solutions.</p>
        
        <div className="grid grid-2" style={{ marginTop: '20px' }}>
          <div>
            <label htmlFor="category-filter">Category:</label>
            <select 
              id="category-filter"
              value={selectedCategory} 
              onChange={(e) => setSelectedCategory(e.target.value)}
              style={{ width: '100%', padding: '8px', marginTop: '5px' }}
            >
              <option value="all">All Categories</option>
              {Object.values(ExerciseCategory).map(category => (
                <option key={category} value={category}>{category}</option>
              ))}
            </select>
          </div>
          
          <div>
            <label htmlFor="difficulty-filter">Difficulty:</label>
            <select 
              id="difficulty-filter"
              value={selectedDifficulty} 
              onChange={(e) => setSelectedDifficulty(e.target.value)}
              style={{ width: '100%', padding: '8px', marginTop: '5px' }}
            >
              <option value="all">All Levels</option>
              {Object.values(DifficultyLevel).map(difficulty => (
                <option key={difficulty} value={difficulty}>{difficulty}</option>
              ))}
            </select>
          </div>
        </div>
      </div>

      <div className="grid">
        {filteredExercises.map(exercise => (
          <div key={exercise.id} className={`card exercise-card ${getCategoryClass(exercise.category)}`}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '10px' }}>
              <h3 style={{ margin: 0 }}>{exercise.title}</h3>
              <span className={`difficulty-badge ${getDifficultyClass(exercise.difficulty)}`}>
                {Object.values(DifficultyLevel)[exercise.difficulty]}
              </span>
            </div>
            
            <p style={{ color: '#666', marginBottom: '15px' }}>{exercise.description}</p>
            
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
              <span style={{ fontSize: '14px', color: '#666' }}>
                Category: <strong>{Object.values(ExerciseCategory)[exercise.category]}</strong>
              </span>
              <span style={{ fontSize: '14px', color: '#666' }}>
                Expected: <strong>{exercise.expectedExecutionTimeMs}ms</strong>
              </span>
            </div>
            
            {canAccessExercise(Object.values(DifficultyLevel)[exercise.difficulty]) ? (
              <Link to={`/exercise/${exercise.id}`} className="btn btn-primary" style={{ width: '100%' }}>
                Start Exercise
              </Link>
            ) : (
              <div>
                <button className="btn btn-primary" disabled style={{ width: '100%', opacity: 0.5 }}>
                  🔒 Premium Only
                </button>
                <p style={{ fontSize: '12px', color: '#f59e0b', marginTop: '8px', textAlign: 'center' }}>
                  Upgrade to Premium to access this exercise
                </p>
              </div>
            )}
          </div>
        ))}
      </div>

      {filteredExercises.length === 0 && (
        <div className="card">
          <h3>No exercises found</h3>
          <p>Try adjusting your filters or check back later for new exercises.</p>
        </div>
      )}
    </div>
  );
};

export default ExerciseList;


