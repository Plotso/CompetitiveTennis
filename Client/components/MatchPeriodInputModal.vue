<script setup lang="ts">
import { Result, MatchPeriodOutputModel, MatchPeriodInputModel, MatchOutcomeInputModel, EventStatus, EventActor, MatchOutcome, ScoreInputModel } from "@/types"
import { useAuthStore } from "~/stores/auth"
import { storeToRefs } from 'pinia';
const authStore = useAuthStore();
const config = useRuntimeConfig();
const { user } = storeToRefs(authStore);

const emit = defineEmits(['close'])

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

const includeScores = ref(false)
const matchPeriods = props.existingMatchPeriods ? ref(props.existingMatchPeriods) : ref<MatchPeriodInputModel[]>([
  { set: 1, game: 1, status: EventStatus.NotStarted, matchId: props.matchId, server: EventActor.Unknown, winner: MatchOutcome.Unknown, isTiebreak: false, scores: [] }
]);

const selectedSet = ref(matchPeriods ? 1 : 0); // Index of currently selected set
const selectedGame = ref(matchPeriods ? 1 : 0);

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

const hasInvalidScores = computed(() => {
  return matchPeriods.value.some(period =>
    period.scores.some(score => !isScoreValid(score.participant1Points) || !isScoreValid(score.participant2Points))
  );
});

const addSet = () => {
  matchPeriods.value.push({
    set: selectedSet.value + 1,
    game: 1,
    matchId: props.matchId,
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
  var pointsInPeriod = matchPeriods.value[periodIndex].scores.length;
  matchPeriods.value[periodIndex].scores.push({
    periodPointNumber: pointsInPeriod + 1,
    participant1Points: '0',
    participant2Points: '0',
    pointWinner: MatchOutcome.Unknown
  })
}


const addPeriod = () => {
  matchPeriods.value.push({
    set: selectedSet.value,
    game: matchPeriods.value.length ? matchPeriods.value[matchPeriods.value.length - 1].game + 1 : 1,
    matchId: props.matchId,
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
    matchId: props.matchId,
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
const addMatchPeriods = async () => {
  const participantInput: MatchOutcomeInputModel = {
    outcomeCondition: null,
    matchPeriods: matchPeriods.value
  }

  console.log('addMatchPeriods called')

  try {
    const response = await fetch(`${config.public.tournamentsBase}/Matches/AddPeriodInfo/${props.matchId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${authStore.token}`
      },
      body: JSON.stringify(matchPeriods.value),
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
        <button class="delete" aria-label="close" @click="close"></button>
      </header>
      <div class="notification is-danger" v-if="showErrorNotification">
        <button class="delete" @click="hideErrorNotification"></button>
        {{ errorNotification }}
      </div>
      <section class="modal-card-body">
        <form @submit.prevent="">

          <div class="field">
            <label class="switch-label">
              <input v-model="includeScores" type="checkbox" @click="includeScores = !includeScores"
                class="switch-checkbox" />
              <span class="switch-slider"></span>
              <span class="switch-text">Include Scores</span>
            </label>
          </div>

          <div>

            <div class="tabs is-boxed">
              <ul>
                <li v-for="(setNumber, setIndex) in setsForMatch" :key="setIndex"
                  :class="{ 'is-active': selectedSet === setNumber }">
                  <a @click="selectedSet = setNumber" class="custom-link has-text-weight-semibold">Set {{ setNumber
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
                <h3 class="title is-5">Details for Game {{ gameInfo.game }}</h3>
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
                    <div class="field">
                      <select v-model="matchPeriods[getPeriodIndex].status" required>
                        <option :value="EventStatus.NotStarted">Not Started</option>
                        <option :value="EventStatus.InProgress">In Progress</option>
                        <option :value="EventStatus.Ended">Ended</option>
                      </select>
                    </div>

                    <!-- Server Select -->
                    <div class="field">
                      <select v-model="matchPeriods[getPeriodIndex].server"
                        :disabled="selectedGame > 1 && matchPeriods[getPeriodIndex].server !== EventActor.Unknown"
                        required>
                        <option :value="EventActor.Unknown">Unknown</option>
                        <option :value="EventActor.Home">{{ homeParticipantName ?? "Home" }}</option>
                        <option :value="EventActor.Away">{{ awayParticipantName ?? "Away" }}</option>
                      </select>
                    </div>

                    <!-- Winner Select -->
                    <div class="field">
                      <select v-model="matchPeriods[getPeriodIndex].winner" required>
                        <option :value="MatchOutcome.Unknown">Unknown</option>
                        <option :value="MatchOutcome.Participant1Won">{{ homeParticipantName ?? "Home" }}</option>
                        <option :value="MatchOutcome.Participant2Won">{{ awayParticipantName ?? "Away" }}</option>
                      </select>
                    </div>

                    <!-- Is Tiebreak Checkbox -->
                    <div class="field">
                      <input v-model="matchPeriods[getPeriodIndex].isTiebreak" type="checkbox" />
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
                    <div class="control buttons is-centered">
                      <button @click="addScore(selectedSet, selectedGame)" class="button is-rounded">Add Score for Set
                        {{ selectedSet }}, Game {{ selectedGame }}</button>
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
                :disabled="matchPeriods.length == 0 || hasInvalidScores" @click="addMatchPeriods">Add
                MatchPeriods</button>
            </div>
          </div>
        </form>
      </section>
      <footer class="modal-card-foot">
        <button class="button" @click="close">Cancel</button>
      </footer>
    </div>
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
</style>