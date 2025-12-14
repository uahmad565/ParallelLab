import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/About.css';

const About: React.FC = () => {
  return (
    <div className="about-page">
      {/* Hero Section */}
      <section className="about-hero">
        <div className="about-hero-content">
          <h1>About ParallelLab</h1>
          <p className="about-hero-subtitle">
            Empowering developers to master parallel programming through hands-on practice
          </p>
        </div>
      </section>

      {/* Mission Section */}
      <section className="about-section">
        <div className="about-container">
          <h2>Our Mission</h2>
          <p className="about-mission-text">
            ParallelLab is dedicated to making parallel programming accessible and practical for developers
            of all skill levels. We believe that mastering concurrent programming is essential in today's
            multi-core computing environment, and the best way to learn is through hands-on practice with
            real-world exercises.
          </p>
          <p>
            Our platform provides a comprehensive learning environment where you can practice parallel
            programming concepts, compare your solutions with optimal implementations, and track your
            progress as you advance from beginner to expert.
          </p>
        </div>
      </section>

      {/* Features Section */}
      <section className="about-features">
        <div className="about-container">
          <h2>Key Features</h2>
          <div className="features-grid">
            <div className="feature-card">
              <div className="feature-icon">💻</div>
              <h3>Hands-On Exercises</h3>
              <p>
                Practice with real-world parallel programming challenges covering threads, tasks, LINQ,
                and more. Each exercise is designed to teach specific concepts and best practices.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-icon">⚡</div>
              <h3>Performance Comparison</h3>
              <p>
                See how your solution performs compared to ideal implementations. Get detailed performance
                metrics and learn optimization techniques to improve your code efficiency.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-icon">📊</div>
              <h3>Multiple Difficulty Levels</h3>
              <p>
                Progress from beginner to expert with exercises tailored to your skill level. Start with
                basic concepts and advance to complex parallel programming scenarios.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-icon">🚀</div>
              <h3>Real-Time Code Execution</h3>
              <p>
                Write and test your code directly in the browser. Our secure sandbox environment executes
                your code safely and provides instant feedback on correctness and performance.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-icon">📈</div>
              <h3>Progress Tracking</h3>
              <p>
                Monitor your learning journey with detailed statistics. Track solved exercises, view
                submission history, and see your improvement over time across different categories.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-icon">🎯</div>
              <h3>Comprehensive Topics</h3>
              <p>
                Learn all aspects of parallel programming including Threads, Tasks, LINQ, Parallel.For,
                Concurrent Collections, Async/Await, and PLINQ. Build a complete understanding of
                concurrent programming.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Learning Path Section */}
      <section className="about-section">
        <div className="about-container">
          <h2>Your Learning Path</h2>
          <div className="learning-path">
            <div className="path-step">
              <div className="step-number">1</div>
              <div className="step-content">
                <h3>Start with Basics</h3>
                <p>Begin with beginner-friendly exercises that introduce fundamental parallel programming concepts.</p>
              </div>
            </div>
            <div className="path-step">
              <div className="step-number">2</div>
              <div className="step-content">
                <h3>Practice Regularly</h3>
                <p>Solve exercises across different categories to build a comprehensive understanding.</p>
              </div>
            </div>
            <div className="path-step">
              <div className="step-number">3</div>
              <div className="step-content">
                <h3>Compare & Learn</h3>
                <p>Review ideal solutions to understand best practices and optimization techniques.</p>
              </div>
            </div>
            <div className="path-step">
              <div className="step-number">4</div>
              <div className="step-content">
                <h3>Master Advanced Topics</h3>
                <p>Challenge yourself with expert-level exercises and complex parallel programming scenarios.</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="about-cta">
        <div className="about-container">
          <h2>Ready to Start Learning?</h2>
          <p>Join ParallelLab today and begin your journey to mastering parallel programming.</p>
          <Link to="/exercises" className="btn-about-primary">
            Browse Exercises
          </Link>
        </div>
      </section>
    </div>
  );
};

export default About;

