import React from 'react';
import { Link } from 'react-router-dom';

const Navbar: React.FC = () => {
  return (
    <nav className="navbar">
      <div className="navbar-content">
        <Link to="/" style={{ textDecoration: 'none' }}>
          <h1>ParallelLab</h1>
        </Link>
        <div className="nav-links">
          <Link to="/">Home</Link>
          <Link to="/exercises">Practice</Link>
          <Link to="/submissions">Submissions</Link>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;


