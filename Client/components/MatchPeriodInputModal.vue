<script setup lang="ts">
import { Result, MatchPeriodOutputModel, MatchPeriodInputModel, MatchOutcomeInputModel, EventStatus, EventActor, MatchOutcome, MatchPeriodOutcome, ScoreInputModel, ScoreShortOutputModel } from "@/types"
import { useAuthStore } from "~/stores/auth"
import { storeToRefs } from 'pinia';
import { useRouter } from 'vue-router';
const authStore = useAuthStore();
const config = useRuntimeConfig();
const { user } = storeToRefs(authStore);

const emit = defineEmits(['close'])
const router = useRouter();

const props = defineProps({
  isOpen: Boolean,
  matchId: Number,
  existingMatchPeriods: {
    type: [Array] as PropType<MatchPeriodOutputModel[]>,
    required: true,
    default: []
  },
  homeParticipantName: String,
  awayParticipantName: String
});

const close = () => {
  emit('close');
};
const errorNotification = ref("")
const showErrorNotification = ref(false)

const hideErrorNotification = () => {
  showErrorNotification.value = false;
}


const isUnauthorizedModalOpen = ref(false);
const isConfirmationModalOpen = ref(false);
const showDeleteButton = ref(false);

const isInitialScoreInput = props.existingMatchPeriods == null || props.existingMatchPeriods.length == 0 ? true : false;

const includeScores = ref(true)
const convertToEnumValues = (period) => ({
  ...period,
  status: EventStatus[period.status], // Convert to number for enum compatibility
  server: EventActor[period.server],
  winner: MatchPeriodOutcome[period.winner],
  scores: period.scores.map((score: ScoreShortOutputModel) => ({
    ...score,
    pointWinner: MatchPeriodOutcome[score.pointWinner]
  }))
});

const matchPeriods = props.existingMatchPeriods && props.existingMatchPeriods.length > 0 ? ref(props.existingMatchPeriods.map(convertToEnumValues)) : ref<MatchPeriodInputModel[]>([
  { set: 1, game: 1, status: EventStatus.NotStarted, server: EventActor.Unknown, winner: MatchOutcome.Unknown, isTiebreak: false, scores: [] }
]);


const matchWinner = ref<MatchOutcome|null>(null); // Stores the winner (Participant1 or Participant2)
const selectedSet = ref(matchPeriods ? 1 : 0); // Index of currently selected set
const selectedGame = ref(matchPeriods ? 1 : 0);

const matchWinnerName = computed(() => {
  if(matchWinner.value == null)
    return "Unknown";
  if(matchWinner.value === MatchOutcome.Participant1Won)
    return props.homeParticipantName;
  if(matchWinner.value === MatchOutcome.Participant2Won)
    return props.awayParticipantName;
  return "Tie";
})

// Computed property to filter sets
const setsForMatch = computed(() => {
  // Extract all set numbers and remove duplicates using Set
  const sets = matchPeriods.value.map(period => period.set);
  return [...new Set(sets)]; // Convert Set back to an array
});

const getPeriodIndex = computed(() => {
  return matchPeriods.value.findIndex(period => period.set === selectedSet.value && period.game === selectedGame.value);
});

// Computed property to filter games by the selected set
const gamesForSelectedSet = computed(() => {
  return matchPeriods.value
    .filter(period => period.set === selectedSet.value) // Filter by the currently selected set
    .map(period => ({
      game: period.game,
      status: period.status,
      server: period.server,
      winner: period.winner,
      isTiebreak: period.isTiebreak,
      scores: period.scores,
    }));
});


const canAddDeuceInSelectedSetAndGame = computed(() => {
  // Find the current period for the selected set and game
  const currentPeriod = matchPeriods.value.find(period => 
    period.set === selectedSet.value && period.game === selectedGame.value
  );

  if (!currentPeriod) return false; // No period found for the current selection

  // Get the last score in the current period
  const lastScore = currentPeriod.scores[currentPeriod.scores.length - 1];
  if (!lastScore) return false; // No scores yet in the period

  const { participant1Points, participant2Points } = lastScore;

  // If this is a tiebreak, check if the difference is 1
  if (currentPeriod.isTiebreak) {
    const p1Points = parseInt(participant1Points);
    const p2Points = parseInt(participant2Points);

    return Math.abs(p1Points - p2Points) === 1;
  }

  // For regular play, check if the score is eligible for deuce
  const validScores = ['0', '15', '30', '40', 'Adv'];
  const p1Index = validScores.indexOf(participant1Points);
  const p2Index = validScores.indexOf(participant2Points);

  // Ensure both scores are valid and check if the difference is 1
  return p1Index !== -1 && p2Index !== -1 && Math.abs(p1Index - p2Index) === 1;
});

