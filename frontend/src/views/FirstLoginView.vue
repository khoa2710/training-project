<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const newPassword = ref('')
const confirmPassword = ref('')
const error = ref('')
const loading = ref(false)

const submit = async () => {
  error.value = ''

  if (!newPassword.value) {
    error.value = 'New password is required.'
    return
  }
  if (newPassword.value.length < 8) {
    error.value = 'Password must be at least 8 characters.'
    return
  }
  if (newPassword.value !== confirmPassword.value) {
    error.value = 'Passwords do not match.'
    return
  }

  loading.value = true
  try {
    await auth.completeFirstLogin(newPassword.value)
  } catch (e: unknown) {
    if (e instanceof Error) {
      error.value = e.message
    } else {
      error.value = 'Failed to update password'
    }
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="login-page">
    <div class="auth-box">
      <h1>Change Password</h1>
      <p>This is your first login. You must set a new password.</p>

      <label>New password *</label>
      <input v-model="newPassword" type="password" placeholder="Enter new password" />

      <label>Confirm password *</label>
      <input v-model="confirmPassword" type="password" placeholder="Confirm new password" />

      <p class="error" v-if="error">{{ error }}</p>

      <button :disabled="loading || !newPassword || !confirmPassword" @click="submit">
        {{ loading ? 'Saving...' : 'Save' }}
      </button>
    </div>
  </div>
</template>

<style scoped>
/* same as login style to keep consistent */
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
