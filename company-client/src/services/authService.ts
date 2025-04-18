import { api } from './api';
import { UserLoginDto, UserRegistrationDto, AuthResponseDto, UserResponseDto } from '../types/auth';

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