const canAddLoveInSelectedSetAndGame = computed(() => {
  const currentPeriod = matchPeriods.value.find(
    (period) => period.set === selectedSet.value && period.game === selectedGame.value
  );

  if (!currentPeriod) return false; // No period found for the current selection

  const lastScore = currentPeriod.scores[currentPeriod.scores.length - 1];
  if (!lastScore) return false; // No scores yet in the period

  // Check if exactly one of the participants has a score of 0
  const isParticipant1AtLove = lastScore.participant1Points === '0';
  const isParticipant2AtLove = lastScore.participant2Points === '0';

  return (isParticipant1AtLove && !isParticipant2AtLove) || (!isParticipant1AtLove && isParticipant2AtLove);
});

const canNextPointBeWinner = computed(() => {
  const currentPeriod = matchPeriods.value.find(
    (period) => period.set === selectedSet.value && period.game === selectedGame.value
  );

  if (!currentPeriod) return false; // No period found for the current selection

  const lastScore = currentPeriod.scores[currentPeriod.scores.length - 1];
  if (!lastScore) return false; // No scores yet in the period

  // If it's not a tiebreak, check if one participant has 40 points and the scores are unequal
  if (!currentPeriod.isTiebreak) {
    return (
      (lastScore.participant1Points === '40' || lastScore.participant2Points === '40') &&
      lastScore.participant1Points !== lastScore.participant2Points
    );
  }

  // If it's a tiebreak, check if one participant has at least 6 points with a point difference of 1 or more
  const p1Points = parseInt(lastScore.participant1Points);
  const p2Points = parseInt(lastScore.participant2Points);

  return (p1Points >= 6 || p2Points >= 6) && Math.abs(p1Points - p2Points) >= 1;
});

const hasWinnerInPeriod = computed(() => {
  const currentPeriod = matchPeriods.value.find(
    (period) => period.set === selectedSet.value && period.game === selectedGame.value
  );

  if (!currentPeriod) return false; // No period found for the current selection

  const lastScore = currentPeriod.scores[currentPeriod.scores.length - 1];
  if (!lastScore) return false; // No scores yet in the period
  return lastScore.isWinningPoint;
});



const hasInvalidScores = computed(() => {
  return matchPeriods.value.some(period =>
    period.scores.some(score => !isScoreValid(score.participant1Points) || !isScoreValid(score.participant2Points))
  );
});

const areAllPeriodsEnded = computed(() => {
  return matchPeriods.value.every(period => period.status === EventStatus.Ended);
});


const isSettingsDropdownOpen = ref(false);

const toggleSettingsDropdown = () => {
  isSettingsDropdownOpen.value = !isSettingsDropdownOpen.value;
};
const isMatchWinnerModalOpen = ref(false);
const openMatchWinnerModal = () => {
  if (areAllPeriodsEnded.value) {
    // Calculate the winner based on scores
    matchWinner.value = calculateMatchWinner();
    isMatchWinnerModalOpen.value = true;
  }
  else {
    saveMatchPeriods(false)
  }
};

const handleConfirmWinner = () => {
  saveMatchPeriods(true); // Save scores and finalize the match
};

const handleSaveScores = () => {
  saveMatchPeriods(false); // Save scores without finalizing the match
};


const closeMatchWinnerModal = () => {
  isMatchWinnerModalOpen.value = false;
};

const calculateMatchWinner = (): MatchOutcome => {
  const setsWon = { Participant1: 0, Participant2: 0 };

  // Iterate through each set
  setsForMatch.value.forEach(setNumber => {
    let gamesWonInSet = { Participant1: 0, Participant2: 0 };

    // Filter games for the current set
    const gamesInSet = matchPeriods.value.filter(period => period.set === setNumber);

    // Count games won by each participant in the set
    gamesInSet.forEach(game => {
      if (game.winner === MatchOutcome.Participant1Won) {
        gamesWonInSet.Participant1++;
      } else if (game.winner === MatchOutcome.Participant2Won) {
        gamesWonInSet.Participant2++;
      }
    });

    // Determine the set winner
    if (gamesWonInSet.Participant1 > gamesWonInSet.Participant2) {
      setsWon.Participant1++;
    } else if (gamesWonInSet.Participant2 > gamesWonInSet.Participant1) {
      setsWon.Participant2++;
    }
  });

  // Determine the match winner based on sets won
  if (setsWon.Participant1 > setsWon.Participant2) {
    return MatchOutcome.Participant1Won;
  } else if (setsWon.Participant2 > setsWon.Participant1) {
    return MatchOutcome.Participant2Won;
  } else {
    return MatchOutcome.Draw; // Handle tie if applicable
  }
};


