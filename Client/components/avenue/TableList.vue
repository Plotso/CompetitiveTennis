<script setup lang="ts">
import { defineProps } from 'vue';
import { AvenueOutputModel, Surface, CourtsInfo, CourtType } from '~/types';

const props = defineProps({
  avenues: {
    type: Array as () => AvenueOutputModel[],
    required: true
  },
  showTableHeaders: Boolean
});

// const getSurfaceLabel = (surface: Surface): string => {
//   return Number.isInteger(surface) ? Surface[surface] : surface.toString();
// };

const getSurfaceLabel = (surface: Surface): string => Surface[surface];
const getCourtTypeLabel = (courtType: CourtType): string => CourtType[courtType];

// Helper to extract distinct court types from the avenue’s courts data
// const getDistinctCourtTypes = (avenue: AvenueOutputModel): string[] => {
//   const types = new Set<string>();
//   avenue.courts.forEach((court: CourtsInfo) => {
//     Object.keys(court.availableCourtsByType).forEach((type) => types.add(type));
//   });
//   return Array.from(types);
// };
// const getDistinctCourtTypes = (avenue: AvenueOutputModel): CourtType[] => {
//   const allCourtTypes = avenue.courts.flatMap(court =>
//     Object.keys(court.availableCourtsByType).map(key => parseInt(key) as CourtType)
//   );
//   return Array.from(new Set(allCourtTypes));
// }
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
  <div class="table-container">
    <table class="table is-striped is-fullwidth">
      <thead v-if="showTableHeaders">
        <tr>
          <th>Image</th>
          <th>Info</th>
          <th>Surfaces</th>
          <th>Court Types</th>
          <th>Verification</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="avenue in avenues" :key="avenue.id">
          <td>
            <img alt="avenue badge"
                 src="https://previews.123rf.com/images/woters/woters1606/woters160600042/57889049-tennis-club-vintage-badge-symbol-or-logo-design-template.jpg"
                 width="75" height="75">
          </td>
          <td>
            <NuxtLink :to="`/avenues/${avenue.id}`" class="custom-link has-text-weight-semibold">
              {{ avenue.name }}
            </NuxtLink>
            <p class="mb-1">
              {{ avenue.city }}, {{ avenue.location }}
            </p>
          </td>
          <td>
            <div class="tags">
              <span class="tag" v-for="courtInfo in avenue.courts" :key="courtInfo.surface">
                {{ courtInfo.surface }}
              </span>
            </div>
          </td>
          <td>
            <div class="tags">
              <span class="tag" v-for="type in getDistinctCourtTypes(avenue)" :key="type">
                {{ type }}
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
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
/* Add any styles you need here */
</style>
