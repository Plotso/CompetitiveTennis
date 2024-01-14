<template>
    <div class="modal" :class="{'is-active': isOpen}">
      <div class="modal-background"></div>
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">{{ title }}</p>
          <button class="delete" aria-label="close" @click="close"></button>
        </header>
        <section class="modal-card-body">
          <p>{{ message }}</p>
        </section>
        <footer class="modal-card-foot">
          <button class="button is-danger" @click="confirm">Confirm</button>
          <button class="button" @click="close">Cancel</button>
        </footer>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">

  const emit = defineEmits(['confirm', 'close'])
  
  const props = defineProps({
    isOpen: Boolean,
    title: String,
    message: String
  });
  
  const confirm = () => {
    emit('confirm');
  };
  
  const close = () => {
    emit('close');
  };
  </script>
  
  <style scoped>
  /* Add your scoped styles here */
  </style>
  