const addSet = () => {
  matchPeriods.value.push({
    set: selectedSet.value + 1,
    game: 1,
    status: EventStatus.NotStarted,
    winner: MatchOutcome.Unknown,
    server: EventActor.Unknown,
    isTiebreak: false,
    scores: [],
  });
  selectedSet.value = matchPeriods.value.length - 1; // Select the newly added set
  selectedGame.value = 1;
};

const addScore = (set: number, game: number) => {
  //matchPeriods.value.find(mp => mp.set == set && mp.game == game)?.scores.push({participant1Points: '0', participant2Points:'0'})
  //ToDo: Add logic here
  var periodIndex = matchPeriods.value.findIndex(period => period.set === set && period.game === game);
  if (periodIndex === -1) return;
  matchPeriods.value[periodIndex].status = EventStatus.InProgress;
  var pointsInPeriod = matchPeriods.value[periodIndex].scores.length;
  if(pointsInPeriod == 0){    
    matchPeriods.value[periodIndex].scores.push({
      periodPointNumber: pointsInPeriod + 1,
      participant1Points: '0',
      participant2Points: '0',
      pointWinner: MatchOutcome.Unknown
    })
  }
  else{
    
    var scoresInPeriod = matchPeriods.value[periodIndex].scores;
    var sortedScores = scoresInPeriod.sort(s => s.periodPointNumber);
    var lastScore = sortedScores[sortedScores.length - 1];
    matchPeriods.value[periodIndex].scores.push({
      periodPointNumber: pointsInPeriod + 1,
      participant1Points: lastScore.participant1Points,
      participant2Points: lastScore.participant2Points,
      pointWinner: MatchOutcome.Unknown
    })
  }
}

const addDeuceScore = (set: number, game: number) => {
  var periodIndex = matchPeriods.value.findIndex(period => period.set === set && period.game === game);
  if (periodIndex === -1) return;
  matchPeriods.value[periodIndex].status = EventStatus.InProgress;
  var scoresInPeriod = matchPeriods.value[periodIndex].scores;
  var pointsInPeriod = scoresInPeriod.length;
  if(pointsInPeriod == 0)
      return addScore(set, game);

  var sortedScores = scoresInPeriod.sort(s => s.periodPointNumber);
  var lastScore = sortedScores[sortedScores.length - 1];
  var lastPointWinner = lastScore.pointWinner;
  var nextPointWinner = lastPointWinner === MatchOutcome.Participant1Won 
    ? MatchOutcome.Participant2Won 
    : lastPointWinner === MatchOutcome.Participant2Won 
      ? MatchOutcome.Participant1Won 
      : MatchOutcome.Unknown;

  console.log(lastScore.participant1Points);
  console.log(lastScore.participant2Points);
  var deuceScore = getTheHigherScoreWithoutAdv(lastScore.participant1Points, lastScore.participant2Points);
  console.log(deuceScore);
  
  matchPeriods.value[periodIndex].scores.push({
    periodPointNumber: pointsInPeriod + 1,
    participant1Points: deuceScore,
    participant2Points: deuceScore,
    pointWinner: nextPointWinner
  })

}

const getTheHigherScoreWithoutAdv = (participant1Points: string, participant2Points: string) =>
{
    // Return '40' if either participant's score is 'Adv'
    if(participant1Points == 'Adv' || participant1Points == 'Adv')
      return '40';      
    
    // Convert scores to numbers and return the higher score
    return parseInt(participant1Points) >= parseInt(participant2Points) ? participant1Points : participant2Points;
}

const addLoveScore = (set: number, game: number) => {
  const periodIndex = matchPeriods.value.findIndex((period) => period.set === set && period.game === game);
  if (periodIndex === -1) return;
  matchPeriods.value[periodIndex].status = EventStatus.InProgress;

  const currentPeriod = matchPeriods.value[periodIndex];
  const scoresInPeriod = currentPeriod.scores;
  if(scoresInPeriod.length == 0)
    return addScore(set, game)
  const lastScore = scoresInPeriod[scoresInPeriod.length - 1];

  // Determine the next score for each participant
  const nextParticipant1Points = getNextScore(lastScore.participant1Points, currentPeriod.isTiebreak);
  const nextParticipant2Points = getNextScore(lastScore.participant2Points, currentPeriod.isTiebreak);

  // Determine which participant is at 0 to increase their score
  const newScore = {
    periodPointNumber: scoresInPeriod.length + 1,
    participant1Points: lastScore.participant1Points === '0' ? lastScore.participant1Points : nextParticipant1Points,
    participant2Points: lastScore.participant2Points === '0' ? lastScore.participant2Points : nextParticipant2Points,
    pointWinner: lastScore.participant1Points === '0' ? MatchOutcome.Participant2Won : MatchOutcome.Participant1Won,
  };

  console.log(newScore);
  // Add the new score to the period
  scoresInPeriod.push(newScore);
};

