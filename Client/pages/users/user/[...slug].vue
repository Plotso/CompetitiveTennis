<script setup lang="ts">

const route = useRoute();
const config = useRuntimeConfig();

</script>

<template>
    <div>
        Page of {{ route.params.slug }}
    </div>
</template>

<style scoped>

</style>