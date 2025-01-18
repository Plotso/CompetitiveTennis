<script setup lang="ts">
  import { TournamentOutputModel, ParticipantShortOutputModel, TournamentType, Surface } from '@/types';
  import { Ref } from 'vue';
  
  defineProps({
    tournaments: {
      type: Array as () => TournamentOutputModel[],
      required: true,
    },
    user: Object, // Optional, define type based on your user model
    showParticipationColumn: Boolean,
  });
  
  defineEmits(['participate', 'openParticipateDoublesModal', 'openParticipateTeamModal', 'openParticipantRemovalModal']);
  
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
            <td>
              <div>
                Entry fee - {{ tournament.entryFee ? `${tournament.entryFee} BGN` : 'Free' }}
              </div>
              <div>
                Prize - {{ tournament.prize ? `${tournament.prize} BGN` : 'N/A' }}
              </div>
            </td>
            <td v-if="showParticipationColumn && user?.username">
              <p v-if="!tournament.participants.find(p => p.players.find(pp => pp.username == user.username))">
                <BaseParticipateButton v-if="tournament.type == 'Singles'"
                                       :has-max-participants="tournament.participants.length === tournament.maxParticipants"
                                       :is-disabled="hasTournamentStarted(tournament)"
                                       @participate="participate(tournament.id)"/>
  
                <BaseParticipateButton v-if="tournament.type == 'Doubles'"
                                       :has-max-participants="tournament.participants.length === tournament.maxParticipants"
                                       :is-disabled="hasTournamentStarted(tournament)"
                                       @participate="openParticipateDoublesModal(tournament.id, tournament.participants)"/>
  
                <BaseParticipateButton v-if="tournament.type == 'Teams'"
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
  