// Helper function to determine the next score
const getNextScore = (currentScore: string, isTiebreak: boolean) => {
  if (isTiebreak) {
    // For tiebreak, increment numerically
    return (parseInt(currentScore) + 1).toString();
  } else {
    // For regular scoring, follow tennis points progression
    const regularPoints = ['0', '15', '30', '40', 'Adv'];
    const currentIndex = regularPoints.indexOf(currentScore);
    return currentIndex < regularPoints.length - 1 ? regularPoints[currentIndex + 1] : 'Adv';
  }
};

const addWinningPoint = (set: number, game: number) => {
  const periodIndex = matchPeriods.value.findIndex((period) => period.set === set && period.game === game);
  if (periodIndex === -1) return;

  const currentPeriod = matchPeriods.value[periodIndex];
  const scoresInPeriod = currentPeriod.scores;
  if(scoresInPeriod.length == 0)
    return addScore(set, game)
  const lastScore = scoresInPeriod[scoresInPeriod.length - 1];

  // Determine which participant has the higher score, accounting for "Adv"
  const p1ScoreValue = getScoreValue(lastScore.participant1Points);
  const p2ScoreValue = getScoreValue(lastScore.participant2Points);
  if(p1ScoreValue == p2ScoreValue){    
    errorNotification.value = `Cannot add winning point if previously both players had equal points.`
    showErrorNotification.value = true
    return;
  }
  const isParticipant1Winning = p1ScoreValue > p2ScoreValue;

  // Add the winning point with `isWinningPoint` set to true
  const newScore = {
    periodPointNumber: scoresInPeriod.length + 1,
    participant1Points: lastScore.participant1Points,
    participant2Points: lastScore.participant2Points,
    pointWinner: isParticipant1Winning ? MatchOutcome.Participant1Won : MatchOutcome.Participant2Won,
    isWinningPoint: true,
  };

  
  matchPeriods.value[periodIndex].status = EventStatus.Ended;  
  matchPeriods.value[periodIndex].winner = isParticipant1Winning ? MatchOutcome.Participant1Won : MatchOutcome.Participant2Won;

  // Push the winning score with the same points but marked as a winning point
  scoresInPeriod.push(newScore);
};

const removeWinningPoint = (set: number, game: number) => {
  const periodIndex = matchPeriods.value.findIndex((period) => period.set === set && period.game === game);
  if (periodIndex === -1) return; // No period found for the selected set and game

  const scoresInPeriod = matchPeriods.value[periodIndex].scores;
  
  // Ensure there's at least one score to check
  if (scoresInPeriod.length === 0) return;

  // Get the last score in the array
  const lastScore = scoresInPeriod[scoresInPeriod.length - 1];

  // Remove the last score only if it is marked as a winning point
  if (lastScore.isWinningPoint) {
    scoresInPeriod.pop(); // Remove the last element    
    matchPeriods.value[periodIndex].status = EventStatus.InProgress;  
    matchPeriods.value[periodIndex].winner = MatchOutcome.Unknown;
  }
};


// Helper function to get a numerical comparison value for scores
const getScoreValue = (score: string): number => {
  if (score === "Adv") return 50; // Treat "Adv" as higher than "40"
  return parseInt(score) || 0; // Convert score to number, default to 0 if invalid
};





const addPeriod = () => {
  matchPeriods.value.push({
    set: selectedSet.value,
    game: matchPeriods.value.length ? matchPeriods.value[matchPeriods.value.length - 1].game + 1 : 1,
    status: EventStatus.NotStarted,
    winner: MatchOutcome.Unknown,
    server: EventActor.Unknown,
    isTiebreak: false,
    scores: [],
  });
}


