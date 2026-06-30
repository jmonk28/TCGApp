import { useState, useEffect, useContext } from 'react'
import { BrowserRouter, Routes, Route } from "react-router"
import Home from './pages/Home'
import Login from './pages/Login'
import Register from './pages/Register'
import { AuthContext } from './context/AuthContext'
import './App.css'

function App() {

    const { setAccessToken, setUser } = useContext(AuthContext);

    async function grabToken() {
        const resp = await fetch("https://localhost:7207/api/Login/refresh", {
            method: 'GET',
            credentials: 'include'
        });

        if (!resp.ok) {
            setAccessToken(null);
            setUser(null);
        }
        else {
            const data = await resp.json();
            setAccessToken(data.accessToken);
            setUser(data.username);
        }
    }

    useEffect(() => {
        grabToken();
    }, []);

  return (
    <BrowserRouter>
       <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
       </Routes>
    </BrowserRouter>
  )
}

export default App
