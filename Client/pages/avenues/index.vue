<script setup lang="ts">
definePageMeta({
  layout: "default-transparent",
});
import { useAuthStore } from '@/stores/auth'
const authStore = useAuthStore();

const page = ref(1);
const itemsPerPage = ref(10);
const totalAvenues = ref(0);
const keyword = ref<string>('');
const city = ref<string>('');
const country = ref<string>('');

const totalPages = computed(() => Math.ceil(totalAvenues.value / itemsPerPage.value));

const handleTotalAvenuesUpdate = (total: number) => {
  totalAvenues.value = total;
};

const handlePageChange = (newPage: number) => {
  page.value = newPage;
};

const handleItemsPerPageChange = (newItemsPerPage: number) => {
  itemsPerPage.value = newItemsPerPage;
};

const handleSearch = (searchInput: string) => {
  keyword.value = searchInput;
};

</script>
<template>
    <div class="view-window">
        <Banner title="All Avenues" background-img="/imgs/avenues-banner.png">
            <div>
                <div v-if="authStore.user.username" class="buttons is-centered">
                    <hr>
                    <NuxtLink to="/avenues/create" class="button is-primary">Create Avenue</NuxtLink>
                    <hr>
                </div>
            </div>
        </Banner>
        
    <BaseSearchBar placeholder="Search for an avenue..." @search="handleSearch" />
    <AvenueQueryList
      :keyword="keyword"
      :city="city"
      :country="country"
      :page="page"
      :itemsPerPage="itemsPerPage"
      @updateTotalAvenues="handleTotalAvenuesUpdate"
    />
    <BasePagination
      :current-page="page"
      :total-pages="totalPages"
      :items-per-page="itemsPerPage"
      :items-per-page-options="[10, 20, 30, 50, 100]"
      :max-items-per-page="25"
      :total-items="totalAvenues"
      @page-change="handlePageChange"
      @items-per-page-change="handleItemsPerPageChange"
    />
    </div>
</template>

<style scoped>

.search-bar {
  margin-top: 1rem;
}
</style>