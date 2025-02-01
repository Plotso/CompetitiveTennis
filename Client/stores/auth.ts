// @ts-nocheck
// disabled ts due to issue with POST request when sending useFetch<T> requests https://github.com/nuxt/nuxt/issues/20150
import {Auth, Result} from 'types'

export const useAuthStore = defineStore("auth", () => {
    const router = useRouter();
    const config = useRuntimeConfig();
    const userState:UserState = ref({
      email: ref(""),
      username: ref(""),
      isAdmin: ref(false)
    });

    const user = ref({})
    const token = ref("");

    async function register(userInput:Auth.RegisterInputModel)  {
      const { data, error, pending } = await useFetch(() => `/Identity/Register`, {
        baseURL: config.public.authBase,
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(userInput)
      })

      if(!error.value){
        user.value = data.value.data
        token.value = data.value.data.token
        localStorage.setItem('token', token.value!)
        localStorage.setItem('user', JSON.stringify(data.value.data))

        const addAcc = {'firstName': data.value.data.firstName, 'lastName': data.value.data.lastName}
        const accCreate = await useFetch(() => `/Accounts/Add`, {
          baseURL: config.public.tournamentsBase,
          method: 'POST',
          headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token.value}` },
          body: JSON.stringify(addAcc)
        })
        router.push("/")
      }      
    }

    async function login(userInput:Auth.UserInputModel) {
      const { data, error, pending } = await useFetch<Result<Auth.UserOutputModel>>(() => `/Identity/Login`, {
        baseURL: config.public.authBase,
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(userInput)
      });
      if(!data.value)
      {
        console.log("No data received")
        return;
      }

      user.value = data.value.data
      token.value = data.value.data.token
      localStorage.setItem('token', token.value!)
      localStorage.setItem('user', JSON.stringify(data.value.data))
      router.push("/")
    }

      
    async function loadFromStorage() {
      const storageToken = localStorage.getItem('token')
      if (storageToken) {
        token.value = storageToken
      } else {
        //$router.push('/register')
      }
      const userStr = localStorage.getItem('user');
      if (userStr)
      {
        const userData = JSON.parse(userStr);
        user.value = userData
      }
    }

    async function resetStore() {
      token.value = ""
      user.value = {}
    }
    async function logout() {
      // reset store to original state
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      resetStore();
    }

    // getters
    const isAuthenticated = computed(() => {// can use (() => !!user.value)
        !user.value ? false : true 
    })

    return {user, resetStore, token, isAuthenticated, register, login, logout, loadFromStorage}
})