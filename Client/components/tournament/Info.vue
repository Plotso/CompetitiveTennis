<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { Surface, TournamentType, Result, TournamentStage, TournamentOutputModel, SlimTournamentOutputModel, ParticipantInputModel, MatchOutcome, EventStatus, MatchShortOutputModel, MatchPeriodOutcome } from "@/types"
import {useAuthStore} from "~/stores/auth"
import { useParticipantNameBuilder } from '~/composables/useParticipantNameBuilder'
import { useMatchHelper  } from '~/composables/useMatchHelper'
const { buildHomeParticipantName, buildAwayParticipantName } = useParticipantNameBuilder();
const { getStageString, getStageStringFromMatch, getResult, isMatchWinner } = useMatchHelper();
const props = defineProps({
    data: {type: Object as PropType<Result<SlimTournamentOutputModel>>, required: true}
})
const authStore = useAuthStore();
const config = useRuntimeConfig();
const { user } = storeToRefs(useAuthStore());

const tData = toRef(props, "data")
const tournament = ref(tData.value.data)
console.log(tournament)
console.log(tournament.id)
console.log(tournament.matches)
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
const removeParticipantId = ref(-1);

const isParticipantRemovalModalOpen = ref(false);

const openParticipantRemovalModal = (participantId: number) => {
  removeParticipantId.value = participantId;
  isParticipantRemovalModalOpen.value = true;
};

const closeParticipantRemovalModal = () => {
  isParticipantRemovalModalOpen.value = false;
};
const isAddGuestModalOpen = ref(false);

const openAddGuestModal = () => {
  isAddGuestModalOpen.value = true;
};

const closeAddGuestModal = () => {
  isAddGuestModalOpen.value = false;
};
const isAddSinglesParticipantModalOpen = ref(false);

const openParticipateSinglesModal = () => {
  isAddSinglesParticipantModalOpen.value = true;
};

const closeAddSinglesParticipantModal = () => {
  isAddSinglesParticipantModalOpen.value = false;
};
const isOrganiserParticipateDoublesModalOpen = ref(false);

const openOrganiserParticipateDoublesModal = () => {
    console.log('part doubles');
    isOrganiserParticipateDoublesModalOpen.value = true;
};

const closeOrganiserParticipateDoublesModal = () => {
  isOrganiserParticipateDoublesModalOpen.value = false;
};
const isParticipateDoublesModalOpen = ref(false);

const openParticipateDoublesModal = () => {
    console.log('part doubles');
  isParticipateDoublesModalOpen.value = true;
};

const openParticipateTeamModal = () => {
    console.log('part teams');
    //ToDo: currently not supported
}

const closeParticipateDoublesModal = () => {
    isParticipateDoublesModalOpen.value = false;
};

const startDate = computed(() => new Date(tournament.value.startDate).toLocaleDateString(undefined, options).replace(' at', ''));
const endDate = computed(() => new Date(tournament.value.endDate).toLocaleDateString(undefined, options).replace(' at', ''));
const hasTournamentStarted = computed(() => tournament.value.matches.length > 0);

const isAuthorized = computed(() => {
    return authStore.user && (authStore.user.username == tournament.value.organiser.username || authStore.user.hasAdministrativeRights)
});

const isAuthenticated = computed(() => {
    return authStore.user && authStore.user.username
});

const removeParticipantErrorNotification = ref("")
const showRemoveParticipantErrorNotification = ref(false)

const hideRemoveParticipantErrorNotification = () => {
    showRemoveParticipantErrorNotification.value = false;
}


const participate = async (tournamentId: number) => {
    // Send participate request
    // ToDo: Add logic for doubles + teams
    console.log(tournamentId);
    const participantInput:ParticipantInputModel = {
        name: `${user.value.firstName} ${user.value.lastName} (${user.value.username})`,
        points: null,
        isGuest: false,
        tournamentId: tournamentId,
        teamId: null
    }

    try {
    showLoadingModal.value = true;
    const response = await fetch(`${config.public.tournamentsBase}/Tournaments/ParticipateSingle`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization' : `Bearer ${authStore.token}`
      },
      body: JSON.stringify(participantInput),
    });

    if (response.ok) {
        showLoadingModal.value = false;      
        await refreshNuxtData();
    } else {
        showLoadingModal.value = false;
      if(response.status == 401){
        errorNotification.value = `User not authorized to do the selected activity`
      }
      else{
        errorNotification.value = `An error occurred during participation for tournament. Code: ${response.status}`
      }
      showErrorNotification.value = true;
      console.error(`Failed to participate tournament. Status: ${response.status}`);
    }
  } catch (error) {
    showLoadingModal.value = false;
    console.error('An error occurred while participating for the tournament', error);
  }
}


