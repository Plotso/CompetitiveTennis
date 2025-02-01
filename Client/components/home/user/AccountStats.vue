<script setup lang="ts">
import { AccountStats, Result } from '~/types'
import { useTournamentsApi } from '~/composables/useTournamentsApi'

const props = defineProps<{ username: string }>()

const showLoadingModal = ref(true);
const errorNotification = ref("")
const showErrorNotification = ref(false)

const hideErrorNotification = () => {
    showErrorNotification.value = false;
}

const method = 'GET';
const options = {
    method
}
const { data, pending, refresh, error } = await useTournamentsApi<Result<AccountStats>>(`/Accounts/Stats/${props.username}`, options);
if (error.value) {
    console.log('data', data.value)
    console.log('pending', pending.value)
    console.log('error', error.value)
    refresh()
}
if (data?.value?.data) {
    showLoadingModal.value = false
}
if (error.value) {
    errorNotification.value = "Error loading account stats"
    showErrorNotification.value = true
}
</script>

<template>
  <div v-if="pending">
      <BaseLoading></BaseLoading>
  </div>
  <div class="account-stats" v-else>
    <div class="notification is-danger" v-if="showErrorNotification">
            <button class="delete" @click="hideErrorNotification"></button>
            {{errorNotification}}
        </div>
    <div class="stats-item">
      <strong>Singles Rating:</strong> <span>{{ data?.data?.playerRating }}</span>
    </div>
    <div class="stats-item">
      <strong>Doubles Rating:</strong> <span>{{ data?.data?.doublesRating }}</span>
    </div>
    <div class="stats-item">
      <strong>Matches Played:</strong> <span>{{ data?.data?.matchesPlayed }}</span>
    </div>
    <div class="stats-item">
      <strong>Tournaments Played:</strong> <span>{{ data?.data?.tournamentsPlayed }}</span>
    </div>
    <div class="stats-item">
      <strong>Tournaments Won:</strong> <span>{{ data?.data?.tournamentsWon }}</span>
    </div>

  </div>
</template>


<style scoped>
.loading {
  padding: 1rem;
  text-align: center;
}
.error {
  color: red;
  padding: 1rem;
}
.stat-item {
  margin-bottom: 0.5rem;
}
strong {
  color: #00d1b2 !important;
}
</style> 