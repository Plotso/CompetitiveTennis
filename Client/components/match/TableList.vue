<script setup lang="ts">
  import { MatchShortOutputModel, ParticipantShortOutputModel, TournamentType, EventStatus } from '@/types';
import { useParticipantNameBuilder } from '~/composables/useParticipantNameBuilder'
import { useMatchHelper  } from '~/composables/useMatchHelper'
const { buildHomeParticipantName, buildAwayParticipantName } = useParticipantNameBuilder();
const { getStageString, getStageStringFromMatch, getResult, isMatchWinner } = useMatchHelper();
  
  defineProps({
    matches: {
      type: Array as () => MatchShortOutputModel[],
      required: true,
    },
    showTableHeaders: Boolean,
  });
  
  defineEmits(['participate', 'openParticipateDoublesModal', 'openParticipateTeamModal', 'openParticipantRemovalModal']);
  
  const clayImg = ref('https://www.publicdomainpictures.net/pictures/400000/nahled/clay-tennis-court-with-balls.jpg')
  const options: Intl.DateTimeFormatOptions = {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        hour12: false
      };

      
const isGameDetailsModalOpen = ref(false);
const selectedMatch = ref<MatchShortOutputModel|null>(null);

const openGameDetailsModal = (match: MatchShortOutputModel) => {
  selectedMatch.value = match;
  isGameDetailsModalOpen.value = true;
};

const closeGameDetailsModal = () => {
  isGameDetailsModalOpen.value = false;
  selectedMatch.value = null;
};
  </script>

<template>
    <div class="table-container">
      <table class="table is-striped is-fullwidth">
        <thead v-if="showTableHeaders">
          <tr>
            <th></th>
            <th>Start Date</th>
            <th>Match Id</th>
            <th>Stage</th>
            <th>Player 1</th>
            <th>Player 2</th>
            <th>Result</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="match in matches" :key="match.id">
            <td>
              
            <NuxtLink :to="`/matches/${match.id}`" class="custom-link has-text-weight-semibold">
              Details
            </NuxtLink>
            </td>
            <td>
              
              {{ new Date(match.startDate).toLocaleDateString(undefined, options).replace(' at', '') }}
            </td>
            <td>{{ match.id }}</td>
            <td>{{ getStageString(match.stage) }}</td>
            <td :class="{ 'tournament-match-winner': isMatchWinner(match, 'home') }">
                {{ buildHomeParticipantName(match, true, false) }}
            </td>
            <td :class="{ 'tournament-match-winner': isMatchWinner(match, 'away') }">
                {{ buildAwayParticipantName(match, true, false) }}
            </td>
            <td>
              <span v-if="match.status === EventStatus[EventStatus.NotStarted]"><font-awesome-icon icon="fa-solid fa-calendar-days" /> &nbsp</span>
              <span v-if="match.status === EventStatus[EventStatus.InProgress]"><font-awesome-icon icon="fa-solid fa-hourglass-half" /> &nbsp</span>
              <span v-if="match.status === EventStatus[EventStatus.Ended]"><font-awesome-icon icon="fa-solid fa-circle-check" /> &nbsp</span>
              <span v-if="match.results">{{ getResult(match) }} &nbsp</span>
              <button class="button is-small is-rounded" v-if="match.results"><font-awesome-icon icon="fa-regular fa-eye" @click="openGameDetailsModal(match)"/></button>
              <!--
              <span v-for="setResult in match.results?.setResults" :key="setResult.setNumber">{{ setResult.homeSideGamesWon }}:{{ setResult.awaySideGamesWon }}&nbsp</span>
              -->
            </td>
          </tr>
        </tbody>
      </table>
      <!-- Game Details Modal -->
<MatchScoreDetailsSlimModal
  :isOpen="isGameDetailsModalOpen"
  :match="selectedMatch"
  @close="closeGameDetailsModal"
/>
    </div>
  </template>  

<style scoped>
.card {
    margin-bottom: 2rem;
}

.image-overlay-left {
    position: absolute;
    bottom: 1px;
    /* Adjust the position as needed */
    left: 5px;
    /* Adjust the position as needed */
    padding: 5px;
    /* Adjust the padding as needed */
    z-index: 1;
    /* Bring the span element above the image */
}



.image-overlay-right {
    position: absolute;
    bottom: 1px;
    /* Adjust the position as needed */
    right: 5px;
    /* Adjust the position as needed */
    padding: 5px;
    /* Adjust the padding as needed */
    z-index: 1;
    /* Bring the span element above the image */
}

.img-custom {
    
    width: 125%;
}

.is-centered-custom {

  padding-left: 100px;
}

.custom-box {
    background-color: rgb(248, 248, 235);
}
</style>
  