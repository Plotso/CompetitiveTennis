// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
    typescript: {
      shim: false
    },
    modules: [
      // ...
      '@pinia/nuxt',
    ],
    css: [
      '@fortawesome/fontawesome-svg-core/styles.css'
    ],
    runtimeConfig: {
      public: { //https://nuxt.com/docs/guide/going-further/runtime-config
        authBase: 'https://localhost:7183', // can be overridden by NUXT_PUBLIC_AUTH_BASE environment variable
        tournamentsBase: 'https://localhost:7277', // can be overridden by NUXT_PUBLIC_TOURNAMENTS_BASE environment variable
      }
    },
    imports: {
      dirs: [
        
      // Scan top-level modules
      'composables',
      // ... or scan modules nested one level deep with a specific name and file extension
      'composables/*/index.{ts,js,mjs,mts}',
      // ... or scan all modules within given directory
      'composables/**',
      'server/**'
      ]      
    },
    pinia: {
      autoImports: [
        // automatically imports `defineStore`
        "defineStore", // import { defineStore } from 'pinia'
        ["defineStore", "definePiniaStore"], // import { defineStore as definePiniaStore } from 'pinia'
      ],
    }
})
