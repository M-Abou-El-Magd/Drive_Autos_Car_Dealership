export function getStoredToken() {
  return localStorage.getItem('car_dealership_token');
}

export function setStoredToken(token) {
  localStorage.setItem('car_dealership_token', token);
}

export function clearStoredToken() {
  localStorage.removeItem('car_dealership_token');
}

export function decodeToken(token) {
  try {
    const payload = token.split('.')[1];
    const decoded = JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')));
    return {
      userId: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || decoded.nameid || null,
      email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || decoded.email || null,
      role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decoded.role || null
    };
  } catch (error) {
    return null;
  }
}
