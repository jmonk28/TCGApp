import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import Navbar from './Navbar';
import { AuthContext } from "../context/AuthContext";

export default function Login() {
    const navigate = useNavigate();
    const { setAccessToken } = useContext(AuthContext);
    const { setUser } = useContext(AuthContext);

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState({});
  const [serverError, setServerError] = useState('');
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setServerError('');
  }, [email, password]);

  function validate() {
    const e = {};
    if (!email.trim()) {
      e.email = 'Email is required.';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      e.email = 'Enter a valid email address.';
    }

    if (!password) {
      e.password = 'Password is required.';
    } else if (password.length < 6) {
      e.password = 'Password must be at least 6 characters.';
    }

    setErrors(e);
    return Object.keys(e).length === 0;
  }

  async function handleSubmit(ev) {
    ev.preventDefault();
    setServerError('');
    if (!validate()) return;

    setLoading(true);
    try {
        const resp = await fetch('https://localhost:7207/api/Login/login', {
            method: 'POST',
            credentials: 'include',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: email.trim(), password: password }),
      });

      if (!resp.ok) {
        console.log("So our login is failing here");
        const body = await resp.json().catch(() => ({}));
        setServerError(body.message || 'Login failed.');
        setLoading(false);
        return;
      }

      const data = await resp.json();
      // Receive token from backend
        if (data.accessToken) {
            //Place the access token in the current React context
            console.log(data.accessToken);
            console.log(data.username);
          setAccessToken(data.accessToken);
          setUser(data.username);
          //Navigate to home
          navigate("/");
      } else {
        setServerError('Invalid response from server.');
      }
    } catch (err) {
      setServerError('Network error. Please try again. Details: ' + err);
    } finally {
      setLoading(false);
    }
  }

    return (
    <>
    <Navbar />
    <main aria-labelledby="login-heading" style={{margin: '2rem auto' }}>
      <h1 id="login-heading">Sign in</h1>

      <form onSubmit={handleSubmit} noValidate>
        <div style={{ marginBottom: 12 }}>
          <label htmlFor="email">Email</label>
          <input
            id="email"
            name="email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            aria-invalid={!!errors.email}
            aria-describedby={errors.email ? 'email-error' : undefined}
            style={{ display: 'block', width: '100%', padding: 8 }}
          />
          {errors.email && (
            <div id="email-error" role="alert" style={{ color: 'crimson', marginTop: 6 }}>
              {errors.email}
            </div>
          )}
        </div>

        <div style={{ marginBottom: 12 }}>
          <label htmlFor="password">Password</label>
          <input
            id="password"
            name="password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            aria-invalid={!!errors.password}
            aria-describedby={errors.password ? 'password-error' : undefined}
            style={{ display: 'block', width: '100%', padding: 8 }}
          />
          {errors.password && (
            <div id="password-error" role="alert" style={{ color: 'crimson', marginTop: 6 }}>
              {errors.password}
            </div>
          )}
        </div>

        {serverError && (
          <div role="alert" style={{ color: 'crimson', marginBottom: 12 }} aria-live="assertive">
            {serverError}
          </div>
        )}

        <button type="submit" disabled={loading} style={{ padding: '10px 16px' }}>
          {loading ? 'Signing in...' : 'Sign in'}
        </button>
      </form>
    </main>
  </>
  );
}