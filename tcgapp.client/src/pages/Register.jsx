import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Navbar from './Navbar';

function Register() {

    const navigate = useNavigate();

    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [errors, setErrors] = useState({});
    const [serverError, setServerError] = useState('');
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        setServerError('');
    }, [username, email, password]);

    async function usernameExists(username) {
        try {
            const resp = await fetch(`https://localhost:7207/api/Login/username/${username.trim()}`, {
                method: 'GET'
            });

            if (resp.status == 404) {
                console.log("We should be returning false here");
                return false;
            }
            if (resp.ok) {
                console.log("We should not be returning true here");
                return true;
            }
            throw new Error("Unexpected response status " + resp.status);

        } catch (err) {
            setServerError('Failed to fetch username. Details: ' + err);
        }
    }

    async function validate() {
        const e = {};
        
        if (!username.trim()) {
            e.username = 'Username is required';
        } else if (await usernameExists(username)) {
            e.username = 'Username is already in use';
            console.log("Why are we here?");
        };

        if (!email.trim()) {
            e.email = 'Email is required.';
        } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            e.email = 'Enter a valid email address.';
        } else if (email.length > 50) {
            e.email = "Email cannot be more than 50 characters";
        }
          

        if (!password) {
            e.password = 'Password is required.';
        } else if (password.length < 6) {
            e.password = 'Password must be at least 6 characters.';
        }

        setErrors(e);
        console.log("Errors: " + Object.keys(e).length);
        return Object.keys(e).length === 0;
    }

    async function handleSubmit(ev) {
        ev.preventDefault();
        setServerError('');
        if (!(await validate())) return;

        setLoading(true);
        try {
            const resp = await fetch('https://localhost:7207/api/Register/newuser', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    Username: username.trim(),
                    Email: email.trim(),
                    PasswordHash: password
                }),
            });

            if (!resp.ok) {
                const body = await resp.json().catch(() => ({}));
                setServerError(body.message || 'Registration failed.');
                setLoading(false);
                return;
            }

        } catch (err) {
            setServerError("Registration failed. Details: " + err);
        }
        finally {
            setLoading(false)
        }
        console.log("Registration successful!");
        navigate("/login");
    }

  return (
      <>
          <Navbar />
          <main aria-labelledby="register-heading" style={{ margin: '2rem auto' }}>
              <h1 id="register-heading">Register</h1>

              <form onSubmit={handleSubmit} noValidate>

                  <div style={{ marginBottom: 12 }}>
                      <label htmlFor="username">Username</label>
                      <input
                          id="username"
                          name="username"
                          type="username"
                          value={username}
                          onChange={(e) => setUsername(e.target.value)}
                          aria-invalid={!!errors.username}
                          aria-describedby={errors.username ? 'username-error' : undefined}
                          style={{ display: 'block', width: '100%', padding: 8 }}
                      />
                      {errors.username && (
                          <div id="username-error" role="alert" style={{ color: 'crimson', marginTop: 6 }}>
                              {errors.username}
                          </div>
                      )}
                  </div>

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
                      {loading ? 'Attempting to register user...' : 'Register'}
                  </button>
              </form>
          </main>
      </>
  );
}

export default Register;