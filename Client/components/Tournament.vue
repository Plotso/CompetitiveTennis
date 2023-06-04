<script setup lang="ts">
import { Surface, TournamentType, Result, TournamentOutputModel, ParticipantInputModel } from "@/types"
import {useAuthStore} from "~/stores/auth"
const props = defineProps({
    data: {type: Object as PropType<Result<TournamentOutputModel>>, required: true}
})
const authStore = useAuthStore();
const config = useRuntimeConfig();

const tData = toRef(props, "data")
const tournament = ref(tData.value.data)
//const comp = computed(() => props.data) //Would work the same way as toRef from above
const options: Intl.DateTimeFormatOptions = {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        hour12: false
      };


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

const startDate = computed(() => new Date(tournament.value.startDate).toLocaleDateString(undefined, options).replace(' at', ''));
const endDate = computed(() => new Date(tournament.value.endDate).toLocaleDateString(undefined, options).replace(' at', ''));

const isAuthorized = computed(() => {
    return authStore.user && (authStore.user.username == tournament.value.organiser.username || authStore.user.hasAdministrativeRights)
});

const removeParticipantErrorNotification = ref("")
const showRemoveParticipantErrorNotification = ref(false)

const hideRemoveParticipantErrorNotification = () => {
    showRemoveParticipantErrorNotification.value = false;
}
</script>

<template>
    <div class="container">

        <div class="container">
    <h1 class="title is-1 has-text-centered">{{ tournament.title }} 
      <NuxtLink :to="`/tournaments/edit/${tournament.id}`" v-if="isAuthorized" class="edit-button"><font-awesome-icon icon="fa-solid fa-pen-to-square" /></NuxtLink>
      <span>{{ " " }}</span>
      <NuxtLink :to="`/tournaments/delete/${tournament.id}`" v-if="isAuthorized" class="delete-button"><font-awesome-icon icon="fa-solid fa-trash" /></NuxtLink>
    </h1>
    <h4 class="subtitle has-text-centered">{{ tournament.avenue.name }}, {{ tournament.avenue.city }}</h4>

    
    
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
          <p><strong><NuxtLink class="custom-link" :to="`/avenues/${tournament.avenue.id}`">{{ tournament.avenue.name }}</NuxtLink></strong> - {{ tournament.avenue.location }}</p>
          <p>{{ tournament.avenue.city }}, {{ tournament.avenue.country }} </p>
        </div>
        
        <div class="box">
          <h2 class="subtitle has-text-centered">Organizer</h2>
          <p>{{ tournament.organiser.firstName }} {{ tournament.organiser.lastName }} ({{tournament.organiser.username}})</p>
          <p>{{ tournament.organiser.lastName }}</p>
        </div>
      </div>
    </div>
    
    <div class="box">
      <h2 class="subtitle has-text-centered"><font-awesome-icon icon="fa-solid fa-users" /> Participants </h2>
      <div class="has-text-centered" v-if="isAuthorized">
        <button class="button" @click="openAddGuestModal">
          Add Guest
        </button>
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
                        <font-awesome-icon icon="fa-solid fa-user-secret" />
                        {{ participant.name }} 
                        <div v-if="participant.players.some(x => x)">
                            (
                                <span v-for="player in participant.players" :key="player.id">
                                    {{ player.firstName }} {{ player.lastName }} ({{player.username}} | {{player.rating}})  
                                </span>
                            )
                        </div>
                        <button class="button is-danger remove-participant-button" v-if="isAuthorized" @click="openParticipantRemovalModal(participant.id)"><font-awesome-icon icon="fa-solid fa-user-minus" /></button>
                    </div>
                    <div v-else>                        
                        <div v-if="participant.players.some(x => x)">
                               <span v-if="participant.players.length > 1"><font-awesome-icon icon="fa-solid fa-people-arrows" /></span><span v-else><font-awesome-icon icon="fa-solid fa-user" /></span>  {{ '' }}
                                <span v-for="player in participant.players" :key="player.id"> 
                                    {{ player.firstName }} {{ player.lastName }} ({{player.username}} | {{player.playerRating}})
                                    
                                <button class="button is-danger remove-participant-button" v-if="isAuthorized" @click="openParticipantRemovalModal(participant.id)"><font-awesome-icon icon="fa-solid fa-user-minus" /></button>
                                </span>                            
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
      <table class="table is-striped is-fullwidth">
        <thead>
          <tr>
            <th>Player 1</th>
            <th>Player 2</th>
            <th>Score</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="match in tournament.matches" :key="match.Id">
            <td>{{ match.player1 }}</td>
            <td>{{ match.player2 }}</td>
            <td>{{ match.score }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
    </div>

    <!--MODALS-->
    <AddGuestModal
      :isOpen="isAddGuestModalOpen"
      :tournamentId="tournament.id"
      @close="closeAddGuestModal"
    />

<RemoveParticipantModal
  :isOpen="isParticipantRemovalModalOpen"
  title="Participant Removal Confirmation"
  message="Are you sure you want to remove participant?"
  :tournamentId="tournament.id"
  :participantId="removeParticipantId"
  @close="closeParticipantRemovalModal"
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
</style>