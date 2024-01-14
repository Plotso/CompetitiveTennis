<template>
    <div class="modal" :class="{'is-active': isOpen}">
      <div class="modal-background"></div>
      <div class="modal-content">
        <Loading></Loading>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  
  const props = defineProps({
    isOpen: Boolean
  });
  </script>
  
  <style scoped>
  /* Add your scoped styles here */
  </style>
  