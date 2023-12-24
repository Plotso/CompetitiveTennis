<script setup lang="ts">
const points = [
        {
          id: 1,
          logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
          description: 'Description for Point 1',
        },
        {
          id: 2,
          logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
          description: 'Description for Point 2',
        },
        {
          id: 3,
          logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
          description: 'Description for Point 1',
        },
        {
          id: 4,
          logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
          description: 'Description for Point 2',
        },
        // Add more points as needed
      ]
</script>

<template>
    <div class="points-list">
      <div class="container">
        <div class="columns is-centered is-multiline">
          <div class="column is-4" v-for="point in points" :key="point.id">
            <div class="point">
              <div class="point-logo">
                <font-awesome-icon icon="fa-solid fa-check" />
              </div>
              <div class="point-description">
                <p>{{ point.description }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>

<style scoped>
.points-list {
  /* Center the component */
  display: flex;
  justify-content: center;
}

.point {
  /* Add styles for each point */
  text-align: center;
  margin-bottom: 1.5rem;
}

.point-logo img {
  max-width: 100%;
  height: auto;
}

.point-description p {
  margin-top: 1rem;
}
</style>