const addGamePeriod = () => {
  let server = EventActor.Unknown;
  if (gamesForSelectedSet.value.length && gamesForSelectedSet.value.length > 0) {
    var previousGameInAnonymousArrayIndex = gamesForSelectedSet.value.length - 1;
    var previousGameServer = gamesForSelectedSet.value[previousGameInAnonymousArrayIndex].server;
    if (previousGameServer == EventActor.Home)
      server = EventActor.Away;
    if (previousGameServer == EventActor.Away)
      server = EventActor.Home;
  }
  //ToDo: Get server of previous game
  matchPeriods.value.push({
    set: selectedSet.value,
    game: gamesForSelectedSet.value.length + 1,
    status: EventStatus.NotStarted,
    winner: MatchOutcome.Unknown,
    server: server,
    isTiebreak: false,
    scores: [],
  });
}


const isScoreValid = (scoreValue: string) => {
  const validScores = ['0', '15', '30', '40', 'Adv'];

  return validScores.includes(scoreValue)
};



const openConfirmationModal = () => {
  isConfirmationModalOpen.value = true;
};
const closeConfirmationModal = () => {
  isConfirmationModalOpen.value = false;
};


const closeUnathorizedModal = () => {
  isUnauthorizedModalOpen.value = false;
};

const deletePeriodsAfterSelected = () => {
  console.log('Hello');
  if(selectedSet.value == 1 && selectedGame.value == 1) return;

  var selectedSetOriginalValue = selectedSet.value;
  var selectedGameOriginalValue = selectedGame.value;

  if(selectedGame.value >= 1) {
    selectedGame.value = selectedGameOriginalValue - 1;
  }
  else{
    selectedSet.value = selectedSetOriginalValue - 1;
    selectedGame.value = 1; //ToDo: Decide whether to show first or last game of previous set
  }
  matchPeriods.value = matchPeriods.value.filter((period) => {
    // Keep periods that are either before the selected set or, if within the selected set, before the selected game
    return period.set < selectedSetOriginalValue || (period.set === selectedSetOriginalValue && period.game < selectedGameOriginalValue);
  });
};

const deleteMathPeriodsAfterSetAndGameInclusive = async () => {
  if(isInitialScoreInput){
    deletePeriodsAfterSelected();
    closeConfirmationModal();
    return;
  }
  try {
    const response = await fetch(`${config.public.tournamentsBase}/Matches/DeleteMatchPeriodsAfterSetAndGameInclusive/${props.matchId}?set=${selectedSet.value}&game=${selectedGame.value}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        'Authorization' : `Bearer ${authStore.token}`
      }
    });

    if (response.ok) {       
      await refreshNuxtData();
      router.push(`/matches/${props.matchId}`);
    } else {
      if(response.status == 401){
        isUnauthorizedModalOpen.value = true;
      }
      console.error(`Failed to delete match periods for match ${props.matchId} after set ${selectedSet.value} and game ${selectedGame.value}. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while delete scores for match', error);
  }
};

const saveMatchPeriods = async (isEnded = false) => {

  console.log('addMatchPeriods called')

  try {
    const response = await fetch(`${config.public.tournamentsBase}/Matches/AddPeriodInfo/${props.matchId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${authStore.token}`
      },
      body: JSON.stringify({
        matchPeriods: matchPeriods.value,
        isEnded, // Add the isFinished property
      }),
    });

    if (response.ok) {
      await refreshNuxtData();
      close();
    } else {
      if (response.status == 401) {
        errorNotification.value = `User not authorized to enter match score`
      }
      else {
        errorNotification.value = `An error occurred during match period input information for match ${props.matchId}. Code: ${response.status}`
      }
      showErrorNotification.value = true;
      console.error(`Failed to AddPeriodInfo for match ${props.matchId}. Status: ${response.status}`);
    }
  } catch (error) {
    console.error('An error occurred while adding doubles participants', error);
  };
}
</script>


