<script setup lang="ts">
//import { TournamentInputModel } from 'types'
import { AvenueOutputModel, TournamentInputModel, Surface, TournamentType, Result, TournamentOutputModel } from '@/types';
import { storeToRefs } from 'pinia';
import {useAuthStore} from "~/stores/auth"
definePageMeta({
  'auth': true
})
const route = useRoute();
const config = useRuntimeConfig();
const router = useRouter();
const authStore = useAuthStore();

// Fetch tournament data based on the given ID
const tournamentId = route.params.id;
const {data, pending, refresh, error} = await useFetch<Result<TournamentOutputModel>>(() => `/Tournaments/${tournamentId}`,{
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
const { user } = storeToRefs(useAuthStore());
if(user.value && (user.value.username != data.value?.data.organiser.username && !user.value.hasAdministrativeRights)){
  router.push(`/tournaments/${tournamentId}`)
}
const showCashPrize = ref(false);
const showPoints = ref(false);
const form = ref<TournamentInputModel>({
  title: '',
  rules: '',
  description: '',
  type: TournamentType.Singles,
  surface: Surface.Hard,
  entryFee: null,
  prize: null,
  courtsAvailable: 0,
  minParticipants: 0,
  maxParticipants: 0,
  matchWonPoints: null,
  setWonPoints: null,
  gameWonPoints: null,
  isIndoor: false,
  isLeague: false,
  startDate: new Date(),
  endDate: new Date(),
  avenueId: 200
})

if(data.value){
  form.value.title = data.value.data.title;
  form.value.rules = data.value.data.rules;
  form.value.description = data.value.data.description;
  form.value.type = data.value.data.type;
  form.value.surface = data.value.data.surface;
  form.value.entryFee = data.value.data.entryFee;
  form.value.prize = data.value.data.prize;
  form.value.courtsAvailable = data.value.data.courtsAvailable;
  form.value.minParticipants = data.value.data.minParticipants;
  form.value.maxParticipants = data.value.data.maxParticipants;
  form.value.matchWonPoints = data.value.data.matchWonPoints;
  form.value.setWonPoints = data.value.data.setWonPoints;
  form.value.gameWonPoints = data.value.data.gameWonPoints;
  form.value.startDate = data.value.data.startDate;
  form.value.endDate = data.value.data.endDate;
  form.value.avenueId = data.value.data.avenue.id;

  if(data.value.data.prize){
    showCashPrize.value = true;
  }
  if(data.value.data.matchWonPoints || data.value.data.setWonPoints || data.value.data.gameWonPoints){
    showPoints.value = true;
  }
  
}

const avenues = await useFetch<Result<AvenueOutputModel[]>>(() => `/Avenues/All`, {
  baseURL: config.public.tournamentsBase
})
if (avenues.error.value) {
  console.log('data', avenues.data.value)
  console.log('pending', avenues.pending.value)
  console.log('error', avenues.error.value)
  refresh()
}

const isUnauthorizedModalOpen = ref(false);
const updateTournament = async () => {
  try {
    const response = await fetch(`${config.public.tournamentsBase}/Tournaments/Edit/${tournamentId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authStore.token}`,
      },
      body: JSON.stringify(form.value),
    });

    if (response.ok) {
      router.push(`/tournaments/${tournamentId}`);
    } else {
      if(response.status == 401){
        isUnauthorizedModalOpen.value = true;
      }
      console.error(`Failed to update tournament. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while updating the tournament', error);
  }
};

const avenueLocation = ref("")
watch(avenues, () => {
  if (avenues.data.value.data && avenues.data.value.data.length > form.value.avenueId) {
    avenueLocation.value = avenues.data.value.data.find(avenue => avenue.id === form.value.avenueId)
  }
})

const selectedAvenue = computed(() => {
  const selectedId = form.value.avenueId
  return avenues.data.value?.data.find((avenue) => avenue.id === selectedId)
})

const openGoogleMaps = () => {
  if (selectedAvenue.value) {
    const queryInfo = `${selectedAvenue.value.location} ${selectedAvenue.value.name} ${selectedAvenue.value.city}`
    const location = encodeURIComponent(queryInfo); //(selectedAvenue.value.location);
    const googleMapsUrl = `https://www.google.com/maps/search/?api=1&query=${location}`;
    window.open(googleMapsUrl, '_blank');
  }
};

const isConfirmationModalOpen = ref(false);

const openConfirmationModal = () => {
  isConfirmationModalOpen.value = true;
};

const closeConfirmationModal = () => {
  isConfirmationModalOpen.value = false;
};

const openUnathorizedModal = () => {
  isUnauthorizedModalOpen.value = true;
};

const closeUnathorizedModal = () => {
  router.push(`/tournaments/${tournamentId}`);
  isConfirmationModalOpen.value = false;
};

const cancel = () => {
  router.push(`/tournaments/${tournamentId}`);
};
</script>

<template>
  <div class="container">
    <h1 class="title is-1 has-text-centered">Edit Tournament</h1>
    <form @submit.prevent="updateTournament">
      <div class="field">
        <label class="label">Title</label>
        <div class="control">
          <input class="input" type="text" v-model="form.title" required />
        </div>
      </div>
      <div class="field">
        <label class="label">Rules</label>
        <div class="control">
          <textarea class="input" type="text" v-model="form.rules" required></textarea>
        </div>
      </div>
      <div class="field">
        <label class="label">Description</label>
        <div class="control">
          <textarea class="input" type="text" v-model="form.description" required></textarea>
        </div>
      </div>
      <div class="field">
        <label class="label">Type</label>
        <div class="control">
          <div class="select is-fullwidth">
            <select v-model="form.type" required>
              <option value="">Select Type</option>
              <option value="Singles">Singles</option>
              <option value="Doubles">Doubles</option>
            </select>
          </div>
        </div>
      </div>
      <div class="field">
        <label class="label">Surface</label>
        <div class="control">
          <div class="select is-fullwidth">
            <select v-model="form.surface" required>
                  <option v-for="surface in Object.values(Surface).filter(el => typeof(el) === 'string')" :value="surface" :key="surface">
                    {{ surface }}
                  </option>
            </select>
          </div>
        </div>
      </div>
      <div class="field">
        <button class="toggle-button" :class="{ 'active': showCashPrize }" @click="showCashPrize = !showCashPrize">
          {{ showCashPrize ? 'Tournament with Cash Prize' : 'No Cash Prize ' }} <font-awesome-icon icon="fa-solid fa-hand-point-left" />
        </button>
        <span>{{ " " }}</span>
        <button class="toggle-button" :class="{ 'active': showPoints }" @click="showPoints = !showPoints">
          {{ showPoints ? 'Tournament with points' : 'Tournaments without points (click to change)' }} <font-awesome-icon icon="fa-solid fa-hand-point-left" />
        </button>
      </div>

      <div v-if="showCashPrize">
        <div class="field">
          <label class="label">Entry Fee (optional)</label>
          <div class="control">
            <input class="input" type="number" v-model="form.entryFee"
              :placeholder="form.entryFee === null ? '---' : ''" />
          </div>
        </div>

        <div class="field">
          <label class="label">Prize (optional)</label>
          <div class="control">
            <input class="input" type="number" v-model="form.prize" :placeholder="form.prize === null ? '---' : ''" />
          </div>
        </div>
      </div>
      <div v-if="showPoints">
      <div class="field">
        <label class="label">Match Won Points (optional)</label>
        <div class="control">
          <input class="input" type="number" v-model="form.matchWonPoints"
            :placeholder="form.matchWonPoints === null ? '---' : ''" />
        </div>
      </div>
      <div class="field">
        <label class="label">Set Won Points (optional)</label>
        <div class="control">
          <input class="input" type="number" v-model="form.setWonPoints"
            :placeholder="form.setWonPoints === null ? '---' : ''" />
        </div>
      </div>
      <div class="field">
        <label class="label">Game Won Points (optional)</label>
        <div class="control">
          <input class="input" type="number" v-model="form.gameWonPoints"
            :placeholder="form.gameWonPoints === null ? '---' : ''" />
        </div>
      </div>
    </div>
      <div class="field">
        <label class="label">Courts Available</label>
        <div class="control">
          <input class="input" type="number" v-model="form.courtsAvailable" />
        </div>
      </div>
      <div class="field">
        <label class="label">Min Participants</label>
        <div class="control">
          <input class="input" type="number" v-model="form.minParticipants" />
        </div>
      </div>
      <div class="field">
        <label class="label">Max Participants</label>
        <div class="control">
          <input class="input" type="number" v-model="form.maxParticipants" />
        </div>
      </div>
      <div class="field">
        <label class="label">Indoor</label>
        <div class="control">
          <label class="radio">
            <input type="radio" v-model="form.isIndoor" value="true" />
            <span>Yes</span>
          </label>
          <label class="radio">
            <input type="radio" v-model="form.isIndoor" value="false" />
            <span>No</span>
          </label>
        </div>
      </div>

      <div class="field">
        <label class="label">League</label>
        <div class="control">
          <label class="radio">
            <input type="radio" v-model="form.isLeague" value="true" />
            <span>Yes</span>
          </label>
          <label class="radio">
            <input type="radio" v-model="form.isLeague" value="false" />
            <span>No</span>
          </label>
        </div>
      </div>
      <div class="field">
        <label class="label">Start Date</label>
        <div class="control">
          <input class="input" type="datetime-local" v-model="form.startDate" required />
        </div>
      </div>
      <div class="field">
        <label class="label">End Date</label>
        <div class="control">
          <input class="input" type="datetime-local" v-model="form.endDate" required />
        </div>
      </div>
      <!-- Add other form fields based on the TournamentInputModel properties -->
      <div class="field">
        <label class="label">Avenue</label>
        <div class="control">
          <div class="select is-fullwidth">
            <select v-model="form.avenueId" required>
              <option value="">Select Avenue</option>
              <option v-for="avenue in avenues.data.value.data" :key="avenue.id" :value="avenue.id">{{ avenue.name }}, {{ avenue.city
              }}, {{ avenue.country }}</option>
            </select>
            <div v-if="selectedAvenue" class="field">
              <span v-if="selectedAvenue">Location: {{ selectedAvenue.location }}</span>
              <button v-if="selectedAvenue" class="button is-link" @click="openGoogleMaps">View on Google Maps</button>
              <br>
            </div>
          </div>
        </div>
      </div>
      <br>
      <div class="field">

      </div>
      <div class="field">
        <div class="control buttons is-centered">
          <button class="button is-primary " type="submit">Edit</button>
          <button class="button " @click="openConfirmationModal">Cancel</button>
        </div>
      </div>

      <ModalsConfirmationModal
      :isOpen="isConfirmationModalOpen"
      title="Confirmation"
      message="Are you sure you want to cancel tournament creation?"
      @confirm="cancel"
      @close="closeConfirmationModal"
    />

<ModalsDangerModal
:isOpen="isUnauthorizedModalOpen"
title="Unauthorized!"
message="You are not authorized to edit this tournament!"
@close="closeUnathorizedModal"
/>
    </form>
  </div>
</template>

<style scoped>
.toggle-button {
  padding: 8px 12px;
  border: none;
  border-radius: 4px;
  background-color: #ccc;
  color: #fff;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.toggle-button.active {
  background-color: #00c853;
}
</style>