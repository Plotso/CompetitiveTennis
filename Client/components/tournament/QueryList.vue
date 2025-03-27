<script setup lang="ts">
import { date } from 'zod';
import { useTournamentsApi } from '~/composables/useTournamentsApi';
import { TournamentQuery, TournamentOutputModel, TournamentType, Surface, Result, SearchOutputModel, SortOptions } from '~/types';

const tournaments = ref<TournamentOutputModel[]>([]);
const showLoadingModal = ref(true);
const tournamentsCount = ref(0);

const props = defineProps({
    username: {
        type: String,
        required: false
    },
    isOngoing: {
        type: Boolean,
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
    page: {
        type: Number,
        required: false
    },
    itemsPerPage: {
        type: Number,
        required: false
    },
    keyword: {
        type: String,
        required: false
    },
    sortOptions: {
        type: SortOptions,
        required: false
    },    
    tournamentType: { type: Number as PropType<TournamentType | null>, required: false },
    hasPrize: { type: Boolean as PropType<boolean | null>, required: false },
    surface: { type: Number as PropType<Surface | null>, required: false },
    isIndoor: { type: Boolean as PropType<boolean | null>, required: false },
    city: {
        type: String,
        required: false
    },
    showParticipationColumn: {
        type: Boolean,
        required: false
    },
    showMoneyRelatedColumns: {
        type: Boolean,
        required: false
    }
});

const emit = defineEmits(['updateTotalTournaments']);


const errorNotification = ref("")
const showErrorNotification = ref(false)

const hideErrorNotification = () => {
    showErrorNotification.value = false;
}

//Using computed property here ensure that data is reactive and refetches it upon every update of any prop.
const query = computed<TournamentQuery>(() => ({
    keyword: props.keyword,
    sortOptions: props.sortOptions,
    additionalSortOptions: props.accountSortOptions,
    page: props.page || 1,
    itemsPerPage: props.itemsPerPage || 10,
    participantUsernames: props.username ? [props.username] : undefined,
    isOngoingAtDateTime: props.isOngoing ? new Date().toISOString() : undefined,
    dateRangeFrom: props.dateRangeFrom ? props.dateRangeFrom.toISOString() : undefined,
    dateRangeUntil: props.dateRangeUntil ? props.dateRangeUntil.toISOString() : undefined,
    tournamentType: props.tournamentType ?? undefined,
    hasPrize: props.hasPrize ?? undefined,
    surface: props.surface ?? undefined,
    isIndoor: props.isIndoor ?? undefined,
    city: props.city,
}));
const method = 'GET';
const options = {
    query,
    method
}

watch(query, () => {
    showLoadingModal.value = true
});
const apiResponse = await useTournamentsApi<Result<SearchOutputModel<TournamentOutputModel>>>('/Tournaments/Search', options);
watchEffect(() => {
    if (apiResponse.error.value) {
    errorNotification.value = "Error loading tournaments"
    showErrorNotification.value = true
    showLoadingModal.value = false
  }  
  if (apiResponse.data.value?.data.results) {
    tournaments.value = apiResponse.data.value.data.results
    tournamentsCount.value = apiResponse.data.value.data.total
    emit('updateTotalTournaments', tournamentsCount.value);
    showLoadingModal.value = false
    showErrorNotification.value = false
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
        <TournamentTableList v-if="tournaments.length > 0"
            :tournaments="tournaments"
            :show-participation-column="showParticipationColumn"
            :show-money-related-columns="showMoneyRelatedColumns"
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