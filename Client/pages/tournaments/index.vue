<script setup lang="ts">
import { useAuthStore } from '~/stores/auth';
import { TournamentType, Surface } from '~/types';

definePageMeta({
  layout: "default-transparent",
});

const authStore = useAuthStore();

const page = ref(1);
const itemsPerPage = ref(10);
const totalTournaments = ref(0);
const keyword = ref('');

// Filter states (all nullable with default null)
const selectedType = ref<TournamentType | null>(null);
const selectedHasPrize = ref<boolean | null>(null);
const selectedSurface = ref<Surface | null>(null);
const selectedDateFrom = ref<string | null>(null); // Date input as string
const selectedDateUntil = ref<string | null>(null); // Date input as string
const selectedIsIndoor = ref<boolean | null>(null);

const selectedTypeInput = ref<TournamentType | null>(null);
const selectedHasPrizeInput = ref<boolean | null>(null);
const selectedSurfaceInput = ref<Surface | null>(null);
const selectedDateFromInput = ref<string | null>(null); // Date input as string
const selectedDateUntilInput = ref<string | null>(null); // Date input as string
const selectedIsIndoorInput = ref<boolean | null>(null);

const showFilters = ref(false);
const toggleFilters = () => {
  showFilters.value = !showFilters.value;
};

const applyFilters = () => {
  page.value = 1; 
  showFilters.value = false;

  selectedType.value = selectedTypeInput.value;
  selectedHasPrize.value = selectedHasPrizeInput.value;
  selectedSurface.value = selectedSurfaceInput.value;
  selectedDateFrom.value = selectedDateFromInput.value;
  selectedDateUntil.value = selectedDateUntilInput.value;
  selectedIsIndoor.value = selectedIsIndoorInput.value;
};

const surfaceValues = Object.values(Surface)
  .filter(value => typeof value === 'number') as Surface[];

// Compute total pages for pagination
const totalPages = computed(() => Math.ceil(totalTournaments.value / itemsPerPage.value));

// Handle total tournaments update from TournamentQueryList
const handleTotalTournamentsUpdate = (total: number) => {
  totalTournaments.value = total;
};

// Handle page change
const handlePageChange = (newPage: number) => {
  page.value = newPage;
};

// Handle items per page change
const handleItemsPerPageChange = (newItemsPerPage: number) => {
  itemsPerPage.value = newItemsPerPage;
  page.value = 1; // Reset to first page
};

// Handle search input
const handleSearch = (searchInput: string) => {
  keyword.value = searchInput;
  page.value = 1; // Reset to first page on search
};
</script>

<template>
  <div class="view-window">
    <Banner title="All Tournaments" background-img="/imgs/ongoing-tournament-banner.png">
      <div>
        <div v-if="authStore.user.username" class="buttons is-centered">
          <hr>
          <NuxtLink to="/tournaments/create" class="button is-primary">Create Tournament</NuxtLink>
          <hr>
        </div>
      </div>
    </Banner>

    <BaseSearchBar placeholder="Search for a tournament..." @search="handleSearch" />

    <div class="field has-addons has-addons-centered">
      <!-- Filters Button -->
      <button class="button is-rounded is-primary" @click="toggleFilters">
        <span class="icon is-small">
          <font-awesome-icon icon="fa-solid fa-filter" />
        </span>
        <span>{{ showFilters ? 'Hide Filters' : 'Filters' }}</span>
      </button>
    </div>

    <!-- Filters Panel (visible when showFilters is true) -->
    <div v-if="showFilters" class="box filters-panel">
      <!-- Type Filter -->
      <div class="field">
        <label class="label">Type</label>
        <div class="control">
          <div class="select">
            <select v-model="selectedTypeInput">
              <option :value="null">All Types</option>
              <option :value="TournamentType.Singles">Singles</option>
              <option :value="TournamentType.Doubles">Doubles</option>
            </select>
          </div>
        </div>
      </div>

      <!-- HasPrize Filter -->
      <div class="field">
        <label class="label">Has Prize</label>
        <div class="control">
          <div class="select">
            <select v-model="selectedHasPrizeInput">
              <option :value="null">Any</option>
              <option :value="true">Yes</option>
              <option :value="false">No</option>
            </select>
          </div>
        </div>
      </div>

      <!-- Surface Filter -->
      <div class="field">
        <label class="label">Surface</label>
        <div class="control">
          <div class="select">
            <select v-model="selectedSurfaceInput">
              <option :value="null">All Surfaces</option>
              <option v-for="surface in surfaceValues" :key="surface" :value="surface">
                {{ Surface[surface] }}
              </option>
            </select>
          </div>
        </div>
      </div>

      <!-- Date Range Filter (From) -->
      <div class="field">
        <label class="label">Date From</label>
        <div class="control">
          <input class="input" type="date" v-model="selectedDateFromInput">
        </div>
      </div>

      <!-- Date Range Filter (Until) -->
      <div class="field">
        <label class="label">Date Until</label>
        <div class="control">
          <input class="input" type="date" v-model="selectedDateUntilInput">
        </div>
      </div>

      <!-- IsIndoor Filter -->
      <div class="field">
        <label class="label">Is Indoor</label>
        <div class="control">
          <div class="select">
            <select v-model="selectedIsIndoorInput">
              <option :value="null">Any</option>
              <option :value="true">Yes</option>
              <option :value="false">No</option>
            </select>
          </div>
        </div>
      </div>

      <!-- Apply Button -->
      <div class="field">
        <div class="control">
          <button class="button is-primary" @click="applyFilters">Apply</button>
        </div>
      </div>
    </div>

    <!-- Pass filters to TournamentQueryList -->
    <TournamentQueryList
      :keyword="keyword"
      :page="page"
      :itemsPerPage="itemsPerPage"
      :tournament-type="selectedType"
      :has-prize="selectedHasPrize"
      :surface="selectedSurface"
      :date-range-from="selectedDateFrom ? new Date(selectedDateFrom) : null"
      :date-range-until="selectedDateUntil ? new Date(selectedDateUntil) : null"
      :is-indoor="selectedIsIndoor"
      :show-participation-column="true"
      :show-money-related-columns="true"
      @update-total-tournaments="handleTotalTournamentsUpdate"
    />

    <BasePagination
      :current-page="page"
      :total-pages="totalPages"
      :items-per-page="itemsPerPage"
      :items-per-page-options="[10, 20, 30, 50, 100]"
      :max-items-per-page="25"
      :total-items="totalTournaments"
      @page-change="handlePageChange"
      @items-per-page-change="handleItemsPerPageChange"
    />
  </div>
</template>

<style scoped>

.search-bar {
  margin-top: 1rem;
}

.filters-panel {
  margin-top: 1rem;
  padding: 1rem;
}
</style>