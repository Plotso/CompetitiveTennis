<script setup lang="ts">

definePageMeta({
  layout: "default-alt",
});
import {MatchShortOutputModel, Result} from "types"
import MatchOverview from "~/components/MatchOverview.vue";
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
    <div v-if="pending" class="view-window">
        <Loading></Loading>
    </div>
    <div v-else class="view-window">        
        <MatchTournamentHeader :tournamentId="data?.data.tournamentId"></MatchTournamentHeader>
        <MatchOverview :data="data" :organiserUsername="tournamentOrganiserUsername.data.value.data" v-if="data"></MatchOverview>
        <MatchScoreOverview :data="data" :organiserUsername="tournamentOrganiserUsername.data.value.data" v-if="data"></MatchScoreOverview>
    </div>
</template>

<style scoped>

</style>