export const useIdentityApi = async <T>(endpoint: string, options?: any) => {
    const config = useRuntimeConfig()
    
    // Handle both "/Identity/Login" and "Identity/Login" (without leading slash)
    const url = endpoint.startsWith('/')
      ? endpoint
      : `/${endpoint}`
  
    return useFetch<T>(url, {
      baseURL: config.public.authBase,
      ...options
    })
  }
  