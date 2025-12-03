export interface User {
  email: string;
}

export interface AuthResponse {
  token: string;
  email: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
}

export interface QuoteRequest {
  mode: string;
  movementType: string;
  incoterms: string;
  packageType: string;
  country: string;
  city: string;
  currency: string;
  amount: number;
}

export interface QuoteResponse {
  id: string;
  userEmail: string;
  country: string;
  city: string;
  mode: string;
  movementType: string;
  incoterms: string;
  packageType: string;
  originalCurrency: string;
  originalAmount: number;
  usdAmount: number;
  createdDate: string;
}
