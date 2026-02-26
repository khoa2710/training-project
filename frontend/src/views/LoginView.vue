<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'

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
    if (e instanceof Error) {
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
      <h1>Online Asset Management</h1>
      <p>Login to continue</p>
      <label>Username *</label>
      <input v-model="username" type="text" placeholder="Enter username" />

      <label>Password *</label>
      <input v-model="password" type="password" placeholder="Enter password" />

      <p class="error" v-if="error">{{ error }}</p>

      <button :disabled="!username || !password || loading" @click="submit">
        {{ loading ? 'Logging in...' : 'Login' }}
      </button>
    </div>
  </div>
</template>

<style scoped>
.login-page {
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background: radial-gradient(circle at top,left, #fff, #f5f5f5);
}
.auth-box {
  width: 360px;
  border: 1px solid #dcdcdc;
  border-radius: 10px;
  padding: 28px;
  background: white;
  box-shadow: 0 5px 25px rgba(0, 0, 0, 0.08);
}
.auth-box h1 {
  margin-bottom: 10px;
  color: #d42d1c;
}
.auth-box label {
  margin-top: 14px;
  display: block;
}
.auth-box input {
  width: 100%;
  padding: 10px;
  margin-top: 4px;
  border-radius: 6px;
  border: 1px solid #ccc;
}
button {
  margin-top: 16px;
  width: 100%;
  background: #e53e17;
  color: white;
  border: 0;
  border-radius: 6px;
  padding: 10px;
  font-weight: 700;
  cursor: pointer;
}
button:disabled {
  opacity: 0.65;
  cursor: not-allowed;
}
.error {
  color: #af1f1f;
  margin-top: 8px;
}
</style>
