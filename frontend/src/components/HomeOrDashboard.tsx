import React from 'react';
import { useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import Home from './Home';
import Dashboard from './Dashboard';

const HomeOrDashboard: React.FC = () => {
  const { isAuthenticated, isLoading } = useAuth();
  const location = useLocation();

  if (isLoading) {
    return (
      <div className="loading">
        <div className="spinner"></div>
      </div>
    );
  }

  // If at /dashboard
  if (location.pathname === '/dashboard') {
    // Anonymous users trying to access dashboard - show them login prompt
    if (!isAuthenticated) {
      return <Home />;
    }
    // Authenticated users see dashboard
    return <Dashboard />;
  }

  // If at / (root)
  // Authenticated users see dashboard, anonymous users see home
  return isAuthenticated ? <Dashboard /> : <Home />;
};

export default HomeOrDashboard;

