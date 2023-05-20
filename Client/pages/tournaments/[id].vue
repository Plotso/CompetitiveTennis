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

function rename(){
    data.value.data.title += " Gosho";
}
</script>

<template>
    <div v-if="pending">
        <Loading></Loading>
    </div>
    <div v-else>
        <!--
        <button @click="rename" class="button"> Add Gosho</button>
        Gosho {{ $route.params }}
        <h1>{{ data?.data.title }}</h1>
        <p>{{ data?.data.description }}</p>
-->
        
        <Tournament :data="data" v-if="data"></Tournament>
    </div>
</template>

<style scoped>

</style>