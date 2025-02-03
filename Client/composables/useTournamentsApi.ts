export const useTournamentsApi = async <T>(endpoint: string, options?: any) => {
    const config = useRuntimeConfig()
    
    // Handle both "/Tournaments/All" and "Tournaments/All" (without leading slash)
    const url = endpoint.startsWith('/')
      ? endpoint
      : `/${endpoint}`
  
    return useFetch<T>(url, {
      baseURL: config.public.tournamentsBase,
      ...options
    })
  }
  