const generateDraw = async (tournamentId: number) => {
    try {
    showLoadingModal.value = true;
    const response = await fetch(`${config.public.tournamentsBase}/Tournaments/GenerateTournamentDraw?tournamentId=${tournamentId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization' : `Bearer ${authStore.token}`
      }
    });

    if (response.ok) {
        showLoadingModal.value = false;      
        await refreshNuxtData();
    } else {
        showLoadingModal.value = false;
      if(response.status == 401){
        errorNotification.value = `User not authorized to generate matches for tournament`
      }
      else{
        errorNotification.value = `An error occurred during generation of matches for tournament. Code: ${response.status}`
      }
      showErrorNotification.value = true;
      console.error(`Failed to generate tournament draw. Status: ${response.status}`);
    }
  } catch (error) {
    showLoadingModal.value = false;
    console.error('An error occurred while participating for the tournament', error);
  }
}
</script>

<template>
  <br>
  <br>
  <br>
    <div class="container">

        <div class="container">
    <h1 class="title is-1 has-text-centered">{{ tournament.title }} 
      <NuxtLink :to="`/tournaments/edit/${tournament.id}`" v-if="isAuthorized" class="edit-button"><font-awesome-icon icon="fa-solid fa-pen-to-square" /></NuxtLink>
      <span>{{ " " }}</span>
      <NuxtLink :to="`/tournaments/delete/${tournament.id}`" v-if="isAuthorized" class="delete-button"><font-awesome-icon icon="fa-solid fa-trash" /></NuxtLink>
    </h1>
    <h4 class="subtitle has-text-centered">{{ tournament.avenue.name }}, {{ tournament.avenue.city }}</h4>

    <div v-if="isAuthenticated && !hasTournamentStarted" class="buttons is-centered">
      
      <p v-if="!tournament.participants.find(p => p.players.find(pp => pp.username == user.username))">
          <BaseParticipateButton v-if="tournament.type == 'Singles'"
          :has-max-participants="tournament.participants.length === tournament.maxParticipants"
          @participate="participate(tournament.id)"/>

          <BaseParticipateButton v-if="tournament.type == 'Doubles'"
          :has-max-participants="tournament.participants.length === tournament.maxParticipants"
          @participate="openParticipateDoublesModal()"/>

          <BaseParticipateButton v-if="tournament.type == 'Teams'"
          :has-max-participants="tournament.participants.length === tournament.maxParticipants"
          @participate="openParticipateTeamModal()"/>
      </p>
      <p v-else>
          <button class="button is-info" @click="openParticipantRemovalModal(tournament.participants.find(p => p.players.find(pp => pp.username == user.username))?.id ?? -1)"
          v-if="tournament.participants.length < tournament.maxParticipants">
          Opt out of tournament
          </button>
      </p>
    </div>
    
    <div class="columns">
      <div class="column is-half">
        
        <div class="box">
          <h2 class="subtitle has-text-centered"><font-awesome-icon icon="fa-solid fa-calendar-days" /> Dates</h2>
          <p><strong>Start Date:</strong> {{ startDate }}</p>
          <p><strong>End Date:</strong> {{ endDate }}</p>
        </div>
        <div class="box">
          <h2 class="subtitle has-text-centered"><font-awesome-icon icon="fa-solid fa-message" /> Description</h2>
          <p>{{ tournament.description }}</p>
        </div>
        
        <div class="box">
          <h2 class="subtitle has-text-centered"><font-awesome-icon icon="fa-solid fa-circle-exclamation" /> Rules</h2>
          <p>{{ tournament.rules }}</p>
        </div>
      </div>
      
      <div class="column is-half">
        <div class="box">
          <h2 class="subtitle has-text-centered"><font-awesome-icon icon="fa-solid fa-circle-info" /> Details</h2>
          <ul>
            <li><strong>Type:</strong> {{ TournamentType[tournament.type] }}</li>
            <li><strong>Surface:</strong> {{ Surface[tournament.surface] }}</li>
            <li><strong>Entry Fee:</strong> {{ tournament.entryFee ? '$' + tournament.entryFee : 'Free' }}</li>
            <li><strong>Prize:</strong> {{ tournament.prize ? '$' + tournament.prize : 'Not specified' }}</li>
            <li><strong>Courts Available:</strong> {{ tournament.courtsAvailable }}</li>
            <li><strong>Participants:</strong> {{ tournament.minParticipants }} - {{ tournament.maxParticipants }}</li>
            <li v-if="tournament.matchWonPoints"><strong>Match Won Points:</strong> {{ tournament.matchWonPoints ? tournament.matchWonPoints : 'Not specified' }}</li>
            <li v-if="tournament.setWonPoints"><strong>Set Won Points:</strong> {{ tournament.setWonPoints ? tournament.setWonPoints : 'Not specified' }}</li>
            <li v-if="tournament.gameWonPoints"><strong>Game Won Points:</strong> {{ tournament.gameWonPoints ? tournament.gameWonPoints : 'Not specified' }}</li>
          </ul>
        </div>
        
        <div class="box">
          <h2 class="subtitle has-text-centered"><font-awesome-icon icon="fa-solid fa-location-dot" /> Location</h2>
          <p><strong><NuxtLink class="custom-link-light" :to="`/avenues/${tournament.avenue.id}`">{{ tournament.avenue.name }}</NuxtLink></strong> - {{ tournament.avenue.location }}</p>
          <p>{{ tournament.avenue.city }}, {{ tournament.avenue.country }} </p>
        </div>
        
        <div class="box">
          <h2 class="subtitle has-text-centered">Organizer</h2>
          <p><strong><NuxtLink class="custom-link-light" :to="`/users/profile/${tournament.organiser.username}`">{{ tournament.organiser.firstName }} {{ tournament.organiser.lastName }} ({{tournament.organiser.username}})</NuxtLink></strong></p>
          <p>{{ tournament.organiser.firstName }} {{ tournament.organiser.lastName }} ({{tournament.organiser.username}})</p>
        </div>
      </div>
    </div>
    
    <div class="box">
      <h2 class="subtitle has-text-centered"><font-awesome-icon icon="fa-solid fa-users" /> Participants </h2>
      <div class="has-text-centered" v-if="isAuthorized && !hasTournamentStarted">
        <div class="buttons is-centered">
          <BaseParticipateButton          
          :has-max-participants="tournament.participants.length === tournament.maxParticipants"
          button-label="Add Guest"
          @participate="openAddGuestModal()"/>
        
          <BaseParticipateButton v-if="tournament.type == 'Singles'"
          :has-max-participants="tournament.participants.length === tournament.maxParticipants"
          button-label="Add Participant"
          @participate="openParticipateSinglesModal()"/>

          <BaseParticipateButton v-if="tournament.type == 'Doubles'"
          :has-max-participants="tournament.participants.length === tournament.maxParticipants"
          button-label="Add Doubles Participant"
          @participate="openOrganiserParticipateDoublesModal()"/>

          <BaseParticipateButton v-if="tournament.type == 'Teams'"
          :has-max-participants="tournament.participants.length === tournament.maxParticipants"
          button-label="Add Team Participant"
          @participate="openParticipateTeamModal()"/>
        </div>
      </div>
      

      <div class="notification is-danger" v-if="showRemoveParticipantErrorNotification">
            <button class="delete" @click="hideRemoveParticipantErrorNotification"></button>
            {{removeParticipantErrorNotification}}
        </div>
      <div class="list">

        <ul>
        <div class="list-item">
            <li v-for="participant in tournament.participants" :key="participant.id">
              
              <br>
                <div class="list-item-title">
                    <div v-if="participant.isGuest">
                      
                        <span v-if="participant.players.length == 1"><font-awesome-icon icon="fa-solid fa-people-arrows" />  &nbsp; *</span>  {{ '' }}
                        <font-awesome-icon icon="fa-solid fa-user-secret" />
                        {{ participant.name }}
                        <span v-if="participant.players.some(x => x)">,
                          <font-awesome-icon icon="fa-solid fa-user" />{{ '' }}
                                <span v-for="player in participant.players" :key="player.id">
                                    {{ player.firstName }} {{ player.lastName }} ({{player.username}} | {{player.playerRating}})  
                                </span>
                            
                        </span>
                        <button class="button is-danger remove-participant-button" v-if="isAuthorized && !hasTournamentStarted" @click="openParticipantRemovalModal(participant.id)"><font-awesome-icon icon="fa-solid fa-user-minus" /></button>
                    </div>
                    <div v-else>                        
                        <div v-if="participant.players.some(x => x)">
                               <span v-if="participant.players.length > 1"><font-awesome-icon icon="fa-solid fa-people-arrows" /></span><span v-else><font-awesome-icon icon="fa-solid fa-user" /></span>  {{ '' }}
                                <span v-for="(player, index) in participant.players" :key="player.id"> 
                                    {{ player.firstName }} {{ player.lastName }} ({{player.username}} | {{player.playerRating}})<span v-if="participant.players.length - 1 > index">, </span>
                                    
                                </span>           
                                <button class="button is-danger remove-participant-button" v-if="isAuthorized && !hasTournamentStarted" @click="openParticipantRemovalModal(participant.id)"><font-awesome-icon icon="fa-solid fa-user-minus" /></button>                 
                        </div>
                    </div>
                </div>
            </li>
        </div>
      </ul>
      </div>
    </div>
    
    <div class="box">
      <h2 class="subtitle has-text-centered"><font-awesome-icon icon="fa-solid fa-ranking-star" /> Matches</h2>
      <div class="has-text-centered" v-if="isAuthorized && !hasTournamentStarted && tournament.participants.length >= tournament.minParticipants">
        <div class="buttons is-centered">
          <button class="button" @click="generateDraw(tournament.id)">
            Generate Tournament Draw
          </button>
        </div>
      </div>
      <MatchTableList
        :matches="tournament.matches"
        :showTableHeaders="true"
      />
    </div>
  </div>
    </div>

    <!--MODALS-->
    <ModalsLoadingModal
      :isOpen="showLoadingModal"
    />
    <TournamentAddGuestModal
      :isOpen="isAddGuestModalOpen"
      :tournamentId="tournament.id"
      @close="closeAddGuestModal"
    />

<TournamentRemoveParticipantModal
  :isOpen="isParticipantRemovalModalOpen"
  title="Participant Removal Confirmation"
  message="Are you sure you want to remove participant?"
  :tournamentId="tournament.id"
  :participantId="removeParticipantId"
  @close="closeParticipantRemovalModal"
/>

<TournamentParticipateSinglesModal
:isOpen="isAddSinglesParticipantModalOpen"
:tournamentId="tournament.id"
@close="closeAddSinglesParticipantModal"
/>

<TournamentParticipateDoublesModal
:isOpen="isOrganiserParticipateDoublesModalOpen"
:includeCurrentUser="false"
:tournamentId="tournament.id"
:tournamentParticipants="tournament.participants"
@close="closeOrganiserParticipateDoublesModal"
/>

<TournamentParticipateDoublesModal
:isOpen="isParticipateDoublesModalOpen"
:includeCurrentUser="true"
:tournamentId="tournament.id"
:tournamentParticipants="tournament.participants"
@close="closeParticipateDoublesModal"
/>

<!-- Game Details Modal -->
<MatchScoreDetailsSlimModal
  :isOpen="isGameDetailsModalOpen"
  :match="selectedMatch"
  @close="closeGameDetailsModal"
/>
</template>

<style scoped>
.container {
  margin-top: 20px;
}

.box {
  margin-bottom: 20px;
}

.remove-participant-button {
  font-size: x-small;
}

.custom-link-light {
  text-decoration: underline;
}

/* Winner styling */
.tournament-match-winner {
  font-weight: bold;
  color: #00d1b2;
}
</style>