<script setup lang="ts">
import {useAuthStore} from "~/stores/auth"

const authStore = useAuthStore();
const config = useRuntimeConfig();
const emit = defineEmits(['close'])

const props = defineProps({
  isOpen: Boolean,
  title: String,
  message: String,
  tournamentId: Number,
  participantId: Number
});

const close = () => {
  emit('close');
};

const removeParticipantErrorNotification = ref("")
const showRemoveParticipantErrorNotification = ref(false)

const hideRemoveParticipantErrorNotification = () => {
    showRemoveParticipantErrorNotification.value = false;
}

const removeParticipant = async () => {
  try {
    const response = await fetch(`${config.public.tournamentsBase}/Tournaments/RemoveParticipantFromTournament?tournamentId=${props.tournamentId}&participantId=${props.participantId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization' : `Bearer ${authStore.token}`
      }
    });

    if (response.ok) {  
        await refreshNuxtData();
        close();
    } else {
      if(response.status == 401){
        removeParticipantErrorNotification.value = `User not authorized to do the selected activity`
      }
      else{
        removeParticipantErrorNotification.value = `An error occurred during opt out attempt for tournament. Code: ${response.status}`
      }
      showRemoveParticipantErrorNotification.value = true;
      console.error(`Failed to create tournament. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while creating the tournament', error);
  }
}
</script>

<template>
    <div class="modal" :class="{'is-active': isOpen}">
      <div class="modal-background"></div>
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">{{ title }}</p>
          <button class="delete" aria-label="close" @click="close"></button>
        </header>     
         <div class="notification is-danger" v-if="showRemoveParticipantErrorNotification">
            <button class="delete" @click="hideRemoveParticipantErrorNotification"></button>
            {{removeParticipantErrorNotification}}
        </div>
        <section class="modal-card-body">
          <p>{{ message }}</p>
        </section>
        <footer class="modal-card-foot">
          <button class="button is-danger" @click="removeParticipant">Confirm</button>
          <button class="button" @click="close">Cancel</button>
        </footer>
      </div>
    </div>
  </template>
  

  
  <style scoped>
  /* Add your scoped styles here */
  </style>
  