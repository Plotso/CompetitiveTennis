<script setup lang="ts">
  import {useAuthStore} from "@/stores/auth"
  const authStore = useAuthStore();
  onBeforeMount(() => {
    if(!authStore.user.username)
      authStore.loadFromStorage();
  })

  useHead({
    link: [
      {rel: 'stylesheet', href: 'https://cdn.jsdelivr.net/npm/bulma@0.9.4/css/bulma.min.css'}
    ]
  })

  const tourId = ref(1);
</script>

<template>
  <div>
    <!--
      <NuxtWelcome />
      -->
  </div>
  <NuxtLayout>
    
    <SeoKit />
    <NuxtPage/>

    <!--
    TournamentId: {{ tourId }}
    <button @click="tourId++">+</button>
    <button @click="tourId--">-</button>
        <NuxtLink to="/">Home</NuxtLink> | 
        <NuxtLink to="/tournaments">Tournaments</NuxtLink> | 
        <NuxtLink :to="'tournaments'">TournamentsName</NuxtLink> | 
        <NuxtLink :to="{name: 'tournaments'}">TournamentsNameBr</NuxtLink> | 
        <NuxtLink :to="`/tournaments/${tourId}`">Tournaments {{tourId}}</NuxtLink> |
        <NuxtLink :to="`/tournaments/create`">Create Tournament</NuxtLink>
        -->
  </NuxtLayout>
</template>

<style>
@media (min-width: 769px) {
  .is-hidden-desktop {
    display: none;
  }
}
  
.custom-link {
    color: green;
}

.edit-button {
  color: rgb(0, 248, 45);
}

.delete-button {
  color: rgb(255, 1, 1);
}
</style>
