// @ts-nocheck
// disabled ts due to issue with POST request when sending useFetch<T> requests https://github.com/nuxt/nuxt/issues/20150

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

    async function register(userInput:RegisterInputModel)  {
      const { data } = await useFetch(() => `/Identity/Register`, {
        baseURL: config.public.authBase,
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(userInput)
      })

      user.value = data.value
      token.value = data.value.token
      localStorage.setItem('token', token.value!)
      localStorage.setItem('user', JSON.stringify(value.value))
      router.push("/")
    }

    async function login(userInput:UserInputModel) {
      const { data } = await useFetch<UserOutputModel>(() => `/Identity/Login`, {
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

      user.value = data.value
      token.value = data.value.token
      localStorage.setItem('token', token.value!)
      localStorage.setItem('user', JSON.stringify(data.value))
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