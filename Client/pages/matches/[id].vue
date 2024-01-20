<script setup lang="ts">
import {MatchShortOutputModel, Result} from "types"
const route = useRoute();
const config = useRuntimeConfig();


const tournamentOrganiserUsername = await useFetch<Result<string>>(() => `/Matches/GetOrganiserUsername/${route.params.id}`, {
  baseURL: config.public.tournamentsBase
})

if (tournamentOrganiserUsername.error.value) {
  console.log('data', tournamentOrganiserUsername.data.value)
  console.log('pending', tournamentOrganiserUsername.pending.value)
  console.log('error', tournamentOrganiserUsername.error.value)
  tournamentOrganiserUsername.refresh()
}

const {data, pending, refresh, error} = await useFetch<Result<MatchShortOutputModel>>(() => `/Matches/${route.params.id}`,{
    baseURL: config.public.tournamentsBase
})
if(error.value){
    console.log('data', data.value)
    console.log('pending', pending.value)
    console.log('error', error.value)
    refresh()
}
if(error && error.statusCode == 404)
{
    //ToDo: Redirect to NotFound page
}
</script>

<template>
    <div v-if="pending">
        <Loading></Loading>
    </div>
    <div v-else>        
        <Match :data="data" :organiserUsername="tournamentOrganiserUsername.data.value.data" v-if="data"></Match>
    </div>
</template>

<style scoped>

</style>