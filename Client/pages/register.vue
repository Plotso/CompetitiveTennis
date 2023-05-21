<script setup lang="ts">
import { storeToRefs } from 'pinia'
import {useAuthStore} from "@/stores/auth"
definePageMeta({
    layout: 'authentication',    
    middleware: ['login']
})
const authStore = useAuthStore();

const router = useRouter();
onMounted(() =>{
    const { user } = storeToRefs(authStore);
    if(user.value.username){
        router.push('/')
    }
} )

const user = ref({
      email: '',
      username: '',
      firstName: '',
      lastName: '',
      password: ''
    } as RegisterInputModel)



const submitForm = async () => {
    try{
        console.log(user.value);
        await authStore.register(user.value)
        // Redirect to home page
        //this.$router.push('/')
    } catch (e) {
        console.error(e)
      }
}
</script>

<template>
    <div>
        <form @submit.prevent="submitForm">
      <div>
        <label>Email:</label>
        <input type="email" v-model="user.email" required>
      </div>
      <div>
        <label>Username:</label>
        <input type="text" v-model="user.username" required>
      </div>
      <div>
        <label>First name:</label>
        <input type="text" v-model="user.firstName" required>
      </div>
      <div>
        <label>Last name:</label>
        <input type="text" v-model="user.lastName" required>
      </div>
      <div>
        <label>Password:</label>
        <input type="password" v-model="user.password" required>
      </div>
      <button>Register</button>
    </form>
    </div>
</template>

<style scoped>

</style>