<script setup lang="ts">
import {TournamentOutputModel, Result} from "types"
const route = useRoute();
const config = useRuntimeConfig();

const {data, pending, refresh, error} = await useFetch<Result<TournamentOutputModel>>(() => `/Tournaments/${route.params.id}`,{
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
        <BaseLoading></BaseLoading>
    </div>
    <div v-else>        
        <TournamentInfo :data="data" v-if="data"></TournamentInfo>
    </div>
</template>

<style scoped>

</style>