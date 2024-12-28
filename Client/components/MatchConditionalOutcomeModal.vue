<script setup lang="ts">
import { ref, defineProps } from 'vue';
import { OutcomeCondition, MatchPeriodOutcome } from '@/types';
import { useAuthStore } from "~/stores/auth"

const config = useRuntimeConfig();

const props = defineProps({
  isOpen: Boolean,
  matchId: { type: Number, required: true },
  homeSideName: { type: String, required: true },
  awaySideName: { type: String, required: true },
});

const emit = defineEmits(['close']);

const closeModal = () => {
  emit('close');
};

// Form data
const formInput = ref({
  outcomeCondition: null as OutcomeCondition | null,
  matchOutcome: null as MatchPeriodOutcome | null,
});

// Form validation errors
const formErrors = ref({
  outcomeCondition: '',
  matchOutcome: '',
});

const isConfirmationModalOpen = ref(false);

// Validate form
const validateForm = () => {
  formErrors.value.outcomeCondition = formInput.value.outcomeCondition === null ? 'Outcome Condition is required.' : '';
  formErrors.value.matchOutcome = formInput.value.matchOutcome === null ? 'Match Outcome is required.' : '';

  return !formErrors.value.outcomeCondition && !formErrors.value.matchOutcome;
};

const openConfirmationModal = () => {
  if (validateForm()) {
    isConfirmationModalOpen.value = true;
  }
};

const closeConfirmationModal = () => {
  isConfirmationModalOpen.value = false;
};

const sendRequest = async () => {
  try {
    const response = await fetch(`${config.public.tournamentsBase}/Matches/UpdateMatchOutcomeDueToCustomCondition/${props.matchId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${useAuthStore().token}`,
      },
      body: JSON.stringify(formInput.value),
    });

    if (response.ok) {
      closeConfirmationModal();
      closeModal();
      await refreshNuxtData();
    } else {
      console.error('Failed to send request:', response.status);
      console.error(response);
    }
  } catch (error) {
    console.error('An error occurred:', error);
  }
};

const handleConfirm = () => {
  sendRequest();
};
</script>

<template>
  <div class="modal" :class="{ 'is-active': isOpen }">
    <div class="modal-background" @click="closeModal"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">Custom Match Outcome</p>
        <button class="delete" aria-label="close" @click="closeModal"></button>
      </header>

      <section class="modal-card-body">
        <form @submit.prevent>
          <!-- Outcome Condition -->
          <div class="field">
            <label class="label">Outcome Condition</label>
            <div class="control">
              <div class="select is-fullwidth">
                <select v-model="formInput.outcomeCondition">
                  <option disabled value="">Select an Outcome Condition</option>
                  <option :value="OutcomeCondition.Points">Points</option>
                  <option :value="OutcomeCondition.Injury">Injury</option>
                  <option :value="OutcomeCondition.Disqualification">Disqualification</option>
                  <option :value="OutcomeCondition.Withdrawal">Withdrawal</option>
                </select>
              </div>
            </div>
            <p class="help is-danger">{{ formErrors.outcomeCondition }}</p>
          </div>

          <!-- Match Outcome -->
          <div class="field">
            <label class="label">Match Outcome</label>
            <div class="control">
              <div class="select is-fullwidth">
                <select v-model="formInput.matchOutcome">
                  <option disabled value="">Select Match Outcome</option>
                  <option :value="MatchPeriodOutcome.ParticipantOne">{{ homeSideName }}</option>
                  <option :value="MatchPeriodOutcome.ParticipantTwo">{{ awaySideName }}</option>
                </select>
              </div>
            </div>
            <p class="help is-danger">{{ formErrors.matchOutcome }}</p>
          </div>
        </form>
      </section>

      <footer class="modal-card-foot">
        <button class="button is-success" @click="openConfirmationModal">Save Match Outcome</button>
        <button class="button" @click="closeModal">Cancel</button>
      </footer>
    </div>

    <!-- Confirmation Modal -->
    <MatchWinnerModal
      v-if="isConfirmationModalOpen"
      :isOpen="isConfirmationModalOpen"
      :title="'Confirm Match Outcome'"
      :message="'Are you sure you want to submit this match outcome? Submitting match outcome will finalize the match. Only the points scored until now will be taken into consideration when calculating player rating adjustments. '"
      :winner="formInput.matchOutcome === MatchPeriodOutcome.ParticipantOne ? homeSideName : awaySideName"
      :is-scores-input="false"
      @confirm-winner="handleConfirm"
      @close="closeConfirmationModal"
    />
  </div>
</template>

<style scoped>
.modal-card {
  max-width: 500px;
}

.field {
  margin-bottom: 1.5rem;
}

.help.is-danger {
  color: #f14668;
}
</style>
