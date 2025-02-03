<script setup lang="ts">
</script>

<template>
  <div class="container">
    <header class="special">
            <h1><strong>Upcoming Tournaments</strong></h1>
            <p class="summary">View all upcoming tournaments</p>
        </header>
    <TournamentQueryList :dateRangeFrom="new Date()"></TournamentQueryList>

    <div class="buttons is-centered">
        <NuxtLink to="/tournaments" class="button is-primary">View All</NuxtLink>

    </div>
  </div>
</template>

<style scoped>
header {
    
  display: flex;
  flex-direction: column;
  align-items: center;
}

h1 strong {
  font-size: 1.75rem; /* Adjust title size */
  font-weight: bold;
  color: #00d1b2;
  margin: 0; /* Ensure no extra margin around */
}

.summary {
    font-size: 18px;
    line-height: 24px;
}
</style>