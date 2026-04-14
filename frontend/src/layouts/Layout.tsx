import { Outlet, NavLink } from 'react-router-dom'
import { BookOpen, Newspaper, Home, LogIn } from 'lucide-react'
import './Layout.css'

export default function Layout() {
  return (
    <div className="layout">
      <nav className="navbar">
        <div className="navbar-brand">
          <BookOpen size={28} />
          <span>Hemeroteca</span>
        </div>
        <div className="navbar-links">
          <NavLink to="/" end><Home size={18} /> Inicio</NavLink>
          <NavLink to="/libros"><BookOpen size={18} /> Libros</NavLink>
          <NavLink to="/revistas"><Newspaper size={18} /> Revistas</NavLink>
          <NavLink to="/login"><LogIn size={18} /> Acceder</NavLink>
        </div>
      </nav>
      <main className="main-content">
        <Outlet />
      </main>
      <footer className="footer">
        <p>© 2024 Hemeroteca Digital</p>
      </footer>
    </div>
  )
}
