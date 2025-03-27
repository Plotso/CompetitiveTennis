<script setup lang="ts">

const props = defineProps({
  currentPage: { type: Number, required: true },
  totalPages: { type: Number, required: true },
  itemsPerPage: { type: Number, default: null },
  itemsPerPageOptions: { type: Array as () => number[] | null, default: null },
  maxItemsPerPage: { type: Number, default: null },
  totalItems: { type: Number, default: null },
});

const emit = defineEmits<{
  (e: 'page-change', page: number): void;
  (e: 'items-per-page-change', value: number): void;
}>();

// Function to generate pagination items based on currentPage and totalPages
const getPaginationItems = (currentPage: number, totalPages: number): (number | string)[] => {
  if (totalPages <= 5) {
    // If 5 or fewer pages, show all pages
    return Array.from({ length: totalPages }, (_, i) => i + 1);
  } else {
    const items: (number | string)[] = [1]; // Always include page 1

    // Add ellipsis if there's a gap between page 1 and the pages around currentPage
    if (currentPage >= 4) {
      items.push('...');
    }

    // Determine the range of pages around currentPage
    let start = Math.max(2, currentPage - 1);
    let end = Math.min(totalPages - 1, currentPage + 1);

    // Adjust start and end based on position
    if (currentPage <= 3) {
      end = 4; // Show more pages after if near the start
    } else if (currentPage >= totalPages - 2) {
      start = totalPages - 3; // Show more pages before if near the end
    }

    // Add the pages around currentPage
    for (let i = start; i <= end; i++) {
      items.push(i);
    }

    // Add ellipsis if there's a gap between the pages around currentPage and the last page
    if (currentPage <= totalPages - 3) {
      items.push('...');
    }

    items.push(totalPages); // Always include the last page
    return items;
  }
};

const paginationItems = computed(() => getPaginationItems(props.currentPage, props.totalPages));

const actualOptions = computed(() => {
  // Default options if none provided
  const defaultOptions = [10, 20, 30, 50, 100];

  // If itemsPerPageOptions is provided, use it; otherwise, use default
  let baseOptions = props.itemsPerPageOptions || defaultOptions;

  // Filter options based on maxItemsPerPage if provided
  let options = props.maxItemsPerPage
    ? baseOptions.filter((opt) => opt <= props.maxItemsPerPage)
    : [...baseOptions];

  let hasCustomValue = false;

  // Include current itemsPerPage if it's set and not in the options
  if (props.itemsPerPage && !options.includes(props.itemsPerPage)) {
    options.push(props.itemsPerPage);
    hasCustomValue = true;
  }

  if (props.totalItems && props.totalItems <= props.itemsPerPage ){
    return hasCustomValue ? [props.itemsPerPage] : [options.sort((a, b) => a - b)[0]];
  }

  // Sort options in ascending order
  return options.sort((a, b) => a - b);
});

// Handlers for Previous and Next buttons
const handlePrevious = () => {
  if (props.currentPage > 1) {
    emit('page-change', props.currentPage - 1);
  }
};

const handleNext = () => {
  if (props.currentPage < props.totalPages) {
    emit('page-change', props.currentPage + 1);
  }
};
</script>

<template>
  <div class="pagination-container">
    <nav class="pagination is-centered is-rounded" role="navigation" aria-label="pagination">
    <!-- Previous Button -->
    <a
      class="pagination-previous"
      :class="{ 'is-disabled': currentPage === 1 }"
      @click.prevent="handlePrevious"
    >
      Previous
    </a>

    <!-- Next Button -->
    <a
      class="pagination-next"
      :class="{ 'is-disabled': currentPage === totalPages }"
      @click.prevent="handleNext"
    >
      Next
    </a>

    <!-- Pagination List -->
    <ul class="pagination-list">
      <li v-for="(item, index) in paginationItems" :key="index">
        <!-- Page Number Link -->
        <a
          v-if="typeof item === 'number'"
          class="pagination-link"
          :class="{ 'is-current': item === currentPage }"
          :aria-label="'Goto page ' + item"
          :aria-current="item === currentPage ? 'page' : null"
          @click.prevent="emit('page-change', item)"
        >
          {{ item }}
        </a>

        <!-- Ellipsis -->
        <span v-else class="pagination-ellipsis">…</span>
      </li>
    </ul>

    <!-- Items Per Page Dropdown (shown if itemsPerPage and options are provided) -->
    <div v-if="actualOptions.length > 0" class="pagination-items-per-page">
      <label for="itemsPerPage">Items per page:</label>
      <div class="select is-rounded is-success is-focused">
        <select
          id="itemsPerPage"
          :value="itemsPerPage"
          @change="emit('items-per-page-change', parseInt($event.target.value, 10))"
        >
          <option v-for="option in actualOptions" :key="option" :value="option">
            {{ option }}
          </option>
        </select>
      </div>
    </div>
  </nav>
  </div>
</template>

<style scoped>

.pagination-container {
  display: flex;
  justify-content: center;
  margin-top: 1rem;
}
.pagination-items-per-page {
  /* margin-top: 1rem; */
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.pagination-previous, .pagination-next {
}

.is-current {
  background-color: var(--bulma-pagination-current-color, #00d1b2);
  color: white;
  border-color: #00d1b2;
}
</style>