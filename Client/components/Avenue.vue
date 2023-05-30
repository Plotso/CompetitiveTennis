<script setup lang="ts">
import { Surface, CourtType, TournamentType, Result, AvenueOutputModel } from "@/types"
import {useAuthStore} from "~/stores/auth"
const props = defineProps({
    data: {type: Object as PropType<Result<AvenueOutputModel>>, required: true}
})
const authStore = useAuthStore();

const avenueData = toRef(props, "data")
const avenue = ref(avenueData.value.data)

const getSurfaceLabel = (surface: Surface): string => {
  return Number.isInteger(surface) ? Surface[surface] : surface.toString();
};

const getTournamentType = (type: TournamentType): string => {
  return Number.isInteger(type) ? TournamentType[type] : type.toString();
};

const formatDate = (date: Date): string => {
  const options: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  };
  return new Date(date).toLocaleDateString(undefined, options);
};
</script>

<template>
    <div class="container">
    <h1 class="title is-1 has-text-centered">{{ avenue.name }} 
      <NuxtLink :to="`/avenues/edit/${avenue.id}`" v-if="authStore.user && authStore.user.hasAdministrativeRights"><font-awesome-icon icon="fa-solid fa-pen-to-square" /></NuxtLink></h1>
    <h2 class="subtitle has-text-centered"><font-awesome-icon icon="fa-solid fa-location-dot" /> {{ avenue.location }}, {{ avenue.city }}, {{ avenue.country }}</h2>
        <hr>
    <div class="section box">
      <div class="columns">
        <div class="column">
          <h3 class="title is-5 has-text-centered"><font-awesome-icon icon="fa-solid fa-message" /> Details:</h3>
          <p>{{ avenue.details }}</p>
        </div>
        <div class="column">
          <h3 class="title is-5 has-text-centered"><font-awesome-icon icon="fa-solid fa-circle-check" /> Verification:</h3>
          <p>
            <font-awesome-icon v-if="avenue.isVerified" icon="fa-solid fa-check" />
            <font-awesome-icon v-else icon="fa-solid fa-xmark" />
            {{ avenue.isVerified ? 'Verified' : 'Not Verified' }}
        </p>
        </div>
      </div>
    </div>

    <div class="section box">
      <h3 class="title is-4 has-text-centered">&#x1F3BE; Courts:</h3>
      <div class="columns">
        <div class="column" v-for="court in avenue.courts" :key="court.surface">
          <h4 class="title is-5">{{ getSurfaceLabel(court.surface) }}:</h4>
          <p v-for="(count, type) in court.availableCourtsByType" :key="type">
            {{ type }}: {{ count }}
          </p>
        </div>
      </div>
    </div>

    <div class="section box">
      <h3 class="title is-4 has-text-centered"><font-awesome-icon icon="fa-solid fa-trophy" /> Tournaments:</h3>
      <div class="table-container">
        <table class="table is-fullwidth is-hoverable">
        <thead>
          <tr>
            <th>Title</th>
            <th>Type</th>
            <th>Surface</th>
            <th>Entry Fee</th>
            <th>Prize</th>
            <th>Start Date</th>
            <th>End Date</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="tournament in avenue.tournaments" :key="tournament.id">
            <td><NuxtLink class="custom-link" :to="`/tournaments/${tournament.id}`">{{ tournament.title }}</NuxtLink></td>
            <td>{{ getTournamentType(tournament.type) }}</td>
            <td>{{ getSurfaceLabel(tournament.surface) }}</td>
            <td>{{ tournament.entryFee ? `$${tournament.entryFee}` : 'N/A' }}</td>
            <td>{{ tournament.prize ? `$${tournament.prize}` : 'N/A' }}</td>
            <td>{{ formatDate(tournament.startDate) }}</td>
            <td>{{ formatDate(tournament.endDate) }}</td>
          </tr>
        </tbody>
      </table>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>