// User registration data
export interface UserRegistrationDto {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

// User login data
export interface UserLoginDto {
  username: string;
  password: string;
}

// User response data
export interface UserResponseDto {
  id: number;
  username: string;
  email: string;
}

// Authentication response data
export interface AuthResponseDto {
  token: string;
  user: UserResponseDto;
}

// Authentication context state
export interface AuthState {
  user: UserResponseDto | null;
  token: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
}

// Authentication context actions
export type AuthAction =
  | { type: 'LOGIN_SUCCESS'; payload: AuthResponseDto }
  | { type: 'LOGIN_FAILURE'; payload: string }
  | { type: 'LOGOUT' }
  | { type: 'CLEAR_ERROR' }
  | { type: 'SET_LOADING'; payload: boolean };

export interface User {
  id: number;
  username: string;
  email: string;
  role: string;
}

export interface LoginCredentials {
  username: string;
  password: string;
}

export interface RegisterCredentials extends LoginCredentials {
  email: string;
  confirmPassword: string;
}

export interface AuthContextType {
  state: AuthState;
  login: (credentials: LoginCredentials) => Promise<void>;
  register: (credentials: RegisterCredentials) => Promise<boolean>;
  logout: () => void;
  clearError: () => void;
} 