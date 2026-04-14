import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Layout from './layouts/Layout'
import Home from './views/Home'
import Libros from './features/libros/views/Libros'
import Revistas from './features/revistas/views/Revistas'
import Login from './features/auth/views/Login'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="libros" element={<Libros />} />
          <Route path="revistas" element={<Revistas />} />
          <Route path="login" element={<Login />} />
          <Route path="*" element={<Navigate to="/" />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App
