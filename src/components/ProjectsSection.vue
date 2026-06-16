<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'

const { t, locale } = useI18n()

// Bypass t() for object data - access messages directly
const p1 = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  return msgs.projects?.p1
})
const p2 = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  return msgs.projects?.p2
})

function scrollTo(href: string) {
  if (href === '#') return
  const el = document.querySelector(href)
  if (el) el.scrollIntoView({ behavior: 'smooth' })
}
</script>

<template>
  <section id="projects" class="py-24 px-6 relative">
    <div class="max-w-6xl mx-auto">
      <!-- Section Tag -->
      <p
        class="text-center mb-2"
        style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: var(--font-mono); letter-spacing: 2px;"
      >
        {{ t('projects.sectionTag') }}
      </p>

      <!-- Title -->
      <h2
        class="text-center mb-2"
        style="font-size: 36px; font-family: var(--font-heading); font-weight: 700; color: var(--text-primary);"
      >
        {{ t('projects.title') }}
      </h2>

      <!-- Subtitle -->
      <p
        class="text-center mb-16"
        style="font-size: 14px; color: var(--text-secondary);"
      >
        {{ t('projects.subtitle') }}
      </p>

      <!-- Project Cards Grid -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
        <!-- Project 1: 七日杀 -->
        <div v-if="p1" class="cyber-card l-corner p-8 flex flex-col">
          <!-- Tag + Status -->
          <div class="flex items-center justify-between mb-5">
            <span
              style="font-size: 11px; font-family: var(--font-mono); color: var(--cyan); letter-spacing: 2px;"
            >
              {{ p1.tag }}
            </span>
            <span
              class="inline-flex items-center gap-2 px-3 py-1 rounded-pill text-xs font-mono tracking-wider"
              style="background: rgba(0,255,136,0.1); border: 1px solid rgba(0,255,136,0.3); color: #00FF88;"
            >
              <span class="w-1.5 h-1.5 rounded-full" style="background: #00FF88; animation: pulse-dot 1.5s ease-in-out infinite;" />
              {{ p1.status }}
            </span>
          </div>

          <!-- Name -->
          <h3
            class="mb-1"
            style="font-size: 28px; font-weight: 800; color: var(--text-primary);"
          >
            {{ p1.name }}
          </h3>
          <p
            class="mb-5"
            style="font-size: 13px; font-family: var(--font-mono); color: #00E5FF; letter-spacing: 2px; opacity: 0.7;"
          >
            {{ p1.nameEn }}
          </p>

          <!-- Desc -->
          <p
            class="mb-6"
            style="font-size: 14px; color: var(--text-secondary); line-height: 1.7;"
          >
            {{ p1.desc }}
          </p>

          <!-- Features as list -->
          <div class="grid grid-cols-2 gap-x-4 gap-y-2 mb-8">
            <div
              v-for="(feat, i) in p1.features"
              :key="i"
              class="flex items-center gap-2"
            >
              <span class="w-1 h-1 rounded-full flex-shrink-0" style="background: #00E5FF;" />
              <span style="font-size: 13px; font-family: var(--font-mono); color: rgba(255,255,255,0.75);">{{ feat }}</span>
            </div>
          </div>

          <!-- CTA + Key Button v2 -->
          <div class="mt-auto flex items-center gap-3">
            <button
              class="btn-gradient px-6 py-2.5 text-sm font-bold uppercase tracking-wider"
              style="font-family: var(--font-mono)"
              @click="scrollTo(p1.btnHref)"
            >
              {{ p1.btnText }}
            </button>
            <button
              class="px-5 py-2.5 text-sm font-bold uppercase tracking-wider rounded-lg transition-all hover:scale-105"
              style="font-family: var(--font-mono); background: linear-gradient(135deg, rgba(139,0,0,0.3), rgba(220,20,60,0.2)); border: 1px solid rgba(220,20,60,0.4); color: #DC143C;"
              @click="scrollTo('#install')"
            >
              免费获取卡密
            </button>
          </div>
        </div>

        <!-- Project 2: 王者荣耀 -->
        <div v-if="p2" class="cyber-card l-corner p-8 flex flex-col">
          <!-- Tag + Status -->
          <div class="flex items-center justify-between mb-5">
            <span
              style="font-size: 11px; font-family: var(--font-mono); color: var(--cyan); letter-spacing: 2px;"
            >
              {{ p2.tag }}
            </span>
            <span
              class="inline-flex items-center gap-2 px-3 py-1 rounded-pill text-xs font-mono tracking-wider"
              style="background: rgba(0,255,136,0.1); border: 1px solid rgba(0,255,136,0.3); color: #00FF88;"
            >
              <span class="w-1.5 h-1.5 rounded-full" style="background: #00FF88; animation: pulse-dot 1.5s ease-in-out infinite;" />
              {{ p2.status }}
            </span>
          </div>

          <!-- Name -->
          <h3
            class="mb-1"
            style="font-size: 28px; font-weight: 800; color: var(--text-primary);"
          >
            {{ p2.name }}
          </h3>
          <p
            class="mb-5"
            style="font-size: 13px; font-family: var(--font-mono); color: #9D4EDD; letter-spacing: 2px; opacity: 0.7;"
          >
            {{ p2.nameEn }}
          </p>

          <!-- Desc -->
          <p
            class="mb-6"
            style="font-size: 14px; color: var(--text-secondary); line-height: 1.7;"
          >
            {{ p2.desc }}
          </p>

          <!-- Features as list -->
          <div class="grid grid-cols-2 gap-x-4 gap-y-2 mb-8">
            <div
              v-for="(feat, i) in p2.features"
              :key="i"
              class="flex items-center gap-2"
            >
              <span class="w-1 h-1 rounded-full flex-shrink-0" style="background: #9D4EDD;" />
              <span style="font-size: 13px; font-family: var(--font-mono); color: rgba(255,255,255,0.75);">{{ feat }}</span>
            </div>
          </div>

          <!-- CTA Buttons -->
          <div class="mt-auto flex items-center gap-3">
            <button
              class="btn-gradient px-6 py-2.5 text-sm font-bold uppercase tracking-wider"
              style="font-family: var(--font-mono)"
              @click="scrollTo(p2.btnHref)"
            >
              {{ p2.btnText }}
            </button>
            <button
              class="px-5 py-2.5 text-sm font-bold uppercase tracking-wider rounded-lg transition-all hover:scale-105"
              style="font-family: var(--font-mono); background: linear-gradient(135deg, rgba(157,78,221,0.3), rgba(0,229,255,0.2)); border: 1px solid rgba(157,78,221,0.4); color: #9D4EDD;"
              @click="scrollTo('#cooperate')"
            >
              加入社区
            </button>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
