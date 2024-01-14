<template>
    <div class="modal" :class="{'is-active': isOpen}">
      <div class="modal-background"></div>
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title has-text-danger">{{ title }}</p>
        </header>
        <section class="modal-card-body">
          <p>{{ message }}</p>
        </section>
        <footer class="modal-card-foot">
          <button class="button" @click="close">Close</button>
        </footer>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">

  const emit = defineEmits(['close'])
  
  const props = defineProps({
    isOpen: Boolean,
    title: String,
    message: String
  });
  
  const close = () => {
    emit('close');
  };
  </script>
  
  <style scoped>
  /* Add your scoped styles here */
  </style>
  