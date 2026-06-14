<script setup lang="ts">
import NavBar from '@/components/NavBar.vue'
import HeroSection from '@/components/HeroSection.vue'
import ProjectsSection from '@/components/ProjectsSection.vue'
import FeaturesSection from '@/components/FeaturesSection.vue'
import ShowcaseSection from '@/components/ShowcaseSection.vue'
import TechStackSection from '@/components/TechStackSection.vue'
import InstallSection from '@/components/InstallSection.vue'
import RoadmapSection from '@/components/RoadmapSection.vue'
import FaqSection from '@/components/FaqSection.vue'
import CooperateSection from '@/components/CooperateSection.vue'
import HelpSection from '@/components/HelpSection.vue'
import FooterSection from '@/components/FooterSection.vue'
import { useScrollAnimation } from '@/composables/useScrollAnimation'
import { useKonamiCode } from '@/composables/useKonamiCode'
import { ref } from 'vue'

useScrollAnimation()

const showKonami = ref(false)
useKonamiCode(() => {
  showKonami.value = true
  setTimeout(() => { showKonami.value = false }, 5000)
})
</script>

<template>
  <div class="min-h-screen bg-deep-space">
    <NavBar />
    <main>
      <!-- 门户首页 -->
      <HeroSection />
      <ProjectsSection />

      <!-- 项目一：七日杀详情区 -->
      <ShowcaseSection />
      <FeaturesSection />
      <TechStackSection />
      <InstallSection />
      <RoadmapSection />
      <FaqSection />

      <!-- 合作交流 & 帮助 -->
      <CooperateSection />
      <HelpSection />
    </main>
    <FooterSection />

    <!-- Konami Code Easter Egg -->
    <Transition name="konami">
      <div
        v-if="showKonami"
        class="fixed inset-0 z-[9999] pointer-events-none flex items-center justify-center"
      >
        <div class="absolute inset-0 bg-deep-space/80" />
        <div class="relative">
          <svg width="400" height="400" viewBox="0 0 400 400" class="animate-radar-sweep">
            <circle cx="200" cy="200" r="180" fill="none" stroke="#00E5FF" stroke-width="1" opacity="0.3" />
            <circle cx="200" cy="200" r="130" fill="none" stroke="#00E5FF" stroke-width="1" opacity="0.4" />
            <circle cx="200" cy="200" r="80" fill="none" stroke="#00E5FF" stroke-width="1" opacity="0.5" />
            <circle cx="200" cy="200" r="30" fill="none" stroke="#00E5FF" stroke-width="1" opacity="0.6" />
            <line x1="20" y1="200" x2="380" y2="200" stroke="#00E5FF" stroke-width="0.5" opacity="0.3" />
            <line x1="200" y1="20" x2="200" y2="380" stroke="#00E5FF" stroke-width="0.5" opacity="0.3" />
            <circle cx="200" cy="200" r="4" fill="#00E5FF" />
          </svg>
          <div class="absolute inset-0 flex items-center justify-center">
            <span class="font-display text-2xl text-cyan-brand text-glow-cyan tracking-[8px]">KONAMI</span>
          </div>
        </div>
      </div>
    </Transition>
  </div>
</template>

<style scoped>
.konami-enter-active { transition: opacity 0.3s ease; }
.konami-leave-active { transition: opacity 0.5s ease; }
.konami-enter-from, .konami-leave-to { opacity: 0; }
</style>
