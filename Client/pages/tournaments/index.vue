<script setup lang="ts">
import { TournamentOutputModel, Result, TournamentType, Surface, ParticipantInputModel, MultiParticipantInputModel, ParticipantShortOutputModel } from '@/types'; // Update the path as per your project setup
import { storeToRefs } from 'pinia';
import ParticipateDoublesModal from '~/components/ParticipateDoublesModal.vue';
import {useAuthStore} from "~/stores/auth"
const router = useRouter();
const config = useRuntimeConfig();
const authStore = useAuthStore();

const { user } = storeToRefs(useAuthStore());
const clayImg = ref('https://www.publicdomainpictures.net/pictures/400000/nahled/clay-tennis-court-with-balls.jpg')

const tournaments = ref<TournamentOutputModel[]>([]);

const { data, pending, refresh, error } = await useFetch<Result<TournamentOutputModel[]>>(() => `/Tournaments/All`, {
    baseURL: config.public.tournamentsBase
})
if (error.value) {
    console.log('data', data.value)
    console.log('pending', pending.value)
    console.log('error', error.value)
    refresh()
}
if (data?.value?.data) {
    tournaments.value = data.value?.data
}
const showLoadingModal = ref(false)
const errorNotification = ref("")
const showErrorNotification = ref(false)

const hideErrorNotification = () => {
    showErrorNotification.value = false;
}

const getTournamentTypeLabel = (type: TournamentType): string => {
    return Number.isInteger(type) ? TournamentType[type] : type.toString();
};

const getSurfaceLabel = (surface: Surface): string => {
    return  Number.isInteger(surface) ? Surface[surface] : surface.toString();
};

const formatDate = (date: Date): string => {
    const options: Intl.DateTimeFormatOptions = {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
    };
    return new Date(date).toLocaleDateString(undefined, options);
};

const getCourtImg = (surface: Surface): string => {
    console.log(surface)
    if (surface === Surface.Clay)
        return clayImg.value;

}

const removalTournamentId = ref(-1);
const removeParticipantId = ref(-1);
const doubleParticipationTournamentId = ref(-1)
const doubleTournamentsParticipants: Ref<ParticipantShortOutputModel[]> = ref([])
const isParticipantRemovalModalOpen = ref(false);

const hasTournamentStarted = (tournament: TournamentOutputModel) => tournament != null && tournament.matches.length > 0;

const openParticipantRemovalModal = (tournamentId: number, participantId: number) => {
  removeParticipantId.value = participantId;
  removalTournamentId.value = tournamentId;
  isParticipantRemovalModalOpen.value = true;
};

const closeParticipantRemovalModal = () => {
  isParticipantRemovalModalOpen.value = false;
};
const isParticipateDoublesModalOpen = ref(false);

const openParticipateDoublesModal = (tournamentId: number, participants: ParticipantShortOutputModel[]) => {
    console.log('part doubles');
    doubleParticipationTournamentId.value = tournamentId;
  isParticipateDoublesModalOpen.value = true;
  doubleTournamentsParticipants.value = participants;
};

const openParticipateTeamModal = (tournamnetId: number) => {
    console.log('part teams');
    //ToDo: currently not supported
}

const closeParticipateDoublesModal = () => {
    isParticipateDoublesModalOpen.value = false;
};

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

const optOutOfTournament = async (tournamentId: number, participantId: number) => {
    // Send opt-out request request after modal confirmation
    console.log(tournamentId);

    try {
    showLoadingModal.value = true;
    const response = await fetch(`${config.public.tournamentsBase}/Tournaments/RemoveParticipantFromTournament?tournamentId=${tournamentId}&participantId=${participantId}`, {
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
        errorNotification.value = `User not authorized to do the selected activity`
      }
      else{
        errorNotification.value = `An error occurred during opt out attempt for tournament. Code: ${response.status}`
      }
      showErrorNotification.value = true;
      console.error(`Failed to opt out from tournament. Status: ${response.status}`);
    }
  } catch (error) {
    showLoadingModal.value = false;
    console.error('An error occurred while optiong out of the tournament', error);
  }
}


</script>

