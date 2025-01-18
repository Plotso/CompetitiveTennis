<script setup lang="ts">
definePageMeta({
  layout: "default-transparent",
});
import { TournamentOutputModel, Result, SearchOutputModel, TournamentType, Surface, ParticipantInputModel, MultiParticipantInputModel, ParticipantShortOutputModel } from '@/types'; // Update the path as per your project setup
import { storeToRefs } from 'pinia';
import TournamentParticipateDoublesModal from '~/components/tournament/ParticipateDoublesModal.vue';
import {useAuthStore} from "~/stores/auth"
import { useTournamentsApi } from '~/composables/useTournamentsApi';
const router = useRouter();
const config = useRuntimeConfig();
const authStore = useAuthStore();

const { user } = storeToRefs(useAuthStore());

const tournaments = ref<TournamentOutputModel[]>([]);

const { data, pending, refresh, error } = await useTournamentsApi<Result<SearchOutputModel<TournamentOutputModel>>>(`/Tournaments/Search`)
if (error.value) {
    console.log('data', data.value)
    console.log('pending', pending.value)
    console.log('error', error.value)
    refresh()
}

if (data?.value?.data) {
    tournaments.value = data.value?.data.results
}
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
        <Banner title="All Tournaments" background-img="/imgs/ongoing-tournament-banner.png">            
            <div>
                <div v-if="user.username" class="buttons is-centered">
                <hr>
                    <NuxtLink to="/tournaments/create" class="button is-primary">Create Tournament</NuxtLink>
                <hr>
                </div>
            </div>
        </Banner>
    <div v-if="pending">
        <BaseLoading></BaseLoading>
    </div>

    <div class="container" v-else>
        <div class="notification is-danger" v-if="showErrorNotification">
            <button class="delete" @click="hideErrorNotification"></button>
            {{errorNotification}}
        </div>
            <TournamentTableList
                :tournaments="data?.data.results"
                :user="user"
                @participate="participate"
                @openParticipateDoublesModal="openParticipateDoublesModal"
                @openParticipateTeamModal="openParticipateTeamModal"
                @openParticipantRemovalModal="openParticipantRemovalModal"
            />
    </div>

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

<style scoped></style>