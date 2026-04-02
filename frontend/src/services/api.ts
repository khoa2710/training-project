import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5121',
  headers: {
    'Content-Type': 'application/json',
  },
})

// Attach JWT token to every request if available
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('auth_token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Normalize response errors into readable messages
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (!error.response) {
      // No response at all — server unreachable or CORS/network failure
      return Promise.reject(new Error('Cannot connect to server. Please check if the backend is running.'))
    }

    const status = error.response.status
    const serverMessage = error.response.data?.message

    if (status === 401) {
      // Keep store and storage in sync when token expires/invalidates.
      localStorage.removeItem('auth_token')
      localStorage.removeItem('auth_userid')
      localStorage.removeItem('auth_username')
      localStorage.removeItem('auth_role')
      localStorage.removeItem('auth_isFirstLogin')

      if (window.location.pathname !== '/login') {
        window.location.href = '/login'
      }

      return Promise.reject(new Error(serverMessage ?? 'Unauthorized. Please log in again.'))
    }
    if (status === 403) {
      return Promise.reject(new Error(serverMessage ?? 'You do not have permission to access this resource.'))
    }
    if (status === 400) {
      return Promise.reject(new Error(serverMessage ?? 'Invalid request.'))
    }
    if (status >= 500) {
      return Promise.reject(new Error(serverMessage ?? 'Server error. Please try again later.'))
    }

    return Promise.reject(new Error(serverMessage ?? `Request failed (${status}).`))
  },
)

export default api
