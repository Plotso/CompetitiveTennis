<script setup lang="ts">
//ToDo: SEO optimise this page
const points = [
  {
    id: 1,
    logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
    fa_logo: 'fa-solid fa-globe',
    title: 'Be Part of Something Bigger',
    description: 'Join a platform that aims to grow the sport of tennis, foster community engagement, and create opportunities for players of all ages and skill levels.',
  },
  {
    id: 2,
    logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
    fa_logo: 'fa-solid fa-users',
    title: 'Community and Camaraderie',
    description: 'Easily find and connect with tennis enthusiasts in your area, arrange matches, and build lasting friendships within a vibrant tennis community.',
  },
  {
    id: 3,
    logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
    fa_logo: 'fa-solid fa-calendar-alt',
    title: 'Seamless Tournament Management',
    description: 'Effortlessly join or create tournaments, manage registrations, and track results through our user-friendly platform.',
  },
  {
    id: 4,
    logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
    fa_logo: 'fa-solid fa-trophy',
    title: 'Fair and Innovative Draw System',
    description: 'Compete with confidence in tournaments with a fair draw system. Our rating-based draw generation ensures exciting and balanced matches for everyone.',
  },
  {
    id: 5,
    logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
    fa_logo: 'fa-solid fa-chart-line',
    title: 'Build Your Tennis Profile',
    description: 'Track your progress with a dynamic player profile. Showcase your skills, climb the rankings, and watch your tennis journey unfold.',
  },
  {
    id: 6,
    logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
    fa_logo: 'fa-solid fa-arrow-up-right-dots',
    title: 'Boost Your Skills',
    description: 'Participate in tournaments, and challenge yourself to improve your game in a supportive environment',
  },
  {
    id: 7,
    logo: 'https://img.freepik.com/premium-vector/location-point-icon-vector-illustration_9999-17225.jpg?w=2000',
    fa_logo: 'fa-solid fa-heart-pulse',
    title: 'Promote an Active Lifestyle & Boost Your Sport Enthusiasm',
    description: 'Engage in regular physical activity, improve your health, and experience the numerous physical and mental benefits of competitive tennis. Turn your passion into a lifestyle while having fun and staying fit.',
  }
];


</script>

<template>
  <div class="points-list">
    <div class="container">
      <div class="columns is-centered is-multiline">
        <div class="column is-4" v-for="point in points" :key="point.id">
          <div class="point">
            <div class="point-header">
              <font-awesome-icon :icon="point.fa_logo" class="point-icon" />
              <h2 class="point-title">{{ point.title }}</h2>
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
  padding-top: 1%;
}

.point {
  /* Add styles for each point */
  text-align: center;
  margin-bottom: 1.5rem;
}

.point-header {
  /* Style the header (icon + title) */
  display: flex;
  flex-direction: column;
  align-items: center;
  color: #00d1b2;
}

.point-icon {
  font-size: 3rem; /* Adjust icon size */
  margin-bottom: 0.5rem;
}

.point-title {
  font-size: 1.75rem; /* Adjust title size */
  font-weight: bold;
  color: #00d1b2;
  margin: 0; /* Ensure no extra margin around */
}

.point-description p {
  margin-top: 1rem;
}
</style>

