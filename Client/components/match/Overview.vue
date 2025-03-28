<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { Surface, TournamentType, Result, TournamentOutputModel, SlimTournamentOutputModel, ParticipantInputModel, MatchShortOutputModel, EventStatus, MatchPeriodOutcome, OutcomeCondition } from "@/types"
import { useAuthStore } from "~/stores/auth"
import MatchConditionalOutcomeModal from './ConditionalOutcomeModal.vue';
import { useParticipantNameBuilder } from '~/composables/useParticipantNameBuilder'
const { buildHomeParticipantName, buildAwayParticipantName } = useParticipantNameBuilder()
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


const showLoadingModal = ref(false)


const isMatchPeriodInputModalOpen = ref(false);

const openMatchPeriodInputModal = () => {
  isMatchPeriodInputModalOpen.value = true;
};

const closeMatchPeriodInputModal = () => {
  isMatchPeriodInputModalOpen.value = false;
};


const isMatchConditionalOutcomeModalOpen = ref(false);

const openMatchConditionalOutcomeModal = () => {
  isMatchConditionalOutcomeModalOpen.value = true;
};

const closeMatchConditionalOutcomeModal = () => {
  isMatchConditionalOutcomeModalOpen.value = false;
};

const startDate = computed(() => new Date(match.value.startDate).toLocaleDateString(undefined, options).replace(' at', ''));

const isAuthorized = computed(() => {
  return authStore.user && (authStore.user.username == organiserUsernameInfo || authStore.user.hasAdministrativeRights)
});

const isAuthenticated = computed(() => {
  return authStore.user && authStore.user.username
});

const homePlayerRanking = computed(() => {
  if (
    match.value.homeParticipant &&
    match.value.homeParticipant.players &&
    match.value.homeParticipant.players.length === 1
  ) {
    return match.value.homeParticipant.players[0].playerRating || 'N/A';
  } else {
    return 'N/A';
  }
});
const awayPlayerRanking = computed(() => {
  if (
    match.value.awayParticipant &&
    match.value.awayParticipant.players &&
    match.value.awayParticipant.players.length === 1
  ) {
    return match.value.awayParticipant.players[0].playerRating || 'N/A';
  } else {
    return 'N/A';
  }
});

const getScore = () => {
  var matchValue = match.value;
  if(!matchValue || !matchValue.results || !matchValue.results.setResults || matchValue.results.setResults.length == 0)
    return "";
  var result = matchValue.results.setResults.reduce(
    (result, set) => {
      if (set.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantOne]) {
        result.participant1Wins++;
      } else if (set.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantTwo]) {
        result.participant2Wins++;
      }
      return result;
    },
    { participant1Wins: 0, participant2Wins: 0 } // Initial accumulator
  );
  return `${result.participant1Wins}:${result.participant2Wins}`
};
const formatEventStatus = (status: EventStatus) => {
  console.log(status)
  switch(status) {
    case EventStatus[EventStatus.NotStarted]: {
      return "Not Started";
    }
    case EventStatus[EventStatus.InProgress]: {
      return "Ongoing";
    }
    case EventStatus[EventStatus.Ended]: {
      return "Ended";
    }
    default: {
      return "Unknown"
    }
  }
};

const formatOutcomeCondition = (outcomeCondition: OutcomeCondition) => {
  switch(outcomeCondition) {
    case OutcomeCondition[OutcomeCondition.Points]: {
      return "Points 🎾";
    }
    case OutcomeCondition[OutcomeCondition.Injury]: {
      return "Injury 🤕";
    }
    case OutcomeCondition[OutcomeCondition.Disqualification]: {
      return "Disqualification 🧑‍⚖️";
    }
    case OutcomeCondition[OutcomeCondition.Withdrawal]: {
      return "Player Withdrawal 🏳️";
    }
    default: {
      return "Unknown"
    }
  }
};

</script>

<template>
  <br>
  <br>
  <br>
  <div class="container">
      <div class="match-overview columns is-flex is-justify-content-space-evenly has-text-centered">
        <!-- Home Player -->
        <div class="player-info is-one-third">
          <!--
            <img :src="homePlayerImage" alt="Home Player Image" />
            -->
          <p class="player-name">{{ buildHomeParticipantName(match, true, false) }}</p>
          <p class="player-ranking">{{ homePlayerRanking }}</p>
        </div>

        <!-- Match Info -->
        <div class="match-info is-one-third">
          <p class="start-date is-size-6">{{ startDate }}
        <NuxtLink :to="`/matches/edit/${match.id}`" v-if="isAuthorized" class="edit-button"><font-awesome-icon
            icon="fa-solid fa-pen-to-square" /></NuxtLink>
        <span>{{ " " }}</span>
        <NuxtLink :to="`/matches/delete/${match.id}`" v-if="isAuthorized" class="delete-button"><font-awesome-icon
            icon="fa-solid fa-trash" /></NuxtLink></p>
          <p class="score is-size-1" :class="{ 'match-in-progress': match.status === EventStatus.InProgress }">
            {{ getScore() }}
          </p>
          <p class="event-status is-size-4 is-uppercase is-size-4-mobile">{{ formatEventStatus(match.status) }}</p>
          
          <p class="outcome-condition is-size-7" v-if="match.outcomeCondition">- {{ formatOutcomeCondition(match.outcomeCondition) }} -</p>
        </div>

        <!-- Away Player -->
        <div class="player-info is-one-third">
          <!--
          <img :src="awayPlayerImage" alt="Away Player Image" />
            -->
          <p class="player-name">{{ buildAwayParticipantName(match, true, false) }}</p>
          <p class="player-ranking">{{ awayPlayerRanking }}</p>
        </div>
      </div>

      <div v-if="isAuthorized" class="buttons is-centered">

        <p>          
          <button class="button is-primary is-centered"  @click="openMatchPeriodInputModal()">
            Add Match Period Info
          </button>

          <button class="button is-primary is-centered" @click="openMatchConditionalOutcomeModal()">
          Add Conditional Outcome
          </button>
        </p>
      </div>


  </div>

  <!--MODALS-->
  <ModalsLoadingModal :isOpen="showLoadingModal" />
  <MatchPeriodInputModal
:isOpen="isMatchPeriodInputModalOpen"
:matchId="mData.data.id"
:homeParticipantName="buildHomeParticipantName(match, true, false)"
:awayParticipantName="buildAwayParticipantName(match, true, false)"
:existingMatchPeriods="mData.data.matchPeriods"
@close="closeMatchPeriodInputModal()"
/>
  <MatchConditionalOutcomeModal
:isOpen="isMatchConditionalOutcomeModalOpen"
:matchId="mData.data.id"
:home-side-name="buildHomeParticipantName(match, true, false)"
:away-side-name="buildAwayParticipantName(match, true, false)"
@close="closeMatchConditionalOutcomeModal()"
/>
</template>

<style scoped>
.container {
}

.box {
  margin-bottom: 20px;
}

.remove-participant-button {
  font-size: x-small;
}

.match-info {}

.player-info {
}

.player-name {
  font-weight: bold;
}

.score {
}
</style>