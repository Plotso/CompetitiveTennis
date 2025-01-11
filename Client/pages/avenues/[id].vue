<script setup lang="ts">
import {AvenueOutputModel, Result} from "types"
const route = useRoute();
const config = useRuntimeConfig();
    
const {data, pending, refresh, error} = await useFetch<Result<AvenueOutputModel>>(() => `/Avenues/${route.params.id}`,{
    baseURL: config.public.tournamentsBase,
    method: 'GET'
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
        <Avenue :data="data" v-if="data"></Avenue>
    </div>
</template>

<style lang="scss" scoped>

</style>