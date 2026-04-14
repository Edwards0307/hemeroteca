import api from './api'
import type { Categoria } from '../models'

export const categoriasService = {
  getAll: () => api.get<Categoria[]>('/categorias'),
  create: (categoria: Partial<Categoria>) => api.post<Categoria>('/categorias', categoria),
  update: (id: number, categoria: Partial<Categoria>) => api.put(`/categorias/${id}`, categoria),
  delete: (id: number) => api.delete(`/categorias/${id}`),
}
