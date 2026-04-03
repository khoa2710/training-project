<script setup lang="ts">
import { ref } from 'vue'
import api from '@/services/api'
import { useAuthStore } from '@/stores/auth'

const emit = defineEmits<{ (e: 'close'): void }>()

const auth = useAuthStore()

const oldPassword = ref('')
const newPassword = ref('')
const showOld = ref(false)
const showNew = ref(false)
const saving = ref(false)
const success = ref(false)
const errorMsg = ref('')

const save = async () => {
  errorMsg.value = ''
  if (!oldPassword.value || !newPassword.value) {
    errorMsg.value = 'Both fields are required.'
    return
  }
  if (newPassword.value.length < 8) {
    errorMsg.value = 'New password must be at least 8 characters.'
    return
  }
  saving.value = true
  try {
    await api.put(`/api/users/${auth.userId}/change-password`, {
      oldPassword: oldPassword.value,
      newPassword: newPassword.value,
    })
    success.value = true
  } catch (err: unknown) {
    if (err instanceof Error) {
      errorMsg.value = err.message
    } else {
      errorMsg.value = 'Failed to change password. Check your old password.'
    }
  } finally {
    saving.value = false
  }
}

const close = () => {
  oldPassword.value = ''
  newPassword.value = ''
  success.value = false
  errorMsg.value = ''
  emit('close')
}
</script>

<template>
  <div class="modal-overlay" @click.self="close">
    <div class="modal">
      <div class="modal-header">
        <h3>Change password</h3>
      </div>

      <!-- Success state -->
      <div v-if="success" class="modal-body success-body">
        <p class="success-msg">Your password has been changed successfully!</p>
        <div class="modal-footer">
          <button class="btn-cancel" @click="close">Close</button>
        </div>
      </div>

      <!-- Form state -->
      <div v-else class="modal-body">
        <div v-if="errorMsg" class="error-msg">{{ errorMsg }}</div>

        <div class="field">
          <label>Old password</label>
          <div class="input-wrap">
            <input
              :type="showOld ? 'text' : 'password'"
              v-model="oldPassword"
              autocomplete="current-password"
            />
            <button class="eye-btn" @click="showOld = !showOld" type="button" tabindex="-1">
              <svg v-if="showOld" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94"/><path d="M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19"/><line x1="1" y1="1" x2="23" y2="23"/></svg>
            </button>
          </div>
        </div>

        <div class="field">
          <label>New password</label>
          <div class="input-wrap">
            <input
              :type="showNew ? 'text' : 'password'"
              v-model="newPassword"
              autocomplete="new-password"
            />
            <button class="eye-btn" @click="showNew = !showNew" type="button" tabindex="-1">
              <svg v-if="showNew" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94"/><path d="M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19"/><line x1="1" y1="1" x2="23" y2="23"/></svg>
            </button>
          </div>
        </div>

        <div class="modal-footer">
          <button class="btn-save" :disabled="saving" @click="save">
            {{ saving ? 'Saving...' : 'Save' }}
          </button>
          <button class="btn-cancel" @click="close">Cancel</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.35);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}
.modal {
  background: #fff;
  border-radius: 10px;
  width: 420px;
  max-width: 92vw;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.18);
  overflow: hidden;
}
.modal-header {
  background: #f5f5f5;
  padding: 16px 24px;
  border-bottom: 1px solid #e0e0e0;
}
.modal-header h3 {
  margin: 0;
  font-size: 18px;
  font-weight: 700;
  color: #cf2b30;
}
.modal-body {
  padding: 24px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}
.success-body {
  gap: 20px;
}
.success-msg {
  font-size: 14px;
  color: #333;
  margin: 0;
}
.error-msg {
  font-size: 13px;
  color: #cf2b30;
  background: #fff0f0;
  border: 1px solid #f5c6c6;
  border-radius: 6px;
  padding: 8px 12px;
}
.field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}
.field label {
  font-size: 13px;
  font-weight: 600;
  color: #444;
}
.input-wrap {
  position: relative;
  display: flex;
  align-items: center;
}
.input-wrap input {
  width: 100%;
  height: 40px;
  border: 1px solid #ccc;
  border-radius: 6px;
  padding: 0 40px 0 12px;
  font-size: 14px;
  outline: none;
  transition: border-color 0.15s;
}
.input-wrap input:focus {
  border-color: #cf2b30;
}
.eye-btn {
  position: absolute;
  right: 10px;
  background: none;
  border: none;
  cursor: pointer;
  color: #666;
  display: flex;
  align-items: center;
  padding: 0;
}
.eye-btn:hover {
  color: #333;
}
.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  padding-top: 4px;
}
.btn-save {
  height: 38px;
  padding: 0 24px;
  background: #cf2b30;
  color: #fff;
  border: none;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.15s;
}
.btn-save:hover:not(:disabled) {
  background: #b82428;
}
.btn-save:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
.btn-cancel {
  height: 38px;
  padding: 0 20px;
  background: #fff;
  color: #333;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.15s;
}
.btn-cancel:hover {
  background: #f5f5f5;
}
</style>
