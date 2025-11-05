import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './components/Home';
import ExerciseList from './components/ExerciseList';
import ExerciseDetail from './components/ExerciseDetail';
import SubmissionHistory from './components/SubmissionHistory';
import './App.css';

function App() {
  return (
    <Router future={{ v7_startTransition: true, v7_relativeSplatPath: true }}>
      <div className="App">
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/exercises" element={
            <div className="container">
              <ExerciseList />
            </div>
          } />
          <Route path="/exercise/:id" element={
            <div className="container">
              <ExerciseDetail />
            </div>
          } />
          <Route path="/submissions" element={
            <div className="container">
              <SubmissionHistory />
            </div>
          } />
        </Routes>
      </div>
    </Router>
  );
}

export default App;


