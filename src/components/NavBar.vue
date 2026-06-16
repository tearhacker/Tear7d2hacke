<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useLocale } from '@/composables/useLocale'

const { t } = useI18n()
const { isZh, toggleLocale } = useLocale()
const activeSection = ref('home')
const isScrolled = ref(false)

const navItems = [
  { key: 'home', href: '#home' },
  { key: 'project1', href: '#project-7d2d' },
  { key: 'project2', href: '#project-hok' },
  { key: 'cooperate', href: '#cooperate' },
  { key: 'help', href: '#help' },
]

function scrollTo(href: string) {
  const el = document.querySelector(href)
  if (el) el.scrollIntoView({ behavior: 'smooth' })
}

function handleScroll() {
  isScrolled.value = window.scrollY > 50
  const sections = navItems
    .map(item => ({
      key: item.key,
      el: document.querySelector(item.href),
    }))
    .filter(s => s.el)

  for (let i = sections.length - 1; i >= 0; i--) {
    const el = sections[i].el as HTMLElement
    if (el.getBoundingClientRect().top <= 100) {
      activeSection.value = sections[i].key
      break
    }
  }
}

onMounted(() => window.addEventListener('scroll', handleScroll))
onUnmounted(() => window.removeEventListener('scroll', handleScroll))
</script>

<template>
  <nav
    class="fixed top-0 left-0 right-0 z-50 flex items-center justify-between px-6"
    :style="{
      height: '64px',
      background: isScrolled ? '#0A0E17F2' : '#0A0E17CC',
      backdropFilter: 'blur(12px)',
      borderBottom: '1px solid #00E5FF26',
      transition: 'background 200ms ease',
    }"
  >
    <!-- Left: Logo -->
    <div class="flex items-center gap-2 shrink-0">
      <span
        class="inline-block rounded-full"
        :style="{
          width: '16px',
          height: '16px',
          background: '#00E5FF',
          boxShadow: '0 0 8px rgba(0,229,255,0.6)',
        }"
      />
      <span class="font-bold text-white text-base tracking-wide" style="font-family: var(--font-heading)">
        TearHeart
      </span>
      <span class="text-white/50 text-sm" style="font-family: var(--font-mono)">
        · GAMING
      </span>
    </div>

    <!-- Center: Nav anchors -->
    <div class="hidden md:flex items-center gap-1">
      <button
        v-for="item in navItems"
        :key="item.key"
        class="relative px-3 py-1 uppercase tracking-[1.2px] text-xs transition-colors duration-200"
        :style="{
          fontFamily: 'Consolas, monospace',
          color: activeSection === item.key ? '#00E5FF' : '#FFFFFFB3',
        }"
        @click="scrollTo(item.href)"
        @mouseenter="($event.target as HTMLElement).style.color = '#00E5FF'"
        @mouseleave="($event.target as HTMLElement).style.color = activeSection === item.key ? '#00E5FF' : '#FFFFFFB3'"
      >
        {{ t(`nav.${item.key}`) }}
        <span
          v-if="activeSection === item.key"
          class="absolute bottom-0 left-1/2 -translate-x-1/2 h-[2px] w-4"
          style="background: #00E5FF; border-radius: 1px"
        />
      </button>
    </div>

    <!-- Right: Locale toggle + Download CTA -->
    <div class="flex items-center gap-3 shrink-0">
      <!-- Language toggle pill -->
      <button
        class="flex items-center justify-center text-xs font-mono tracking-wider transition-colors duration-200"
        :style="{
          width: '52px',
          height: '28px',
          borderRadius: '14px',
          border: '1px solid #00E5FF40',
          color: '#00E5FF',
          background: '#00E5FF0A',
        }"
        @click="toggleLocale"
      >
        {{ isZh ? '中' : 'EN' }}
      </button>

      <!-- Download CTA -->
      <a
        href="https://github.com/tearhacker/Tear7d2hacke/releases"
        target="_blank"
        rel="noopener noreferrer"
        class="btn-gradient px-5 py-1.5 text-xs font-bold uppercase tracking-wider cursor-pointer no-underline"
        style="font-family: var(--font-mono)"
      >
        {{ t('nav.download') }}
      </a>
    </div>
  </nav>
</template>
