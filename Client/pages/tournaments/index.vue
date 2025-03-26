<script setup lang="ts">
import { ref, computed } from 'vue';
import { useAuthStore } from '~/stores/auth';

definePageMeta({
  layout: "default-transparent",
});

const authStore = useAuthStore();

const page = ref(1);
const itemsPerPage = ref(10);
const totalTournaments = ref(0);
const keyword = ref('');

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

    <TournamentQueryList
      :keyword="keyword"
      :page="page"
      :itemsPerPage="itemsPerPage"
      :showParticipationColumn="true"
      :showMoneyRelatedColumns="true"
      @updateTotalTournaments="handleTotalTournamentsUpdate"
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
</style>