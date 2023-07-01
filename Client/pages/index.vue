<script setup lang="ts">
import {useAuthStore} from "~/stores/auth"
const authStore = useAuthStore();
</script>

<template>
    <div>
        Hi
        <Banner/>
        <AuthHomeSection v-if="authStore.user.username"/>
        <PublicHomeSection v-else/>
        <UpcomingCourses/>
    </div>
</template>

<style scoped>

</style>