<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import api from '@/services/api'

const auth = useAuthStore()
const users = ref<Array<{ id: number; username: string; role: string; createdAt: string; isFirstLogin: boolean }>>([])
const loading = ref(false)
const error = ref('')

const fetchUsers = async () => {
  loading.value = true
  error.value = ''

  try {
    const { data } = await api.get('/api/users')

    interface UserData {
      id: number
      username: string
      role: string
      createdAt: string
      isFirstLogin: boolean
    }

    users.value = (data as UserData[]).map((item) => ({
      id: item.id,
      username: item.username,
      role: item.role,
      createdAt: item.createdAt || '',
      isFirstLogin: item.isFirstLogin,
    }))
  } catch (err: unknown) {
    if (err instanceof Error) {
      error.value = err.message
    } else {
      error.value = 'Error loading users'
    }
  } finally {
    loading.value = false
  }
}

onMounted(fetchUsers)
</script>

<template>
  <div class="dashboard-wrapper">
    <aside class="sidebar">
      <div class="brand">Nash Tech.</div>
      <div class="title">Online Asset Management</div>
      <nav>
        <router-link to="/dashboard" class="nav-link">Home</router-link>
        <router-link to="/dashboard" class="nav-link">Manage User</router-link>
        <router-link to="/dashboard" class="nav-link">Manage Asset</router-link>
        <router-link to="/dashboard" class="nav-link">Manage Assignment</router-link>
        <router-link to="/dashboard" class="nav-link">Request for Returning</router-link>
        <router-link to="/dashboard" class="nav-link">Report</router-link>
      </nav>
      <button class="logout-btn" @click="auth.logout()">Logout</button>
    </aside>

    <main class="content">
      <header class="topbar">
        <h1>Home</h1>
        <div class="profile">Welcome, {{ auth.username }} ({{ auth.role }})</div>
      </header>

      <section class="card">
        <h2>My Assignment</h2>
        <div v-if="loading">Loading users...</div>
        <div v-if="error" class="error">{{ error }}</div>

        <table v-if="!loading && !error">
          <thead>
            <tr>
              <th>#</th>
              <th>Username</th>
              <th>Role</th>
              <th>First login</th>
              <th>Created at</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="user in users" :key="user.id">
              <td>{{ user.id }}</td>
              <td>{{ user.username }}</td>
              <td>{{ user.role }}</td>
              <td>{{ user.isFirstLogin ? 'Yes' : 'No' }}</td>
              <td>{{ new Date(user.createdAt).toLocaleString() }}</td>
            </tr>
          </tbody>
        </table>
      </section>
    </main>
  </div>
</template>

<style scoped>
.dashboard-wrapper {
  display: grid;
  grid-template-columns: 270px 1fr;
  min-height: 100vh;
}
.sidebar {
  padding: 24px;
  background: white;
  border-right: 1px solid #eee;
}
.brand {
  font-size: 1.5rem;
  font-weight: bold;
  color: #e53e17;
}
.title {
  margin-top: 8px;
  font-weight: bold;
  color: #333;
}
nav {
  display: flex;
  flex-direction: column;
  gap: 10px;
  margin-top: 24px;
}
.nav-link {
  padding: 10px;
  border-radius: 8px;
  color: #333;
  text-decoration: none;
}
.nav-link.router-link-active {
  background: #f5f5f5;
}
.logout-btn {
  margin-top: 20px;
  width: 100%;
  background: #e53e17;
  color: white;
  border: none;
  padding: 10px;
  border-radius: 8px;
}
.content {
  background: #f7f7f7;
  padding: 24px;
}
.topbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 20px;
  background: #e53e17;
  color: white;
  padding: 16px;
  border-radius: 8px;
}
.card {
  background: white;
  border-radius: 8px;
  padding: 20px;
}
table {
  width: 100%;
  border-collapse: collapse;
}
th,
 td {
  border: 1px solid #e6e6e6;
  padding: 10px;
  text-align: left;
}
.error { color: #af1f1f; }
</style>