<template>
  <div class="modal" :class="{ 'is-active': isOpen }">
    <div class="modal-background"></div>
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">Add Match Periods Info</p>
        <!-- Configuration Icon and Dropdown -->
        <div class="settings-wrapper">
          <span class="icon settings-icon" @click="toggleSettingsDropdown">
            <font-awesome-icon icon="fas fa-cog"/>
          </span>
          <div v-if="isSettingsDropdownOpen" class="settings-dropdown">
            <label class="switch-label">
              <input v-model="includeScores" type="checkbox" class="switch-checkbox" />
              <span class="switch-slider"></span>
              <span class="switch-text">Include Scores</span>
            </label>
            <label class="switch-label">
              <input v-model="showDeleteButton" type="checkbox" class="switch-checkbox" />
              <span class="switch-slider"></span>
              <span class="switch-text">Delete Button</span>
            </label>
          </div>
        </div>
        <button class="delete" aria-label="close" @click="close"></button>
      </header>
      <div class="notification is-danger" v-if="showErrorNotification">
        <button class="delete" @click="hideErrorNotification"></button>
        {{ errorNotification }}
      </div>
      <section class="modal-card-body">
        <form @submit.prevent="">

          <div>

            <div class="tabs is-boxed">
              <ul>
                <li v-for="(setNumber, setIndex) in setsForMatch" :key="setIndex"
                  :class="{ 'is-active': selectedSet === setNumber }">
                  <a @click="selectedSet = setNumber, selectedGame = 1" class="custom-link has-text-weight-semibold">Set {{ setNumber
                    }}</a>
                </li>
              </ul>
              <button class="button is-small ml-2 is-primary  is-rounded" @click="addSet">+</button>
            </div>

            <!-- Inner Tabs for Games inside the selected Set -->
            <div v-for="(setNumber, setIndex) in setsForMatch" :key="setIndex" v-show="selectedSet === setNumber" v-if="setsForMatch">
              <div class="tabs is-small is-boxed">
                <ul>
                  <li v-for="(gameInfo, gameIndex) in gamesForSelectedSet" :key="gameIndex"
                    :class="{ 'is-active': selectedGame === gameInfo.game }">
                    <a @click="selectedGame = gameInfo.game" class="custom-link has-text-weight-semibold">Game {{
                      gameInfo.game }}</a>
                  </li>
                </ul>
                <button class="button is-small ml-2  is-primary is-rounded" @click="addGamePeriod()">+</button>
              </div>

              <!-- Game Details for the selected game -->
              <div v-for="(gameInfo, gameIndex) in gamesForSelectedSet" :key="gameIndex"
                v-show="selectedGame === gameInfo.game" v-if="gamesForSelectedSet">
                <div>
                  <h3 class="title is-5">Details for Game {{ gameInfo.game }}</h3>
                  
                        <div v-if="matchPeriods">
                          
                          <button v-if="showDeleteButton" @click="openConfirmationModal" class="button is-rounded is-small is-danger">Delete match periods & scores after Set
                            {{ selectedSet }}, Game {{ selectedGame }} inclusive</button>
                        </div>
                </div>
                <!-- Period Info -->
                <div class="input-group">
                  <!-- First row for labels -->
                  <div class="input-labels">
                    <div>Status</div>
                    <div>Server</div>
                    <div>Winner</div>
                    <div>Is Tiebreak</div>
                  </div>

                  <!-- Second row for inputs -->
                  <div class="input-fields">
                    <!-- Status Select -->
                    <div class="select field is-rounded has-custom-icon">
                      <select v-model="matchPeriods[getPeriodIndex].status" required>
                        <option :value="EventStatus.NotStarted">Not Started</option>
                        <option :value="EventStatus.InProgress">In Progress</option>
                        <option :value="EventStatus.Ended">Ended</option>
                      </select>
                    </div>

                    <!-- Server Select -->
                    <div class="select field is-rounded has-custom-icon">
                      <select v-model="matchPeriods[getPeriodIndex].server"
                        :disabled="selectedGame > 1 && matchPeriods[getPeriodIndex].server !== EventActor.Unknown"
                        required>
                        <option :value="EventActor.Unknown">Unknown</option>
                        <option :value="EventActor.Home">{{ homeParticipantName ?? "Home" }}</option>
                        <option :value="EventActor.Away">{{ awayParticipantName ?? "Away" }}</option>
                      </select>
                    </div>

                    <!-- Winner Select -->
                    <div class="select field is-rounded has-custom-icon">
                      <select v-model="matchPeriods[getPeriodIndex].winner" required>
                        <option :value="MatchOutcome.Unknown">Unknown</option>
                        <option :value="MatchOutcome.Participant1Won">{{ homeParticipantName ?? "Home" }}</option>
                        <option :value="MatchOutcome.Participant2Won">{{ awayParticipantName ?? "Away" }}</option>
                      </select>
                    </div>

                    <!-- Is Tiebreak Checkbox -->
                    <div class="field">
                      <input v-model="matchPeriods[getPeriodIndex].isTiebreak" type="checkbox" class="modern-checkbox"/>
                    </div>
                  </div>
                </div>


                <!-- Score Info -->
                <div class="field">

                  <div v-show="includeScores">
                    <div v-for="(score, scoreIndex) in gameInfo.scores" :key="scoreIndex" class="score-row">
                      <div class="score-points-wrapper">
                        <div class="score-points">
                          <!-- Input for Participant 1 Points with validation -->
                          <input v-model="score.participant1Points" class="participant-points"
                            :class="{ 'invalid-score': !isScoreValid(score.participant1Points) }"
                            :placeholder="homeParticipantName + 'Points'" />

                          <span class="dash">-</span>

                          <!-- Input for Participant 2 Points with validation -->
                          <input v-model="score.participant2Points" class="participant-points"
                            :class="{ 'invalid-score': !isScoreValid(score.participant2Points) }"
                            :placeholder="awayParticipantName + 'Points'" />
                        </div>

                        <!-- Point winner dropdown -->
                        <div class="point-winner">
                          <label>Point Winner:</label>
                          <select v-model="score.pointWinner" required>
                            <option :value="MatchOutcome.Unknown">Unknown</option>
                            <option :value="MatchOutcome.Participant1Won">{{ homeParticipantName ?? "Home" }}</option>
                            <option :value="MatchOutcome.Participant2Won">{{ awayParticipantName ?? "Away" }}</option>
                          </select>
                        </div>
                      </div>

                      <!-- Validation message -->
                      <div class="validation-message"
                        v-if="!isScoreValid(score.participant1Points) || !isScoreValid(score.participant2Points)">
                        * Invalid score in {{ !isScoreValid(score.participant1Points) ? homeParticipantName ?? "Home" :
                          awayParticipantName ?? "Away" }}! Please use 0, 15, 30, 40, or Adv.
                      </div>
                    </div>

                    <!-- Button to add score -->
                    <div v-if="!hasWinnerInPeriod">
                      <div class="control buttons is-centered">
                        <button v-if="canAddLoveInSelectedSetAndGame && !canNextPointBeWinner" @click="addLoveScore(selectedSet, selectedGame)" class="button is-rounded">Love</button>
                        <button v-if="canAddDeuceInSelectedSetAndGame" @click="addDeuceScore(selectedSet, selectedGame)" class="button is-rounded">Deuce</button>
                      </div>
                      <div class="control buttons is-centered" >
                        <button v-if="canNextPointBeWinner" @click="addWinningPoint(selectedSet, selectedGame)" class="button is-rounded is-success">
                          Game
                        </button>
                        <button @click="addScore(selectedSet, selectedGame)" class="button is-rounded" v-if="!canNextPointBeWinner || !canAddDeuceInSelectedSetAndGame">Add Score for Set
                          {{ selectedSet }}, Game {{ selectedGame }}</button>
                      </div>
                    </div>
                    <div v-if="!isInitialScoreInput && hasWinnerInPeriod">
                      <div class="control buttons is-centered">
                        <button @click="removeWinningPoint(selectedSet, selectedGame)" class="button is-rounded is-danger">
                          Remove Winning Point
                        </button>
                      </div>
                    </div>
                  </div>
                </div>

                <hr />
              </div>
              <div v-else class="control buttons is-centered">
                <button class="button is-secondary is-centered  is-rounded" @click="addGamePeriod()">Add Game Info for
                  Set {{ selectedSet }}</button>
              </div>
            </div>
            
            <div v-else class="control buttons is-centered">
                <button class="button is-secondary is-centered  is-rounded" @click="addGamePeriod()">Add new Set to begin with...</button>
                
                <br />
                <br />
                <br />
              </div>
          </div>
          <div class="field">
            <div class="control buttons is-centered">
              <button class="button is-primary is-rounded" type="submit"
                :disabled="matchPeriods.length == 0 || hasInvalidScores" @click="openMatchWinnerModal">Save MatchPeriods</button>
            </div>
          </div>
        </form>
      </section>
      <footer class="modal-card-foot">
        <button class="button" @click="close">Cancel</button>
      </footer>
    </div>
    
    <ConfirmationModal
      :isOpen="isConfirmationModalOpen"
      title="Confirmation"
      message="Are you sure you want to delete certain scores for the match?"
      @confirm="deleteMathPeriodsAfterSetAndGameInclusive"
      @close="closeConfirmationModal"
    />
    <MatchWinnerModal
  v-if="isMatchWinnerModalOpen"
  :isOpen="isMatchWinnerModalOpen"
  :title="'Match Winner Confirmation'"
  :message="`${matchWinnerName} would be the outcome of the match if submitted with current scores. If the match will have more points to be played/added, or the output of the game is an extraordinary case, please select 'Save Scores Without Finalizing'. This will allow to manually add winner later. If the winner seems wrongs, click Cancel and revise the scores.`"
  :winner="matchWinnerName"
  @confirm-winner="handleConfirmWinner"
  @save-scores="handleSaveScores"
  @close="closeMatchWinnerModal"
