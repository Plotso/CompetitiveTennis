<script setup lang="ts">
import { AvenueOutputModel, Result, Surface, CourtsInfo, CourtType} from '@/types';
const config = useRuntimeConfig();

const { data, pending, refresh, error } = await useFetch<Result<AvenueOutputModel[]>>(() => `/Avenues/All`, {
    baseURL: config.public.tournamentsBase
})
if (error.value) {
    console.log('data', data.value)
    console.log('pending', pending.value)
    console.log('error', error.value)
    refresh()
}

const getSurfaceLabel = (surface: Surface): string => {
    return Surface[surface];
};

const getDistinctCourtTypes = (avenue: AvenueOutputModel): CourtType[] => {
  return Array.from(
    new Set(
      avenue.courts.reduce((types: CourtType[], courts: CourtsInfo) => {
        return [
          ...types,
          ...Object.keys(courts.availableCourtsByType) as CourtType[],
        ];
      }, [])
    )
  );
}; 

</script>
<template>
    <div v-if="pending">
        <Loading></Loading>
    </div>
    <div class="container" v-else>
        <h1 class="title is-1 has-text-centered">All Avenues</h1>
        <div class="table-container">
            <table class="table is-striped is-fullwidth">
                <tbody>
                    <tr v-for="avenue in data.data" :key="avenue.id">
                        <td>
                            <img alt="avenue badge"
                                src="https://previews.123rf.com/images/woters/woters1606/woters160600042/57889049-tennis-club-vintage-badge-symbol-or-logo-design-template.jpg"
                                width="75" height="75">
                        </td>
                        <td>
                            <NuxtLink :to="`/avenues/${avenue.id}`" class="has-text-weight-semibold">{{
                                avenue.name }}</NuxtLink>

                            <p class="mb-1">
                                    {{ avenue.city }}, {{ avenue.location }}
                            </p>
                        </td>
                        <td>
                            <div class="tags">
                                <span class="tag" v-for="courtInfo in avenue.courts"  :key="courtInfo.surface">{{ getSurfaceLabel(courtInfo.surface) }}</span>
                            </div>
                        </td>
                        <td>
                            <div class="tags" v-for="courtInfo in getDistinctCourtTypes(avenue)"  :key="courtInfo">
                                <span class="tag">
                                    {{ courtInfo }}
                                    <br>
                                </span>
                            </div>
                        </td>
                        <td>
                            <div class="tags">
                                <span  class="tag">
                                    <font-awesome-icon v-if="avenue.isVerified" icon="fa-solid fa-check" />
                                    <font-awesome-icon v-else icon="fa-solid fa-xmark" />
                                    {{ avenue.isVerified ? 'Verified' : 'Not Verified' }}
                                </span>
                            </div>
                        </td>
                        <td>
                            <a href="/" class="button is-primary">
                                Participate
                            </a>
                        </td>
                        <hr>
                    </tr>
                    <!-- Add more tournament rows here -->
                </tbody>
            </table>

        </div>
    </div>
</template>

<style scoped>

</style>