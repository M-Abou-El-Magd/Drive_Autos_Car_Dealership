import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createCar, uploadCarImage } from '../api/carService';

const initialForm = {
  make: '',
  model: '',
  year: 2024,
  price: 0,
  color: ''
};

const NewCarPage = ({ user }) => {
  const [form, setForm] = useState(initialForm);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);
  const [imageFile, setImageFile] = useState(null);
  const [preview, setPreview] = useState('');
  const navigate = useNavigate();

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleImageChange = (event) => {
    const file = event.target.files?.[0] || null;
    setImageFile(file);
    setPreview(file ? URL.createObjectURL(file) : '');
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setLoading(true);
    setError('');
    setSuccess('');

    try {
      let imageUrl = undefined;
      if (imageFile) {
        console.log('Uploading image:', imageFile.name);
        const uploadResult = await uploadCarImage(imageFile);
        imageUrl = uploadResult?.imageUrl;
        console.log('Upload result:', uploadResult, 'imageUrl:', imageUrl);
      }

      if (user?.role !== 'Seller') {
        throw new Error('Only sellers can list a car.');
      }

      const payload = {
        make: form.make,
        model: form.model,
        year: Number(form.year),
        price: Number(form.price),
        color: form.color,
        sellerId: Number(user.userId),
        imageUrl
      };
      console.log('Creating car with payload:', payload);
      const carResult = await createCar(payload);
      console.log('Car created:', carResult);
      setSuccess('Car created successfully.');
      navigate('/cars');
    } catch (err) {
      console.error('Create car error:', err);
      setError(err?.response?.data || err.message || 'Unable to create car.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card">
      <h1 className="page-title">Add New Car</h1>
      {user?.role !== 'Seller' ? (
        <div className="alert error">
          Only users with the Seller role can list new cars. Please sign in with a Seller account.
        </div>
      ) : (
        <>
          <form className="form-grid" onSubmit={handleSubmit}>
            <div className="input-group">
              <label htmlFor="make">Make</label>
              <input id="make" name="make" value={form.make} onChange={handleChange} required />
            </div>

            <div className="input-group">
              <label htmlFor="model">Model</label>
              <input id="model" name="model" value={form.model} onChange={handleChange} required />
            </div>

            <div className="input-group">
              <label htmlFor="year">Year</label>
              <input id="year" name="year" type="number" min="1886" max="2100" value={form.year} onChange={handleChange} required />
            </div>

            <div className="input-group">
              <label htmlFor="price">Price</label>
              <input id="price" name="price" type="number" step="0.01" min="0" value={form.price} onChange={handleChange} required />
            </div>

            <div className="input-group">
              <label htmlFor="color">Color</label>
              <input id="color" name="color" value={form.color} onChange={handleChange} required />
            </div>

            <div className="input-group">
              <label htmlFor="image">Car image</label>
              <input id="image" name="image" type="file" accept="image/*" onChange={handleImageChange} />
              {preview && <img src={preview} alt="Preview" style={{ width: '100%', borderRadius: 16, marginTop: 12 }} />}
            </div>

            <button className="primary" type="submit" disabled={loading}>
              {loading ? 'Saving...' : 'Create Car'}
            </button>
          </form>

          {error && <div className="alert error">{error}</div>}
          {success && <div className="alert success">{success}</div>}
        </>
      )}
    </div>
  );
};

export default NewCarPage;
