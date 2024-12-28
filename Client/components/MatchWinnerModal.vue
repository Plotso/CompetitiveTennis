<template>
    <div class="modal" :class="{'is-active': isOpen}">
      <div class="modal-background"></div>
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">{{ title }}</p>
          <button class="delete" aria-label="close" @click="close"></button>
        </header>
        <section class="modal-card-body">
          <slot name="body">
            <p>{{ message }}</p>
          </slot>
        </section>
            <footer class="modal-card-foot">
                <div class="buttons">
            <slot name="actions">
                <button class="button is-success is-responsive is-centered" @click="emitConfirmWinner">Confirm Match Outcome is {{ winner }}</button>
                <button class="button is-info is-responsive is-centered" @click="emitSaveScores" v-if="isScoresInput">The game is not finished but still send scores without match winner</button>
                <button class="button is-danger is-responsive is-centered" @click="close">Cancel</button>
            </slot>
        </div>
        </footer>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
import { MatchOutcome } from '~/types';

  const emit = defineEmits(['confirm-winner', 'save-scores', 'close']);
  
  const props = defineProps({
    isOpen: Boolean,
    title: String,
    message: String,
    winner: String, // Optional prop to pass the match winner's name
    isScoresInput: Boolean, // Optional prop to show/hide the scores input fields
  });
  
  const emitConfirmWinner = () => {
    emit('confirm-winner');
  };
  
  const emitSaveScores = () => {
    emit('save-scores');
  };
  
  const close = () => {
    emit('close');
  };
  </script>
  