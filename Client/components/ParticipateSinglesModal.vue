
<script setup lang="ts">
import { Result, AccountOutputModel, ParticipantInputModel, MultiParticipantInputModel } from "@/types"
import { useAuthStore } from "~/stores/auth"
import { storeToRefs } from 'pinia';
const authStore = useAuthStore();
const config = useRuntimeConfig();

const emit = defineEmits(['close'])

const props = defineProps({
  isOpen: Boolean,
  tournamentId: Number
});

const close = () => {
  emit('close');
};
const errorNotification = ref("")
const showErrorNotification = ref(false)

const hideErrorNotification = () => {
  showErrorNotification.value = false;
}

const hasGuest = ref(false)
const guestName = ref('')
const participantAccountId = ref(-1);

const { data, pending, refresh, error } = await useFetch<Result<AccountOutputModel[]>>(() => `/Accounts/All`, {
  baseURL: config.public.tournamentsBase
})
if (error.value) {
  console.log(`Failed to fetch accounts info. ErrorCode: ${error.value.statusCode}`)
  refresh()
}
const isNameValid = computed(() => {
  return guestName.value != undefined && guestName.value != null && guestName.value != '' && guestName.value.length >= 3
});

const addSinglesParticipant = async () => {
    // Send participate request
    // ToDo: Add logic for doubles + teams
    console.log(props.tournamentId);
    var participant = data?.value?.data.find(a => a.id == participantAccountId.value);
    const participantInput:ParticipantInputModel = {
        name: null, //`${participant?.firstName} ${participant?.lastName} (${participant?.username})`,
        points: null,
        isGuest: false,
        tournamentId: props.tournamentId ?? -1,
        teamId: null
    }

    let participantId = -1;

    var bodyJson = `{
        "participantInput":${JSON.stringify(participantInput)},
        "accountId":${participantAccountId.value}
    }`

    var bodyObj = {
      accountId: participantAccountId.value,
      participantInput: participantInput
    };

    try {
    const response = await fetch(`${config.public.tournamentsBase}/Tournaments/AddSinglesParticipant`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization' : `Bearer ${authStore.token}`
      },
      body: bodyJson,
    });

    if (response.ok) {     
        await refreshNuxtData();
        close();
    } else {
      if(response.status == 401){
        errorNotification.value = `User not authorized to do the selected activity`
      }
      else{
        errorNotification.value = `An error occurred during participation for tournament. Code: ${response.status}`
      }
      showErrorNotification.value = true;
      console.error(`Failed to participate for tournament. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while participating for singles draw for the tournament', error);
  }
}
</script>

<template>
  <div class="modal" :class="{ 'is-active': isOpen }">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">Add Participant</p>
        <button class="delete" aria-label="close" @click="close"></button>
      </header>
      <div class="notification is-danger" v-if="showErrorNotification">
        <button class="delete" @click="hideErrorNotification"></button>
        {{ errorNotification }}
      </div>
      <section class="modal-card-body">
        <form @submit.prevent="addSinglesParticipant">
          <div class="field">
            <label class="label">Select Player</label>
            <div class="control">
              <div class="select is-fullwidth">
                <select v-model="participantAccountId" :required="!hasGuest">
                  <option value="">Select Player</option>
                  <option v-for="account in data?.data" :key="account.id" :value="account.id" :disabled="account.id == participantAccountId">
                    {{ account.firstName }} {{ account.lastName}} ({{ account.username }} | Rating: {{ account.playerRating }})
                  </option>
                </select>
              </div>
            </div>
          </div>
          <div class="field">
            <div class="control buttons is-centered">
              <button class="button is-primary " type="submit" :disabled="participantAccountId <= 0">Add Participant</button>
            </div>
          </div>
        </form>
      </section>
      <footer class="modal-card-foot">
        <button class="button" @click="close">Cancel</button>
      </footer>
    </div>
  </div>
</template>

<style scoped></style>