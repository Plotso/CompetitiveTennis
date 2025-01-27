<script setup lang="ts">
  import { MatchShortOutputModel, ParticipantInfo, TournamentType, EventStatus } from '@/types';
import { useParticipantNameBuilder } from '~/composables/useParticipantNameBuilder'
import { useMatchHelper  } from '~/composables/useMatchHelper'
const { buildHomeParticipantName, buildAwayParticipantName } = useParticipantNameBuilder();
const { getStageString, getStageStringFromMatch, getResult, isMatchWinner } = useMatchHelper();
  
const props =  defineProps({
    matches: {
      type: Array as () => MatchShortOutputModel[],
      required: true,
    },
    username: {
      type: String,
      required: false
    },
    showTableHeaders: {
      type: Boolean,
      required: false,
      default: true
    },
    showResults: {
      type: Boolean,
      required: false,
      default: true
    }
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

const isUserParticipant = (participant: ParticipantInfo, username: string) => {
  return participant.players.some(player => player.username === username);
};

const isUserInTeam = (team: 'home' | 'away', match: MatchShortOutputModel) => {
  if (team === 'home') {
    return isUserParticipant(match.homeParticipant, props.username);
  } else if (team === 'away') {
    return isUserParticipant(match.awayParticipant, props.username);
  }
  return false;
};

const opponentName = (match: MatchShortOutputModel) => {
  if (isUserInTeam('home', match)) {
    return buildAwayParticipantName(match, true, false);
  } else if (isUserInTeam('away', match)) {
    return buildHomeParticipantName(match, true, false);
  }
  return '';
}

const isUserMatchWinner = (match: MatchShortOutputModel) => {
  return isMatchWinner(match, isUserInTeam('home', match) ? 'home' : 'away');
}
  </script>

<template>
    <div class="table-container">
      <table class="table is-striped is-fullwidth no-borders">
        <thead v-if="showTableHeaders">
          <tr>
            <th>vs</th>
            <th>Start Date</th>
            <th>Stage</th>
            <th v-if="showResults">Result</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="match in matches" :key="match.id">
            <td v-if="username">
              <NuxtLink :to="`/matches/${match.id}`" class="custom-link has-text-weight-semibold">
                {{ opponentName(match) }}
            </NuxtLink>              
            </td>
            <td v-else>
              <NuxtLink :to="`/matches/${match.id}`" class="custom-link has-text-weight-semibold">
                {{ buildHomeParticipantName(match, true, false) }} vs {{ buildAwayParticipantName(match, true, false) }}
            </NuxtLink>              
            </td>
            <td>              
              {{ new Date(match.startDate).toLocaleDateString(undefined, options).replace(' at', '') }}
            </td>
            <td>{{ getStageStringFromMatch(match) }}</td>
            <td v-if="showResults">
              <span v-if="match.status === EventStatus[EventStatus.NotStarted]"><font-awesome-icon icon="fa-solid fa-calendar-days" /> &nbsp</span>
              <span v-if="match.status === EventStatus[EventStatus.InProgress]"><font-awesome-icon icon="fa-solid fa-hourglass-half" /> &nbsp</span>
              <span v-if="match.status === EventStatus[EventStatus.Ended]">
                <font-awesome-icon class="won-icon" icon="fa-solid fa-check" v-if="isUserMatchWinner(match)" /><font-awesome-icon class="lost-icon" icon="fa-solid fa-xmark" v-else /> 
                 &nbsp
              </span>
              <span v-if="match.results">                
                {{ getResult(match) }} &nbsp
                </span>
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
.won-icon {
  color: green;
}

.lost-icon {
  color: red;
}

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
  