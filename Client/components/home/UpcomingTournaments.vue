<script setup lang="ts">
import { useTournamentsApi } from '~/composables/useTournamentsApi';
import { TournamentQuery, TournamentOutputModel, Result, SearchOutputModel } from '~/types';

const tournaments = ref<TournamentOutputModel[]>([]);
const showLoadingModal = ref(true);


const errorNotification = ref("")
const showErrorNotification = ref(false)

const hideErrorNotification = () => {
    showErrorNotification.value = false;
}

const showParticipationButtons = ref(false);

const query: TournamentQuery = {
    //dateRangeFrom: new Date().toISOString(),
    page: 1,
    itemsPerPage: 10,
};
const method = 'GET';
const options = {
    query,
    method
}
const { data, pending, refresh, error } = await useTournamentsApi<Result<SearchOutputModel<TournamentOutputModel>>>('/Tournaments/Search', options);
if (error.value) {
    console.log('data', data.value)
    console.log('pending', pending.value)
    console.log('error', error.value)
    refresh()
}
if (data?.value?.data.results) {
    tournaments.value = data.value?.data.results
    showLoadingModal.value = false
}
else {
    errorNotification.value = "Error loading upcoming tournaments"
    showErrorNotification.value = true
}
</script>

<template>
  <div class="container">
    <header class="special">
            <h1><strong>Upcoming Tournaments</strong></h1>
        </header>
    <div v-if="pending">
        <BaseLoading></BaseLoading>
    </div>
    <div v-else>
        <div class="notification is-danger" v-if="showErrorNotification">
                <button class="delete" @click="hideErrorNotification"></button>
                {{errorNotification}}
            </div>
        <TournamentTableList v-else
            :tournaments="tournaments"
        />
    </div>

    <div class="buttons is-centered">
        <NuxtLink to="/tournaments" class="button is-primary">View All</NuxtLink>

    </div>
  </div>
</template>

<style scoped>
header {
    
  display: flex;
  flex-direction: column;
  align-items: center;
}

h1 strong {
  font-size: 1.75rem; /* Adjust title size */
  font-weight: bold;
  color: #00d1b2;
  margin: 0; /* Ensure no extra margin around */
}

.summary {
    font-size: 18px;
    line-height: 24px;
}
</style>