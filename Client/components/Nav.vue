<script setup lang="ts">
import {useAuthStore} from "../stores/auth"
const authStore = useAuthStore();

const isActive = ref(false);

function toggle() {
  isActive.value = !isActive.value
}

function classData(classInfo: string): string {
  return isActive ? `${classInfo} is-active` : classInfo
}
</script>

<template>
  <div>
    <nav class="navbar" role="navigation" aria-label="main navigation">
      <div class="navbar-brand">
        <NuxtLink to="/" class="navbar-item"><img src="@/public/logo-new.png"></NuxtLink>

        <a role="button" class="navbar-burger" aria-label="menu" aria-expanded="true" data-target="navbarBasicExample"
          @click="toggle()">
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
        </a>
      </div>

      <div class="navbar-menu">
        <div class="navbar-start">
          <NuxtLink to="/" class="navbar-item">Home</NuxtLink>          
          <NuxtLink to="/tournaments" class="navbar-item">Tournaments</NuxtLink>
          <NuxtLink to="/avenues" class="navbar-item">Avenues</NuxtLink>
          <NuxtLink to="/about" class="navbar-item">About</NuxtLink>

          <div class="navbar-item has-dropdown is-hoverable">
            <a class="navbar-link">
              More
            </a>

            <div class="navbar-dropdown">
              <NuxtLink to="/about" class="navbar-item">About</NuxtLink>
              <a class="navbar-item">
                Jobs
              </a>
              <a class="navbar-item">
                Contact
              </a>
              <hr class="navbar-divider">
              <a class="navbar-item">
                Report an issue
              </a>
            </div>
          </div>
        </div>

        <div class="navbar-end">
          <div class="navbar-item">
            <h1 class="welcome-msg" v-if="authStore.user.username">Welcome, <NuxtLink :to="`/users/user/${authStore.user.username}`" >{{ authStore.user.username }}</NuxtLink>!</h1>
            <div class="buttons" v-if="authStore.user.username">
              <a class="button is-light" @click="authStore.logout">
                Logout
              </a>
            </div>
            <div class="buttons" v-else>
              <a class="button is-primary">
                <strong><NuxtLink to="/register" class="button-link">Sign up</NuxtLink></strong>
              </a>
              <a class="button is-light">
                <NuxtLink to="/login" class="button-link-dark">Log in</NuxtLink>
              </a>
            </div>
          </div>
        </div>
      </div>
    </nav>
  </div>
</template>

<style scoped>
.welcome-msg {
  padding-right: 5px;
}

.button-link {
  color: aliceblue;
}

.button-link-dark {
  color: black;
}
</style>