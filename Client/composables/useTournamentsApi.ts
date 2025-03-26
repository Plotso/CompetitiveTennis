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

export const useTournamentsApiAsync = async <T>(endpoint: string, options?: any) => {
  const config = useRuntimeConfig();
  
  // Ensure the endpoint starts with a "/"
  const url = endpoint.startsWith('/') ? endpoint : `/${endpoint}`;
  console.log('useTournamentsApiAsync called:', url, options);
  
  return await useAsyncData<T>(
    url, 
    () => $fetch(url, { baseURL: config.public.tournamentsBase, ...options }),
    { ssr: true, lazy: true,
      retry: 3,        // Retry up to 3 times if fetch fails
      retryDelay: 1000 // Wait 1 second between retries 
      }
  );
}

