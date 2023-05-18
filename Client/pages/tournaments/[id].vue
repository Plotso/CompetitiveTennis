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

    function rename(){
        data.value.data.title += " Gosho";
    }
</script>

<template>
    <div>
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