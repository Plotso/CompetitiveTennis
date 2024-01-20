<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { Surface, TournamentType, Result, TournamentOutputModel, SlimTournamentOutputModel, ParticipantInputModel, MatchShortOutputModel } from "@/types"
import {useAuthStore} from "~/stores/auth"
const props = defineProps({
    data: {type: Object as PropType<Result<MatchShortOutputModel>>, required: true},
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
</script>

<template>
  <br>
  <br>
  <br>
    <div class="container">
      <div>
        <NuxtLink :to="`/tournaments/${match.tournamentId}`" class="button">Go back to tournament</NuxtLink>
      </div>
        <div>show blocks for each participant showing their name</div>
        <div class="container">
          <h1 class="title is-1 has-text-centered">{{ startDate }} 
            <NuxtLink :to="`/matches/edit/${match.id}`" v-if="isAuthorized" class="edit-button"><font-awesome-icon icon="fa-solid fa-pen-to-square" /></NuxtLink>
            <span>{{ " " }}</span>
            <NuxtLink :to="`/matches/delete/${match.id}`" v-if="isAuthorized" class="delete-button"><font-awesome-icon icon="fa-solid fa-trash" /></NuxtLink>
          </h1>
      
          <div>show score</div>

          <div v-if="isAuthorized" class="buttons is-centered">
            
            <p>
              TODO: Add add/edit scores button
            </p>
          </div>
      
          <div class="box">
            Insert scores here      
          </div>
      </div>
    </div>

    <!--MODALS-->
    <LoadingModal
      :isOpen="showLoadingModal"
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