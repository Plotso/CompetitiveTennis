<script setup lang="ts">
import { defineProps, ref, computed, watch, onMounted } from 'vue';
import { MatchShortOutputModel, MatchPeriodOutputModel, MatchPeriodOutcome, EventActor } from '~/types';

const props = defineProps({
  match: { type: Object as PropType<MatchShortOutputModel | null>, required: true },
});

// Extract the distinct sets from the matchPeriods
const sets = computed(() => {
  if (!props.match || !props.match.matchPeriods) return [];
  return [...new Set(props.match.matchPeriods.map(period => period.set))];
});

// Active set state (default to the first set if available)
const activeSet = ref(sets.value.length > 0 ? sets.value[0] : null);

// Filter match periods based on the selected set
const matchPeriodsForSet = computed(() => {
  if (!activeSet.value) return [];
  return props.match?.matchPeriods?.filter(period => period.set === activeSet.value) || [];
});

// Track cumulative game scores
const cumulativeScores = ref([]);

// Update cumulative scores based on the selected set
const updateCumulativeScores = () => {
  if (!matchPeriodsForSet.value) return;

  let home = 0;
  let away = 0;
  cumulativeScores.value = matchPeriodsForSet.value.map(period => {
    if (period.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantOne]) {
      home++;
    } else if (period.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantTwo]) {
      away++;
    }
    return { home, away, period };
  });
};

// Watch for changes in `activeSet` and update cumulative scores
watch(activeSet, updateCumulativeScores);

// Ensure the component initializes properly
watch(sets, (newSets) => {
  if (newSets.length > 0 && activeSet.value === null) {
    activeSet.value = newSets[0]; // Set the first set as active
  }
});

// Initialize cumulative scores for the first set
onMounted(() => {
  if (sets.value.length > 0) {
    activeSet.value = sets.value[0];
    updateCumulativeScores();
  }
});

// Helper function to determine the winner
const isGameWinner = (period: MatchPeriodOutputModel, side: 'home' | 'away') => {
  return (side === 'home' && period.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantOne]) ||
         (side === 'away' && period.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantTwo]);
};

// Helper function to get the server icon
const getServerIcon = (period: MatchPeriodOutputModel, side: 'home' | 'away') => {
  return period.server === (side === 'home' ? EventActor[EventActor.Home] : EventActor[EventActor.Away]) ? '🎾' : '';
};

// Helper function to get points for a game
const getPointsForGame = (period: MatchPeriodOutputModel) => {
  return period.scores
    .sort((a, b) => a.periodPointNumber - b.periodPointNumber)
    .filter(score => !score.isWinningPoint)
    .map(score => `${score.participant1Points}:${score.participant2Points}`)
    .join(', ');
};
const setActiveSet = (newActiveSet: number) => {
  activeSet.value = newActiveSet;
}
</script>

<template>
  <div class="point-by-point">
    <!-- Check if sets are available -->
    <div v-if="sets.length > 0">
      <!-- Set Selection Buttons -->
      <div class="buttons is-centered">
        <button
          v-for="set in sets"
          :key="'set-button-' + set"
          class="button"
          :class="{ 'is-active': set === activeSet }"
           @click="setActiveSet(set)"
        >
          Set {{ set }}
        </button>
      </div>

      <!-- Centered Title -->
      <p class="title is-4 has-text-centered">POINT BY POINT - SET {{ activeSet }}</p>

      <!-- Points by Game -->
      <div v-for="(score, index) in cumulativeScores" :key="'score-' + index">
        <!-- Game Row -->
        <div class="game-info">
          
          <p class="has-text-centered">
            <span :class="{ 'has-text-weight-bold': isGameWinner(score.period, 'home')}">
              {{ getServerIcon(score.period, 'home') }} {{ score.home }}
            </span>
            -
            <span :class="{ 'has-text-weight-bold': isGameWinner(score.period, 'away')}">
              {{ score.away }} {{ getServerIcon(score.period, 'away') }}
            </span>
          </p>
        </div>

        <!-- Points Row -->
        <div class="points-info">
          <p class="has-text-centered">{{ getPointsForGame(score.period) }}</p>
        </div>

        <hr />
      </div>
    </div>

    <!-- Handle cases where no sets are available -->
    <p v-else class="has-text-centered has-text-grey">No data available for this match.</p>
  </div>
</template>

<style scoped>
.point-by-point {
  margin-top: 20px;
  padding: 10px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
}

.buttons.is-centered {
  margin-bottom: 20px;
}

.button.is-active {
  background-color: #00d1b2;
  color: white;
  border-color: #00d1b2;
}

.game-info {
  margin-bottom: 10px;
}

.points-info {
  margin-bottom: 20px;
  font-size: 1rem;
  color: #4a4a4a;
}

hr {
  margin-top: 10px;
  margin-bottom: 10px;
}

.score-entry-left {
  padding-left: 1%;
}
.score-entry-right {
  padding-right: 1%;
}
</style>
