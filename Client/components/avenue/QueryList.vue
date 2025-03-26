<script setup lang="ts">
import { useTournamentsApi } from '~/composables/useTournamentsApi';
import { AvenueQuery, SortOptions, SearchOutputModel, Result, AvenueOutputModel, Surface, CourtType } from '~/types';

const avenues = ref<AvenueOutputModel[]>([]);
const showLoadingModal = ref(true);
const avenuesCount = ref(0);

const props = defineProps({
  keyword: { type: String, required: false },
  city: { type: String, required: false },
  country: { type: String, required: false },
  sortOptions: { type: Number as PropType<SortOptions>, required: false },
  surface: { type: Number as PropType<Surface | null>, required: false },
  courtType: { type: Number as PropType<CourtType | null>, required: false },
  page: { type: Number, required: false, default: 1 },
  itemsPerPage: { type: Number, required: false, default: 10 },
});

const emit = defineEmits(['updateTotalAvenues']);

const errorNotification = ref("");
const showErrorNotification = ref(false);

const hideErrorNotification = () => {
  showErrorNotification.value = false;
};

const query = computed<AvenueQuery>(() => ({
  keyword: props.keyword,
  city: props.city,
  country: props.country,
  sortOptions: props.sortOptions,
  surface: props.surface,
  courtType: props.courtType,
  page: props.page,
  itemsPerPage: props.itemsPerPage,
}));

const method = 'GET';
const options = { query, method };

const apiResponse = await useTournamentsApi<Result<SearchOutputModel<AvenueOutputModel>>>('/Avenues/Search', options);

watchEffect(() => {
  if (apiResponse.error.value) {
    errorNotification.value = "Error loading avenues";
    showErrorNotification.value = true;
    showLoadingModal.value = false;
  }
  if (apiResponse.data.value?.data.results) {
    avenues.value = apiResponse.data.value.data.results;
    avenuesCount.value = apiResponse.data.value.data.total;
    emit('updateTotalAvenues', avenuesCount.value);
    showLoadingModal.value = false;
  }
});
</script>

<template>
  <div class="container">
    <div v-if="showLoadingModal">
      <BaseLoading></BaseLoading>
    </div>
    <div v-else>
      <div class="notification is-danger" v-show="showErrorNotification">
        <button class="delete" @click="hideErrorNotification"></button>
        {{ errorNotification }}
      </div>
      <AvenueTableList v-if="avenues" :avenues="avenues" />
    </div>
  </div>
</template>