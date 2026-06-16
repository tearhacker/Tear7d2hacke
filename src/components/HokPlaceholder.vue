<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import CyberGrid from './CyberGrid.vue'

const { t } = useI18n()
</script>

<template>
  <section id="project-hok" class="relative py-32 px-6 overflow-hidden">
    <CyberGrid />

    <!-- Central glow -->
    <div
      class="absolute pointer-events-none"
      :style="{
        width: '600px',
        height: '600px',
        left: '50%',
        top: '50%',
        transform: 'translate(-50%, -50%)',
        background: 'radial-gradient(circle, rgba(255,215,0,0.08) 0%, rgba(157,78,221,0.05) 40%, transparent 70%)',
        animation: 'breathe 4s ease-in-out infinite',
      }"
    />

    <div class="relative z-10 max-w-3xl mx-auto text-center">
      <!-- Tag -->
      <p
        class="mb-4"
        style="font-size: 11px; font-family: var(--font-mono); color: rgba(255,255,255,0.35); letter-spacing: 3px;"
      >
        PROJECT 02  ——  HONOR OF KINGS
      </p>

      <!-- Title -->
      <h2
        class="mb-3"
        style="font-size: clamp(36px, 5vw, 56px); font-weight: 800; color: var(--text-primary); text-shadow: 0 0 30px rgba(255,215,0,0.3);"
      >
        {{ t('projects.p2.name') }}
      </h2>

      <!-- English name -->
      <p
        class="mb-6"
        style="font-size: 18px; font-family: var(--font-mono); color: #9D4EDD; letter-spacing: 4px;"
      >
        {{ t('projects.p2.nameEn') }}
      </p>

      <!-- Description -->
      <p
        class="mb-10"
        style="font-size: 16px; color: var(--text-secondary); line-height: 1.8;"
      >
        {{ t('projects.p2.desc') }}
      </p>

      <!-- 未开发状态大徽章 -->
      <div
        class="inline-flex items-center gap-3 px-8 py-4 rounded-pill"
        style="background: rgba(255,215,0,0.08); border: 1px solid rgba(255,215,0,0.2);"
      >
        <span
          class="w-3 h-3 rounded-full"
          style="background: #FFD700; animation: pulse-dot 2s ease-in-out infinite;"
        />
        <span
          class="font-mono font-bold text-lg tracking-widest"
          style="color: #FFD700;"
        >
          {{ t('projects.p2.status') }}
        </span>
      </div>

      <!-- 斜线遮罩装饰 -->
      <div
        class="mt-12 mx-auto"
        style="width: 200px; height: 2px; background: repeating-linear-gradient(90deg, #FFD70026 0px, #FFD70026 8px, transparent 8px, transparent 16px);"
      />
    </div>
  </section>
</template>
