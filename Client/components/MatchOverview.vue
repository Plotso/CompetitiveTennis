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


const showLoadingModal = ref(false)

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
  // Format and return the match score
  // You can use match.scores to calculate the score
  return '0-0'
};
const formatEventStatus = (status: EventStatus) => {
  switch(status) {
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
          <p class="player-name">{{ match.homeParticipant.name }}</p>
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
        </div>

        <!-- Away Player -->
        <div class="player-info is-one-third">
          <!--
          <img :src="awayPlayerImage" alt="Away Player Image" />
            -->
          <p class="player-name">{{ match.awayParticipant.name }}</p>
          <p class="player-ranking">{{ awayPlayerRanking }}</p>
        </div>
      </div>

      <div v-if="isAuthorized" class="buttons is-centered">

        <p>
          TODO: Add add/edit scores button
        </p>
      </div>


  </div>

  <!--MODALS-->
  <LoadingModal :isOpen="showLoadingModal" />
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