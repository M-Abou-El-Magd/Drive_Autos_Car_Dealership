import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../api/authService';

const SignupPage = () => {
  const [form, setForm] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
    role: 'Client'
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setLoading(true);
    setError('');
    setSuccess('');

    if (form.password !== form.confirmPassword) {
      setError('Passwords do not match.');
      setLoading(false);
      return;
    }

    try {
      await register({
        name: form.name,
        email: form.email,
        password: form.password,
        role: form.role
      });
      setSuccess('Signup complete. Please log in to continue.');
      setForm({ name: '', email: '', password: '', confirmPassword: '', role: 'Client' });
      setTimeout(() => {
        navigate('/login');
      }, 1200);
    } catch (err) {
      setError(err?.response?.data || 'Signup failed.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card card-panel">
      <div className="panel-top">Register Your Account</div>
      <h1 className="page-title">Start your journey with Drive Autos</h1>
      <p className="subtitle">
        Create an account for client or seller access. Your credentials are stored in the app&apos;s SQLite database.
      </p>

      <form className="form-grid" onSubmit={handleSubmit}>
        <div className="input-group">
          <label htmlFor="name">Full name</label>
          <input id="name" name="name" value={form.name} onChange={handleChange} required />
        </div>

        <div className="input-group">
          <label htmlFor="email">Email</label>
          <input id="email" name="email" type="email" value={form.email} onChange={handleChange} required />
        </div>

        <div className="input-group">
          <label htmlFor="password">Password</label>
          <input id="password" name="password" type="password" value={form.password} onChange={handleChange} required />
        </div>

        <div className="input-group">
          <label htmlFor="confirmPassword">Confirm password</label>
          <input id="confirmPassword" name="confirmPassword" type="password" value={form.confirmPassword} onChange={handleChange} required />
        </div>

        <div className="input-group">
          <label htmlFor="role">Account type</label>
          <select id="role" name="role" value={form.role} onChange={handleChange}>
            <option value="Client">Client</option>
            <option value="Seller">Seller</option>
          </select>
        </div>

        <button className="primary" type="submit" disabled={loading}>
          {loading ? 'Creating account...' : 'Create account'}
        </button>
      </form>

      {error && <div className="alert error">{error}</div>}
      {success && <div className="alert success">{success}</div>}
    </div>
  );
};

export default SignupPage;
