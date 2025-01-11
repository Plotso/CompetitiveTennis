<script setup lang="ts">
import { defineProps } from 'vue';
import { MatchShortOutputModel, MatchPeriodOutcome } from '~/types';
import { useParticipantNameBuilder } from '~/composables/useParticipantNameBuilder'
const { buildHomeParticipantName, buildAwayParticipantName } = useParticipantNameBuilder()

const props = defineProps({
  match: { type: Object as PropType<MatchShortOutputModel | null>, required: true },
});

// Compute the number of sets from the match results
const setCount = computed(() => props.match?.results?.setResults?.length ?? 0);

// Helper function to get games won by a player for a specific set
const getGamesWon = (setNumber: number, side: 'home' | 'away') => {
  const set = props.match?.results?.setResults?.find(s => s.setNumber === setNumber);
  if (!set) return '-';
  return side === 'home' ? set.homeSideGamesWon : set.awaySideGamesWon;
};

// Determine the winner
const isWinner = (side: 'home' | 'away') => {
  const outcome = props.match?.outcome;
  return (side === 'home' && outcome === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantOne]) || (side === 'away' && outcome === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantTwo]);
};

// Get total sets won by each player
const getTotalSetsWon = (side: 'home' | 'away') => {
  return props.match?.results?.setResults?.filter(set => {
    if (side === 'home') return set.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantOne];
    if (side === 'away') return set.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantTwo];
  }).length ?? 0;
};

const showTableHead = false;
</script>

<template>
  <div class="table-container">
    <table class="table is-fullwidth is-hoverable summary-table no-borders">
      <thead v-if="showTableHead">
        <tr>
          <th class="player-column">Player</th>
          <th class="sets-won-column">Sets Won</th>
          <th v-for="set in setCount" :key="'set-header-' + set" class="set-column">Set {{ set }}</th>
        </tr>
      </thead>
      <tbody>
        <!-- Home Player Row -->
        <tr>
          <td :class="{ 'winner': isWinner('home') }">{{ buildHomeParticipantName(match, false, false) }}</td>
          <td class="sets-won has-text-weight-bold">{{ getTotalSetsWon('home') }}</td>
          <td v-for="set in setCount" :key="'home-' + set" class="game-score">{{ getGamesWon(set, 'home') }}</td>
        </tr>

        <!-- Away Player Row -->
        <tr>
          <td :class="{ 'winner': isWinner('away') }">{{ buildAwayParticipantName(match, false, false) }}</td>
          <td class="sets-won has-text-weight-bold">{{ getTotalSetsWon('away') }}</td>
          <td v-for="set in setCount" :key="'away-' + set" class="game-score">{{ getGamesWon(set, 'away') }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
/* Add padding and spacing to the table container */
.table-container {
  margin-top: 20px;
  padding: 10px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
}

/* Adjust table headers */
.summary-table th {
  background-color: #4a90e2;
  color: white;
  text-align: center;
  font-weight: bold;
  vertical-align: middle;
}

/* Column-specific styling */
.player-column {
  text-align: left;
  width: 40%;
}

.sets-won-column {
  text-align: center;
  width: 15%;
}

.set-column {
  text-align: center;
  width: auto;
}

/* Table rows styling */
.summary-table td {
  text-align: right;
  vertical-align: middle;
  font-size: 1.1rem;
}

.summary-table td:first-child {
  text-align: left;
  font-size: 1.2rem;
}

/* Winner styling */
.winner {
  font-weight: bold;
  color: #00d1b2;
}

/* Highlight the game scores */
.game-score {
  font-size: 1.2rem;
  color: #4a4a4a;
}

/* Sets Won Styling */
.sets-won {
  font-size: 1.3rem;
  color: #363636;
}

/* Add hover effect */
.summary-table tbody tr:hover {
  background-color: #e6f7ff;
}

/* Responsive Styling */
@media (max-width: 768px) {
  .summary-table th,
  .summary-table td {
    font-size: 0.9rem;
  }
}

.no-borders th,
.no-borders td {
  border: none;
  padding: 10px;
}
</style>
