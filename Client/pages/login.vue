<script setup lang="ts">
import { useAuthStore } from "@/stores/auth"
import { storeToRefs } from 'pinia'
definePageMeta({
    layout: 'authentication',
    middleware: ['login']
})
const authStore = useAuthStore();

const termsAgreed = ref(false);

const router = useRouter();
onMounted(() => {
    const { user } = storeToRefs(authStore);
    if (user.value.username) {
        router.push('/')
    }
})

const user = ref({
    loginInfo: '',
    password: '',
    emailLogin: true
} as UserInputModel)

const submitForm = async () => {
    try {

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
        <section class="is-relative section py-20">
            <div class="is-absolute is-left-0 is-bottom-0 is-right-0 has-mw-5xl has-background-white" style="height: 100%; width: 100%;" >
            </div>
            <div class="is-relative container">
                <div class="columns is-vcentered">
                    <div class="column is-6 mb-8 mb-0-desktop  is-hidden-mobile">
                        <img src="~/public/logo.png" alt="competitive-tennis login page">
                        <p class="is-size-5 has-text-grey-dark">Embrace the challenge. Sign in and let your journey on the field begin. Success is waiting for those who dare to take the first step.</p>
                    </div>
                    <div class="column is-5" style="padding-top:20%">
                        <div class="box p-6 px-10-desktop py-12-desktop has-background-white has-text-centered">
                            <form  @submit.prevent="submitForm">
                                <img src="~/public/logo.png" alt="competitive-tennis login page" class="is-hidden-desktop" >
                                <span class="has-text-primary has-text-weight-semibold">Sign In</span>
                                <h3 class="title is-4 mt-4 mb-12">Login to an existing account</h3>

                                <div class="field">
                                    <p class="control has-icons-left has-icons-right">
                                        <input class="input" type="text" placeholder="Email or username" v-model="user.loginInfo"  required>
                                        <span class="icon is-small is-left">
                                            <font-awesome-icon icon="fa-solid fa-user" />
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
                                    </p>
                                </div>
                                <div class="field">
                                    <input type="checkbox" class="form-check-input custom-checkbox" id="email-login" v-model="user.emailLogin" >
                                    <label class="form-check-label" for="email-login">Use email to login</label>
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
                                            Login
                                        </button>
                                    </p>
                                </div>
                                <div class="has-text-centered">
                                    <p class="is-size-7"> Don't have an account? <NuxtLink to="/register" class="has-text-primary">Sign up</NuxtLink></p>
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
.custom-checkbox {
    color: green !important;
}

</style>