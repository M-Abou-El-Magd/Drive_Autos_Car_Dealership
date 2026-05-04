import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { login } from '../api/authService';

const LoginPage = ({ onLogin }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    setLoading(true);
    setError('');

    try {
      const response = await login({ email, password });
      onLogin(response.token);
      navigate('/cars');
    } catch (err) {
      setError(err?.response?.data || 'Login failed. Check your credentials.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card card-panel">
      <div className="panel-top">Secure login</div>
      <h1 className="page-title">Sign in to Drive Autos</h1>
      <p className="subtitle">Access listings, manage your vehicles, or connect with customers and sellers.</p>

      <form className="form-grid" onSubmit={handleSubmit}>
        <div className="input-group">
          <label htmlFor="email">Email</label>
          <input id="email" type="email" value={email} onChange={(event) => setEmail(event.target.value)} required />
        </div>

        <div className="input-group">
          <label htmlFor="password">Password</label>
          <input id="password" type="password" value={password} onChange={(event) => setPassword(event.target.value)} required />
        </div>

        <button className="primary" type="submit" disabled={loading}>
          {loading ? 'Signing in...' : 'Sign in'}
        </button>
      </form>

      {error && <div className="alert error">{error}</div>}

      <p className="bottom-text">
        New here? <Link to="/signup">Create an account</Link>
      </p>
    </div>
  );
};

export default LoginPage;
