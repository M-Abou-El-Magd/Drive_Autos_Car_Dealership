import { Link } from 'react-router-dom';

const Navbar = ({ user, onLogout }) => {
  return (
    <nav className="navbar">
      <div className="brand-block">
        <Link to="/" className="brand-logo">
          Drive Autos
        </Link>
      </div>
      <div className="nav-links">
        <Link to="/">Home</Link>
        <Link to="/cars">Inventory</Link>
        {!user && <Link to="/signup" className="button-link">Sign up</Link>}
        {user?.role === 'Seller' && <Link to="/cars/new" className="button-link">List a car</Link>}
        {user ? (
          <>
            <span className="badge nav-badge">{user.role}</span>
            <button className="secondary" onClick={onLogout}>
              Logout
            </button>
          </>
        ) : (
          <Link to="/login" className="button-link button-primary">Login</Link>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
