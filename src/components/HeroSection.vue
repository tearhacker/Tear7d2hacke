<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import HeroRadar from './HeroRadar.vue'

const { t } = useI18n()

function scrollToProjects() {
  document.querySelector('#projects')?.scrollIntoView({ behavior: 'smooth' })
}
</script>

<template>
  <section
    id="home"
    class="relative flex items-center overflow-hidden"
    :style="{
      minHeight: '720px',
      height: '100vh',
      background: 'var(--deep-space)',
    }"
  >
    <!-- Cyber grid background -->
    <div class="cyber-grid absolute inset-0 pointer-events-none" />

    <!-- Central radial glow (breathing) -->
    <div
      class="absolute pointer-events-none"
      :style="{
        width: '800px',
        height: '800px',
        left: '50%',
        top: '50%',
        transform: 'translate(-50%, -50%)',
        background: 'radial-gradient(circle, rgba(0,229,255,0.12) 0%, rgba(157,78,221,0.08) 40%, transparent 70%)',
        animation: 'breathe 2.2s ease-in-out infinite',
      }"
    />

    <!-- Scanline animation -->
    <div
      class="absolute left-0 right-0 pointer-events-none"
      :style="{
        height: '2px',
        background: 'linear-gradient(90deg, transparent, rgba(0,229,255,0.25), transparent)',
        animation: 'scanline 2.6s linear infinite',
      }"
    />

    <!-- Content -->
    <div class="relative z-10 w-full max-w-7xl mx-auto px-6 flex flex-col lg:flex-row items-center gap-10 lg:gap-0">
      <!-- Left side (60%) -->
      <div class="flex-1 lg:flex-[3] flex flex-col gap-5">
        <!-- Status badge -->
        <div class="flex items-center gap-2">
          <span
            class="inline-block rounded-full"
            :style="{
              width: '8px',
              height: '8px',
              background: 'var(--online)',
              animation: 'pulse-dot 1.5s ease-in-out infinite',
            }"
          />
          <span
            class="uppercase tracking-wider"
            :style="{
              fontSize: '12px',
              fontFamily: 'Consolas, monospace',
              color: 'var(--online)',
            }"
          >
            {{ t('hero.statusOnline') }}
          </span>
          <span
            :style="{
              fontSize: '12px',
              fontFamily: 'Consolas, monospace',
              color: 'var(--text-weak)',
            }"
          >
            {{ t('hero.version') }}
          </span>
        </div>

        <!-- Main title -->
        <h1
          class="text-glow-cyan leading-tight"
          :style="{
            fontSize: 'clamp(56px, 7vw, 88px)',
            fontFamily: 'var(--font-body)',
            fontWeight: 900,
            color: '#FFFFFF',
            textShadow: '0 0 40px rgba(0,229,255,0.5), 0 0 80px rgba(0,229,255,0.2)',
          }"
        >
          {{ t('hero.titleCn') }}
        </h1>

        <!-- Second line -->
        <h2
          :style="{
            fontSize: 'clamp(20px, 2.5vw, 32px)',
            fontFamily: 'Consolas, monospace',
            letterSpacing: '6px',
            color: '#00E5FF',
          }"
        >
          {{ t('hero.titleEn') }}
        </h2>

        <!-- Subtitle -->
        <p
          :style="{
            fontSize: '16px',
            color: 'rgba(255,255,255,0.7)',
            maxWidth: '560px',
          }"
        >
          {{ t('hero.subtitle') }}
        </p>

        <!-- Feature lines -->
        <div class="flex flex-col gap-2 mt-1">
          <div class="flex items-center gap-2" :style="{ fontSize: '14px', color: 'rgba(255,255,255,0.65)' }">
            <svg width="8" height="8" viewBox="0 0 8 8" fill="none" stroke="#00E5FF" stroke-width="1"><polygon points="4,0 8,4 4,8 0,4" /></svg>
            <span>{{ t('hero.desc1') }}</span>
          </div>
          <div class="flex items-center gap-2" :style="{ fontSize: '14px', color: 'rgba(255,255,255,0.65)' }">
            <svg width="8" height="8" viewBox="0 0 8 8" fill="none" stroke="#9D4EDD" stroke-width="1"><polyline points="4,0 2,3 5,3 3,8" /></svg>
            <span>{{ t('hero.desc2') }}</span>
          </div>
          <div class="flex items-center gap-2" :style="{ fontSize: '14px', color: 'rgba(255,255,255,0.65)' }">
            <svg width="8" height="8" viewBox="0 0 8 8" fill="none" stroke="#00E5FF" stroke-width="1"><circle cx="4" cy="4" r="3" /></svg>
            <span>{{ t('hero.desc3') }}</span>
          </div>
        </div>

        <!-- CTA buttons -->
        <div class="flex items-center gap-4 mt-4">
          <button class="btn-gradient px-8 py-3 text-sm font-bold uppercase tracking-wider" style="font-family: var(--font-mono)" @click="scrollToProjects">
            {{ t('hero.btnExplore') }}
          </button>
          <a href="https://github.com/tearhacker" target="_blank" rel="noopener" class="btn-outline px-8 py-3 text-sm font-semibold uppercase tracking-wider no-underline" style="font-family: var(--font-mono); display: inline-flex; align-items: center;">
            {{ t('hero.btnGithub') }}
          </a>
        </div>
      </div>

      <!-- Right side (40%) -->
      <div class="flex-1 lg:flex-[2] flex flex-col items-center justify-center relative">
        <!-- Radar glow -->
        <div
          class="absolute pointer-events-none"
          :style="{
            width: '600px',
            height: '600px',
            background: 'radial-gradient(circle, rgba(0,229,255,0.08) 0%, transparent 60%)',
            animation: 'breathe 2.2s ease-in-out infinite',
          }"
        />

        <!-- Radar component -->
        <HeroRadar />

        <!-- Floating data badges -->
        <div
          class="absolute flex items-center gap-1.5 px-3 py-1.5 rounded-md"
          :style="{
            top: '10%',
            right: '0',
            background: '#1A1F2EE6',
            border: '1px solid #00E5FF26',
            fontFamily: 'Consolas, monospace',
            fontSize: '11px',
          }"
        >
          <span style="color: var(--text-weak)">{{ t('hero.scanLabel') }}</span>
          <span style="color: #00E5FF">{{ t('hero.scanValue') }}</span>
        </div>

        <div
          class="absolute flex items-center gap-1.5 px-3 py-1.5 rounded-md"
          :style="{
            bottom: '25%',
            left: '0',
            background: '#1A1F2EE6',
            border: '1px solid #00E5FF26',
            fontFamily: 'Consolas, monospace',
            fontSize: '11px',
          }"
        >
          <span style="color: var(--text-weak)">{{ t('hero.pingLabel') }}</span>
          <span style="color: var(--online)">{{ t('hero.pingValue') }}</span>
        </div>

        <div
          class="absolute flex items-center gap-1.5 px-3 py-1.5 rounded-md"
          :style="{
            bottom: '8%',
            right: '5%',
            background: '#1A1F2EE6',
            border: '1px solid #00E5FF26',
            fontFamily: 'Consolas, monospace',
            fontSize: '11px',
          }"
        >
          <span style="color: var(--text-weak)">{{ t('hero.modLabel') }}</span>
          <span style="color: #9D4EDD">{{ t('hero.modValue') }}</span>
        </div>
      </div>
    </div>
  </section>
</template>
