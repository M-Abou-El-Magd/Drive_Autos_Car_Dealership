import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { fetchCarById, updateCar, deleteCar } from '../api/carService';
import { reserveCar } from '../api/reservationService';

const CarDetailPage = ({ user }) => {
  const { id } = useParams();
  const [car, setCar] = useState(null);
  const [form, setForm] = useState({ make: '', model: '', year: '', price: '', color: '', sold: false });
  const [imageUrl, setImageUrl] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [reservationMessage, setReservationMessage] = useState('');
  const [reservationSuccess, setReservationSuccess] = useState('');
  const [reservationLoading, setReservationLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const loadCar = async () => {
      try {
        const data = await fetchCarById(id);
        setCar(data);
        setImageUrl(data.imageUrl || '');
        setForm({
          make: data.make,
          model: data.model,
          year: data.year,
          price: data.price,
          color: data.color,
          sold: data.sold
        });
      } catch (err) {
        setError(err?.response?.data || 'Unable to load the selected car.');
      } finally {
        setLoading(false);
      }
    };

    loadCar();
  }, [id]);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleUpdate = async (event) => {
    event.preventDefault();
    setSaving(true);
    setError('');
    setSuccess('');

    try {
      await updateCar(id, {
        make: form.make,
        model: form.model,
        year: Number(form.year),
        price: Number(form.price),
        color: form.color,
        sold: form.sold
      });
      setSuccess('Car updated successfully.');
      navigate('/cars');
    } catch (err) {
      setError(err?.response?.data || 'Update failed.');
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!confirm('Delete this car?')) return;
    setError('');

    try {
      await deleteCar(id);
      navigate('/cars');
    } catch (err) {
      setError(err?.response?.data || 'Delete failed.');
    }
  };

  const handleReserve = async () => {
    setReservationLoading(true);
    setError('');
    setReservationSuccess('');

    try {
      const reservation = await reserveCar(id, reservationMessage || 'I would like to reserve this car.');
      setReservationSuccess(`Reservation confirmed. Your reservation ID is ${reservation.id}.`);
      setReservationMessage('');
    } catch (err) {
      const serverMessage = err?.response?.data?.title || err?.response?.data?.message || err?.response?.data;
      if (err?.response?.status === 401) {
        setError('Reservation failed. Please log in again and try.');
      } else {
        setError(serverMessage || err?.message || 'Reservation failed.');
      }
    } finally {
      setReservationLoading(false);
    }
  };

  if (loading) {
    return <div className="card">Loading car details...</div>;
  }

  if (!car) {
    return <div className="card">Car not found.</div>;
  }

  const isSellerOwner = user?.role === 'Seller' && Number(user.userId) === car.sellerId;
  const canEdit = isSellerOwner || user?.role === 'Admin';
  const isClient = user?.role === 'Client';

  return (
    <div className="card">
      <h1 className="page-title">{isClient ? 'Car details & reservation' : canEdit ? 'Edit Car' : 'Car details'}</h1>
      <div style={{ marginBottom: '20px' }}>
        {imageUrl && (
          <img
            src={imageUrl}
            alt={`${car.make} ${car.model}`}
            style={{ width: '100%', borderRadius: 20, marginBottom: 20, maxHeight: 380, objectFit: 'cover' }}
          />
        )}
        <strong>Car ID:</strong> {car.id} <br />
        <strong>Seller ID:</strong> {car.sellerId}
      </div>

      {isClient ? (
        <div className="form-grid">
          <div className="input-group">
            <label htmlFor="reservationMessage">Reservation note</label>
            <textarea
              id="reservationMessage"
              value={reservationMessage}
              onChange={(event) => setReservationMessage(event.target.value)}
              rows={5}
              placeholder="Write a short reservation message for the dealership"
              required
            />
          </div>

          <button className="primary" type="button" onClick={handleReserve} disabled={reservationLoading}>
            {reservationLoading ? 'Reserving...' : 'Reserve this car for 24 hours'}
          </button>

          {reservationSuccess && <div className="alert success">{reservationSuccess}</div>}
          {error && <div className="alert error">{error}</div>}
        </div>
      ) : canEdit ? (
        <>
          <form className="form-grid" onSubmit={handleUpdate}>
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

            {isSellerOwner && (
              <div className="input-group">
                <label>
                  <input
                    type="checkbox"
                    checked={form.sold}
                    onChange={(e) => setForm((prev) => ({ ...prev, sold: e.target.checked }))}
                  />
                  Mark as Sold
                </label>
              </div>
            )}

            <button className="primary" type="submit" disabled={saving}>
              {saving ? 'Saving...' : 'Update Car'}
            </button>
          </form>

          <button className="secondary" onClick={handleDelete} style={{ marginTop: '16px' }}>
            Delete Car
          </button>

          {error && <div className="alert error">{error}</div>}
          {success && <div className="alert success">{success}</div>}
        </>
      ) : (
        <div className="card">
          <p>You can view the car details, but only the selling agent or an admin may edit or delete this listing.</p>
        </div>
      )}
    </div>
  );
};

export default CarDetailPage;
