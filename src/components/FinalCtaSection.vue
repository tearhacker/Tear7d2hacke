<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import CyberGrid from './CyberGrid.vue'

const { t } = useI18n()
</script>

<template>
  <section
    id="download"
    class="relative w-full flex items-center justify-center overflow-hidden"
    style="height: 320px;"
  >
    <!-- Cyber Grid Background -->
    <CyberGrid />

    <!-- Central Radial Glow (breathing 4s) -->
    <div
      class="absolute inset-0 pointer-events-none"
      :style="{
        background: 'radial-gradient(ellipse 1000px 400px at 50% 50%, rgba(0,229,255,0.12), rgba(157,78,221,0.08), transparent 70%)',
        animation: 'breathe 4s ease-in-out infinite',
        zIndex: 1,
      }"
    />

    <!-- Content -->
    <div class="relative z-10 text-center px-6">
      <!-- Tag -->
      <p
        class="font-mono text-[14px] tracking-[10px] uppercase mb-4"
        style="color: var(--text-weak);"
      >
        {{ t('cta.tag') }}
      </p>

      <!-- Main Title -->
      <h2
        class="font-mono font-bold text-[56px] leading-tight mb-3"
        style="color: var(--text-primary); text-shadow: 0 0 40px rgba(0,229,255,0.6), 0 0 80px rgba(0,229,255,0.3);"
      >
        {{ t('cta.title') }}
      </h2>

      <!-- Subtitle -->
      <p
        class="font-body text-base mb-8"
        style="color: var(--text-secondary);"
      >
        {{ t('cta.subtitle') }}
      </p>

      <!-- Dual CTA -->
      <div class="flex items-center justify-center gap-4 mb-6">
        <a
          href="#"
          class="btn-gradient inline-flex items-center justify-center font-mono font-bold text-base"
          style="width: 320px; height: 64px;"
        >
          {{ t('cta.btnDownload') }}
        </a>
        <a
          href="https://github.com/tearhacker"
          class="btn-outline inline-flex items-center justify-center font-mono font-semibold text-sm px-6"
          style="height: 64px;"
        >
          {{ t('cta.btnSource') }}
        </a>
      </div>

      <!-- Footer Info -->
      <p
        class="font-mono text-xs"
        style="color: var(--text-weak);"
      >
        {{ t('cta.footer') }}
      </p>
    </div>
  </section>
</template>
