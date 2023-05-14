<script setup lang="ts">

  import {useAuthStore} from "@/stores/auth"
  const authStore = useAuthStore();
  onMounted(() => {
    if(!authStore.user.username)
      authStore.loadFromStorage();
  })
</script>

<template>
  <div>
    <NuxtWelcome />
  </div>
  <NuxtLayout>
    <NuxtPage/>
  </NuxtLayout>
</template>
