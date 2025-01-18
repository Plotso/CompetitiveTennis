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
    dateRangeFrom: new Date().toISOString(),
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

    <h1>Upcoming Tournaments</h1>
    <div v-if="pending">
        <BaseLoading></BaseLoading>
    </div>
    <div v-else>
        <div class="notification is-danger" v-if="showErrorNotification">
                <button class="delete" @click="hideErrorNotification"></button>
                {{errorNotification}}
            </div>
        <div v-else>
            <ul>
                <li v-for="tournament in tournaments" :key="tournament.id">
                {{ tournament.title }} - {{ tournament.startDate }}
                </li>
            </ul>
        </div>
    </div>

    <!--MODALS-->
    <!-- <ModalsLoadingModal
      :isOpen="showLoadingModal"
    /> -->
  </div>
</template>

<style scoped>
/* Add your styles here */
</style>