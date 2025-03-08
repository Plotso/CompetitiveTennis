<script setup lang="ts">
import { ref, computed } from 'vue';
import { AccountQuery, SortOptions, SearchOutputModel, Result, AccountOutputModel, AccountSortOptions } from '~/types';

const isSingles = ref(true);

const toggleRating = () => {
  isSingles.value = !isSingles.value;
  console.log('accountSortOption', accountSortOption.value);
};

const accountSortOption = computed(() => 
  isSingles.value 
    ? AccountSortOptions[AccountSortOptions.SinglesRatingDescending ]
    : AccountSortOptions[AccountSortOptions.DoublesRatingDescending ]
);

const refreshAccounts = () => {
  console.log('Refreshing accounts');
  
  refreshNuxtData();
};

// Watch for changes in `activeSet` and update cumulative scores
watch(accountSortOption, refreshAccounts);
</script>

<template>
  <div class="view-window">
    <Banner title="Player Leaderboard" background-img="/imgs/avenues-banner.png">
      <!-- Toggle button placed inside the Banner -->
      <button @click="toggleRating" class="toggle-rating-button">
        Switch to {{ isSingles ? 'Doubles' : 'Singles' }} Rating
      </button>
    </Banner>

    <AccountQueryList
      :key="accountSortOption"
      :accountSortOptions="accountSortOption" 
      :showSinglesRating="isSingles" 
      :showDoublesRating="!isSingles">
    </AccountQueryList>
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
</style>