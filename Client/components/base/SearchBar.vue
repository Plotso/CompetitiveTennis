<script lang="ts" setup>
const props = defineProps({
  placeholder: {
    type: String,
    default: 'Search...',
  },
});

const emit = defineEmits<{
  (e: 'search', text: string): void;
}>();

const oldValue = ref<string>('');
const searchText = ref<string>('');

const handleSearch = () => {
  const trimmedText = searchText.value.trim();
  if (trimmedText || oldValue.value) {
    emit('search', trimmedText);
    oldValue.value = trimmedText;
  }
};
</script>

<template>
  <div class="field has-addons has-addons-centered search-bar">
    <!-- <div class="control has-icons-left"> -->
    <div class="control">
      <input
        class="input is-rounded "
        type="text"
        v-model="searchText"
        :placeholder="placeholder"
        @keyup.enter="handleSearch"
      />
      <!-- <span class="icon is-left">
        <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
      </span> -->
    </div>
    <div class="control">
      <button
        class="button is-primary is-rounded "
        :disabled="!searchText.trim() && !oldValue.trim()"
        @click="handleSearch"
      >
      <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
        <!-- Search &nbsp; <font-awesome-icon icon="fa-solid fa-magnifying-glass" /> -->
      </button>
    </div>
  </div>
</template>

<style scoped>
</style>