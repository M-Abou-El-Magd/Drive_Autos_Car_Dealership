import { useMemo, useState } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Navbar from './components/Navbar';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import SignupPage from './pages/SignupPage';
import CarsPage from './pages/CarsPage';
import NewCarPage from './pages/NewCarPage';
import CarDetailPage from './pages/CarDetailPage';
import { clearStoredToken, decodeToken, getStoredToken, setStoredToken } from './utils/auth';

function App() {
  const initialToken = getStoredToken();
  const [token, setToken] = useState(initialToken);
  const user = useMemo(() => (token ? decodeToken(token) : null), [token]);

  const handleLogin = (jwt) => {
    setStoredToken(jwt);
    setToken(jwt);
  };

  const handleLogout = () => {
    clearStoredToken();
    setToken(null);
  };

  const requireAuth = (component) => {
    return token ? component : <Navigate to="/login" replace />;
  };

  const requireSeller = (component) => {
    if (!token) return <Navigate to="/login" replace />;
    return user?.role === 'Seller' ? component : <Navigate to="/cars" replace />;
  };

  return (
    <div>
      <Navbar user={user} onLogout={handleLogout} />
      <div className="container">
        <Routes>
          <Route path="/" element={<HomePage user={user} />} />
          <Route path="/login" element={<LoginPage onLogin={handleLogin} />} />
          <Route path="/signup" element={<SignupPage />} />
          <Route path="/cars" element={requireAuth(<CarsPage user={user} />)} />
          <Route path="/cars/new" element={requireSeller(<NewCarPage user={user} />)} />
          <Route path="/cars/:id" element={requireAuth(<CarDetailPage user={user} />)} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
