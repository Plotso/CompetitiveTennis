<script setup lang="ts">
import { useTournamentsApi } from '~/composables/useTournamentsApi';
import { AccountQuery, SortOptions, SearchOutputModel, Result, AccountOutputModel, AccountSortOptions } from '~/types';

const accounts = ref<AccountOutputModel[]>([]);
const showLoadingModal = ref(true);

const props = defineProps({
    keyword: {
        type: String,
        required: false
    },
    page: {
        type: Number,
        required: false
    },
    itemsPerPage: {
        type: Number,
        required: false
    },
    sortOptions: {
        type: SortOptions,
        required: false
    },
    accountSortOptions: {
        type: Number as PropType<AccountSortOptions>,
        required: false
    },
    showSinglesRating: Boolean,
    showDoublesRating: Boolean,
});


const errorNotification = ref("")
const showErrorNotification = ref(false)

const hideErrorNotification = () => {
    showErrorNotification.value = false;
}

console.log(props.accountSortOptions);

const query: AccountQuery = {    
    keyword: props.keyword,
    sortOptions: props.sortOptions,
    additionalSortOptions: props.accountSortOptions,
    page: props.page ? props.page : 1,
    itemsPerPage: props.itemsPerPage ? props.itemsPerPage : 10
};
const method = 'GET';
const options = {
    query,
    method
}
const apiResponse = await useTournamentsApi<Result<SearchOutputModel<AccountOutputModel>>>('/Accounts/Search', options);
watchEffect(() => {
    if (apiResponse.error.value) {
    errorNotification.value = "Error loading accounts"
    showErrorNotification.value = true
    showLoadingModal.value = false
  }  
  if (apiResponse.data.value?.data.results) {
    accounts.value = apiResponse.data.value.data.results
    showLoadingModal.value = false
  }
})
</script>

<template>
  <div class="container">
    <div v-if="showLoadingModal">
        <BaseLoading></BaseLoading>
    </div>
    <div v-else>
        <div class="notification is-danger" v-show="showErrorNotification">
                <button class="delete" @click="hideErrorNotification"></button>
                {{errorNotification}}
            </div>
        <AccountTableList v-if="accounts"
            :accounts="accounts"
            :showSinglesRating="showSinglesRating"
            :showDoublesRating="showDoublesRating"
        />
    </div>
  </div>
</template>

<style scoped>
header {
    
  display: flex;
  flex-direction: column;
  align-items: center;
}

h1 strong {
  font-size: 1.75rem; /* Adjust title size */
  font-weight: bold;
  color: #00d1b2;
  margin: 0; /* Ensure no extra margin around */
}

.summary {
    font-size: 18px;
    line-height: 24px;
}
</style>