/>

    <DangerModal
:isOpen="isUnauthorizedModalOpen"
title="Unauthorized!"
message="You are not authorized to execute the desired action!"
@close="closeUnathorizedModal"
/>
  </div>
</template>

<style scoped>
.score-row {
  margin-bottom: 10px;
  padding: 0 20px;
  /* Add padding for spacing from the left */
}

.score-points-wrapper {
  display: flex;
  align-items: center;
  /* Aligns score inputs and point winner on the same row */
  justify-content: space-between;
}

.score-points {
  display: flex;
  align-items: center;
}

.participant-points {
  text-align: center;
  width: 60px;
  /* Increase width for visibility */
  margin: 0 10px;
  /* Adds some spacing between inputs and dash */
}

.dash {
  margin: 0 10px;
  font-size: 1.2rem;
}

.point-winner {
  margin-left: 20px;
}

.validation-message {
  color: red;
  font-size: 0.8rem;
  /* Smaller font size for subtlety */
  margin-top: 5px;
  /* Adds some space between the inputs and the message */
}

.invalid-score {
  border-color: red;
  background-color: #ffe6e6;
}

.input-group {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  /* Four equal columns */
  gap: 10px;
  /* Adds space between each column */
  margin-bottom: 20px;
  /* Adds space below the entire group */
}

