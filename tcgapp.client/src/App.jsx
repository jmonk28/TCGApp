import { useState, useEffect, useContext } from 'react'
import { BrowserRouter, Routes, Route } from "react-router"
import Home from './pages/Home'
import Login from './pages/Login'
import Register from './pages/Register'
import { AuthContext } from './context/AuthContext'
import './App.css'

function App() {

    const { accessToken } = useContext(AuthContext);
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        document.title = 'Home - TCG App';
    }, []);

    useEffect(() => {
        if (accessToken != null) setIsLoggedIn(true);
        else setIsLoggedIn(false);
    }, [accessToken])

  return (
    <BrowserRouter>
       <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/login" element={isLoggedIn ? null : <Login />}  />
              <Route path="/register" element={isLoggedIn ? null : <Register />} />
       </Routes>
    </BrowserRouter>
  )
}

export default App
