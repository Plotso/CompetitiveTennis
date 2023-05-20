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
            <div class="buttons">
              <a class="button is-primary">
                <strong>Sign up</strong>
              </a>
              <a class="button is-light">
                Log in
              </a>
            </div>
          </div>
        </div>
      </div>
    </nav>
  </div>
</template>

<script setup lang="ts">

const isActive = ref(false);

function toggle() {
  isActive.value = !isActive.value
}

function classData(classInfo: string): string {
  return isActive ? `${classInfo} is-active` : classInfo
}
</script>

<style scoped></style>