<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { Surface, TournamentType, Result, TournamentOutputModel, SlimTournamentOutputModel, ParticipantInputModel, MatchShortOutputModel } from "@/types"
import { useAuthStore } from "~/stores/auth"
const config = useRuntimeConfig();
const props = defineProps({
  tournamentId: Number
})
const showLoadingModal = ref(false)
console.log("we're here" + props.tournamentId)
const { data, pending, refresh, error } = await useFetch<Result<string>>(() => `/Tournaments/GetTournamentName/${props.tournamentId}`, {
  baseURL: config.public.tournamentsBase
})
console.log("we're NOW here" + data)
if (error.value) {
  console.log('data', data.value)
  console.log('pending', pending.value)
  console.log('error', error.value)
  refresh()
}
if (error && error.statusCode == 404) {
  //ToDo: Redirect to NotFound page
}
const tournamentName = ref(data.value.data);
</script>

<template>
  <div class="container">
    <div class="tournament-header">
      <NuxtLink :to="`/tournaments/${tournamentId}`" class="tournament-header-item">
        <h1 class="tournament-name"><font-awesome-icon icon="fa fa-trophy" />  {{ tournamentName }}</h1>
      </NuxtLink>
      
    </div>
  </div>

  <!--MODALS-->
  <ModalsLoadingModal :isOpen="showLoadingModal" />
</template>

<style scoped>
.container {
  margin-top: 20px;
  padding-top: 3%;
}

.box {
  margin-bottom: 20px;
}

.remove-participant-button {
  font-size: x-small;
}

.tournament-header-item {
color: #00d1b2;
}
</style>