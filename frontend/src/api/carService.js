import api from './axios';

export async function fetchCars() {
  const response = await api.get('/cars');
  return response.data;
}

export async function fetchCarById(id) {
  const response = await api.get(`/cars/${id}`);
  return response.data;
}

export async function uploadCarImage(file) {
  const formData = new FormData();
  formData.append('file', file);
  try {
    console.log('Uploading file:', file.name, 'size:', file.size, 'type:', file.type);
    const response = await api.post('/cars/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    console.log('Upload response:', response);
    console.log('Upload response data:', response.data);
    console.log('ImageUrl from response:', response.data.imageUrl);
    return response.data;
  } catch (error) {
    console.error('Upload error:', error);
    throw error;
  }
}

export async function createCar(payload) {
  const response = await api.post('/cars', payload);
  return response.data;
}

export async function updateCar(id, payload) {
  await api.put(`/cars/${id}`, payload);
}

export async function deleteCar(id) {
  await api.delete(`/cars/${id}`);
}

export async function markCarAsSold(id) {
  const response = await api.post(`/cars/${id}/mark-sold`);
  return response.data;
}
