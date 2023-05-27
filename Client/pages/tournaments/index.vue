<script setup lang="ts">
import { TournamentOutputModel, Result, TournamentType, Surface } from '@/types'; // Update the path as per your project setup
import { useAuthStore } from '@/stores/auth'
const route = useRoute();
const config = useRuntimeConfig();
const authStore = useAuthStore();

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

</script>

<template>
    <div v-if="pending">
        <Loading></Loading>
    </div>

    <div class="container" v-else>
        <h1 class="title is-1 has-text-centered">All Tournaments</h1>
        <div>
            <hr>
            <div v-if="authStore.user.username" class="buttons is-centered">
                <NuxtLink to="/tournaments/create" class="button is-primary">Create Tournament</NuxtLink>
            </div>
            <hr>
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
                        <td>
                            <a href="/" class="button is-primary"
                                v-if="tournament.participants.length < tournament.maxParticipants">
                                Participate
                            </a>
                            <a class="button is-primary" href="/" v-else disabled>
                                Participate
                            </a>
                        </td>
                        <hr>
                    </tr>
                    <!-- Add more tournament rows here -->
                </tbody>
            </table>

        </div>
    </div>

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