<template>
    <div class="view-window">
        <Banner title="All Tournaments">            
            <div>
                <div v-if="user.username" class="buttons is-centered">
                <hr>
                    <NuxtLink to="/tournaments/create" class="button is-primary">Create Tournament</NuxtLink>
                <hr>
                </div>
            </div>
        </Banner>
    <div v-if="pending">
        <Loading></Loading>
    </div>

    <div class="container" v-else>
        <!--
        <h1 class="title is-1 has-text-centered">All Tournaments</h1>
        <div>
            <hr>
            <div v-if="user.username" class="buttons is-centered">
                <NuxtLink to="/tournaments/create" class="button is-primary">Create Tournament</NuxtLink>
            </div>
            <hr>
        </div>
    -->

        <div class="notification is-danger" v-if="showErrorNotification">
            <button class="delete" @click="hideErrorNotification"></button>
            {{errorNotification}}
        </div>
        <div class="table-container">
            <table class="table is-striped is-fullwidth">
                <tbody>
                    <tr v-for="tournament in data.data" :key="tournament.id">
                        <td>
                            <img alt="tournament badge"
                                src="https://previews.123rf.com/images/madabatman/madabatman2007/madabatman200700012/150920417-abstract-tennis-logo-design-in-vector-quality.jpg"
                                width="75" height="75">
                        </td>
                        <td>
                            <NuxtLink :to="`/tournaments/${tournament.id}`" class="custom-link has-text-weight-semibold">{{
                                tournament.title }}</NuxtLink>

                            <p class="mb-1">
                                <NuxtLink :to="`avenues/${tournament.avenue.id}`" class="custom-link">
                                    {{ tournament.avenue.name }}, {{ tournament.avenue.city }}
                                </NuxtLink>
                            </p>
                            <p>{{ formatDate(tournament.startDate) }} - {{ formatDate(tournament.endDate) }}</p>
                        </td>
                        <td>
                            <div class="tags">
                                <span class="tag">{{ getTournamentTypeLabel(tournament.type) }}
                                    {{ tournament.minParticipants }}-{{ tournament.maxParticipants }}</span>
                            </div>
                        </td>
                        <td>
                            <div class="tags">
                                <span class="tag">Outdoor</span>
                                <span class="tag">{{ getSurfaceLabel(tournament.surface) }}</span>
                            </div>
                        </td>
                        <td>
                            <div>
                                Entry fee - {{ tournament.entryFee ? `${tournament.entryFee} BGN` : 'Free' }}
                            </div>
                            <div>
                                Prize - {{ tournament.prize ? `${tournament.prize} BGN` : 'N/A' }}
                            </div>
                        </td>
                        <td v-if="user.username">
                            <p v-if="!tournament.participants.find(p => p.players.find(pp => pp.username == user.username))">
                                <ParticipateButton v-if="tournament.type == 'Singles'"
                                :has-max-participants="tournament.participants.length === tournament.maxParticipants"
                                :is-disabled="hasTournamentStarted(tournament)"
                                @participate="participate(tournament.id)"/>

                                <ParticipateButton v-if="tournament.type == 'Doubles'"
                                :has-max-participants="tournament.participants.length === tournament.maxParticipants"
                                :is-disabled="hasTournamentStarted(tournament)"
                                @participate="openParticipateDoublesModal(tournament.id, tournament.participants)"/>

                                <ParticipateButton v-if="tournament.type == 'Teams'"
                                :has-max-participants="tournament.participants.length === tournament.maxParticipants"
                                :is-disabled="hasTournamentStarted(tournament)"
                                @participate="openParticipateTeamModal(tournament.id)"/>
                            </p>
                            <p v-else>
                                <button class="button is-info" @click="openParticipantRemovalModal(tournament.id, tournament.participants.find(p => p.players.find(pp => pp.username == user.username))?.id ?? -1)"
                                :disabled="hasTournamentStarted(tournament)">
                                Opt out of tournament
                                </button>
                            </p>
                            
                        </td>
                        <hr>
                    </tr>
                    <!-- Add more tournament rows here -->
                </tbody>
            </table>

        </div>
    </div>

    <!--MODALS-->
    <LoadingModal
      :isOpen="showLoadingModal"
    />

    <ParticipateDoublesModal
    :isOpen="isParticipateDoublesModalOpen"
    :includeCurrentUser="true"
    :tournamentId="doubleParticipationTournamentId"
    @close="closeParticipateDoublesModal"
    />

    <ParticipateDoublesModal
    :isOpen="isParticipateDoublesModalOpen"
    :includeCurrentUser="true"
    :tournamentId="doubleParticipationTournamentId"
    :tournamentParticipants="doubleTournamentsParticipants"
    @close="closeParticipateDoublesModal"
    />

    
    <RemoveParticipantModal
    :isOpen="isParticipantRemovalModalOpen"
    title="Opt out of tournament confirmal"
    message="Are you sure you want to opt out from the tournament?"
    :tournamentId="removalTournamentId"
    :participantId="removeParticipantId"
    @close="closeParticipantRemovalModal"
    />

    <!--
    <div class="container" v-if="!pending">
        <h1 class="title is-1 has-text-centered">All Tournaments</h1>
        <div class="box">
            <div class="content">
                <div class="columns is-multiline">
                    <div class="column is-one-third custom-box" v-for="tournament in data.data" :key="tournament.id">
                        <div class="media  is-centered">
                            <div class="media-left">
                                <figure class="image img-custom is-4by3 image-container">
                                    <img :src="clayImg" alt="Tournament Image">
                                    <div class="tags ">
                                        <span class="tag is-dark image-overlay-left"><font-awesome-icon
                                                icon="fa-solid fa-calendar-days" /> {{ formatDate(tournament.startDate) }} -
                                            {{ formatDate(tournament.endDate) }}</span>
                                        <span> </span>
                                        <span class="tag image-overlay-right">Clay</span>
                                    </div>
                                </figure>
                                <div class="media-content is-centered-custom">
                                    <p class="title is-5 has-text-centered">
                                        <NuxtLink :to="`/tournaments/${tournament.id}`" class="has-text-weight-semibold">{{
                                            tournament.title }}</NuxtLink>
                                    </p>
                                    <p class="subtitle is-6 has-text-centered">
                                        <NuxtLink :to="`avenues/${tournament.avenue.id}`">
                                            {{ tournament.avenue.name }}, {{ tournament.avenue.city }}
                                        </NuxtLink>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container" v-if="!pending">
        <h1 class="title">All Tournaments</h1>
        <div class="columns is-multiline">
            <div v-for="tournament in data.data" :key="tournament.id" class="column is-one-third">
                <div class="card">
                    <div class="card-image">
                        <figure class="image is-4by3 backwards">
                            <img src="https://placekitten.com/800/600" alt="Tournament Image">
                        </figure>
                    </div>
                    <div class="card-content">
                        <div class="content">
                            <p class="title is-5">
                                <NuxtLink :to="`/tournaments/${tournament.id}`">{{ tournament.title }}</NuxtLink>
                            </p>
                            <p class="subtitle is-6">{{ getTournamentTypeLabel(tournament.type) }}</p>
                            <p><strong>Description:</strong> {{ tournament.description }}</p>
                            <p><strong>Surface:</strong> {{ getSurfaceLabel(tournament.surface) }}</p>
                            <p><strong>Start Date:</strong> {{ formatDate(tournament.startDate) }}</p>
                            <p><strong>End Date:</strong> {{ formatDate(tournament.endDate) }}</p>
                            <p><strong>Entry Fee:</strong> {{ tournament.entryFee ? `$${tournament.entryFee}` : 'Free' }}
                            </p>
                            <p><strong>Prize:</strong> {{ tournament.prize ? `$${tournament.prize}` : 'N/A' }}</p>
                            <p><strong>Available Courts:</strong> {{ tournament.courtsAvailable }}</p>
                            <p><strong>Participants:</strong> {{ tournament.participants.length }}</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="container" v-if="!pending">
        <h1 class="title">All Tournaments</h1>
        <div class="table-container">
            <table class="table is-fullwidth is-hoverable">
                <thead>
                    <tr>
                        <th>Tournament</th>
                        <th>Type</th>
                        <th>Surface</th>
                        <th>Start Date</th>
                        <th>End Date</th>
                        <th>Entry Fee</th>
                        <th>Prize</th>
                        <th>Courts</th>
                        <th>Participants</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="tournament in data.data" :key="tournament.id">
                        <td>
                            <NuxtLink :to="`/tournaments/${tournament.id}`">{{ tournament.title }}</NuxtLink>
                        </td>
                        <td>{{ getTournamentTypeLabel(tournament.type) }}</td>
                        <td>{{ getSurfaceLabel(tournament.surface) }}</td>
                        <td>{{ formatDate(tournament.startDate) }}</td>
                        <td>{{ formatDate(tournament.endDate) }}</td>
                        <td>{{ tournament.entryFee ? `$${tournament.entryFee}` : 'Free' }}</td>
                        <td>{{ tournament.prize ? `$${tournament.prize}` : 'N/A' }}</td>
                        <td>{{ tournament.courtsAvailable }}</td>
                        <td>{{ tournament.participants.length }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        
    </div>
-->
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