<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import axios from 'axios'

const auth = useAuthStore()
const username = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

const submit = async () => {
  error.value = ''
  if (!username.value || !password.value) {
    error.value = 'Username and password are required.'
    return
  }

  loading.value = true
  try {
    await auth.login(username.value, password.value)
  } catch (e: unknown) {
    if (axios.isAxiosError(e)) {
      error.value = e.response?.data?.message || 'Username or password is incorrect'
    } else if (e instanceof Error) {
      error.value = e.message
    } else {
      error.value = 'Login failed'
    }
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="login-page">
    <div class="auth-box">
      <h1>Welcome to Online Asset Management</h1>
      <p>Sign in to manage users, assignments and assets.</p>

      <div class="field">
        <label>Username *</label>
        <input v-model="username" type="text" placeholder="Enter username" />
      </div>

      <div class="field">
        <label>Password *</label>
        <input v-model="password" type="password" placeholder="Enter password" />
      </div>

      <p class="error" v-if="error">{{ error }}</p>

      <button :disabled="!username || !password || loading" @click="submit">
        {{ loading ? 'Logging in...' : 'Login' }}
      </button>
    </div>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap');

.login-page {
  position: fixed;
  inset: 0;
  width: 100vw;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 24px;
  background: linear-gradient(135deg, #f5f7fb 0%, #e9edf5 100%);
  font-family: 'Roboto', sans-serif;
  z-index: 1000;
}

.auth-box {
  width: min(420px, 100%);
  border-radius: 14px;
  padding: 32px;
  background: #ffffff;
  box-shadow: 0 18px 40px rgba(15, 35, 64, 0.15);
  border: 1px solid rgba(226, 229, 237, 0.9);
}

.auth-box h1 {
  margin: 0 0 10px;
  font-size: clamp(1.4rem, 3vw, 1.9rem);
  font-weight: 700;
  color: #d32f26;
}

.auth-box p {
  margin: 0 0 20px;
  color: #5d6f85;
  font-size: 0.96rem;
}

.field {
  margin-bottom: 16px;
}

.field label {
  display: block;
  margin-bottom: 6px;
  color: #485763;
  font-weight: 500;
}

.field input {
  width: 100%;
  padding: 12px 14px;
  border: 1px solid #ccd9e7;
  border-radius: 8px;
  font-size: 0.95rem;
  color: #1c2a39;
  box-sizing: border-box;
  transition: border-color 0.2s ease, box-shadow 0.2s ease;
}

.field input:focus {
  outline: none;
  border-color: #d32f26;
  box-shadow: 0 0 0 4px rgba(211, 47, 38, 0.12);
}

button {
  margin-top: 8px;
  width: 100%;
  background: #d32f26;
  color: #fff;
  border: 0;
  border-radius: 8px;
  padding: 12px;
  font-size: 1rem;
  font-weight: 700;
  cursor: pointer;
  transition: transform 0.15s ease, background-color 0.15s ease;
}

button:hover:not(:disabled) {
  background: #b82a20;
  transform: translateY(-1px);
}

button:disabled {
  opacity: 0.55;
  cursor: not-allowed;
}

.error {
  color: #d71d0f;
  margin-bottom: 12px;
  font-weight: 600;
  min-height: 1.1em;
}
</style>
