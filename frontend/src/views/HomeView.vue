<template>
  <div class="home-page">
    <div class="home-box">
      <h1>Online Asset Management</h1>
      <p>Welcome! Please log in to continue.</p>
      <router-link to="/login" class="btn">Go to Login</router-link>
    </div>
  </div>
</template>

<style scoped>
.home-page {
  min-height: 100vh;
  width: 100vw;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #f5f8fd;
  margin: 0;
  padding: 0;
}
.home-box {
  text-align: center;
  background: #ffffff;
  width: min(560px, 90vw);
  padding: 42px 30px;
  border-radius: 16px;
  box-shadow: 0 18px 36px rgba(15, 35, 64, 0.12);
  border: 1px solid #e7ebf2;
}
.home-box h1 {
  font-size: clamp(2rem, 4vw, 2.5rem);
  font-weight: 700;
  margin-bottom: 14px;
  color: #d32f26;
}
.home-box p {
  font-size: 1.04rem;
  color: #4f637a;
  margin-bottom: 22px;
}
.btn {
  background: #e53e17;
  color: white;
  border: none;
  border-radius: 8px;
  padding: 12px 22px;
  font-size: 1rem;
  font-weight: 700;
  text-decoration: none;
}
</style>
