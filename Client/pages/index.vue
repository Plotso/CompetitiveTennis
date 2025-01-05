<script setup lang="ts">
definePageMeta({
  layout: "default-transparent",
});
import {useAuthStore} from "~/stores/auth"
const authStore = useAuthStore();
</script>

<template>
    <div>
        <Banner title="Participate in locally or globally held tennis tournaments" description="Tennis is a sport for everyone. Join the competitive side of it and enjoy meeting with new people that share your passion.">
            
        </Banner>
        <AuthHomeSection v-if="authStore.user.username"/>
        <PublicHomeSection v-else/>
        <UpcomingTournaments/>
    </div>
</template>

<style scoped>

</style>