<script setup lang="ts">
import { Surface, CourtType } from '~/types';

const props = defineProps<{
  city: string | null;
  country: string | null;
  surface: Surface | null;
  courtType: CourtType | null;
}>();

const emit = defineEmits<{
  (e: 'applyFilters', filters: {
    city: string | null;
    country: string | null;
    surface: Surface | null;
    courtType: CourtType | null;
  }): void;
  (e: 'resetFilters'): void;
}>();

const cityInput = ref<string | null>(props.city);
const countryInput = ref<string | null>(props.country);
const surfaceInput = ref<Surface | null>(props.surface);
const courtTypeInput = ref<CourtType | null>(props.courtType);

const showFilters = ref(false);

const toggleFilters = () => {
  showFilters.value = !showFilters.value;
};

const applyFilters = () => {
  emit('applyFilters', {
    city: cityInput.value,
    country: countryInput.value,
    surface: surfaceInput.value,
    courtType: courtTypeInput.value,
  });
  showFilters.value = false;
};

const resetFilters = () => {
  cityInput.value = null;
  countryInput.value = null;
  surfaceInput.value = null;
  courtTypeInput.value = null;
  emit('resetFilters');
};

const surfaceValues = Object.values(Surface).filter(value => typeof value === 'number') as Surface[];
const courtTypeValues = Object.values(CourtType).filter(value => typeof value === 'number') as CourtType[];
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
        <!-- City Filter -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">City</label>
            <div class="control">
              <input class="input is-rounded" type="text" v-model="cityInput" placeholder="Enter city">
            </div>
          </div>
        </div>

        <!-- Country Filter -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">Country</label>
            <div class="control">
              <input class="input is-rounded" type="text" v-model="countryInput" placeholder="Enter country">
            </div>
          </div>
        </div>

        <!-- Surface Filter -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">Surface</label>
            <div class="control">
              <div class="select is-fullwidth is-rounded">
                <select v-model="surfaceInput">
                  <option :value="null">All Surfaces</option>
                  <option v-for="surface in surfaceValues" :key="surface" :value="surface">
                    {{ Surface[surface] }}
                  </option>
                </select>
              </div>
            </div>
          </div>
        </div>

        <!-- CourtType Filter -->
        <div class="column is-3">
          <div class="field">
            <label class="label has-text-centered">Court Type</label>
            <div class="control">
              <div class="select is-fullwidth is-rounded">
                <select v-model="courtTypeInput">
                  <option :value="null">All Court Types</option>
                  <option v-for="courtType in courtTypeValues" :key="courtType" :value="courtType">
                    {{ CourtType[courtType] }}
                  </option>
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
.filters-panel {
  margin-top: 1rem;
  padding: 1rem;
}
</style>