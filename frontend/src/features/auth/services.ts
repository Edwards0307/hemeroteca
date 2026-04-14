import api from '../../services/api'
import type { LoginRequest } from '../../models'

export const authService = {
  login: (data: LoginRequest) => api.post<{ token: string }>('/auth/login', data),
  registro: (data: LoginRequest) => api.post('/auth/registro', data),
}
