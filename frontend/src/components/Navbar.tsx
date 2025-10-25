import React from 'react';
import { Link } from 'react-router-dom';

const Navbar: React.FC = () => {
  return (
    <nav className="navbar">
      <div className="container">
        <div className="navbar-content">
          <Link to="/" style={{ textDecoration: 'none', color: 'white' }}>
            <h1>ParallelLab</h1>
          </Link>
          <div className="nav-links">
            <Link to="/">Exercises</Link>
            <Link to="/submissions">My Submissions</Link>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;


