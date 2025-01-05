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
    <section class="is-relative section py-20">
            <div class="is-absolute is-left-0 is-bottom-0 is-right-0 has-mw-5xl has-background-white" style="height: 100%; width: 100%;" >
            </div>
            <div class="is-relative container">
                <div class="columns is-vcentered">
                    <div class="column is-6 mb-8 mb-0-desktop  is-hidden-mobile">
                        <img src="~/public/logo.png" alt="competitive-tennis register page">
                        <p class="is-size-5 has-text-grey-dark">Unleash your inner champion and ignite the fire within. Sign up now and let your dreams take flight on the wings of dedication, passion, and perseverance.</p>
                    </div>
                    <div class="column is-5" style="padding-top:20%">
                        <div class="box p-6 px-10-desktop py-12-desktop has-background-white has-text-centered">
                            <form  @submit.prevent="submitForm">
                                
                                <img src="~/public/logo.png" alt="competitive-tennis register page" class="is-hidden-desktop">
                                <span class="has-text-primary has-text-weight-semibold">Sign Up</span>
                                <h3 class="title is-4 mt-4 mb-12">Create an account</h3>

                                <div class="field">
                                    <p class="control has-icons-left has-icons-right">
                                        <input class="input" type="email" placeholder="Email" v-model="user.email"  required>
                                        <span class="icon is-small is-left">
                                            <font-awesome-icon icon="fa-solid fa-envelope" />
                                        </span>
                                        <span class="icon is-small is-right">
                                            <i class="fas fa-check"></i>
                                        </span>
                                    </p>
                                </div>

                                <div class="field">
                                    <p class="control has-icons-left has-icons-right">
                                        <input class="input" type="text" placeholder="Username" v-model="user.username"  required>
                                        <span class="icon is-small is-left">
                                            <font-awesome-icon icon="fa-solid fa-user" />
                                        </span>
                                        <span class="icon is-small is-right">
                                            <i class="fas fa-check"></i>
                                        </span>
                                    </p>
                                </div>

                                <div class="field">
                                    <p class="control has-icons-left has-icons-right">
                                        <input class="input" type="text" placeholder="First Name" v-model="user.firstName"  required>
                                        <span class="icon is-small is-left">
                                          <font-awesome-icon icon="fa-solid fa-signature" />
                                        </span>
                                        <span class="icon is-small is-right">
                                            <i class="fas fa-check"></i>
                                        </span>
                                    </p>
                                </div>

                                <div class="field">
                                    <p class="control has-icons-left has-icons-right">
                                        <input class="input" type="text" placeholder="Last name" v-model="user.lastName"  required>
                                        <span class="icon is-small is-left">
                                          <font-awesome-icon icon="fa-solid fa-signature" />
                                        </span>
                                        <span class="icon is-small is-right">
                                            <i class="fas fa-check"></i>
                                        </span>
                                    </p>
                                </div>
                                <div class="field">
                                    <p class="control has-icons-left">
                                        <input class="input" type="password" placeholder="Password" v-model="user.password" required>
                                        <span class="icon is-small is-left">
                                          <font-awesome-icon icon="fa-solid fa-lock" />
                                        </span>
                                        <span class="is-small is-size-7">
                                            * Requires at least 6 characters with at least 1 digit, upper and lower case characters
                                        </span>
                                    </p>
                                </div>
                                <!--
                                <div class="field">                
                                    <input type="checkbox" class="form-check-input" id="terms" v-model="termsAgreed">
                                    <label class="form-check-label" for="terms">
                                        By signing up, you agree to our <NuxtLink to="/terms">Terms</NuxtLink>, <NuxtLink to="/privacy"></NuxtLink>Privacy Policy</NuxtLink> and <NuxtLink to="cookies">Cookies Policy</NuxtLink>.
                                    </label>
                                    
                                </div>
                                -->
                                <div class="field">
                                    <p class="control">
                                        <button class="button is-primary is-fullwidth">
                                            Register
                                        </button>
                                    </p>
                                </div>
                                <div class="has-text-centered">
                                    <p class="is-size-7"> Already have an account? <NuxtLink to="/login" class="has-text-primary">Sign in</NuxtLink></p>
                                    <hr>
                                    <p class="is-size-7"> <NuxtLink to="/" class="has-text-primary">Back to home</NuxtLink></p>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</template>

<style scoped>
</style>