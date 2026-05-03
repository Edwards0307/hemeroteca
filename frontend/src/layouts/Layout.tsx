import { Outlet, NavLink, useNavigate } from 'react-router-dom'
import { BookOpen, Newspaper, Home, LogIn, LogOut, User } from 'lucide-react'
import { useAuth } from '../app/AuthContext'
import './Layout.css'

export default function Layout() {
  const { isAuthenticated, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/')
  }

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
          {isAuthenticated ? (
            <span className="navbar-user">
              <User size={16} /> Admin
              <button className="btn-logout" onClick={handleLogout}>
                <LogOut size={16} /> Salir
              </button>
            </span>
          ) : (
            <NavLink to="/login"><LogIn size={18} /> Acceder</NavLink>
          )}
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
