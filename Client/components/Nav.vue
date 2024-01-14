<script setup lang="ts">
import { boolean } from "zod";
import {useAuthStore} from "../stores/auth"
const authStore = useAuthStore();

const props = defineProps({
    isTransparent: {
      type: Boolean,
      default: true
    }
  });

const isActive = ref(false);
const scrolled = ref(false);
if(!props.isTransparent)
{
  scrolled.value = true;
}

function toggle() {
  isActive.value = !isActive.value
  scrolled.value = isActive.value;
  if(!props.isTransparent)
  {
    scrolled.value = true;
  }
}

function classData(classInfo: string): string {
  return isActive ? `${classInfo} is-active` : classInfo
}

onMounted(() => {
  window.addEventListener('scroll', handleScroll);
});

function handleScroll() {
  scrolled.value = window.scrollY > 0;
  if(!props.isTransparent)
  {
    scrolled.value = true;
  }
}

function linkClicked() {  
  isActive.value = false
  scrolled.value = false;
}
</script>

<template>
  <div>
    <nav class="navbar" role="navigation" aria-label="main navigation" :class="{ 'scrolled': scrolled }">
      <div class="navbar-brand">
        <NuxtLink to="/" class="navbar-item"><img src="@/public/logo-new.png"></NuxtLink>

        <a role="button" class="navbar-burger" aria-label="menu" aria-expanded="true" data-target="navbarBasicExample"
          @click="toggle()">
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
        </a>
      </div>

      <div class="navbar-menu" :class="{ 'is-active': isActive }">
        <div class="navbar-start">
          <NuxtLink to="/" :class="[{'not-scrolled': !scrolled}, {'transparent': isActive || isTransparent}, 'navbar-item']" @click="linkClicked">Home</NuxtLink>          
          <NuxtLink to="/tournaments" :class="[{'not-scrolled': !scrolled}, {'transparent': isTransparent}, 'navbar-item']" @click="linkClicked">Tournaments</NuxtLink>
          <NuxtLink to="/avenues" :class="[{'not-scrolled': !scrolled}, {'transparent': isTransparent}, 'navbar-item']" @click="linkClicked">Avenues</NuxtLink>
          <NuxtLink to="/about" :class="[{'not-scrolled': !scrolled}, {'transparent': isTransparent}, 'navbar-item']" @click="linkClicked">About</NuxtLink>

          <div class="navbar-item has-dropdown is-hoverable" :class="[{ 'scrolled': scrolled }, {'transparent': isTransparent}]">
            <a :class="[{'not-scrolled': !scrolled}, 'navbar-link', 'more-dropdown', {'transparent': isTransparent}]">
              More
            </a>

            <div class="navbar-dropdown" :class="[{ 'scrolled': scrolled }, {'transparent': isTransparent}]">
              <NuxtLink to="/about" :class="[{'not-scrolled': !scrolled}, {'transparent': isTransparent}, 'navbar-item']" @click="linkClicked">About</NuxtLink>
              <a :class="[{'not-scrolled': !scrolled}, {'transparent': isTransparent}, 'navbar-item']" @click="linkClicked">
                Jobs
              </a>
              <a :class="[{'not-scrolled': !scrolled}, {'transparent': isTransparent}, 'navbar-item']" @click="linkClicked">
                Contact
              </a>
              <hr class="navbar-divider">
              <a :class="[{'not-scrolled': !scrolled}, {'transparent': isTransparent}, 'navbar-item']" @click="linkClicked">
                Report an issue
              </a>
            </div>
          </div>
        </div>

        <div class="navbar-end">
          <div class="navbar-item">
            <h1 :class="['welcome-msg', {'transparent': isTransparent}]" v-if="authStore.user.username">
              Welcome, <NuxtLink :to="`/users/user/${authStore.user.username}`" :class="['user-link', {'transparent': isTransparent}]">{{ authStore.user.username }}</NuxtLink>!
            </h1>
            <div class="buttons" v-if="authStore.user.username">
              <a class="button is-light custom-button" @click="authStore.logout">
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

.welcome-msg.transparent {
  color: white
}

.user-link.transparent {
  color: #00d1b2
}

.button-link {
  color: aliceblue;
}

.button-link-dark {
  color: black;
}

.navbar {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  background-color: transparent;
  transition: background-color 0.3s;
  z-index: 100;
}

.navbar.scrolled {
  background-color: white;
  color: black;
}

.navbar-dropdown {
  background-color: transparent;
  transition: background-color 0.3s;
  z-index: 100;
}

.navbar-dropdown.scrolled {
  background-color: white;
  color: black;
}

.not-scrolled {
  color: white;
}
.more-dropdown {
  
  background-color: transparent;
  transition: background-color 0.3s;
  z-index: 100;
}
.more-dropdown:hover {
  
  background-color: transparent;
  transition: background-color 0.3s;
  z-index: 100;
}
.more-dropdown.scrolled{
  background-color: white;
  color: black;
}

.navbar-link.transparent:hover {
  background-color: transparent;
  color: white;
}

.navbar-link.transparent::after {
  content: "";
  position: absolute;
  bottom: -2px;
  background-color: transparent;
  transition: background-color 0.3s;
}

.navbar-link.transparent {
  position: relative;
}

.navbar-item.transparent:hover {
  background-color: transparent;
  color: white;
}

.navbar-item.transparent {
  position: relative;
}

.navbar-item.transparent::after {
  content: "";
  position: absolute;
  bottom: -2px;
  left: 0;
  width: 100%;
  height: 2px;
  background-color: transparent;
  transition: background-color 0.3s;
}

.navbar-item:hover::after {
  background-color: #00d1b2; /* Change this color to your desired underscore color */
}

.navbar-link::after {
  border-color: #00d1b2; /* Change this color to your desired underscore color */
}
</style>