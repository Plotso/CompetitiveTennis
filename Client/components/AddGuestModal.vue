<script setup lang="ts">
import { ParticipantInputModel} from '@/types'; // Update the path as per your project setup
import {useAuthStore} from "~/stores/auth"
const authStore = useAuthStore();
const config = useRuntimeConfig();

  const emit = defineEmits(['close'])

  const guestName = ref('')
  
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
  const addGuestParticipant = async () => {
    const participantInput:ParticipantInputModel = {
        name: guestName.value,
        points: null,
        isGuest: true,
        tournamentId: props.tournamentId ?? -1,
        teamId: null
    }

    try {
    const response = await fetch(`${config.public.tournamentsBase}/Tournaments/AddGuest`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization' : `Bearer ${authStore.token}`
      },
      body: JSON.stringify(participantInput),
    });

    if (response.ok) {
        await refreshNuxtData();
    } else {
      if(response.status == 401){
        errorNotification.value = `User not authorized to do the selected activity`
      }
      else{
        errorNotification.value = `An error occurred during guest participant registration for tournament. Code: ${response.status}`
      }
      showErrorNotification.value = true;
      console.error(`Failed to add guest participant. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while adding guest participant', error);
  }

    
}
const isNameValid = computed(() => {
      return guestName.value != undefined && guestName.value != null && guestName.value != '' && guestName.value.length >= 3
  });
  </script>

<template>
    <div class="modal" :class="{'is-active': isOpen}">
      <div class="modal-background"></div>
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">Add Guest Participant</p>
          <button class="delete" aria-label="close" @click="close"></button>
        </header>
        <div class="notification is-danger" v-if="showErrorNotification">
            <button class="delete" @click="hideErrorNotification"></button>
            {{errorNotification}}
        </div>
        <section class="modal-card-body">
          <form @submit.prevent="addGuestParticipant">
            
        <div class="field">
          <label class="label">Guest Name</label>
          <div class="control">
            <input class="input" type="text" v-model="guestName" required />
          </div>
          <p class="help" v-show="!isNameValid">The name of the guest player(s). Must be at least 3 characters long.</p>
        </div>
          <div class="field">
            <div class="control buttons is-centered">
              <button class="button is-primary " type="submit" :disabled="!isNameValid">Add Guest</button>
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
  
  <style scoped>
  /* Add your scoped styles here */
  </style> 
  