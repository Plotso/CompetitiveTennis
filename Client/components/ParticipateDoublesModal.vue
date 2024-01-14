
<script setup lang="ts">
import { Result, AccountOutputModel, ParticipantInputModel, MultiParticipantInputModel, ParticipantShortOutputModel } from "@/types"
import { useAuthStore } from "~/stores/auth"
import { storeToRefs } from 'pinia';
const authStore = useAuthStore();
const config = useRuntimeConfig();
const { user } = storeToRefs(authStore);

const emit = defineEmits(['close'])

const props = defineProps({
  isOpen: Boolean,
  includeCurrentUser: Boolean,
  tournamentId: Number,
  tournamentParticipants: {
    type: [Array] as PropType<ParticipantShortOutputModel[]>,
      required: true,
      default: []
  }
});

const isAccountAlreadyPartOfParticipant = ((accountId: number) => {
    return accountId > 0 && props.tournamentParticipants.find(p => p.players.find(pp => pp.id == accountId))?.id != null
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
const secondParticipantAccountId = ref(-1);

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

const addDoublesParticipant = async () => {
  const participantInput: ParticipantInputModel = {
    name: hasGuest.value ? guestName.value : null,
    points: null,
    isGuest: hasGuest.value,
    tournamentId: props.tournamentId ?? -1,
    teamId: null
  }

  if (!hasGuest.value && secondParticipantAccountId.value != -1) {
    secondParticipantAccountId.value = -1
  }

  const accs: number[] = hasGuest.value ? [participantAccountId.value] : secondParticipantAccountId.value == -1 ? [participantAccountId.value] : [participantAccountId.value, secondParticipantAccountId.value];
  console.log("hasGuest.value", hasGuest.value);
  console.log("secondParticipantAccountId.value == -1", secondParticipantAccountId.value == -1);
  console.log("accs", accs);
  console.log("participantAccountId.value", participantAccountId.value);
  console.log("secondParticipantAccountId.value", secondParticipantAccountId.value);
  const multiParticipantInputModel: MultiParticipantInputModel = {
    participantInfo: participantInput,
    accounts: accs,
    includeCurrentUser: props.includeCurrentUser
  }

  try {
    const response = await fetch(`${config.public.tournamentsBase}/Tournaments/ParticipateDoubles`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${authStore.token}`
      },
      body: JSON.stringify(multiParticipantInputModel),
    });

    if (response.ok) {
      await refreshNuxtData();
        close();
    } else {
      if (response.status == 401) {
        errorNotification.value = `User not authorized to do the selected activity`
      }
      else {
        errorNotification.value = `An error occurred during doubles participation registration for tournament. Code: ${response.status}`
      }
      showErrorNotification.value = true;
      console.error(`Failed to add doubles participants. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while adding doubles participants', error);
  };
}
</script>

<template>
  <div class="modal" :class="{ 'is-active': isOpen }">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">Add Doubles Participant</p>
        <button class="delete" aria-label="close" @click="close"></button>
      </header>
      <div class="notification is-danger" v-if="showErrorNotification">
        <button class="delete" @click="hideErrorNotification"></button>
        {{ errorNotification }}
      </div>
      <section class="modal-card-body">
        <form @submit.prevent="addDoublesParticipant">
          <div class="field" v-if="!props.includeCurrentUser">
            <label class="label">Select Player</label>
            <div class="control">
              <div class="select is-fullwidth">
                <select v-model="participantAccountId" :required="!hasGuest">
                  <option value="">Select Partner</option>
                  <option v-for="account in data?.data" :key="account.id" :value="account.id" :disabled="account.id == participantAccountId|| isAccountAlreadyPartOfParticipant(account.id)">
                    {{ account.firstName }} {{ account.lastName}} ({{ account.username }} | Rating: {{ account.playerRating }}) <span v-if="isAccountAlreadyPartOfParticipant(account.id)">(Already participating)</span>
                  </option>
                </select>
              </div>
            </div>
          </div>

          <div class="field has-text-centered">
            <button class="button toggle-button " :class="{ 'active': hasGuest }" @click="hasGuest = !hasGuest">
              {{ hasGuest ? 'Remove Guest Participant' : 'Add Guest' }} <font-awesome-icon icon="fa-solid fa-hand-point-left" />
            </button>
          </div>
          <div class="field" v-if="hasGuest">
            <label class="label">Guest Name</label>
            <div class="control">
              <input class="input" type="text" v-model="guestName" required />
            </div>
            <p class="help" v-show="!isNameValid">The name of the guest player(s). Must be at least 3 characters long.</p>
          </div>
          <div class="field" v-else>
            <label class="label">Select Partner</label>
            <div class="control">
              <div class="select is-fullwidth">
                <select v-model="secondParticipantAccountId" :required="!hasGuest">
                  <option value="">Select Partner</option>
                  <option v-for="account in data?.data" :key="account.id" :value="account.id" 
                  :disabled="(user.username == account.username && props.includeCurrentUser) || (!props.includeCurrentUser && account.id == participantAccountId)|| isAccountAlreadyPartOfParticipant(account.id)">
                    {{ account.firstName }} {{ account.lastName}} ({{ account.username }} | Rating: {{ account.playerRating }})
                    <span v-if="isAccountAlreadyPartOfParticipant(account.id)">(Already participating)</span>
                    <span v-if="!props.includeCurrentUser && account.id == participantAccountId">(Already selected)</span>
                  </option>
                </select>
              </div>
            </div>
          </div>
          <div class="field">
            <div class="control buttons is-centered">
              <button class="button is-primary " type="submit" :disabled="(hasGuest && !isNameValid) ||(!hasGuest && participantAccountId == secondParticipantAccountId)">Add Doubles Participant</button>
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