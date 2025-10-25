import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import ExerciseList from './components/ExerciseList';
import ExerciseDetail from './components/ExerciseDetail';
import SubmissionHistory from './components/SubmissionHistory';
import './App.css';

function App() {
  return (
    <Router future={{ v7_startTransition: true, v7_relativeSplatPath: true }}>
      <div className="App">
        <Navbar />
        <div className="container">
          <Routes>
            <Route path="/" element={<ExerciseList />} />
            <Route path="/exercise/:id" element={<ExerciseDetail />} />
            <Route path="/submissions" element={<SubmissionHistory />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;


