<script setup lang="ts">
import { ref, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'
import FaqItem from './FaqItem.vue'

const { t, locale } = useI18n()

const items = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  const raw = msgs.hokFaq?.items
  if (!raw || !Array.isArray(raw)) return []
  return raw as Array<{q: string; a: string}>
})
const openIndex = ref<number | null>(null)

function toggle(index: number) {
  openIndex.value = openIndex.value === index ? null : index
}
</script>

<template>
  <section id="hok-faq" class="py-24 px-6">
    <div class="mx-auto" style="max-width: 880px;">
      <!-- Section Tag -->
      <p
        class="text-center mb-2"
        style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: var(--font-mono); letter-spacing: 2px;"
      >
        {{ t('hokFaq.sectionTag') }}
      </p>

      <!-- Title -->
      <h2
        class="text-center mb-2"
        style="font-size: 36px; font-family: Consolas, var(--font-mono); font-weight: 700; color: var(--text-primary);"
      >
        {{ t('hokFaq.title') }}
      </h2>

      <!-- Subtitle -->
      <p
        class="text-center mb-14"
        style="font-size: 14px; color: var(--text-secondary);"
      >
        {{ t('hokFaq.subtitle') }}
      </p>

      <!-- FAQ Items -->
      <div class="space-y-3">
        <FaqItem
          v-for="(item, i) in items"
          :key="i"
          :question="item.q"
          :answer="item.a"
          :is-open="openIndex === i"
          :toggle="() => toggle(i)"
        />
      </div>
    </div>
  </section>
</template>
