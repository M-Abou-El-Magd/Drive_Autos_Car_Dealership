import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { fetchCars, markCarAsSold } from '../api/carService';

const CarsPage = ({ user }) => {
  const [cars, setCars] = useState([]);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadCars = async () => {
      try {
        const data = await fetchCars();
        setCars(data);
      } catch (err) {
        setError(err?.response?.data || 'Unable to load cars.');
      } finally {
        setLoading(false);
      }
    };

    loadCars();
  }, []);

  const handleMarkSold = async (carId) => {
    try {
      await markCarAsSold(carId);
      setCars(cars.map(c => c.id === carId ? { ...c, sold: true } : c));
    } catch (err) {
      alert('Failed to mark car as sold');
    }
  };

  return (
    <div>
      <section className="page-header">
        <div>
          <h1 className="page-title">Available Inventory</h1>
          <p className="subtitle">Browse the latest curated cars and discover detailed listings.</p>
        </div>
        {user?.role === 'Seller' && (
          <Link to="/cars/new" className="button-primary">
            Add new car
          </Link>
        )}
      </section>

      {loading ? (
        <div className="alert">Loading cars...</div>
      ) : error ? (
        <div className="alert error">{error}</div>
      ) : (
        <div className="inventory-grid">
          {cars.filter(car => user?.role !== 'Client' || !car.sold).map((car) => (
            <div key={car.id} className="inventory-card">
              <div className="inventory-card-media">
                <img
                  src={car.imageUrl || 'https://images.unsplash.com/photo-1511919884226-fd3cad34687c?auto=format&fit=crop&w=800&q=80'}
                  alt={`${car.make} ${car.model}`}
                />
              </div>
              <div className="inventory-card-top">
                <span className="badge">{car.year}</span>
                <strong>${car.price.toFixed(2)}</strong>
                {user?.role === 'Seller' && Number(user.userId) === car.sellerId && (
                  <label style={{ marginLeft: '10px' }}>
                    <input
                      type="checkbox"
                      checked={car.sold || false}
                      onChange={() => handleMarkSold(car.id)}
                    />
                    Sold
                  </label>
                )}
              </div>
              <h3>{car.make} {car.model}</h3>
              <p className="inventory-meta">Color: {car.color}</p>
              <p className="inventory-meta">Seller ID: {car.sellerId}</p>
              <Link to={`/cars/${car.id}`} className="button-secondary inventory-button">
                View details
              </Link>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default CarsPage;
