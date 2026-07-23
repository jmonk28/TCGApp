// Home.jsx
// Main landing page for the application.

import React, { useState, useEffect, useContext } from 'react';
import Navbar from './Navbar';
import { AuthContext } from '../context/AuthContext';
import CardCarousel from '../assets/CardCarousel';

export default function Home() {

    const { accessToken } = useContext(AuthContext); 
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const cardItems = ["/ecl-88-bitterbloom-bearer.jpg", "/krenko.jpg"]

  useEffect(() => {
    document.title = 'Home - TCG App';
  }, []);

    useEffect(() => {
        if (accessToken != null) setIsLoggedIn(true);
        else setIsLoggedIn(false);
    }, [accessToken])

    return (
    <>
    <Navbar />
    <main className="page-home" aria-labelledby="home-heading" style={{ padding: '100px' }}>
      {/* Page header */}
      <header>
        <h1 id="home-heading">Welcome to TCG App</h1>
        <p className="lead">
          Find the cards you want from the people who have them.
        </p>
      </header>

      {isLoggedIn && (<section aria-label="popular-cards" style={{ marginTop: '20px' }}>
            <h1>Popular This Week</h1>
            <CardCarousel cards={cardItems} container="card-display" cardClass="card-image"/>
      </section>)}

     </main>
    </>
  );
}