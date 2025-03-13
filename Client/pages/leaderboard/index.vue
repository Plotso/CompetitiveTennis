<script setup lang="ts">
import { ref, computed } from 'vue';
import { AccountQuery, SortOptions, SearchOutputModel, Result, AccountOutputModel, AccountSortOptions } from '~/types';
definePageMeta({
  layout: "default-transparent",
});

const isSingles = ref(true);

const page = ref(1);
const itemsPerPage = ref(10);
const totalAccounts = ref(0);
// Calculate total pages based on totalItems
const totalPages = computed(() => Math.ceil(totalAccounts.value / itemsPerPage.value));


const searchText = ref<string>('');

const toggleRating = () => {
  isSingles.value = !isSingles.value;
  console.log('accountSortOption', accountSortOption.value);
};

const accountSortOption = computed(() => 
  isSingles.value 
    ? AccountSortOptions[AccountSortOptions.SinglesRatingDescending ]
    : AccountSortOptions[AccountSortOptions.DoublesRatingDescending ]
);

const handleTotalAccountsUpdate = (totalAccountsUpdate: number) => {
  console.log('Received:', totalAccountsUpdate);

  totalAccounts.value = totalAccountsUpdate;
};

const handlePageChange = (newPage: number) => {
  page.value = newPage;
};

const handleItemsPerPageChange = (newItemsPerPage: number) => {
  itemsPerPage.value = newItemsPerPage;
};
const handleSearch = (searchInput: string) => {
  searchText.value = searchInput;
};

// const refreshAccounts = () => {
//   console.log('Refreshing accounts');
  
//   refreshNuxtData();
// };

// // Watch for changes in `activeSet` and update cumulative scores
// watch(accountSortOption, refreshAccounts);
// watch(page, refreshAccounts);
// watch(itemsPerPage, refreshAccounts);
</script>

<template>
  <div class="view-window">
    <Banner title="Player Leaderboard" background-img="/imgs/avenues-banner.png">
      <!-- Toggle button placed inside the Banner -->      
    <button @click="toggleRating" class="button toggle-rating-button is-rounded switch-rating-button">
        <font-awesome-icon :icon="isSingles ? 'fa-solid fa-people-arrows' : 'fa-solid fa-person'" /> Switch to {{ isSingles ? 'Doubles' : 'Singles' }} Rating
      </button>
    </Banner>
    <BaseSearchBar placeholder="Search for an user..." @search="handleSearch" />

    <AccountQueryList
      :key="accountSortOption"
      :accountSortOptions="accountSortOption"
      :keyword="searchText"
      :page="page"
      :itemsPerPage="itemsPerPage" 
      :showSinglesRating="isSingles" 
      :showDoublesRating="!isSingles"
      @updateTotalAccounts="handleTotalAccountsUpdate">
    </AccountQueryList>

    <BasePagination
      :current-page="page"
      :total-pages="totalPages"
      :items-per-page="itemsPerPage"
      :items-per-page-options="[10, 20, 30, 50, 100]"
      :max-items-per-page="25"
      @page-change="handlePageChange"
      @items-per-page-change="handleItemsPerPageChange"
    />

  </div>
</template>

<style scoped>
.toggle-rating-button {
  margin-top: 1rem;
  padding: 0.5rem 1rem;
  background: #00d1b2;
  color: white;
  border: none;
  cursor: pointer;
}


.search-bar {
  margin-top: 1rem;
}
</style>