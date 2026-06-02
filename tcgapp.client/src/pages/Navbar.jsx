import React, { useState, useEffect } from 'react';
import { Button, Stack } from '@mui/material';
import { useNavigate } from 'react-router-dom';

/*
  Simple, framework-agnostic Navbar component.
  - Uses plain anchor tags so it works whether the app uses React Router or server-side routing.
  - Accepts `links` and `brand` props and an optional `onNavigate` callback that receives the clicked `to` value.
  - Keeps a small internal mobile menu state.
*/

export default function Navbar({ brand = 'TCG App', links = null, onNavigate = null }) {
  const navigate = useNavigate();
  const [mobileOpen, setMobileOpen] = useState(false);
  const [currentPath, setCurrentPath] = useState(() => {
    if (typeof window !== 'undefined' && window.location) {
      return window.location.pathname;
    }
    return '/';
  });

  useEffect(() => {
    function handleLocationChange() {
      setCurrentPath(window.location.pathname);
    }

    // Listen for popstate so active state updates when using browser back/forward
    window.addEventListener('popstate', handleLocationChange);
    return () => window.removeEventListener('popstate', handleLocationChange);
  }, []);

    function handleNav(path) {
        navigate(path);
    }

  const defaultLinks = [
    { to: '/', label: 'Home' },
    { to: '/collection', label: 'Collection' },
    { to: '/decks', label: 'Decks' },
    { to: '/about', label: 'About' },
  ];

  const navLinks = Array.isArray(links) && links.length ? links : defaultLinks;

  function handleClick(e, to) {
    if (onNavigate) {
      e.preventDefault();
      try {
        onNavigate(to);
      } catch (err) {
        // swallow handler errors to avoid breaking navigation UI
        // eslint-disable-next-line no-console
        console.error(err);
      }
      // attempt to update history if handler didn't
      if (typeof window !== 'undefined' && window.history && window.location) {
        if (window.location.pathname !== to) {
          window.history.pushState({}, '', to);
          setCurrentPath(to);
        }
      }
    } else {
      // allow normal navigation and update currentPath after a short delay
      setTimeout(() => {
        if (typeof window !== 'undefined' && window.location) {
          setCurrentPath(window.location.pathname);
        }
      }, 0);
    }
    setMobileOpen(false);
  }

  const styles = {
    nav: {
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'space-between',
      padding: '0.5rem 1rem',
      borderBottom: '1px solid rgba(0,0,0,0.08)',
      background: '#fff',
    },
    brand: {
      fontWeight: 700,
      fontSize: '1.125rem',
      textDecoration: 'none',
      color: '#111',
    },
    navList: {
      display: 'flex',
      gap: '0.75rem',
      alignItems: 'center',
      margin: 0,
      padding: 0,
      listStyle: 'none',
    },
    link: {
      padding: '0.375rem 0.5rem',
      borderRadius: '4px',
      textDecoration: 'none',
      color: '#333',
    },
    active: {
      background: '#f0f0f0',
      color: '#000',
      fontWeight: 600,
    },
    mobileButton: {
      display: 'none',
      background: 'transparent',
      border: 'none',
      fontSize: '1.25rem',
      cursor: 'pointer',
    },
    // responsive rules will be applied inline by checking window width at render time
  };

  return (
    <nav style={styles.nav} role="navigation" aria-label="Main navigation">
      <a href="/" onClick={(e) => handleClick(e, '/')} style={styles.brand}>
        {brand}
      </a>

      {/* Desktop / mobile menu */}
      <ul
        id="main-nav"
        style={{
          ...styles.navList,
          flexDirection: 'row',
          position: 'static',
          right: 'auto',
          top: 'auto',
          background: 'transparent',
          padding: 0,
          boxShadow: 'none',
          display: 'flex'
        }}
      >
        {navLinks.map((ln) => {
          const active = ln.to === currentPath;
          return (
            <li key={ln.to}>
              <a
                href={ln.to}
                onClick={(e) => handleClick(e, ln.to)}
                style={{
                  ...styles.link,
                  ...(active ? styles.active : {}),
                  display: 'inline-block',
                }}
                aria-current={active ? 'page' : undefined}
              >
                {ln.label}
              </a>
            </li>
          );
        })}
          </ul>
          <button style={{ background: '#00FF00' }} onClick={(e) => handleNav('/login') }>Login</button>
    </nav>
  );
}