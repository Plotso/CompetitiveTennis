<script setup lang="ts">
  import { AccountOutputModel } from '@/types';
  
  const props = defineProps({
    accounts: {
      type: Array as () => AccountOutputModel[],
      required: true,
    },
    showTableHeaders: Boolean,
    showSinglesRating: Boolean,
    showDoublesRating: Boolean,
  });

  // watchEffect(() => {
  //   console.log(props.accounts);
  // })
  </script>

<template>
    <div class="table-container">
      <table class="table is-striped is-fullwidth">
        <thead v-if="showTableHeaders">
          <tr>
            <th>Rank</th>
            <th>Account</th>
            <th v-if="showSinglesRating">Singles Rating</th>
            <th v-if="showDoublesRating">Doubles Rating</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(account, index) in accounts" :key="account.id">
            <td>{{index + 1}}</td>
            <td>
              
            <NuxtLink :to="`/users/profile/${account.username}`" class="custom-link has-text-weight-semibold">
              {{account.firstName}} {{account.lastName}} ({{account.username}})
            </NuxtLink>
            </td>
            <td v-if="showSinglesRating">{{account.playerRating}}</td>
            <td v-if="showDoublesRating">{{account.doublesRating}}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </template>  

<style scoped>
.card {
    margin-bottom: 2rem;
}

.image-overlay-left {
    position: absolute;
    bottom: 1px;
    /* Adjust the position as needed */
    left: 5px;
    /* Adjust the position as needed */
    padding: 5px;
    /* Adjust the padding as needed */
    z-index: 1;
    /* Bring the span element above the image */
}



.image-overlay-right {
    position: absolute;
    bottom: 1px;
    /* Adjust the position as needed */
    right: 5px;
    /* Adjust the position as needed */
    padding: 5px;
    /* Adjust the padding as needed */
    z-index: 1;
    /* Bring the span element above the image */
}

.img-custom {
    
    width: 125%;
}

.is-centered-custom {

  padding-left: 100px;
}

.custom-box {
    background-color: rgb(248, 248, 235);
}
</style>
  