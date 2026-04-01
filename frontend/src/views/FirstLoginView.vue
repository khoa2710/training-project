<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const newPassword = ref('')
const confirmPassword = ref('')
const error = ref('')
const loading = ref(false)
const showNew = ref(false)
const showConfirm = ref(false)

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
      <div class="icon-badge">
        <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none"
          stroke="#d32f26" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
          <path d="M7 11V7a5 5 0 0 1 10 0v4" />
        </svg>
      </div>

      <h1>Change Password</h1>
      <p class="subtitle">This is your first login. Please set a new password to continue.</p>

      <div class="field">
        <label>New password <span class="required">*</span></label>
        <div class="input-wrapper">
          <input
            v-model="newPassword"
            :type="showNew ? 'text' : 'password'"
            placeholder="Enter new password"
          />
          <button type="button" class="toggle-eye" @click="showNew = !showNew" tabindex="-1">
            <svg v-if="!showNew" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
            <svg v-else xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"/><line x1="1" y1="1" x2="23" y2="23"/></svg>
          </button>
        </div>
      </div>

      <div class="field">
        <label>Confirm password <span class="required">*</span></label>
        <div class="input-wrapper">
          <input
            v-model="confirmPassword"
            :type="showConfirm ? 'text' : 'password'"
            placeholder="Confirm new password"
          />
          <button type="button" class="toggle-eye" @click="showConfirm = !showConfirm" tabindex="-1">
            <svg v-if="!showConfirm" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
            <svg v-else xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"/><line x1="1" y1="1" x2="23" y2="23"/></svg>
          </button>
        </div>
      </div>

      <ul class="pw-rules">
        <li :class="{ met: newPassword.length >= 8 }">At least 8 characters</li>
        <li :class="{ met: newPassword === confirmPassword && confirmPassword.length > 0 }">Passwords match</li>
      </ul>

      <p class="error" v-if="error">{{ error }}</p>

      <button class="submit-btn" :disabled="loading || !newPassword || !confirmPassword" @click="submit">
        {{ loading ? 'Saving...' : 'Save' }}
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

.icon-badge {
  width: 52px;
  height: 52px;
  border-radius: 12px;
  background: rgba(211, 47, 38, 0.08);
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 18px;
}

.auth-box h1 {
  margin: 0 0 8px;
  font-size: clamp(1.4rem, 3vw, 1.75rem);
  font-weight: 700;
  color: #d32f26;
}

.subtitle {
  margin: 0 0 22px;
  color: #5d6f85;
  font-size: 0.93rem;
  line-height: 1.45;
}

.field {
  margin-bottom: 16px;
}

.field label {
  display: block;
  margin-bottom: 6px;
  color: #485763;
  font-weight: 500;
  font-size: 0.92rem;
}

.required {
  color: #d32f26;
}

.input-wrapper {
  position: relative;
}

.input-wrapper input {
  width: 100%;
  padding: 12px 42px 12px 14px;
  border: 1px solid #ccd9e7;
  border-radius: 8px;
  font-size: 0.95rem;
  color: #1c2a39;
  box-sizing: border-box;
  transition: border-color 0.2s ease, box-shadow 0.2s ease;
  font-family: inherit;
}

.input-wrapper input:focus {
  outline: none;
  border-color: #d32f26;
  box-shadow: 0 0 0 4px rgba(211, 47, 38, 0.12);
}

.toggle-eye {
  position: absolute;
  right: 10px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  padding: 4px;
  margin: 0;
  width: auto;
  color: #8a99ab;
  cursor: pointer;
  display: flex;
  align-items: center;
}

.toggle-eye:hover {
  color: #485763;
}

.pw-rules {
  list-style: none;
  padding: 0;
  margin: 4px 0 8px;
}

.pw-rules li {
  font-size: 0.82rem;
  color: #8a99ab;
  padding: 2px 0 2px 20px;
  position: relative;
  transition: color 0.2s ease;
}

.pw-rules li::before {
  content: '○';
  position: absolute;
  left: 2px;
  font-size: 0.7rem;
  top: 3px;
}

.pw-rules li.met {
  color: #2e8b57;
}

.pw-rules li.met::before {
  content: '✓';
  font-weight: 700;
}

.submit-btn {
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
  font-family: inherit;
}

.submit-btn:hover:not(:disabled) {
  background: #b82a20;
  transform: translateY(-1px);
}

.submit-btn:disabled {
  opacity: 0.55;
  cursor: not-allowed;
}

.error {
  color: #d71d0f;
  margin-bottom: 8px;
  font-weight: 600;
  font-size: 0.9rem;
}
</style>
