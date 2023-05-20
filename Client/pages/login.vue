<script setup lang="ts">
definePageMeta({
    layout: 'authentication',    
    middleware: ['login']
})

import {useAuthStore} from "@/stores/auth"
const authStore = useAuthStore();

const user = ref({
      loginInfo: '',
      password: '',
      emailLogin: true
    } as UserInputModel)

const submitForm = async () => {
    try{

        await authStore.login(user.value)
        // Redirect to home page
        //$router.push('/')
    } catch (e) {
        console.error(e)
      }
}
</script>

<template>
    <div>
        
        <form @submit.prevent="submitForm">
        <div class="form-group">
            <label for="login-info">Username or email address</label>
            <input type="text" class="form-control" id="login-info" v-model="user.loginInfo" required>
        </div>
        <div class="form-group">
            <label for="password">Password</label>
            <input type="password" class="form-control" id="password" v-model="user.password" required>
        </div>
        <div class="form-group form-check">
            <input type="checkbox" class="form-check-input" id="email-login" v-model="user.emailLogin">
            <label class="form-check-label" for="email-login">Use email to login</label>
        </div>
        <button type="submit" class="btn btn-primary">Login</button>
        </form>
    </div>
</template>

<style scoped>

</style>