<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import api from '@/services/api'
import ChangePasswordModal from '@/components/ChangePasswordModal.vue'

const auth = useAuthStore()
const users = ref<Array<{ id: number; username: string; role: string; createdAt: string; isFirstLogin: boolean }>>([])
const loading = ref(false)
const error = ref('')
const showChangePassword = ref(false)

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
    <!-- Top Bar -->
    <header class="topbar">
      <span class="topbar-title">Home</span>
      <button class="topbar-profile" @click="showChangePassword = true">{{ auth.username }} <span class="topbar-role">({{ auth.role }})</span></button>
    </header>

    <!-- Sidebar -->
    <aside class="sidebar">
      <div class="sidebar-brand">
        <div class="brand-logo">Nash<br />Tech.</div>
        <div class="brand-name">Online Asset Management</div>
      </div>
      <nav class="sidebar-nav">
        <router-link to="/dashboard" class="nav-link active">Home</router-link>
        <router-link to="/dashboard" class="nav-link">Manage User</router-link>
        <router-link to="/dashboard" class="nav-link">Manage Asset</router-link>
        <router-link to="/dashboard" class="nav-link">Manage Assignment</router-link>
        <router-link to="/dashboard" class="nav-link">Request for Returning</router-link>
        <router-link to="/dashboard" class="nav-link">Report</router-link>
      </nav>
      <div class="sidebar-footer">
        <button class="logout-btn" @click="auth.logout()">Logout</button>
      </div>
    </aside>

    <!-- Change Password Modal -->
    <ChangePasswordModal v-if="showChangePassword" @close="showChangePassword = false" />

    <!-- Main Content -->
    <main class="content">
      <section class="card">
        <h2 class="card-title">My Assignment</h2>
        <div v-if="loading" class="loading-text">Loading...</div>
        <div v-if="error" class="error">{{ error }}</div>

        <div class="table-wrapper" v-if="!loading && !error">
          <table>
            <thead>
              <tr>
                <th>Asset Code <span class="sort-icon">&#9662;</span></th>
                <th>Asset Name <span class="sort-icon">&#9662;</span></th>
                <th>Category <span class="sort-icon">&#9662;</span></th>
                <th>Assigned Date <span class="sort-icon">&#9662;</span></th>
                <th>State <span class="sort-icon">&#9662;</span></th>
                <th class="th-actions"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="user in users" :key="user.id">
                <td>{{ user.id }}</td>
                <td>{{ user.username }}</td>
                <td>{{ user.role }}</td>
                <td>{{ new Date(user.createdAt).toLocaleDateString() }}</td>
                <td>{{ user.isFirstLogin ? 'Waiting for acceptance' : 'Accepted' }}</td>
                <td class="td-actions">
                  <button class="action-btn accept" title="Accept">&#10003;</button>
                  <button class="action-btn decline" title="Decline">&#10007;</button>
                  <button class="action-btn return" title="Return">&#8635;</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </section>
    </main>
  </div>
</template>

<style scoped>
/* ── Layout Shell ── */
.dashboard-wrapper {
  display: grid;
  grid-template-columns: 230px 1fr;
  grid-template-rows: 60px 1fr;
  grid-template-areas:
    "topbar topbar"
    "sidebar content";
  min-height: 100vh;
  background: #f0f0f0;
}

/* ── Top Bar ── */
.topbar {
  grid-area: topbar;
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 60px;
  background: #cf2b30;
  color: #fff;
  padding: 0 24px;
}
.topbar-title {
  font-size: 20px;
  font-weight: 700;
}
.topbar-profile {
  font-size: 14px;
  font-weight: 500;
  background: none;
  border: none;
  color: #fff;
  cursor: pointer;
  padding: 6px 10px;
  border-radius: 6px;
  transition: background 0.15s;
}
.topbar-profile:hover {
  background: rgba(255, 255, 255, 0.15);
}
.topbar-role {
  font-weight: 400;
  opacity: 0.85;
}

/* ── Sidebar ── */
.sidebar {
  grid-area: sidebar;
  display: flex;
  flex-direction: column;
  background: #f5f5f5;
  border-right: 1px solid #e8e8e8;
  padding: 20px 16px 16px;
}
.sidebar-brand {
  margin-bottom: 20px;
  padding-bottom: 16px;
  border-bottom: 1px solid #e0e0e0;
}
.brand-logo {
  font-size: 18px;
  font-weight: 800;
  color: #cf2b30;
  line-height: 1.15;
  border: 2px solid #cf2b30;
  display: inline-block;
  padding: 4px 8px;
  margin-bottom: 8px;
}
.brand-name {
  font-size: 13px;
  font-weight: 700;
  color: #cf2b30;
}

/* ── Navigation ── */
.sidebar-nav {
  display: flex;
  flex-direction: column;
  gap: 2px;
  flex: 1;
}
.nav-link {
  display: block;
  padding: 10px 12px;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 500;
  color: #333;
  text-decoration: none;
  transition: background 0.15s;
}
.nav-link:hover {
  background: #e8e8e8;
}
.nav-link.active,
.nav-link.router-link-exact-active {
  background: #cf2b30;
  color: #fff;
  font-weight: 600;
}

/* ── Sidebar Footer ── */
.sidebar-footer {
  margin-top: auto;
  padding-top: 16px;
}
.logout-btn {
  width: 100%;
  height: 40px;
  background: #cf2b30;
  color: #fff;
  border: none;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.15s;
}
.logout-btn:hover {
  background: #b82428;
}

/* ── Main Content ── */
.content {
  grid-area: content;
  padding: 24px;
  overflow-y: auto;
}

/* ── Card ── */
.card {
  background: #fff;
  border-radius: 10px;
  padding: 20px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}
.card-title {
  font-size: 16px;
  font-weight: 700;
  color: #cf2b30;
  margin-bottom: 16px;
}

/* ── Table ── */
.table-wrapper {
  overflow-x: auto;
}
table {
  width: 100%;
  border-collapse: collapse;
}
thead th {
  font-size: 13px;
  font-weight: 700;
  color: #333;
  padding: 10px 12px;
  border-bottom: 2px solid #ddd;
  white-space: nowrap;
  text-align: left;
  user-select: none;
}
.sort-icon {
  font-size: 10px;
  margin-left: 2px;
  opacity: 0.5;
}
tbody td {
  font-size: 13px;
  color: #444;
  padding: 10px 12px;
  border-bottom: 1px solid #eee;
  height: 48px;
  vertical-align: middle;
}
tbody tr:hover {
  background: #fafafa;
}
.th-actions {
  width: 100px;
}
.td-actions {
  display: flex;
  align-items: center;
  gap: 6px;
  height: 48px;
}
.action-btn {
  width: 28px;
  height: 28px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  background: transparent;
  transition: background 0.15s;
}
.action-btn.accept {
  color: #cf2b30;
}
.action-btn.accept:hover {
  background: #fde8e8;
}
.action-btn.decline {
  color: #333;
  font-weight: 700;
}
.action-btn.decline:hover {
  background: #eee;
}
.action-btn.return {
  color: #4a90d9;
}
.action-btn.return:hover {
  background: #e8f0fa;
}

/* ── Misc ── */
.loading-text {
  font-size: 14px;
  color: #888;
  padding: 16px 0;
}
.error {
  color: #af1f1f;
  font-size: 14px;
  padding: 8px 0;
}
</style>
