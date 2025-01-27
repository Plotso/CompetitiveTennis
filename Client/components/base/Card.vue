<template>
    <div class="card">
      <header class="card-header">
        <p class="card-header-title">{{ title }}</p>
      </header>
      <div class="card-content">
        <div class="content">
          <p>{{ content }}</p>
          <slot></slot>
          <p v-if="details" class="details">{{ details }}</p>
        </div>
      </div>
    </div>
  </template>
  
<script setup lang="ts">
  defineProps({
    title: {
      type: String,
      required: true,
    },
    content: {
      type: String,
      required: true,
    },
    details: {
      type: String,
      required: false,
    },
  });
</script>
  
<style scoped>
  .card {
    border: 1px solid #ddd;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    margin-bottom: 1rem;
    
  }
  
  .card-header {
    background-color: #00d1b2; /* Bulma's primary color */
    color: #fff;
    padding: 0.5rem 1rem;
    border-bottom: 1px solid #ddd;
    font-weight: bold;
  }

  .card-header-title {
    font-size: 1.25rem;
    color: #fff;
  }
  
  .card-content {
    padding: 1rem;
  }
  
  .details {
    margin-top: 1rem;
    color: #555;
    font-size: 0.9rem;
  }
</style>
  