.input-labels {
  display: contents;
  /* Allows it to align with the input fields */
}

.input-labels>div {
  font-weight: bold;
  /* Makes labels bold */
  text-align: center;
  /* Center-aligns the labels */
}

.input-fields {
  display: contents;
  /* Matches grid layout */
}

.field {
  display: flex;
  justify-content: center;
  /* Centers the input elements */
}

/* Container for the checkbox and label */
.switch-label {
  display: flex;
  align-items: center;
  cursor: pointer;
  margin-bottom: 20px;
  margin-right: 20px;
}

.switch-label-delete {
  display: flex;
  align-items: left;
  cursor: pointer;
  margin-bottom: 20px;
}


/* Hidden checkbox */
.switch-checkbox {
  display: none;
}

/* The slider (the switch) */
.switch-slider {
  position: relative;
  width: 50px;
  height: 25px;
  background-color: #ccc;
  border-radius: 25px;
  transition: background-color 0.3s;
}

/* The circle inside the slider */
.switch-slider::before {
  content: "";
  position: absolute;
  top: 3px;
  left: 3px;
  width: 19px;
  height: 19px;
  background-color: white;
  border-radius: 50%;
  transition: transform 0.3s;
}

/* When the checkbox is checked, the slider and circle move */
.switch-checkbox:checked+.switch-slider {
  background-color: #4caf50;
}

.switch-checkbox:checked+.switch-slider::before {
  transform: translateX(25px);
}

/* Text next to the switch */
.switch-text {
  margin-left: 10px;
  font-size: 1rem;
  font-weight: bold;
}

.settings-wrapper {
  position: relative;
  margin-left: auto;
}

.settings-icon {
  cursor: pointer;
}

.settings-dropdown {
  position: absolute;
  top: 100%;
  right: 0;
  background-color: white;
  border: 1px solid #ddd;
  border-radius: 5px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  padding: 10px;
  width: 200px;
  z-index: 1;
}

/* Additional styles for the switch component */
.switch-label {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

.switch-checkbox {
  display: none;
}

.switch-slider {
  position: relative;
  width: 40px;
  height: 20px;
  background-color: #ccc;
  border-radius: 20px;
  transition: background-color 0.3s;
}

.switch-slider::before {
  content: "";
  position: absolute;
  top: 2px;
  left: 2px;
  width: 16px;
  height: 16px;
  background-color: white;
  border-radius: 50%;
  transition: transform 0.3s;
}

.switch-checkbox:checked + .switch-slider {
  background-color: #4caf50;
}

.switch-checkbox:checked + .switch-slider::before {
  transform: translateX(20px);
}

.switch-text {
  margin-left: 10px;
  font-size: 1rem;
}

.modern-checkbox {
  width: 25px;
  height: 25px;
  border: 2px solid #ccc;
  border-radius: 50%;
  appearance: none;
  cursor: pointer;
  transition: all 0.3s ease;
}

.modern-checkbox:checked {
  background-color: #00d1b2;
  border-color: #00d1b2;
  position: relative;
}

.modern-checkbox:checked::after {
  content: '';
  position: absolute;
  width: 10px;
  height: 10px;
  background-color: white;
  border-radius: 50%;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

</style>