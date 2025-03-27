<script setup lang="ts">
import { ref } from 'vue';
import { TournamentType, Surface } from '~/types';

const props = defineProps<{
  selectedType: TournamentType | null;
  selectedHasPrize: boolean | null;
  selectedSurface: Surface | null;
  selectedDateFrom: string | null;
  selectedDateUntil: string | null;
  selectedIsIndoor: boolean | null;
  selectedCity: string | null;
}>();

const emit = defineEmits<{
  (e: 'applyFilters', filters: {
    selectedType: TournamentType | null;
    selectedHasPrize: boolean | null;
    selectedSurface: Surface | null;
    selectedDateFrom: string | null;
    selectedDateUntil: string | null;
    selectedIsIndoor: boolean | null;
    selectedCity: string | null;
  }): void;
  (e: 'resetFilters'): void;
  (e: 'toggleFilters', isVisible: boolean): void;
}>();

const selectedTypeInput = ref<TournamentType | null>(props.selectedType);
const selectedHasPrizeInput = ref<boolean | null>(props.selectedHasPrize);
const selectedSurfaceInput = ref<Surface | null>(props.selectedSurface);
const selectedDateFromInput = ref<string | null>(props.selectedDateFrom);
const selectedDateUntilInput = ref<string | null>(props.selectedDateUntil);
const selectedIsIndoorInput = ref<boolean | null>(props.selectedIsIndoor);
const selectedCityInput = ref<string | null>(props.selectedCity);

const showFilters = ref(false);

const toggleFilters = () => {
  showFilters.value = !showFilters.value;
  emit('toggleFilters', showFilters.value);
};

const applyFilters = () => {
  emit('applyFilters', {
    selectedType: selectedTypeInput.value,
    selectedHasPrize: selectedHasPrizeInput.value,
    selectedSurface: selectedSurfaceInput.value,
    selectedDateFrom: selectedDateFromInput.value,
    selectedDateUntil: selectedDateUntilInput.value,
    selectedIsIndoor: selectedIsIndoorInput.value,
    selectedCity: selectedCityInput.value,
  });
  showFilters.value = false; // Hide filters after applying
};

const resetFilters = () => {
  selectedTypeInput.value = null;
  selectedHasPrizeInput.value = null;
  selectedSurfaceInput.value = null;
  selectedDateFromInput.value = null;
  selectedDateUntilInput.value = null;
  selectedIsIndoorInput.value = null;
  selectedCityInput.value = null;
};

const surfaceValues = Object.values(Surface)
  .filter(value => typeof value === 'number') as Surface[];
</script>

<template>
  <div class="filters">
    <!-- Filters Button -->
    <div class="field has-addons has-addons-centered">
      <button class="button is-rounded is-primary" @click="toggleFilters">
        <span class="icon is-small">
          <font-awesome-icon icon="fa-solid fa-filter" />
        </span>
        <span>{{ showFilters ? 'Hide Filters' : 'Filters' }}</span>
      </button>
    </div>

    <!-- Filters Panel -->
    <div v-if="showFilters" class="box filters-panel">
      <div class="columns is-multiline is-centered">
        <!-- Type Filter -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">
              <font-awesome-icon icon="fa-solid fa-people-arrows" /> Type
            </label>
            <div class="control">
              <div class="select is-fullwidth is-rounded">
                <select v-model="selectedTypeInput">
                  <option :value="null">All Types</option>
                  <option :value="TournamentType.Singles">Singles</option>
                  <option :value="TournamentType.Doubles">Doubles</option>
                </select>
              </div>
            </div>
          </div>
        </div>

        <!-- Surface Filter -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">
              <font-awesome-icon icon="fa-solid fa-arrow-up-from-ground-water" /> Surface
            </label>
            <div class="control">
              <div class="select is-fullwidth is-rounded">
                <select v-model="selectedSurfaceInput">
                  <option :value="null">All Surfaces</option>
                  <option v-for="surface in surfaceValues" :key="surface" :value="surface">
                    {{ Surface[surface] }}
                  </option>
                </select>
              </div>
            </div>
          </div>
        </div>

        <!-- Date Range Filter (From) -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">
              <font-awesome-icon icon="fa-solid fa-calendar-days" /> Date From
            </label>
            <div class="control">
              <input class="input is-rounded" type="date" v-model="selectedDateFromInput">
            </div>
          </div>
        </div>

        <!-- Date Range Filter (Until) -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">
              <font-awesome-icon icon="fa-solid fa-calendar-days" /> Date Until
            </label>
            <div class="control">
              <input class="input is-rounded" type="date" v-model="selectedDateUntilInput">
            </div>
          </div>
        </div>

        <!-- City Filter -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">
              <font-awesome-icon icon="fa-solid fa-location-dot" /> City
            </label>
            <div class="control">
              <input class="input is-rounded" type="text" v-model="selectedCityInput" placeholder="Input desired city here...">
            </div>
          </div>
        </div>

        <!-- IsIndoor Filter -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">
              <font-awesome-icon icon="fa-solid fa-person-shelter" /> Is Indoor
            </label>
            <div class="control">
              <div class="select is-fullwidth is-rounded">
                <select v-model="selectedIsIndoorInput">
                  <option :value="null">Any</option>
                  <option :value="true">Yes</option>
                  <option :value="false">No</option>
                </select>
              </div>
            </div>
          </div>
        </div>

        <!-- HasPrize Filter -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">
              <font-awesome-icon icon="fa-regular fa-money-bill-1" /> Has Prize
            </label>
            <div class="control">
              <div class="select is-fullwidth is-rounded">
                <select v-model="selectedHasPrizeInput">
                  <option :value="null">Any</option>
                  <option :value="true">Yes</option>
                  <option :value="false">No</option>
                </select>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Apply/Reset Buttons -->
      <div class="field is-grouped is-grouped-centered">
        <div class="control">
          <button class="button is-primary is-rounded" @click="applyFilters">Apply</button>
        </div>
        <div class="control">
          <button class="button is-light is-rounded" @click="resetFilters">Reset</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
label {
  color: #00d1b2;
}

.filters-panel {
  margin-top: 1rem;
  padding: 1rem;
}
</style>