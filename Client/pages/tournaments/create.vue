<script setup lang="ts">
//import { TournamentInputModel } from 'types'
import { AvenueOutputModel, TournamentInputModel, Surface, TournamentType } from '@/types';
import {useAuthStore} from "~/stores/auth"
definePageMeta({
  'auth': true
})
const config = useRuntimeConfig();
const router = useRouter();
const authStore = useAuthStore();

const { data, pending, refresh, error } = await useFetch<Result<AvenueOutputModel[]>>(() => `/Avenues/All`, {
  baseURL: config.public.tournamentsBase
})
if (error.value) {
  console.log('data', data.value)
  console.log('pending', pending.value)
  console.log('error', error.value)
  refresh()
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

const createTournament = async () => {
  try {
    const response = await fetch(`${config.public.tournamentsBase}/Tournaments/Add`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization' : `Bearer ${authStore.token}`
      },
      body: JSON.stringify(form.value),
    });

    if (response.ok) {
      const data = await response.json();
      const tournamentId = data.data;

      router.push(`/tournaments/${tournamentId}`);
    } else {
      // Handle the error case
      console.error(`Failed to create tournament. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while creating the tournament', error);
  }
};

// Check the value of data
//console.log('data', data.value)
//console.log('data.data', data.value.data)

/*
const avenuesLoaded = ref(false)

watch(data, () => {
  if (data.value) {
    avenuesLoaded.value = true
  }
})

const avenueOptions = computed(() => {
  return data.value ? data.value.data.map((avenue) => ({ id: avenue.id, name: avenue.name })) : []
})
*/

const avenueLocation = ref("")
watch(data, () => {
  if (data.value.data && data.value.data.length > form.value.avenueId) {
    avenueLocation.value = data.value.data.find(avenue => avenue.id === form.value.avenueId)
  }
})

const selectedAvenue = computed(() => {
  const selectedId = form.value.avenueId
  return data.value?.data.find((avenue) => avenue.id === selectedId)
})

const openGoogleMaps = () => {
  if (selectedAvenue.value) {
    const queryInfo = `${selectedAvenue.value.location} ${selectedAvenue.value.name} ${selectedAvenue.value.city}`
    const location = encodeURIComponent(queryInfo); //(selectedAvenue.value.location);
    const googleMapsUrl = `https://www.google.com/maps/search/?api=1&query=${location}`;
    window.open(googleMapsUrl, '_blank');
  }
};

const updateSelectedAvenue = () => {
  //const selectedId = form.value.avenueId;
  //selectedAvenue.value = data.value.data.find((avenue) => avenue.id === selectedId);
};

const confirmCancel = () => {
  if (window.confirm("Are you sure you want to cancel?")) {
    router.push("/tournaments");
  }
};

const isConfirmationModalOpen = ref(false);

const openConfirmationModal = () => {
  isConfirmationModalOpen.value = true;
};

const closeConfirmationModal = () => {
  isConfirmationModalOpen.value = false;
};

const cancel = () => {
  router.push("/tournaments");
};
</script>

<template>
  <div class="container">
    <h1 class="title is-1 has-text-centered">Create Tournament</h1>
    <form @submit.prevent="createTournament">
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
        <p class="help">General rules of the tournament including tennis game rules, tournament restrictions and etc.</p>
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
            <select v-model="form.avenueId" required @change="updateSelectedAvenue">
              <option value="">Select Avenue</option>
              <option v-for="avenue in data.data" :key="avenue.id" :value="avenue.id">{{ avenue.name }}, {{ avenue.city
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
          <button class="button is-primary " type="submit">Create</button>
          <button class="button " @click="openConfirmationModal">Cancel</button>
        </div>
      </div>

      <ConfirmationModal
      :isOpen="isConfirmationModalOpen"
      title="Confirmation"
      message="Are you sure you want to cancel tournament creation?"
      @confirm="cancel"
      @close="closeConfirmationModal"
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