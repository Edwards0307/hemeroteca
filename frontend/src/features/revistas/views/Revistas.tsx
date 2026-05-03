import { useState, useEffect, useCallback } from 'react'
import { Plus, Search, Newspaper, Download, Edit, Trash2, X } from 'lucide-react'
import { revistasService } from '../services'
import { categoriasService } from '../../../services/categorias'
import { useAuth } from '../../../app/AuthContext'
import type { Revista, Categoria } from '../../../models'
import '../../../features/libros/views/Libros.css'
import './Revistas.css'

type ModalMode = 'ver' | 'crear' | 'editar'

const formInit = {
  titulo: '',
  autor: '',
  lugarPublicacion: '',
  idioma: 'Español',
  ano: '',
  descripcion: '',
  categoriaId: '',
  tipoDocumento: 'PDF',
}

export default function Revistas() {
  const [revistas, setRevistas] = useState<Revista[]>([])
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [buscar, setBuscar] = useState('')
  const [filtroCategoria, setFiltroCategoria] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [modal, setModal] = useState<{ open: boolean; mode: ModalMode; item?: Revista }>({
    open: false,
    mode: 'ver',
  })
  const [form, setForm] = useState(formInit)
  const [saving, setSaving] = useState(false)
  const { isAdmin } = useAuth()

  const fetchRevistas = useCallback(async () => {
    setLoading(true)
    setError('')
    try {
      const res = await revistasService.getAll(
        filtroCategoria ? Number(filtroCategoria) : undefined,
        buscar || undefined
      )
      setRevistas(res.data)
    } catch {
      setError('No se pudieron cargar las revistas. Verifica la conexión con el servidor.')
    } finally {
      setLoading(false)
    }
  }, [buscar, filtroCategoria])

  useEffect(() => {
    fetchRevistas()
  }, [fetchRevistas])

  useEffect(() => {
    categoriasService.getAll().then(res => setCategorias(res.data)).catch(() => {})
  }, [])

  const openCrear = () => {
    setForm(formInit)
    setModal({ open: true, mode: 'crear' })
  }

  const openEditar = (revista: Revista) => {
    setForm({
      titulo: revista.titulo,
      autor: revista.autor ?? '',
      lugarPublicacion: revista.lugarPublicacion ?? '',
      idioma: revista.idioma ?? 'Español',
      ano: revista.ano?.toString() ?? '',
      descripcion: revista.descripcion ?? '',
      categoriaId: revista.categoriaId.toString(),
      tipoDocumento: revista.tipoDocumento ?? 'PDF',
    })
    setModal({ open: true, mode: 'editar', item: revista })
  }

  const openVer = (revista: Revista) => {
    setModal({ open: true, mode: 'ver', item: revista })
  }

  const closeModal = () => setModal({ open: false, mode: 'ver' })

  const handleSave = async () => {
    if (!form.titulo || !form.categoriaId) return
    setSaving(true)
    try {
      const payload = {
        titulo: form.titulo,
        autor: form.autor || undefined,
        lugarPublicacion: form.lugarPublicacion || undefined,
        idioma: form.idioma || undefined,
        ano: form.ano ? Number(form.ano) : undefined,
        descripcion: form.descripcion || undefined,
        categoriaId: Number(form.categoriaId),
        tipoDocumento: form.tipoDocumento || undefined,
      }
      if (modal.mode === 'crear') {
        await revistasService.create(payload)
      } else if (modal.mode === 'editar' && modal.item) {
        await revistasService.update(modal.item.id, payload)
      }
      closeModal()
      fetchRevistas()
    } catch {
      // modal stays open on error
    } finally {
      setSaving(false)
    }
  }

  const handleDelete = async (id: number) => {
    if (!confirm('¿Estás seguro de eliminar esta revista?')) return
    try {
      await revistasService.delete(id)
      closeModal()
      fetchRevistas()
    } catch {
      alert('No se pudo eliminar la revista')
    }
  }

  return (
    <div className="catalogo">
      <div className="catalogo-header">
        <div>
          <h1>Hemeroteca Digital</h1>
          <p>Explora nuestra colección de revistas y publicaciones</p>
        </div>
        {isAdmin && (
          <button className="btn-agregar" onClick={openCrear}>
            <Plus size={18} /> Agregar Revista
          </button>
        )}
      </div>

      <div className="filtros">
        <div className="search-box">
          <Search size={18} />
          <input
            type="text"
            placeholder="Buscar por título o publicación..."
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

      {loading && <div className="estado-msg">Cargando revistas...</div>}
      {error && <div className="estado-msg error">{error}</div>}
      {!loading && !error && revistas.length === 0 && (
        <div className="estado-msg">No se encontraron revistas.</div>
      )}

      <div className="items-grid">
        {revistas.map(revista => (
          <div key={revista.id} className="item-card revista-card" onClick={() => openVer(revista)}>
            <div className="card-icon revista-icon">
              <Newspaper size={36} color="#e2b96f" />
            </div>
            <div className="card-body">
              <span className="card-badge">{revista.categoria?.nombre ?? 'Sin categoría'}</span>
              <h3>{revista.titulo}</h3>
              {revista.autor && <p className="card-autor">{revista.autor}</p>}
              <div className="card-meta">
                {revista.ano && <span>{revista.ano}</span>}
                {revista.idioma && <span>{revista.idioma}</span>}
                <span className="downloads"><Download size={13} /> {revista.totalDescargas}</span>
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
                {modal.mode === 'crear' && 'Agregar Revista'}
                {modal.mode === 'editar' && 'Editar Revista'}
              </h2>
              <button className="btn-close" onClick={closeModal}><X size={20} /></button>
            </div>

            {modal.mode === 'ver' && modal.item ? (
              <div className="modal-detalle">
                <div className="detalle-grid">
                  {modal.item.autor && <><dt>Autor / Editorial</dt><dd>{modal.item.autor}</dd></>}
                  {modal.item.lugarPublicacion && <><dt>Publicado en</dt><dd>{modal.item.lugarPublicacion}</dd></>}
                  {modal.item.ano && <><dt>Año</dt><dd>{modal.item.ano}</dd></>}
                  {modal.item.idioma && <><dt>Idioma</dt><dd>{modal.item.idioma}</dd></>}
                  {modal.item.tipoDocumento && <><dt>Formato</dt><dd>{modal.item.tipoDocumento}</dd></>}
                  <dt>Descargas</dt><dd>{modal.item.totalDescargas}</dd>
                  {modal.item.categoria && <><dt>Categoría</dt><dd>{modal.item.categoria.nombre}</dd></>}
                </div>
                {modal.item.descripcion && (
                  <p className="detalle-desc">{modal.item.descripcion}</p>
                )}
                {isAdmin && (
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
                      placeholder="Título de la revista"
                    />
                  </div>
                  <div className="form-group">
                    <label>Autor / Editorial</label>
                    <input
                      value={form.autor}
                      onChange={e => setForm(f => ({ ...f, autor: e.target.value }))}
                      placeholder="Nombre del autor o editorial"
                    />
                  </div>
                  <div className="form-group">
                    <label>Lugar de publicación</label>
                    <input
                      value={form.lugarPublicacion}
                      onChange={e => setForm(f => ({ ...f, lugarPublicacion: e.target.value }))}
                      placeholder="Ciudad, País"
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
                      placeholder="Descripción de la revista"
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
