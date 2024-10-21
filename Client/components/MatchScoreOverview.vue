<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { Surface, TournamentType, Result, TournamentOutputModel, SlimTournamentOutputModel, ParticipantInputModel, MatchShortOutputModel, EventStatus } from "@/types"
import { useAuthStore } from "~/stores/auth"
const props = defineProps({
  data: { type: Object as PropType<Result<MatchShortOutputModel>>, required: true },
  organiserUsername: String
})
const authStore = useAuthStore();
const config = useRuntimeConfig();
const { user } = storeToRefs(useAuthStore());

const organiserUsernameInfo = toRef(props, "organiserUsername")
const mData = toRef(props, "data")
const match = ref(mData.value.data)
//const comp = computed(() => props.data) //Would work the same way as toRef from above
const options: Intl.DateTimeFormatOptions = {
  year: 'numeric',
  month: 'long',
  day: 'numeric',
  hour: 'numeric',
  minute: 'numeric',
  hour12: false
};

const activeTab = ref('summary')

const showSummary = ref(true);

const startDate = computed(() => new Date(match.value.startDate).toLocaleDateString(undefined, options).replace(' at', ''));

const isAuthorized = computed(() => {
  return authStore.user && (authStore.user.username == organiserUsernameInfo || authStore.user.hasAdministrativeRights)
});

const isAuthenticated = computed(() => {
  return authStore.user && authStore.user.username
});

const getScore = () => {
  // Format and return the match score
  // You can use match.scores to calculate the score
  return '0-0'
};
const formatEventStatus = (status: EventStatus) => {
  switch (status) {
    case EventStatus.NotStarted: {
      return "Not Started";
    }
    case EventStatus.InProgress: {
      return "Ongoing";
    }
    case EventStatus.Ended: {
      return "Ended";
    }
    default: {
      return "Unknown"
    }
  }
};

const setActiveTab = (newActiveTab: string) => {
  activeTab.value = newActiveTab;
};

const isActive = (tabName: string) => activeTab.value == tabName;


const isMatchPeriodInputModalOpen = ref(false);

const openMatchPeriodInputModal = () => {
  isMatchPeriodInputModalOpen.value = true;
};

const closeMatchPeriodInputModal = () => {
  isMatchPeriodInputModalOpen.value = false;
};

</script>

<template>
  <br>
  <br>
  <br>
  <div class="container">
    <NuxtLink :to="`/tournaments/${match.tournamentId}`" class="custom-link has-text-weight-semibold">Go back to tournament</NuxtLink>
    <div class="score-overview">
      <!-- Tabs for Summary and Point by Point -->
      <div class="tab-container">
        <button @click="setActiveTab('summary')" class="button">Summary</button>
        <button @click="setActiveTab('point-by-point')" class="button" v-show="match.matchPeriods && match.matchPeriods.length > 0">Point by Point</button>
      </div>

      <!-- Summary View -->
      <div v-if="isActive('summary')" class="summary-view">
        <!-- Display summary table -->
        Summary
      </div>

      <!-- Point by Point View -->
      <div v-else class="point-by-point-view">
        <!-- Display point by point view for each set -->
        Point by point
      </div>

      <div class="box">
        Insert scores here
        <div v-if="isAuthorized">
          <button class="button is-primary"  @click="openMatchPeriodInputModal()">
        Add Match Period Info
        </button>
        </div>
      </div>
    </div>
  </div>
  <MatchPeriodInputModal
:isOpen="isMatchPeriodInputModalOpen"
:matchId="mData.data.id"
:homeParticipantName="mData.data.homeParticipant.name"
:awayParticipantName="mData.data.awayParticipant.name"
:existingMatchPeriods="mData.data.matchPeriods"
@close="closeMatchPeriodInputModal()"
/>
</template>

<style scoped>
.container {}

.tab-container {
  /*text-align: center; */
  display: flex;
}

.tab-container button {
  margin: 0 5px;
  margin-right: 10px; /* Adjust as needed */
}
</style>