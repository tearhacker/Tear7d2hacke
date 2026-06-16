<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'

const { t, locale } = useI18n()

const items = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  const raw = msgs.hokTechstack?.items
  if (!raw || !Array.isArray(raw)) return []
  return raw
})
</script>

<template>
  <section class="py-16 px-6">
    <div class="max-w-5xl mx-auto text-center">
      <!-- Section Tag -->
      <p
        style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: Consolas, monospace; letter-spacing: 2px; margin-bottom: 8px;"
      >
        {{ t('hokTechstack.sectionTag') }}
      </p>

      <!-- Title -->
      <h2
        style="font-size: 36px; font-weight: 700; color: #9D4EDD; text-shadow: 0 0 18px rgba(157,78,221,0.5); margin-bottom: 4px;"
      >
        {{ t('hokTechstack.title') }}
      </h2>

      <!-- Subtitle -->
      <p
        style="font-size: 14px; color: #B0B8C8; margin-bottom: 40px;"
      >
        {{ t('hokTechstack.subtitle') }}
      </p>

      <!-- Badges Row -->
      <div style="display: flex; flex-wrap: wrap; justify-content: center; gap: 12px; margin-bottom: 32px;">
        <div
          v-for="(item, idx) in items"
          :key="idx"
          style="
            display: flex; flex-direction: column; align-items: center; justify-content: center;
            width: 120px; height: 60px;
            background: rgba(20,26,42,0.8); border: 1px solid rgba(157,78,221,0.2);
            border-radius: 8px; transition: all 0.2s;
          "
        >
          <span style="font-size: 13px; font-family: Consolas, monospace; font-weight: 700; color: #E8ECF4;">{{ (item as any).name }}</span>
          <span style="font-size: 9px; font-family: Consolas, monospace; color: rgba(255,255,255,0.35); margin-top: 2px;">{{ (item as any).role }}</span>
        </div>
      </div>

      <!-- Footer -->
      <p style="font-size: 12px; font-family: Consolas, monospace; color: rgba(255,255,255,0.3);">
        {{ t('hokTechstack.footer') }}
      </p>
    </div>
  </section>
</template>
