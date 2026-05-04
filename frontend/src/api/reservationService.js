import api from './axios';

export async function reserveCar(carId, message) {
  const response = await api.post('/reservations', { carId: Number(carId), message });
  return response.data;
}
