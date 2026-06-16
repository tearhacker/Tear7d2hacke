<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'

const { t, locale } = useI18n()

const items = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  const raw = msgs.cooperate?.cards
  if (!raw || !Array.isArray(raw)) return []
  return raw as Array<{icon: string; title: string; desc: string}>
})

// SVG icon paths for each icon name
const iconPaths: Record<string, string> = {
  chat: 'M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z',
  handshake: 'M20 7h-4l-2 2-4-4-4 4-2-2H4a2 2 0 0 0-2 2v6a2 2 0 0 0 2 2h4l4 4 4-4h4a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2z',
  bug: 'M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 17.93c-3.95-.49-7-3.85-7-7.93 0-.62.08-1.21.21-1.79L9 15v1c0 1.1.9 2 2 2v1.93z',
  bulb: 'M9 21c0 .55.45 1 1 1h4c.55 0 1-.45 1-1v-1H9v1zm3-19C8.14 2 5 5.14 5 9c0 2.38 1.19 4.47 3 5.74V17c0 .55.45 1 1 1h6c.55 0 1-.45 1-1v-2.26c1.81-1.27 3-3.36 3-5.74 0-3.86-3.14-7-7-7z',
  doc: 'M14 2H6c-1.1 0-1.99.9-1.99 2L4 20c0 1.1.89 2 1.99 2H18c1.1 0 2-.9 2-2V8l-6-6zm2 16H8v-2h8v2zm0-4H8v-2h8v2zm-3-5V3.5L18.5 9H13z',
  star: 'M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z',
}
</script>

<template>
  <section id="cooperate" class="py-24 px-6 relative">
    <div class="max-w-6xl mx-auto">
      <!-- Section Tag -->
      <p
        class="text-center mb-2"
        style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: var(--font-mono); letter-spacing: 2px;"
      >
        {{ t('cooperate.sectionTag') }}
      </p>

      <!-- Title -->
      <h2
        class="text-center mb-2"
        style="font-size: 36px; font-family: var(--font-heading); font-weight: 700; color: var(--text-primary);"
      >
        {{ t('cooperate.title') }}
      </h2>

      <!-- Subtitle -->
      <p
        class="text-center mb-16"
        style="font-size: 14px; color: var(--text-secondary);"
      >
        {{ t('cooperate.subtitle') }}
      </p>

      <!-- Cards Grid -->
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 mb-16">
        <div
          v-for="(card, i) in items"
          :key="i"
          class="cyber-card l-corner p-6 flex flex-col"
        >
          <!-- SVG Icon -->
          <div
            class="mb-4 flex items-center justify-center"
            style="width: 40px; height: 40px; border-radius: 8px; background: rgba(0,229,255,0.08); border: 1px solid rgba(0,229,255,0.15);"
          >
            <svg
              width="20" height="20" viewBox="0 0 24 24"
              fill="none" stroke="#00E5FF" stroke-width="1.5"
              stroke-linecap="round" stroke-linejoin="round"
            >
              <path :d="iconPaths[card.icon] || iconPaths.chat" />
            </svg>
          </div>
          <h3
            class="mb-2"
            style="font-size: 16px; font-weight: 600; color: var(--text-primary);"
          >
            {{ card.title }}
          </h3>
          <p
            style="font-size: 13px; color: var(--text-secondary); line-height: 1.6;"
          >
            {{ card.desc }}
          </p>
        </div>
      </div>

      <!-- Contact Section -->
      <div class="max-w-2xl mx-auto text-center">
        <h3
          class="mb-2"
          style="font-size: 20px; font-weight: 700; color: var(--text-primary);"
        >
          {{ t('cooperate.contactTitle') }}
        </h3>
        <p
          class="mb-8"
          style="font-size: 14px; color: var(--text-secondary);"
        >
          {{ t('cooperate.contactDesc') }}
        </p>

        <div class="flex flex-wrap items-center justify-center gap-4">
          <!-- QQ -->
          <a
            href="https://qm.qq.com/q/nUq0JhNIM8"
            target="_blank"
            rel="noopener"
            class="cyber-card px-5 py-3 flex items-center gap-3 no-underline transition-all duration-200"
            style="border-radius: 12px;"
          >
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#00E5FF" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
              <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
              <circle cx="9" cy="7" r="4" />
              <path d="M23 21v-2a4 4 0 0 0-3-3.87" />
              <path d="M16 3.13a4 4 0 0 1 0 7.75" />
            </svg>
            <span style="font-size: 13px; color: var(--text-secondary); font-family: var(--font-mono);">
              {{ t('cooperate.qq') }}
            </span>
          </a>
          <!-- GitHub -->
          <a
            href="https://github.com/tearhacker/Tear7d2hacke"
            target="_blank"
            rel="noopener"
            class="cyber-card px-5 py-3 flex items-center gap-3 no-underline transition-all duration-200"
            style="border-radius: 12px;"
          >
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#FFD700" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
              <path d="M9 19c-5 1.5-5-2.5-7-3m14 6v-3.87a3.37 3.37 0 0 0-.94-2.61c3.14-.35 6.44-1.54 6.44-7A5.44 5.44 0 0 0 20 4.77 5.07 5.07 0 0 0 19.91 1S18.73.65 16 2.48a13.38 13.38 0 0 0-7 0C6.27.65 5.09 1 5.09 1A5.07 5.07 0 0 0 5 4.77a5.44 5.44 0 0 0-1.5 3.78c0 5.42 3.3 6.61 6.44 7A3.37 3.37 0 0 0 9 18.13V22" />
            </svg>
            <span style="font-size: 13px; color: var(--text-secondary); font-family: var(--font-mono);">
              {{ t('cooperate.github') }}
            </span>
          </a>
        </div>
      </div>
    </div>
  </section>
</template>
