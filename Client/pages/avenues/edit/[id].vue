<script setup lang="ts">
import { ref } from 'vue';
import { AvenueInputModel, CourtsInfo, Surface, CourtType, AvenueOutputModel } from '@/types';
import { useRouter } from 'vue-router';
import { storeToRefs } from 'pinia';
import {useAuthStore} from "~/stores/auth"
definePageMeta({
  'auth': true
})
const form = ref<AvenueInputModel>({
  name: '',
  location: '',
  city: '',
  country: '',
  details: '',
  courts: []
});

const config = useRuntimeConfig();
const router = useRouter();
const route = useRoute();
const authStore = useAuthStore();

const isCourtOptionsVisible = ref(false);;

// Fetch avenue data based on the given ID
const avenueId = route.params.id;
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

console.log('data', data.value)
if(data.value){
  form.value.name = data.value.data.name;
  form.value.location = data.value.data.location;
  form.value.city = data.value.data.city;
  form.value.country = data.value.data.country;
  form.value.details = data.value.data.details;

  if(data.value.data.courts){
    form.value.courts = data.value.data.courts;    
    isCourtOptionsVisible.value = true;
  }
  console.log('form', form.value);
}

const { user } = storeToRefs(useAuthStore());
if(!user.value || !user.value.hasAdministrativeRights){
  router.push(`/avenues/${avenueId}`)
}

const isUnauthorizedModalOpen = ref(false);
const updateAvenue = async () => {
  try {
    const response = await fetch(`${config.public.tournamentsBase}/Avenues/Edit/${avenueId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization' : `Bearer ${authStore.token}`
      },
      body: JSON.stringify(form.value),
    });

    if (response.ok) {
      router.push(`/avenues/${avenueId}`);
    } else {
      if(response.status == 401){
        isUnauthorizedModalOpen.value = true;
      }
      console.error(`Failed to updating avenue. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while updating the avenue', error);
  }
};

const addCourtOption = () => {
  form.value.courts.push({
    surface: Surface.Other,
    availableCourtsByType: {
      [CourtType.Outdoor]: 0,
      [CourtType.Indoor]: 0,
    },
  });
};

const removeCourtOption = (index: number) => {
  form.value.courts.splice(index, 1);
};

const isConfirmationModalOpen = ref(false);

const openConfirmationModal = () => {
  isConfirmationModalOpen.value = true;
};

const closeConfirmationModal = () => {
  isConfirmationModalOpen.value = false;
};

const closeUnathorizedModal = () => {
  router.push(`/avenues/${avenueId}`);
  isConfirmationModalOpen.value = false;
};

const cancel = () => {
  router.push(`/avenues/${avenueId}`);
};
</script>

<template>
  <div class="container">
    <h1 class="title is-1 has-text-centered">Edit Avenue</h1>
    <form @submit.prevent="updateAvenue">
      <div class="field">
        <label class="label">Name</label>
        <div class="control">
          <input class="input" type="text" v-model="form.name" required />
        </div>
      </div>
      <div class="field">
        <label class="label">Location</label>
        <div class="control">
          <input class="input" type="text" v-model="form.location" required />
        </div>
      </div>
      <div class="field">
        <label class="label">City</label>
        <div class="control">
          <input class="input" type="text" v-model="form.city" required />
        </div>
      </div>
      <div class="field">
        <label class="label">Country</label>
        <div class="control">
          <input class="input" type="text" v-model="form.country" required />
        </div>
      </div>
      <div class="field">
        <label class="label">Details</label>
        <div class="control">
          <textarea class="textarea" v-model="form.details" required></textarea>
        </div>
      </div>
      <div class="field">
        <label class="label">Add Courts</label>
        <div class="control">
          <button class="button is-primary" type="button" @click="isCourtOptionsVisible = !isCourtOptionsVisible">
            {{ isCourtOptionsVisible ? 'Hide' : 'Show' }} Court Info per Surface
          </button>
        </div>
      </div>

      <div v-if="isCourtOptionsVisible">
        <hr />
        <div class="field" v-for="(court, index) in form.courts" :key="index">
          <label class="label">Court Info Per Surface {{ index + 1 }}</label>
          <div class="field">
            <label class="label">Surface</label>
            <div class="control">
              <div class="select">
                <select v-model="court.surface">
                  <option v-for="surface in Object.values(Surface).filter(el => typeof(el) === 'string')" :value="surface" :key="surface">
                    {{ surface }}
                  </option>
                </select>
              </div>
            </div>
          </div>
          <div class="field">
            <label class="label">Available Courts (Outdoor)</label>
            <div class="control">
              <input class="input" type="number" v-model="form.courts[index].availableCourtsByType[CourtType[CourtType.Outdoor]]" required />

            </div>
          </div>
          <div class="field">
            <label class="label">Available Courts (Indoor)</label>
            <div class="control">
              <input class="input" type="number" v-model="court.availableCourtsByType[CourtType[CourtType.Indoor]]" required />
            </div>
          </div>
          <div class="field">
            <button class="button is-danger" type="button" @click="removeCourtOption(index)">
              Remove Court Option
            </button>
          </div>
        </div>
        <div class="field">
          <div class="control">
            <button class="button is-primary" type="button" @click="addCourtOption">Add Court Option</button>
          </div>
        </div>
      </div>
      <hr />
      <div class="field">
        <div class="control buttons is-centered">
          <button class="button is-primary" type="submit">Edit</button>
          <button class="button " @click="openConfirmationModal">Cancel</button>
        </div>
      </div>

      <ModalsConfirmationModal
      :isOpen="isConfirmationModalOpen"
      title="Confirmation"
      message="Are you sure you want to cancel avenue creation?"
      @confirm="cancel"
      @close="closeConfirmationModal"
    />

<ModalsDangerModal
:isOpen="isUnauthorizedModalOpen"
title="Unauthorized!"
message="You are not authorized to edit this avenue!"
@close="closeUnathorizedModal"
/>
    </form>
  </div>
</template>
