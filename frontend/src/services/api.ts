import axios from 'axios';
import { Exercise, ExerciseSubmission, CodeSubmissionRequest, CodeSubmissionResponse } from '../types';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const exerciseApi = {
  getAll: async (): Promise<Exercise[]> => {
    const response = await api.get('/exercises');
    return response.data;
  },

  getById: async (id: number): Promise<Exercise> => {
    const response = await api.get(`/exercises/${id}`);
    return response.data;
  },

  getByCategory: async (category: string): Promise<Exercise[]> => {
    const response = await api.get(`/exercises/category/${category}`);
    return response.data;
  },

  getByDifficulty: async (difficulty: string): Promise<Exercise[]> => {
    const response = await api.get(`/exercises/difficulty/${difficulty}`);
    return response.data;
  },
};

export const submissionApi = {
  submit: async (request: CodeSubmissionRequest): Promise<CodeSubmissionResponse> => {
    const response = await api.post('/submissions/submit', request);
    return response.data;
  },

  getByExercise: async (exerciseId: number): Promise<ExerciseSubmission[]> => {
    const response = await api.get(`/submissions/exercise/${exerciseId}`);
    return response.data;
  },

  getByUser: async (userId: string): Promise<ExerciseSubmission[]> => {
    const response = await api.get(`/submissions/user/${userId}`);
    return response.data;
  },

  getById: async (id: number): Promise<ExerciseSubmission> => {
    const response = await api.get(`/submissions/${id}`);
    return response.data;
  },
};

export default api;


