<script setup lang="ts">
definePageMeta({
  layout: "default-transparent",
});
import { TournamentOutputModel, Result, SearchOutputModel, TournamentQuery, TournamentType, Surface, ParticipantInputModel, MultiParticipantInputModel, ParticipantShortOutputModel } from '@/types'; // Update the path as per your project setup
import { storeToRefs } from 'pinia';
import TournamentParticipateDoublesModal from '~/components/tournament/ParticipateDoublesModal.vue';
import {useAuthStore} from "~/stores/auth"
import { useTournamentsApi } from '~/composables/useTournamentsApi';
const router = useRouter();
const config = useRuntimeConfig();
const authStore = useAuthStore();

const { user } = storeToRefs(useAuthStore());

const tournaments = ref<TournamentOutputModel[]>([]);

const query: TournamentQuery = {
    page: 1,
    itemsPerPage: 20,
};
const method = 'GET';
const options = {
    query,
    method
}

const { data, pending, refresh, error } = await useTournamentsApi<Result<SearchOutputModel<TournamentOutputModel>>>(`/Tournaments/Search`, options)
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
                :showParticipationColumn="true"
                :show-money-related-columns="true"
            />
    </div>

    <!--MODALS-->
    <ModalsLoadingModal
      :isOpen="showLoadingModal"
    />
    </div>

    
</template>

<style scoped></style>