import api from './api'
import { Revista } from '../types'

export const revistasService = {
  getAll: (categoriaId?: number, buscar?: string) => {
    const params: Record<string, string | number> = {}
    if (categoriaId) params.categoriaId = categoriaId
    if (buscar) params.buscar = buscar
    return api.get<Revista[]>('/revistas', { params })
  },
  getById: (id: number) => api.get<Revista>(`/revistas/${id}`),
  create: (revista: Partial<Revista>) => api.post<Revista>('/revistas', revista),
  update: (id: number, revista: Partial<Revista>) => api.put(`/revistas/${id}`, revista),
  delete: (id: number) => api.delete(`/revistas/${id}`),
  descargar: (id: number) => api.post(`/revistas/${id}/descargar`),
}
