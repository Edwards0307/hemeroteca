export interface Categoria {
  id: number
  nombre: string
}

export interface Libro {
  id: number
  codigo?: string
  titulo: string
  autor?: string
  editorial?: string
  idioma?: string
  paginas?: number
  ano?: number
  descripcion?: string
  rutaImagen?: string
  rutaArchivo?: string
  tipoDocumento?: string
  fechaPublicacion?: string
  fechaRegistro: string
  totalDescargas: number
  categoriaId: number
  categoria?: Categoria
}

export interface Revista {
  id: number
  titulo: string
  autor?: string
  lugarPublicacion?: string
  idioma?: string
  ano?: number
  descripcion?: string
  rutaArchivo?: string
  tipoDocumento?: string
  fechaPublicacion?: string
  fechaRegistro: string
  totalDescargas: number
  categoriaId: number
  categoria?: Categoria
}

export interface Usuario {
  id: number
  username: string
  fechaCreacion: string
}

export interface LoginRequest {
  username: string
  password: string
}
