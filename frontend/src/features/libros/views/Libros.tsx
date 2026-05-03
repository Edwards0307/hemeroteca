import { useState, useEffect, useCallback } from 'react'
import { Plus, Search, BookOpen, Download, Edit, Trash2, X } from 'lucide-react'
import { librosService } from '../services'
import { categoriasService } from '../../../services/categorias'
import { useAuth } from '../../../app/AuthContext'
import type { Libro, Categoria } from '../../../models'
import './Libros.css'

type ModalMode = 'ver' | 'crear' | 'editar'

const formInit = {
  titulo: '',
  autor: '',
  editorial: '',
  idioma: 'Español',
  paginas: '',
  ano: '',
  descripcion: '',
  categoriaId: '',
  tipoDocumento: 'PDF',
}

export default function Libros() {
  const [libros, setLibros] = useState<Libro[]>([])
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [buscar, setBuscar] = useState('')
  const [filtroCategoria, setFiltroCategoria] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [modal, setModal] = useState<{ open: boolean; mode: ModalMode; item?: Libro }>({
    open: false,
    mode: 'ver',
  })
  const [form, setForm] = useState(formInit)
  const [saving, setSaving] = useState(false)
  const { isAuthenticated } = useAuth()

  const fetchLibros = useCallback(async () => {
    setLoading(true)
    setError('')
    try {
      const res = await librosService.getAll(
        filtroCategoria ? Number(filtroCategoria) : undefined,
        buscar || undefined
      )
      setLibros(res.data)
    } catch {
      setError('No se pudieron cargar los libros. Verifica la conexión con el servidor.')
    } finally {
      setLoading(false)
    }
  }, [buscar, filtroCategoria])

  useEffect(() => {
    fetchLibros()
  }, [fetchLibros])

  useEffect(() => {
    categoriasService.getAll().then(res => setCategorias(res.data)).catch(() => {})
  }, [])

  const openCrear = () => {
    setForm(formInit)
    setModal({ open: true, mode: 'crear' })
  }

  const openEditar = (libro: Libro) => {
    setForm({
      titulo: libro.titulo,
      autor: libro.autor ?? '',
      editorial: libro.editorial ?? '',
      idioma: libro.idioma ?? 'Español',
      paginas: libro.paginas?.toString() ?? '',
      ano: libro.ano?.toString() ?? '',
      descripcion: libro.descripcion ?? '',
      categoriaId: libro.categoriaId.toString(),
      tipoDocumento: libro.tipoDocumento ?? 'PDF',
    })
    setModal({ open: true, mode: 'editar', item: libro })
  }

  const openVer = (libro: Libro) => {
    setModal({ open: true, mode: 'ver', item: libro })
  }

  const closeModal = () => setModal({ open: false, mode: 'ver' })

  const handleSave = async () => {
    if (!form.titulo || !form.categoriaId) return
    setSaving(true)
    try {
      const payload = {
        titulo: form.titulo,
        autor: form.autor || undefined,
        editorial: form.editorial || undefined,
        idioma: form.idioma || undefined,
        paginas: form.paginas ? Number(form.paginas) : undefined,
        ano: form.ano ? Number(form.ano) : undefined,
        descripcion: form.descripcion || undefined,
        categoriaId: Number(form.categoriaId),
        tipoDocumento: form.tipoDocumento || undefined,
      }
      if (modal.mode === 'crear') {
        await librosService.create(payload)
      } else if (modal.mode === 'editar' && modal.item) {
        await librosService.update(modal.item.id, payload)
      }
      closeModal()
      fetchLibros()
    } catch {
      // modal stays open on error
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: number) => {
    if (!confirm('¿Estás seguro de eliminar este libro?')) return
    try {
      await librosService.delete(id)
      closeModal()
      fetchLibros()
    } catch {
      alert('No se pudo eliminar el libro')
    }
  }

  return (
    <div className="catalogo">
      <div className="catalogo-header">
        <div>
          <h1>Biblioteca Digital</h1>
          <p>Explora nuestra colección de libros</p>
        </div>
        {isAuthenticated && (
          <button className="btn-agregar" onClick={openCrear}>
            <Plus size={18} /> Agregar Libro
          </button>
        )}
      </div>

      <div className="filtros">
        <div className="search-box">
          <Search size={18} />
          <input
            type="text"
            placeholder="Buscar por título o autor..."
            value={buscar}
            onChange={e => setBuscar(e.target.value)}
          />
        </div>
        <select value={filtroCategoria} onChange={e => setFiltroCategoria(e.target.value)}>
          <option value="">Todas las categorías</option>
          {categorias.map(c => (
            <option key={c.id} value={c.id}>{c.nombre}</option>
          ))}
        </select>
      </div>

      {loading && <div className="estado-msg">Cargando libros...</div>}
      {error && <div className="estado-msg error">{error}</div>}
      {!loading && !error && libros.length === 0 && (
        <div className="estado-msg">No se encontraron libros.</div>
      )}

      <div className="items-grid">
        {libros.map(libro => (
          <div key={libro.id} className="item-card" onClick={() => openVer(libro)}>
            <div className="card-icon">
              <BookOpen size={36} color="#e2b96f" />
            </div>
            <div className="card-body">
              <span className="card-badge">{libro.categoria?.nombre ?? 'Sin categoría'}</span>
              <h3>{libro.titulo}</h3>
              {libro.autor && <p className="card-autor">{libro.autor}</p>}
              <div className="card-meta">
                {libro.ano && <span>{libro.ano}</span>}
                {libro.idioma && <span>{libro.idioma}</span>}
                <span className="downloads"><Download size={13} /> {libro.totalDescargas}</span>
              </div>
            </div>
          </div>
        ))}
      </div>

      {modal.open && (
        <div className="modal-overlay" onClick={closeModal}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <div className="modal-header">
              <h2>
                {modal.mode === 'ver' && modal.item?.titulo}
                {modal.mode === 'crear' && 'Agregar Libro'}
                {modal.mode === 'editar' && 'Editar Libro'}
              </h2>
              <button className="btn-close" onClick={closeModal}><X size={20} /></button>
            </div>

            {modal.mode === 'ver' && modal.item ? (
              <div className="modal-detalle">
                <div className="detalle-grid">
                  {modal.item.autor && <><dt>Autor</dt><dd>{modal.item.autor}</dd></>}
                  {modal.item.editorial && <><dt>Editorial</dt><dd>{modal.item.editorial}</dd></>}
                  {modal.item.ano && <><dt>Año</dt><dd>{modal.item.ano}</dd></>}
                  {modal.item.idioma && <><dt>Idioma</dt><dd>{modal.item.idioma}</dd></>}
                  {modal.item.paginas && <><dt>Páginas</dt><dd>{modal.item.paginas}</dd></>}
                  {modal.item.tipoDocumento && <><dt>Formato</dt><dd>{modal.item.tipoDocumento}</dd></>}
                  <dt>Descargas</dt><dd>{modal.item.totalDescargas}</dd>
                  {modal.item.categoria && <><dt>Categoría</dt><dd>{modal.item.categoria.nombre}</dd></>}
                </div>
                {modal.item.descripcion && (
                  <p className="detalle-desc">{modal.item.descripcion}</p>
                )}
                {isAuthenticated && (
                  <div className="modal-actions">
                    <button className="btn-editar" onClick={() => openEditar(modal.item!)}>
                      <Edit size={16} /> Editar
                    </button>
                    <button className="btn-eliminar" onClick={() => handleDelete(modal.item!.id)}>
                      <Trash2 size={16} /> Eliminar
                    </button>
                  </div>
                )}
              </div>
            ) : (
              <div className="modal-form">
                <div className="form-grid">
                  <div className="form-group full">
                    <label>Título *</label>
                    <input
                      value={form.titulo}
                      onChange={e => setForm(f => ({ ...f, titulo: e.target.value }))}
                      placeholder="Título del libro"
                    />
                  </div>
                  <div className="form-group">
                    <label>Autor</label>
                    <input
                      value={form.autor}
                      onChange={e => setForm(f => ({ ...f, autor: e.target.value }))}
                      placeholder="Nombre del autor"
                    />
                  </div>
                  <div className="form-group">
                    <label>Editorial</label>
                    <input
                      value={form.editorial}
                      onChange={e => setForm(f => ({ ...f, editorial: e.target.value }))}
                      placeholder="Editorial"
                    />
                  </div>
                  <div className="form-group">
                    <label>Año</label>
                    <input
                      type="number"
                      value={form.ano}
                      onChange={e => setForm(f => ({ ...f, ano: e.target.value }))}
                      placeholder="Año de publicación"
                    />
                  </div>
                  <div className="form-group">
                    <label>Páginas</label>
                    <input
                      type="number"
                      value={form.paginas}
                      onChange={e => setForm(f => ({ ...f, paginas: e.target.value }))}
                      placeholder="Número de páginas"
                    />
                  </div>
                  <div className="form-group">
                    <label>Idioma</label>
                    <input
                      value={form.idioma}
                      onChange={e => setForm(f => ({ ...f, idioma: e.target.value }))}
                      placeholder="Idioma"
                    />
                  </div>
                  <div className="form-group">
                    <label>Categoría *</label>
                    <select
                      value={form.categoriaId}
                      onChange={e => setForm(f => ({ ...f, categoriaId: e.target.value }))}
                    >
                      <option value="">Seleccionar categoría...</option>
                      {categorias.map(c => (
                        <option key={c.id} value={c.id}>{c.nombre}</option>
                      ))}
                    </select>
                  </div>
                  <div className="form-group full">
                    <label>Descripción</label>
                    <textarea
                      value={form.descripcion}
                      onChange={e => setForm(f => ({ ...f, descripcion: e.target.value }))}
                      placeholder="Descripción del libro"
                      rows={3}
                    />
                  </div>
                </div>
                <div className="modal-actions">
                  <button className="btn-cancelar" onClick={closeModal}>Cancelar</button>
                  <button
                    className="btn-guardar"
                    onClick={handleSave}
                    disabled={saving || !form.titulo || !form.categoriaId}
                  >
                    {saving ? 'Guardando...' : 'Guardar'}
                  </button>
                </div>
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  )
}
