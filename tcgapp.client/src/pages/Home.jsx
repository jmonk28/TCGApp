// Home.jsx
// Main landing page for the application.

import React, { useEffect, useContext } from 'react';
import Navbar from './Navbar';
import { AuthContext } from '../context/AuthContext';
import CardCarousel from '../assets/CardCarousel';

export default function Home() {

    const { isLoggedIn } = useContext(AuthContext);
    const cards = [{image: "/ecl-88-bitterbloom-bearer.jpg", name: "Bitterbloom Bearer" },
                   {image: "/krenko.jpg", name: "Krenko, Mob Boss"},
                   {image: "/cloud.png", name: "Cloud, Planet's Champion"}];

  useEffect(() => {
    document.title = 'Home - TCG App';
  }, []);

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
            <CardCarousel cards={cards} container="card-display" cardClass="card-image" numItemsShow={1} numItemsScroll={1} selectOn={false} />
      </section>)}

     </main>
    </>
  );
}