

export default defineEventHandler(async event => {
    /*
    const result = await useFetch<Result<Tournaments.TournamentOutputModel>>(() => `/Tournaments/1`,{
        baseURL: useRuntimeConfig().public.tournamentsBase
    })
    */
    const result = await $fetch(`${useRuntimeConfig().public.tournamentsBase}/Tournaments/1`);
    return result
})