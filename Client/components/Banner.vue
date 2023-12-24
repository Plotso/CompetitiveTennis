<script setup lang="ts">

const props = defineProps({
  backgroundImg: {
    type: String,
    required: true,
    default: "https://cdn.pixabay.com/photo/2016/07/07/16/46/dice-1502706_1280.jpg"
  } , 
  title: {
    type: String,
    required: true
  },
  description: {
    type: String,
    required: true
  }
});
</script>

<template>
    <div class="banner" :style="{ backgroundImage: `url(${backgroundImg})` }">
        <div class="banner-content">
        <h1 class="title">{{title}}</h1>
        <p>{{description}}</p>
            <slot/>
        </div>
    </div>
</template>

<style scoped>
.banner {
  width: 100%;
  height: 500px; /* Adjust height as needed */
  /* Add background image or color */
  background-size: cover;
  background-position: center;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: white;
  text-align: center;
}

.title {
  color: white !important; 
}


.banner-content {
  width: 100%;
  max-width: 1200px; /* Set your desired maximum width */
  padding: 20px;
  margin-top: 5%;
}
</style>