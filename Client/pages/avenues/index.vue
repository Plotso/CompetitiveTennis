<script setup lang="ts">
import { ref, computed } from 'vue';
import { useAuthStore } from '@/stores/auth';
import { Surface, CourtType } from '~/types';

definePageMeta({
  layout: "default-transparent",
});

const authStore = useAuthStore();

const page = ref(1);
const itemsPerPage = ref(10);
const totalAvenues = ref(0);
const maxAvenuesPerPage = ref(100);
const keyword = ref<string>('');
const city = ref<string | null>(null);
const country = ref<string | null>(null);
const surface = ref<Surface | null>(null);
const courtType = ref<CourtType | null>(null);

const totalPages = computed(() => Math.ceil(totalAvenues.value / itemsPerPage.value));

const handleTotalAvenuesUpdate = (total: number) => {
  totalAvenues.value = total;
};

const handlePageChange = (newPage: number) => {
  page.value = newPage;
};

const handleItemsPerPageChange = (newItemsPerPage: number) => {
  itemsPerPage.value = newItemsPerPage;
  page.value = 1;
};

const handleSearch = (searchInput: string) => {
  keyword.value = searchInput;
  page.value = 1; 
};

const handleApplyFilters = (filters: {
  city: string | null;
  country: string | null;
  surface: Surface | null;
  courtType: CourtType | null;
}) => {
  city.value = filters.city;
  country.value = filters.country;
  surface.value = filters.surface;
  courtType.value = filters.courtType;
  page.value = 1; 
};

const handleResetFilters = () => {
  // Lines below are commented since clicking on Reset button should not result in refetching of data unless the user explicitly applies filters
  // city.value = null;
  // country.value = null;
  // surface.value = null;
  // courtType.value = null;
  // page.value = 1;
};
</script>

<template>
  <div class="view-window">
    <Banner title="All Avenues" background-img="/imgs/avenues-banner.png">
      <div>
        <div v-if="authStore.user.username" class="buttons is-centered">
          <hr>
          <NuxtLink to="/avenues/create" class="button is-primary">Create Avenue</NuxtLink>
          <hr>
        </div>
      </div>
    </Banner>

    <BaseSearchBar placeholder="Search for an avenue..." @search="handleSearch" />

    <!-- Integrate the AvenueFilters component -->
    <AvenueFilters
      :city="city"
      :country="country"
      :surface="surface"
      :court-type="courtType"
      @apply-filters="handleApplyFilters"
      @reset-filters="handleResetFilters"
    />

    <AvenueQueryList
      :keyword="keyword"
      :city="city"
      :country="country"
      :surface="surface"
      :court-type="courtType"
      :page="page"
      :itemsPerPage="itemsPerPage"
      @update-total-avenues="handleTotalAvenuesUpdate"
    />

    <BasePagination
      :current-page="page"
      :total-pages="totalPages"
      :items-per-page="itemsPerPage"
      :items-per-page-options="[10, 20, 30, 50, 100]"
      :max-items-per-page="maxAvenuesPerPage"
      :total-items="totalAvenues"
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