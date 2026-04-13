import api from './api'
import { Libro } from '../types'

export const librosService = {
  getAll: (categoriaId?: number, buscar?: string) => {
    const params: Record<string, string | number> = {}
    if (categoriaId) params.categoriaId = categoriaId
    if (buscar) params.buscar = buscar
    return api.get<Libro[]>('/libros', { params })
  },
  getById: (id: number) => api.get<Libro>(`/libros/${id}`),
  create: (libro: Partial<Libro>) => api.post<Libro>('/libros', libro),
  update: (id: number, libro: Partial<Libro>) => api.put(`/libros/${id}`, libro),
  delete: (id: number) => api.delete(`/libros/${id}`),
  descargar: (id: number) => api.post(`/libros/${id}/descargar`),
}
