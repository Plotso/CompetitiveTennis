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


const isUnauthorizedModalOpen = ref(false);
const isConfirmationModalOpen = ref(false);

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

const openConfirmationModal = () => {
  isConfirmationModalOpen.value = true;
};
const closeConfirmationModal = () => {
  isConfirmationModalOpen.value = false;
};


const closeUnathorizedModal = () => {
  isUnauthorizedModalOpen.value = false;
};


const deleteScoresForMath = async () => {
  try {
    const response = await fetch(`${config.public.tournamentsBase}/Matches/DeleteMatchPeriods/${mData.value.data.id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        'Authorization' : `Bearer ${authStore.token}`
      }
    });

    if (response.ok) {       
      await refreshNuxtData();
    } else {
      if(response.status == 401){
        isUnauthorizedModalOpen.value = true;
      }
      console.error(`Failed to delete match periods for match ${mData.value.data.id}. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while delete scores for match', error);
  }
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
       <hr>
      <div class="tab-container">
        <button @click="setActiveTab('summary')" :class="[{'is-active': isActive('summary')}, 'button']">Summary</button>
        <button @click="setActiveTab('point-by-point')" :class="[{'is-active': isActive('point-by-point')}, 'button']" v-show="match.matchPeriods && match.matchPeriods.length > 0">Point by Point</button>
      </div>

      <div v-if="isActive('summary')" class="summary-view">
        <MatchScoreSummary :match="match"/>
      </div>

      <div v-else class="point-by-point-view">
        <MatchScorePointByPoint :match="match"/>
      </div>

      <div class="box ">

        <div v-if="isAuthorized">
          <!--
          <button class="button is-primary is-centered"  @click="openMatchPeriodInputModal()">
            Add Match Period Info
          </button>
          -->
          <form  @submit.prevent="openConfirmationModal" v-if="mData.data.matchPeriods"> 
            
            <div class="field">
              <div class="control buttons is-centered">
                <button class="button is-danger" type="submit">Delete Periods & Scores for Match</button>
              </div>
            </div>
          </form>        
        </div>
      </div>
    </div>
    
    <ConfirmationModal
      :isOpen="isConfirmationModalOpen"
      title="Confirmation"
      message="Are you sure you want to delete all scores for the match?"
      @confirm="deleteScoresForMath"
      @close="closeConfirmationModal"
    />
    <DangerModal
:isOpen="isUnauthorizedModalOpen"
title="Unauthorized!"
message="You are not authorized to execute the desired action!"
@close="closeUnathorizedModal"
/>
  </div>
</template>

<style scoped>

.button.is-active {
  background-color: #00d1b2;
  color: white;
  border-color: #00d1b2;
}

.tab-container {
  /*text-align: center; */
  display: flex;
}

.tab-container button {
  margin: 0 5px;
  margin-right: 10px; /* Adjust as needed */
}
</style>