<script setup lang="ts">

const route = useRoute();
const config = useRuntimeConfig();

</script>

<template>
    <div class="container">
        Page of {{ route.params.slug }}
    </div>
</template>

<style scoped>

.container {
  margin-top: 20px;
  padding-top: 3%;
}
</style>