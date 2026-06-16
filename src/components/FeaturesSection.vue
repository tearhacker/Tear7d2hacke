<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import FeatureCard from './FeatureCard.vue'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'

const { t, locale } = useI18n()

// Bypass t() for array data - access messages directly
const cards = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  const raw = msgs.features?.cards
  if (!raw || !Array.isArray(raw)) return []
  return raw
})
</script>

<template>
  <section id="project-7d2d" class="scroll-enter py-24 px-6">
    <div class="max-w-[1120px] mx-auto">
      <p style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: Consolas, monospace; letter-spacing: 2px; margin-bottom: 8px;">
        {{ t('features.sectionTag') }}
      </p>
      <h2 style="font-size: 36px; font-weight: 700; color: #E8ECF4; margin-bottom: 8px;">
        {{ t('features.title') }}
      </h2>
      <p style="font-size: 14px; color: #B0B8C8; margin-bottom: 56px;">
        {{ t('features.subtitle') }}
      </p>

      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <FeatureCard
          v-for="card in cards"
          :key="card.num"
          :num="card.num"
          :title="card.title"
          :title-en="card.titleEn"
          :desc="card.desc"
          :items="card.items"
        />
      </div>
    </div>
  </section>
</template>
