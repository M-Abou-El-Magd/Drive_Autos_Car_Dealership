import api from './axios';

export async function login(credentials) {
  const response = await api.post('/auth/login', credentials);
  return response.data;
}

export async function register(credentials) {
  const response = await api.post('/users', credentials);
  return response.data;
}
