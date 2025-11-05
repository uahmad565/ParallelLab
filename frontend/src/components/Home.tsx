import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/Home.css';

const Home: React.FC = () => {
  return (
    <div className="home-page">
      {/* Hero Section */}
      <section className="hero-section">
        <div className="hero-container">
          <div className="hero-text">
            <h1 className="hero-title">
              Master Parallel Programming
            </h1>
            <p className="hero-description">
              Solve real-world problems. Eliminate deadlocks. Utilize all CPU cores. 
              Accelerate your career with hands-on practice in concurrent programming.
            </p>
            <div className="hero-actions">
              <Link to="/exercises" className="btn-hero-primary">
                Start Practicing
              </Link>
              <a href="#features" className="btn-hero-secondary">
                Learn More
              </a>
            </div>
            <div className="hero-stats">
              <div className="hero-stat">
                <span className="stat-value">100+</span>
                <span className="stat-label">Challenges</span>
              </div>
              <div className="hero-stat">
                <span className="stat-value">6</span>
                <span className="stat-label">Topics</span>
              </div>
              <div className="hero-stat">
                <span className="stat-value">∞</span>
                <span className="stat-label">Test Cases</span>
              </div>
            </div>
          </div>
          <div className="hero-visual">
            <div className="code-window">
              <div className="code-window-header">
                <div className="window-controls">
                  <span className="control red"></span>
                  <span className="control yellow"></span>
                  <span className="control green"></span>
                </div>
                <span className="window-title">Program.cs</span>
              </div>
              <div className="code-content">
                <pre>{`using System;
using System.Threading.Tasks;

class Solution 
{
    static void Main()
    {
        Parallel.For(0, 100, i =>
        {
            Console.WriteLine($"Core {i}");
        });
    }
}`}</pre>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="features-section" id="features">
        <div className="features-container">
          <div className="section-header">
            <h2>Why Learn Parallel Programming?</h2>
            <p>Essential skills for modern software development</p>
          </div>
          
          <div className="features-grid">
            <div className="feature-item">
              <div className="feature-icon-box">
                <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                </svg>
              </div>
              <h3>Consume All CPU Cores</h3>
              <p>Learn to write applications that utilize 100% of your hardware capabilities, not just a single core.</p>
            </div>

            <div className="feature-item">
              <div className="feature-icon-box">
                <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <path d="M9 11l3 3L22 4" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                  <path d="M21 12v7a2 2 0 01-2 2H5a2 2 0 01-2-2V5a2 2 0 012-2h11" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                </svg>
              </div>
              <h3>Solve Real Problems</h3>
              <p>Work on practical scenarios: data processing, web services, file operations, and more.</p>
            </div>

            <div className="feature-item">
              <div className="feature-icon-box">
                <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <rect x="3" y="11" width="18" height="11" rx="2" ry="2" strokeWidth="2"/>
                  <path d="M7 11V7a5 5 0 0110 0v4" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                </svg>
              </div>
              <h3>Master Deadlock Prevention</h3>
              <p>Understand race conditions, deadlocks, and thread safety. Write robust concurrent code.</p>
            </div>

            <div className="feature-item">
              <div className="feature-icon-box">
                <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <path d="M16 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                  <circle cx="8.5" cy="7" r="4" strokeWidth="2"/>
                  <path d="M20 8v6M23 11h-6" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                </svg>
              </div>
              <h3>Career Growth</h3>
              <p>Stand out in technical interviews. Parallel programming expertise commands premium salaries.</p>
            </div>

            <div className="feature-item">
              <div className="feature-icon-box">
                <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <polyline points="22 12 18 12 15 21 9 3 6 12 2 12" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                </svg>
              </div>
              <h3>Performance Metrics</h3>
              <p>Get instant feedback on execution time and efficiency. Compare with optimal solutions.</p>
            </div>

            <div className="feature-item">
              <div className="feature-icon-box">
                <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <polyline points="16 18 22 12 16 6" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                  <polyline points="8 6 2 12 8 18" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                </svg>
              </div>
              <h3>Interactive Editor</h3>
              <p>Write, test, and debug code in a full-featured editor with syntax highlighting.</p>
            </div>
          </div>
        </div>
      </section>

      {/* Topics Section */}
      <section className="topics-section">
        <div className="topics-container">
          <div className="section-header">
            <h2>Practice Topics</h2>
            <p>Comprehensive coverage of parallel programming concepts</p>
          </div>
          
          <div className="topics-grid">
            <Link to="/exercises" className="topic-card">
              <div className="topic-header">
                <div className="topic-icon">🧵</div>
                <h3>Threads</h3>
              </div>
              <p>Master the fundamentals of multi-threading, synchronization, and thread safety</p>
              <div className="topic-footer">
                <span className="topic-count">15+ challenges</span>
                <span className="topic-arrow">→</span>
              </div>
            </Link>

            <Link to="/exercises" className="topic-card">
              <div className="topic-header">
                <div className="topic-icon">⚡</div>
                <h3>Tasks</h3>
              </div>
              <p>Learn Task Parallel Library, async/await, and modern asynchronous patterns</p>
              <div className="topic-footer">
                <span className="topic-count">20+ challenges</span>
                <span className="topic-arrow">→</span>
              </div>
            </Link>

            <Link to="/exercises" className="topic-card">
              <div className="topic-header">
                <div className="topic-icon">🔍</div>
                <h3>PLINQ</h3>
              </div>
              <p>Parallel LINQ queries for efficient data processing and transformations</p>
              <div className="topic-footer">
                <span className="topic-count">12+ challenges</span>
                <span className="topic-arrow">→</span>
              </div>
            </Link>

            <Link to="/exercises" className="topic-card">
              <div className="topic-header">
                <div className="topic-icon">🔄</div>
                <h3>Parallel.For</h3>
              </div>
              <p>Data parallelism with Parallel.For, custom partitioning, and optimization</p>
              <div className="topic-footer">
                <span className="topic-count">18+ challenges</span>
                <span className="topic-arrow">→</span>
              </div>
            </Link>

            <Link to="/exercises" className="topic-card">
              <div className="topic-header">
                <div className="topic-icon">📦</div>
                <h3>Concurrent Collections</h3>
              </div>
              <p>Thread-safe collections, producer-consumer patterns, lock-free programming</p>
              <div className="topic-footer">
                <span className="topic-count">10+ challenges</span>
                <span className="topic-arrow">→</span>
              </div>
            </Link>

            <Link to="/exercises" className="topic-card">
              <div className="topic-header">
                <div className="topic-icon">🎯</div>
                <h3>Async/Await</h3>
              </div>
              <p>Asynchronous programming, I/O operations, and cancellation patterns</p>
              <div className="topic-footer">
                <span className="topic-count">25+ challenges</span>
                <span className="topic-arrow">→</span>
              </div>
            </Link>
          </div>
        </div>
      </section>

      {/* How It Works Section */}
      <section className="how-it-works-section">
        <div className="how-container">
          <div className="section-header">
            <h2>How It Works</h2>
            <p>Your path to parallel programming mastery</p>
          </div>
          
          <div className="steps-grid">
            <div className="step-card">
              <div className="step-number">1</div>
              <h3>Choose a Challenge</h3>
              <p>Select from exercises covering threads, tasks, PLINQ, and more</p>
            </div>

            <div className="step-card">
              <div className="step-number">2</div>
              <h3>Write Your Solution</h3>
              <p>Code in our interactive editor with full C# support and IntelliSense</p>
            </div>

            <div className="step-card">
              <div className="step-number">3</div>
              <h3>Run Test Cases</h3>
              <p>Execute your code against multiple test cases and see instant results</p>
            </div>

            <div className="step-card">
              <div className="step-number">4</div>
              <h3>Analyze Performance</h3>
              <p>Get detailed metrics, compare with ideal solutions, and improve</p>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="cta-section">
        <div className="cta-container">
          <h2>Start Your Journey Today</h2>
          <p>Join developers worldwide mastering the art of parallel programming</p>
          <Link to="/exercises" className="btn-cta">
            Practice Now
          </Link>
        </div>
      </section>
    </div>
  );
};

export default Home;

