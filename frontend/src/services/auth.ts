import api from './api'
import { LoginRequest } from '../types'

export const authService = {
  login: (data: LoginRequest) => api.post<{ token: string }>('/auth/login', data),
  registro: (data: LoginRequest) => api.post('/auth/registro', data),
}
