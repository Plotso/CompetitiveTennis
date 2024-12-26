<script setup lang="ts">
definePageMeta({
  layout: "default-transparent",
});
import { AvenueOutputModel, Result, Surface, CourtsInfo, CourtType } from '@/types';
import { useAuthStore } from '@/stores/auth'
const config = useRuntimeConfig();
const authStore = useAuthStore();

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
    return Number.isInteger(surface) ? Surface[surface] : surface.toString();
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
        <div v-if="pending">
            <Loading></Loading>
        </div>
        <div class="container" v-else>

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
                                    <span class="tag" v-for="courtInfo in avenue.courts" :key="courtInfo.surface">{{
                                        getSurfaceLabel(courtInfo.surface) }}</span>
                                </div>
                            </td>
                            <td>
                                <div class="tags" v-for="courtInfo in getDistinctCourtTypes(avenue)" :key="courtInfo">
                                    <span class="tag">
                                        {{ courtInfo }}
                                        <br>
                                    </span>
                                </div>
                            </td>
                            <td>
                                <div class="tags">
                                    <span class="tag">
                                        <font-awesome-icon v-if="avenue.isVerified" icon="fa-solid fa-check" />
                                        <font-awesome-icon v-else icon="fa-solid fa-xmark" />
                                        {{ avenue.isVerified ? 'Verified' : 'Not Verified' }}
                                    </span>
                                </div>
                            </td>
                            <hr>
                        </tr>
                    </tbody>
                </table>

            </div>
        </div>
    </div>
</template>

<style scoped></style>