<script setup lang="ts">
  import { TournamentOutputModel, ParticipantShortOutputModel, ParticipantInputModel, TournamentType, Surface } from '@/types';
  import { Ref } from 'vue';
  import { storeToRefs } from 'pinia';
  import {useAuthStore} from "~/stores/auth"
  const authStore = useAuthStore();
  const config = useRuntimeConfig();

  const { user } = storeToRefs(useAuthStore());
  
  defineProps({
    tournaments: {
      type: Array as () => TournamentOutputModel[],
      required: true,
    },
    showParticipationColumn: Boolean,
    showMoneyRelatedColumns: Boolean
  });
  
  const clayImg = ref('https://www.publicdomainpictures.net/pictures/400000/nahled/clay-tennis-court-with-balls.jpg')
const getCourtImg = (surface: Surface): string => {
    console.log(surface)
    if (surface === Surface.Clay)
        return clayImg.value;

}
  
  const getTournamentTypeLabel = (type: string | number): string => {
    return typeof type === 'number' ? TournamentType[type] : type.toString();
  };
  
  const getSurfaceLabel = (surface: string | number): string => {
    return typeof surface === 'number' ? Surface[surface] : surface.toString();
  };

  const hasTournamentStarted = (tournament: TournamentOutputModel) => tournament != null && tournament.matches.length > 0;
  
  const formatDate = (date: string | Date): string => {
    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    };
    return new Date(date).toLocaleDateString(undefined, options);
  };

  const showLoadingModal = ref(false)

  const errorNotification = ref("")
const showErrorNotification = ref(false)

const hideErrorNotification = () => {
    showErrorNotification.value = false;
}

const removalTournamentId = ref(-1);
const removeParticipantId = ref(-1);
const doubleParticipationTournamentId = ref(-1)
const doubleTournamentsParticipants: Ref<ParticipantShortOutputModel[]> = ref([])
const isParticipantRemovalModalOpen = ref(false);

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
    <div class="table-container">
      <table class="table is-striped is-fullwidth">
        <tbody>
          <tr v-for="tournament in tournaments" :key="tournament.id">
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
            <td v-if="showMoneyRelatedColumns">
              <div>
                Entry fee - {{ tournament.entryFee ? `${tournament.entryFee} BGN` : 'Free' }}
              </div>
              <div>
                Prize - {{ tournament.prize ? `${tournament.prize} BGN` : 'N/A' }}
              </div>
            </td>
            <td v-if="showParticipationColumn && user?.username">
              <p v-if="!tournament.participants.find(p => p.players.find(pp => pp.username == user.username))">
                <BaseParticipateButton v-if="tournament.type == TournamentType[TournamentType.Singles]"
                                       :has-max-participants="tournament.participants.length === tournament.maxParticipants"
                                       :is-disabled="hasTournamentStarted(tournament)"
                                       @participate="participate(tournament.id)"/>
  
                <BaseParticipateButton v-if="tournament.type == TournamentType[TournamentType.Doubles]"
                                       :has-max-participants="tournament.participants.length === tournament.maxParticipants"
                                       :is-disabled="hasTournamentStarted(tournament)"
                                       @participate="openParticipateDoublesModal(tournament.id, tournament.participants)"/>
  
                <BaseParticipateButton v-if="tournament.type == TournamentType[TournamentType.Teams]"
                                       :has-max-participants="tournament.participants.length === tournament.maxParticipants"
                                       :is-disabled="hasTournamentStarted(tournament)"
                                       @participate="openParticipateTeamModal(tournament.id)"/>
              </p>
              <p v-else>
                <button class="button is-info" 
                        @click="openParticipantRemovalModal(tournament.id, tournament.participants.find(p => p.players.find(pp => pp.username == user.username))?.id ?? -1)"
                        :disabled="hasTournamentStarted(tournament)">
                  Opt out of tournament
                </button>
              </p>
            </td>
          </tr>
        </tbody>
      </table>
      <!--MODALS-->
      <ModalsLoadingModal
      :isOpen="showLoadingModal"
    />

    <TournamentParticipateDoublesModal
    :isOpen="isParticipateDoublesModalOpen"
    :includeCurrentUser="true"
    :tournamentId="doubleParticipationTournamentId"
    @close="closeParticipateDoublesModal"
    />

    <TournamentParticipateDoublesModal
    :isOpen="isParticipateDoublesModalOpen"
    :includeCurrentUser="true"
    :tournamentId="doubleParticipationTournamentId"
    :tournamentParticipants="doubleTournamentsParticipants"
    @close="closeParticipateDoublesModal"
    />

    
    <TournamentRemoveParticipantModal
    :isOpen="isParticipantRemovalModalOpen"
    title="Opt out of tournament confirmal"
    message="Are you sure you want to opt out from the tournament?"
    :tournamentId="removalTournamentId"
    :participantId="removeParticipantId"
    @close="closeParticipantRemovalModal"
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
  