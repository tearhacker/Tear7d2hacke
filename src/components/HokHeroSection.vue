<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import CyberGrid from './CyberGrid.vue'

const { t } = useI18n()
</script>

<template>
  <section id="hero-hok" class="relative py-28 px-6 overflow-hidden">
    <CyberGrid />

    <!-- Central glow — purple/gold themed -->
    <div
      class="absolute pointer-events-none"
      :style="{
        width: '600px',
        height: '600px',
        left: '50%',
        top: '50%',
        transform: 'translate(-50%, -50%)',
        background: 'radial-gradient(circle, rgba(157,78,221,0.12) 0%, rgba(255,215,0,0.06) 40%, transparent 70%)',
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
        style="font-size: clamp(36px, 5vw, 56px); font-weight: 800; color: var(--text-primary); text-shadow: 0 0 30px rgba(157,78,221,0.4);"
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
        class="mb-8"
        style="font-size: 16px; color: var(--text-secondary); line-height: 1.8;"
      >
        {{ t('projects.p2.desc') }}
      </p>

      <!-- Status badge -->
      <div
        class="inline-flex items-center gap-3 px-8 py-4 rounded-pill"
        style="background: rgba(0,255,136,0.08); border: 1px solid rgba(0,255,136,0.2);"
      >
        <span
          class="w-3 h-3 rounded-full"
          style="background: #00FF88; animation: pulse-dot 1.5s ease-in-out infinite;"
        />
        <span
          class="font-mono font-bold text-lg tracking-widest"
          style="color: #00FF88;"
        >
          {{ t('projects.p2.status') }}
        </span>
      </div>

      <!-- Stats row -->
      <div
        class="mt-12 mx-auto flex items-center justify-center gap-8"
      >
        <div class="text-center">
          <p style="font-size: 28px; font-weight: 800; color: #9D4EDD; font-family: var(--font-display);">2469</p>
          <p style="font-size: 11px; color: rgba(255,255,255,0.4); font-family: var(--font-mono); letter-spacing: 1px;">LINES CODE</p>
        </div>
        <div style="width: 1px; height: 40px; background: rgba(157,78,221,0.2);" />
        <div class="text-center">
          <p style="font-size: 28px; font-weight: 800; color: #FFD700; font-family: var(--font-display);">10</p>
          <p style="font-size: 11px; color: rgba(255,255,255,0.4); font-family: var(--font-mono); letter-spacing: 1px;">FEATURE MODS</p>
        </div>
        <div style="width: 1px; height: 40px; background: rgba(157,78,221,0.2);" />
        <div class="text-center">
          <p style="font-size: 28px; font-weight: 800; color: #00FF88; font-family: var(--font-display);">3</p>
          <p style="font-size: 11px; color: rgba(255,255,255,0.4); font-family: var(--font-mono); letter-spacing: 1px;">CORE MODULES</p>
        </div>
      </div>

      <!-- Decorative line -->
      <div
        class="mt-12 mx-auto"
        style="width: 200px; height: 2px; background: repeating-linear-gradient(90deg, #9D4EDD26 0px, #9D4EDD26 8px, transparent 8px, transparent 16px);"
      />
    </div>
  </section>
</template>
