import { storeToRefs } from 'pinia';
import { useAuthStore } from '@/stores/auth'

export default defineNuxtRouteMiddleware((to, from) => {
    const { user } = storeToRefs(useAuthStore());
    if(user.value.username){
        return navigateTo('/')
    }
})