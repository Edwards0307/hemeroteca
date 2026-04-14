import { BookOpen, Newspaper, Search } from 'lucide-react'
import { useNavigate } from 'react-router-dom'
import './Home.css'

export default function Home() {
  const navigate = useNavigate()

  return (
    <div className="home">
      <div className="hero">
        <h1>Bienvenido a la Hemeroteca Digital</h1>
        <p>Accede a nuestra colección de libros y revistas digitales</p>
        <div className="hero-search">
          <Search size={20} />
          <input type="text" placeholder="Buscar libros o revistas..." />
          <button>Buscar</button>
        </div>
      </div>
      <div className="cards-grid">
        <div className="card" onClick={() => navigate('/libros')}>
          <BookOpen size={48} color="#e2b96f" />
          <h2>Biblioteca</h2>
          <p>Explora nuestra colección de libros digitales organizados por categoría</p>
          <button className="btn-primary">Ver Libros</button>
        </div>
        <div className="card" onClick={() => navigate('/revistas')}>
          <Newspaper size={48} color="#e2b96f" />
          <h2>Hemeroteca</h2>
          <p>Accede a revistas y publicaciones periódicas digitalizadas</p>
          <button className="btn-primary">Ver Revistas</button>
        </div>
      </div>
    </div>
  )
}
