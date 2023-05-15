<script setup lang="ts">
    import {Tournaments, Result} from "types"
    const route = useRoute();
    const config = useRuntimeConfig();
    const {data, error} = await useFetch<Result<Tournaments.TournamentOutputModel>>(() => `/Tournaments/${route.params.id}`,{
        baseURL: config.public.tournamentsBase
    })
    if(error && error.statusCode == 404)
    {
        //ToDo: Redirect to NotFound page
    }
</script>

<template>
    <div>

    </div>
</template>

<style scoped>

</style>