import { defineStore } from 'pinia'
import { ref } from 'vue'
import router from '../router'
import api from '../services/api'

type LoginResponse = {
  id: number
  username: string
  role: string
  isFirstLogin: boolean
  token: string
}

export const useAuthStore = defineStore('auth', () => {
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
    const { data } = await api.post<LoginResponse>('/api/auth/login', {
      username: usernameInput,
      password,
    })

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

    await api.put(`/api/users/${userId.value}`, { password: newPassword })

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
