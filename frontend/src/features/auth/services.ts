import api from '../../services/api'
import type { LoginRequest } from '../../models'

export const authService = {
  login: (data: LoginRequest) => api.post<{ token: string; isAdmin: boolean }>('/auth/login', data),
  registro: (data: LoginRequest) => api.post('/auth/registro', data),
}
