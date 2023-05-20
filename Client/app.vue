<script setup lang="ts">
  import {useAuthStore} from "@/stores/auth"
  const authStore = useAuthStore();
  onMounted(() => {
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

    TournamentId: {{ tourId }}
    <button @click="tourId++">+</button>
    <button @click="tourId--">-</button>
        <NuxtLink to="/">Home</NuxtLink> | 
        <NuxtLink to="/tournaments">Tournaments</NuxtLink> | 
        <NuxtLink :to="'tournaments'">TournamentsName</NuxtLink> | 
        <NuxtLink :to="{name: 'tournaments'}">TournamentsNameBr</NuxtLink> | 
        <NuxtLink :to="`/tournaments/${tourId}`">Tournaments {{tourId}}</NuxtLink>
  </NuxtLayout>
</template>
