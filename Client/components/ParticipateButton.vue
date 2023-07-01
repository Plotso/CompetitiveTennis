<template>
    <div>
        <button class="button is-primary"  @click="participate()"
        v-if="!hasMaxParticipants">
        {{ buttonLabel || 'Participate' }}
        </button>
        <button class="button is-primary" v-else disabled>
          {{ buttonLabel || 'Participate' }}
        </button>
    </div>
  </template>
  
  <script setup lang="ts">

  const emit = defineEmits(['participate'])
  
  const props = defineProps({
    hasMaxParticipants: Boolean,
    buttonLabel: {
      type: String,
      default: null
    }
  });
  
  const participate = () => {
    emit('participate');
  };
  </script>
  
  <style scoped>
  /* Add your scoped styles here */
  </style>
  