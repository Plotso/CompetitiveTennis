<script setup lang="ts">
import { date } from 'zod';
import { useTournamentsApi } from '~/composables/useTournamentsApi';
import { MatchQuery, SortOptions, EventStatus, MatchOutputModel, Result, SearchOutputModel, TournamentType, MatchShortOutputModel } from '~/types';

const matches = ref<MatchShortOutputModel[]>([]);
const showLoadingModal = ref(true);

const props = defineProps({
    username: {
        type: String,
        required: false
    },
    status: {
        type: Number as PropType<EventStatus>,
        required: false
    },
    dateRangeFrom: {
        type: Date,
        required: false
    },
    dateRangeUntil: {
        type: Date,
        required: false
    },
    tournamentType: {
        type: TournamentType,
        required: false
    },
    page: {
        type: Number,
        required: false
    },
    itemsPerPage: {
        type: Number,
        required: false
    },
    sortOptions: {
        type: SortOptions,
        required: false
    }
});


const errorNotification = ref("")
const showErrorNotification = ref(false)

const hideErrorNotification = () => {
    showErrorNotification.value = false;
}

const query: MatchQuery = {    
    status: props.status,
    dateRangeFrom: props.dateRangeFrom ? props.dateRangeFrom.toISOString() : undefined,
    dateRangeUntil: props.dateRangeUntil ? props.dateRangeUntil.toISOString() : undefined,
    participantUsername: props.username ? props.username : undefined,
    tournamentType: props.tournamentType,
    sortOptions: props.sortOptions,
    page: props.page ? props.page : 1,
    itemsPerPage: props.itemsPerPage ? props.itemsPerPage : 10
};
const method = 'GET';
const options = {
    query,
    method
}
const apiResponse = await useTournamentsApi<Result<SearchOutputModel<MatchShortOutputModel>>>('/Matches/Search', options);
watchEffect(() => {
    if (apiResponse.error.value) {
    errorNotification.value = "Error loading matches"
    showErrorNotification.value = true
    showLoadingModal.value = false
  }  
  if (apiResponse.data.value?.data.results) {
    matches.value = apiResponse.data.value.data.results
    showLoadingModal.value = false
  }
})
</script>

<template>
  <div class="container">
    <div v-if="showLoadingModal">
        <BaseLoading></BaseLoading>
    </div>
    <div v-else>
        <div class="notification is-danger" v-show="showErrorNotification">
                <button class="delete" @click="hideErrorNotification"></button>
                {{errorNotification}}
            </div>
        <HomeUserMatchTableList v-if="matches"
            :matches="matches"
            :username="props.username"
        />
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