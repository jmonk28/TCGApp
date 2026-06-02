// Home.jsx
// Main landing page for the application.

import React, { useEffect } from 'react';
import Navbar from './Navbar';

export default function Home() {
  useEffect(() => {
    document.title = 'Home - TCG App';
  }, []);

    return (
    <>
    <Navbar />
    <main className="page-home" aria-labelledby="home-heading" style={{ padding: '24px' }}>
      {/* Page header */}
      <header>
        <h1 id="home-heading">Welcome to TCG App</h1>
        <p className="lead">
          Manage your trading card collections, decks, and matches in one place.
        </p>
      </header>

      {/* Quick actions */}
      <section aria-label="quick actions" style={{ marginTop: '16px' }}>
        <h2>Quick Actions</h2>
        <div style={{ display: 'flex', gap: '12px', flexWrap: 'wrap' }}>
          <a href="/collections" className="btn" role="button">View Collections</a>
          <a href="/decks" className="btn" role="button">Browse Decks</a>
          <a href="/matches" className="btn" role="button">Recent Matches</a>
        </div>
      </section>

      {/* Overview */}
      <section aria-label="overview" style={{ marginTop: '24px' }}>
        <h2>Overview</h2>
        <p>
          This dashboard provides a quick way to jump into your most-used areas.
          Use the navigation links above to explore collections, build decks, or review match history.
        </p>
      </section>

     </main>
    </>
  );
}