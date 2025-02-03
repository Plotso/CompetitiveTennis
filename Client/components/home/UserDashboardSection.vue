<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { useAuthStore } from "~/stores/auth"
import { EventStatus } from "@/types"

const { user } = storeToRefs(useAuthStore());
</script>

<template>
    <div class="container">
        <div class="card-list">
            <div class="columns is-centered is-multiline">                        
                <BaseCard
                title="Active Tournaments"
                class="column is-4">
                    <TournamentQueryList :username="user.username" :is-ongoing="true"/>
                </BaseCard>   

                <BaseCard
                title="Upcoming Matches"
                class="column is-4">
                    <HomeUserMatchQueryList :username="user.username" :status="EventStatus.NotStarted"/>
                </BaseCard>

                <BaseCard
                title="Matches"
                class="column is-4">                    
                    <HomeUserMatchQueryList :username="user.username"/>
                </BaseCard>

                <BaseCard
                title="Upcoming Tournaments"
                class="column is-4">
                    <TournamentQueryList :username="user.username" :dateRangeFrom="new Date()"/>
                </BaseCard>

                <BaseCard
                title="Past Tournaments"
                class="column is-4">
                <TournamentQueryList :username="user.username" :date-range-until="new Date()"/>
                </BaseCard>

                <BaseCard
                title="User Stats"
                class="column is-4">
                    <HomeUserAccountStats :username="user.username" />
                </BaseCard>
            </div>
        </div>
    </div>
</template>

<style scoped>
.card-list {
    margin-top: 1rem;
}
</style>