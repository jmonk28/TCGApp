import { React, useEffect, useContext } from 'react'
import { BrowserRouter, Routes, Route } from "react-router"
import Home from './pages/Home'
import Login from './pages/Login'
import Register from './pages/Register'
import Profile from './pages/Profile'
import { AuthContext } from './context/AuthContext'
import './App.css'

function App() {

    const { isLoggedIn } = useContext(AuthContext);

    useEffect(() => {
        document.title = 'Home - TCG App';
    }, []);

  return (
    <BrowserRouter>
       <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/login" element={isLoggedIn ? null : <Login />}  />
              <Route path="/register" element={isLoggedIn ? null : <Register />} />
              <Route path="/profile" element={isLoggedIn ? <Profile /> : null} />
       </Routes>
    </BrowserRouter>
  )
}

export default App
