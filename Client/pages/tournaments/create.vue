<script setup lang="ts">
//import { TournamentInputModel } from 'types'
import { AvenueOutputModel} from '@/types';
definePageMeta({
  'auth': true
})
const config = useRuntimeConfig();

const { data, pending, refresh, error } = await useFetch<Result<AvenueOutputModel[]>>(() => `/Avenues/All`, {
    baseURL: config.public.tournamentsBase
})
if (error.value) {
    console.log('data', data.value)
    console.log('pending', pending.value)
    console.log('error', error.value)
    refresh()
}

/*
interface TournamentInputModel {
  title: string;
  rules: string;
  description: string;
  avenueIId: number;
  // Rest of the properties
}

const form = ref<TournamentInputModel>({
  title: '',
  rules: '',
  description: '',
  // Initialize other properties
})
*/

const form = ref({
  title: "",
  avenueId: 5
  // Initialize other properties
})
const createTournament = () => {
  // Implement the logic to send the form data to the API for tournament creation
}
</script>

<template>
    <div class="container">
        Create tournament
        <h1>Create Tournament</h1>
    <form @submit.prevent="createTournament">
      <div class="field">
        <label class="label">Title</label>
        <div class="control">
          <input class="input" type="text" v-model="form.title" required />
        </div>
      </div>
      <!-- Add other form fields based on the TournamentInputModel properties -->
      <div class="field">
        <label class="label">Avenue</label>
        <div class="control">
          <div class="select">
            <select v-model="form.avenueId" required>
              <option value="">Select Avenue</option>
              <option v-for="avenue in data.value" :key="avenue.id" :value="avenue.id">{{ avenue.name }}</option>
            </select>
          </div>
        </div>
      </div>
      <div class="field">
        <div class="control">
          <button class="button is-primary" type="submit">Create</button>
        </div>
      </div>
    </form>
    </div>
</template>

<style scoped>

</style>