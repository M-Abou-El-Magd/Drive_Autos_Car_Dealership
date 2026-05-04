import { Link } from 'react-router-dom';

const HomePage = ({ user }) => {
  return (
    <div>
      <section className="hero-card">
        <div className="hero-copy">
          <p className="eyebrow">Premium dealership experience</p>
          <h1>Find your perfect drive.</h1>
          <p className="hero-description">
            Browse exclusive inventory, save favorites, and start your journey with a modern car marketplace
            connected to the backend.
          </p>
          <div className="hero-actions">
            <Link to="/cars" className="button-primary hero-button">
              Explore inventory
            </Link>
            {!user && (
              <Link to="/signup" className="button-secondary hero-button">
                Create account
              </Link>
            )}
          </div>
        </div>
        <div className="hero-image-block">
          <div className="hero-image" />
        </div>
      </section>

      <section className="overview-grid">
        <div className="feature-card feature-blue">
          <h3>Easy registration</h3>
          <p>Join as a client or seller and manage car inventory with a secure login flow.</p>
        </div>
        <div className="feature-card feature-purple">
          <h3>Real-time listing</h3>
          <p>See all available cars from the backend, with details and edit access for sellers.</p>
        </div>
        <div className="feature-card feature-green">
          <h3>Secure storage</h3>
          <p>Credentials and inventory are saved in a local SQLite database for production-like behavior.</p>
        </div>
      </section>

      <section className="card card-panel">
        <h2 className="section-title">Why choose Drive Autos?</h2>
        <p className="subtitle">
          The site uses React and Axios to communicate with a backend API, while the database stores users,
          sellers, and car inventory directly in SQLite.
        </p>
        {user ? (
          <div className="alert success">Welcome back, {user.email}. You can manage your cars or book a test drive.</div>
        ) : (
          <div className="alert">Create your free account to begin browsing and posting cars.</div>
        )}
      </section>
    </div>
  );
};

export default HomePage;
