<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';
import { MatchShortOutputModel } from '~/types';
import { useParticipantNameBuilder } from '~/composables/useParticipantNameBuilder'
const { buildHomeParticipantName, buildAwayParticipantName } = useParticipantNameBuilder()

var props = defineProps({
    isOpen: { type: Boolean, required: true },
    match: { type: Object as PropType<MatchShortOutputModel|null>, required: true }
});

console.log(props)

const emit = defineEmits(['close']);

const closeModal = () => {
    emit('close');
};

// Compute the number of sets from the match results
const setCount = computed(() => props.match?.results?.setResults?.length ?? 0);

// Helper function to get games won by a player for a specific set
const getGamesWon = (match: MatchShortOutputModel|null, setNumber: number, side) => {
  const set = match?.results?.setResults?.find(s => s.setNumber === setNumber);
  if (!set) return '-';
  return side === 'home' ? set.homeSideGamesWon : set.awaySideGamesWon;
};
</script>
<template>
    <div class="modal" :class="{ 'is-active': isOpen }">
      <div class="modal-background" @click="closeModal"></div>
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">{{ match?.stage ?? 'Match Stage' }}</p>
          <button class="delete" aria-label="close" @click="closeModal"></button>
        </header>
        <section class="modal-card-body">
          <table class="table is-fullwidth no-borders">
            <tbody>
              <tr>
                <td class="player-name">{{ buildHomeParticipantName(match, false, false) }}</td>
                <td v-for="set in setCount" :key="'home-' + set" class="games-won">
                  {{ getGamesWon(match, set, 'home') }}
                </td>
              </tr>
              <tr>
                <td class="player-name">{{ buildAwayParticipantName(match, false, false) }}</td>
                <td v-for="set in setCount" :key="'away-' + set" class="games-won">
                  {{ getGamesWon(match, set, 'away') }}
                </td>
              </tr>
            </tbody>
          </table>
        </section>
        <footer class="modal-card-foot">
          <button class="button is-success" @click="closeModal">Close</button>
        </footer>
      </div>
    </div>
  </template>


<style scoped>
/* Adjust the modal to make it visually more appealing */
.custom-modal-card {
  width: 60%;
  max-width: 800px;
}

.player-names {
  text-align: center;
  margin-bottom: 20px;
}

.player {
  font-size: 1.5rem;
  font-weight: bold;
}

.vs {
  font-size: 1.25rem;
  font-style: italic;
  margin: 0 10px;
}

.game-results-table {
  text-align: center;
}

.game-results-table th,
.game-results-table td {
  text-align: center;
  vertical-align: middle;
}

.modal-card-body {
  background-color: #f8f9fa;
  padding: 20px;
}

.modal-card-head {
  background-color: #00d1b2;
  color: white;
  border-bottom: 2px solid #ddd;
}

.modal-card-foot {
  justify-content: center;
  border-top: 2px solid #ddd;
}


.table th, .table td {
  text-align: center;
  vertical-align: middle;
}

.modal-card {
  width: 60%;
  max-width: 800px;
}

.table {
  text-align: center;
  width: 100%;
  border-collapse: separate;
  border-spacing: 0 10px; /* Adds space between rows */
}

.no-borders th,
.no-borders td {
  border: none;
  padding: 10px;
}

.player-column {
  text-align: left;
  font-weight: bold;
}

.player-name {
  text-align: left;
  font-weight: bold;
}

.games-won {
  font-size: 1.2rem;
  font-weight: bold;
}
</style>