<script setup lang="ts">
import { useAuthStore } from '~/stores/auth';
import { TournamentType, Surface } from '~/types';

definePageMeta({
  layout: "default-transparent",
});

const authStore = useAuthStore();

const page = ref(1);
const itemsPerPage = ref(10);
const maxTournamentsPerPage = ref(50);
const totalTournaments = ref(0);
const keyword = ref('');

const selectedType = ref<TournamentType | null>(null);
const selectedHasPrize = ref<boolean | null>(null);
const selectedSurface = ref<Surface | null>(null);
const selectedDateFrom = ref<string | null>(null);
const selectedDateUntil = ref<string | null>(null);
const selectedIsIndoor = ref<boolean | null>(null);
const selectedCity = ref<string | null>(null);

const handleApplyFilters = (filters: {
  selectedType: TournamentType | null;
  selectedHasPrize: boolean | null;
  selectedSurface: Surface | null;
  selectedDateFrom: string | null;
  selectedDateUntil: string | null;
  selectedIsIndoor: boolean | null;
  selectedCity: string | null;
}) => {
  page.value = 1; // Reset to first page
  selectedType.value = filters.selectedType;
  selectedHasPrize.value = filters.selectedHasPrize;
  selectedSurface.value = filters.selectedSurface;
  selectedDateFrom.value = filters.selectedDateFrom;
  selectedDateUntil.value = filters.selectedDateUntil;
  selectedIsIndoor.value = filters.selectedIsIndoor;
  selectedCity.value = filters.selectedCity;
};

const handleResetFilters = () => {
  // Reset logic is handled in the child, but parent state could by synced here if needed
};

// Compute total pages for pagination
const totalPages = computed(() => Math.ceil(totalTournaments.value / itemsPerPage.value));

const handleTotalTournamentsUpdate = (total: number) => {
  totalTournaments.value = total;
};

const handlePageChange = (newPage: number) => {
  page.value = newPage;
};

const handleItemsPerPageChange = (newItemsPerPage: number) => {
  itemsPerPage.value = newItemsPerPage;
  page.value = 1; // Reset to first page
};

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

    <!-- Use the new Filters component -->
    <TournamentFilters
      :selected-type="selectedType"
      :selected-has-prize="selectedHasPrize"
      :selected-surface="selectedSurface"
      :selected-date-from="selectedDateFrom"
      :selected-date-until="selectedDateUntil"
      :selected-is-indoor="selectedIsIndoor"
      :selected-city="selectedCity"
      @apply-filters="handleApplyFilters"
      @reset-filters="handleResetFilters"
    />

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
      :city="selectedCity"
      :show-participation-column="true"
      :show-money-related-columns="true"
      @update-total-tournaments="handleTotalTournamentsUpdate"
    />

    <BasePagination
      :current-page="page"
      :total-pages="totalPages"
      :items-per-page="itemsPerPage"
      :items-per-page-options="[10, 20, 30, 50, 100]"
      :max-items-per-page="maxTournamentsPerPage"
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

.filters {
  margin-bottom: 1rem;
}
</style>