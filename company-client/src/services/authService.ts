import axios from 'axios';
import { UserLoginDto, UserRegistrationDto, AuthResponseDto, UserResponseDto } from '../types/auth';

const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:5001/api';

// Create axios instance with default config
const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add a request interceptor to add the auth token to requests
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Add a response interceptor to handle token expiration
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      // Token expired or invalid, log out the user
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

const authService = {
  // Register a new user
  register: async (userData: UserRegistrationDto): Promise<AuthResponseDto> => {
    const response = await api.post<AuthResponseDto>('/auth/register', userData);
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('user', JSON.stringify(response.data.user));
    }
    return response.data;
  },

  // Login user
  login: async (credentials: UserLoginDto): Promise<AuthResponseDto> => {
    const response = await api.post<AuthResponseDto>('/auth/login', credentials);
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('user', JSON.stringify(response.data.user));
    }
    return response.data;
  },

  // Logout user
  logout: (): void => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  // Get current user
  getCurrentUser: async (): Promise<UserResponseDto | null> => {
    try {
      const response = await api.get<UserResponseDto>('/auth/me');
      return response.data;
    } catch (error) {
      return null;
    }
  },

  // Check if user is authenticated
  isAuthenticated: (): boolean => {
    return !!localStorage.getItem('token');
  },

  // Get token
  getToken: (): string | null => {
    return localStorage.getItem('token');
  },

  // Get user from localStorage
  getUser: (): UserResponseDto | null => {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      return JSON.parse(userStr);
    }
    return null;
  },
};

export default authService; 