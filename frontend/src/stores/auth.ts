import { defineStore } from 'pinia'
import { ref } from 'vue'
import router from '../router'

type LoginResponse = {
  id: number
  username: string
  role: string
  isFirstLogin: boolean
  token: string
}

export const useAuthStore = defineStore('auth', () => {
  const baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'
  const token = ref<string>(localStorage.getItem('auth_token') || '')
  const userId = ref<number>(Number(localStorage.getItem('auth_userid') || '0'))
  const username = ref<string>(localStorage.getItem('auth_username') || '')
  const role = ref<string>(localStorage.getItem('auth_role') || '')
  const isFirstLogin = ref<boolean>(localStorage.getItem('auth_isFirstLogin') === 'true')

  const isAuthenticated = () => !!token.value

  const saveState = (payload: LoginResponse) => {
    token.value = payload.token
    userId.value = payload.id
    username.value = payload.username
    role.value = payload.role
    isFirstLogin.value = payload.isFirstLogin

    localStorage.setItem('auth_token', payload.token)
    localStorage.setItem('auth_userid', payload.id.toString())
    localStorage.setItem('auth_username', payload.username)
    localStorage.setItem('auth_role', payload.role)
    localStorage.setItem('auth_isFirstLogin', payload.isFirstLogin.toString())
  }

  const clearState = () => {
    token.value = ''
    userId.value = 0
    username.value = ''
    role.value = ''
    isFirstLogin.value = false

    localStorage.removeItem('auth_token')
    localStorage.removeItem('auth_userid')
    localStorage.removeItem('auth_username')
    localStorage.removeItem('auth_role')
    localStorage.removeItem('auth_isFirstLogin')
  }

  const login = async (usernameInput: string, password: string) => {
    const resp = await fetch(`${baseUrl}/api/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username: usernameInput, password }),
    })

    if (!resp.ok) {
      const err = await resp.json().catch(() => null)
      throw new Error(err?.message || 'Login failed')
    }

    const data = (await resp.json()) as LoginResponse
    saveState(data)

    if (data.isFirstLogin) {
      router.push('/first-login')
    } else {
      router.push('/dashboard')
    }
  }

  const logout = () => {
    clearState()
    router.push('/login')
  }

  const completeFirstLogin = async (newPassword: string) => {
    if (!userId.value) throw new Error('User ID is missing')

    const resp = await fetch(`${baseUrl}/api/users/${userId.value}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token.value}`,
      },
      body: JSON.stringify({ password: newPassword }),
    })

    if (!resp.ok) {
      const err = await resp.json().catch(() => null)
      throw new Error(err?.message || 'Password change failed')
    }

    isFirstLogin.value = false
    localStorage.setItem('auth_isFirstLogin', 'false')
    router.push('/dashboard')
  }

  return {
    token,
    userId,
    username,
    role,
    isFirstLogin,
    isAuthenticated,
    login,
    logout,
    completeFirstLogin,
  }
})
