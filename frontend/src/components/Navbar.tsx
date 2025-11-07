import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const Navbar: React.FC = () => {
  const { user, logout, isAuthenticated } = useAuth();
  const [showUserMenu, setShowUserMenu] = useState(false);

  const handleLogout = () => {
    logout();
    setShowUserMenu(false);
  };

  const getRoleBadge = (role: string) => {
    if (role === 'Admin') return { text: 'ADMIN', color: '#dc2626' };
    if (role === 'PremiumUser') return { text: 'PREMIUM', color: '#10b981' };
    return { text: 'FREE', color: '#64748b' };
  };

  return (
    <nav className="navbar">
      <div className="navbar-content">
        <Link to="/" style={{ textDecoration: 'none' }}>
          <h1>ParallelLab</h1>
        </Link>
        <div className="nav-links">
          <Link to="/">Home</Link>
          {isAuthenticated && <Link to="/exercises">Practice</Link>}
          {isAuthenticated && <Link to="/submissions">Submissions</Link>}
          
          {isAuthenticated ? (
            <div className="user-menu-container">
              <button 
                className="user-menu-trigger"
                onClick={() => setShowUserMenu(!showUserMenu)}
              >
                <div className="user-avatar">
                  {user?.username.charAt(0).toUpperCase()}
                </div>
                <span className="user-name">{user?.username}</span>
                <span 
                  className="role-badge"
                  style={{ background: getRoleBadge(user?.role || '').color }}
                >
                  {getRoleBadge(user?.role || '').text}
                </span>
              </button>
              
              {showUserMenu && (
                <div className="user-menu-dropdown">
                  <div className="user-menu-header">
                    <div className="user-menu-name">{user?.fullName}</div>
                    <div className="user-menu-email">{user?.email}</div>
                  </div>
                  <div className="user-menu-divider"></div>
                  <button className="user-menu-item" onClick={handleLogout}>
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                      <path d="M9 21H5a2 2 0 01-2-2V5a2 2 0 012-2h4" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                      <polyline points="16 17 21 12 16 7" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                      <line x1="21" y1="12" x2="9" y2="12" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                    </svg>
                    Logout
                  </button>
                </div>
              )}
            </div>
          ) : (
            <>
              <Link to="/login" className="nav-link-button">Sign In</Link>
              <Link to="/register" className="nav-link-button-primary">Sign Up</Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;


