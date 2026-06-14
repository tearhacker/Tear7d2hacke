<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useI18n } from 'vue-i18n'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'

const { t, locale } = useI18n()

// Bypass t() for array data
const tabs = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  const raw = msgs.showcase?.tabs
  if (!raw || !Array.isArray(raw)) return []
  return raw
})
const activeTab = ref(0)

// Map tabs to real screenshot images
const screenshotMap: Record<number, string> = {
  0: new URL('../assets/7daystodieGamePhoto/游戏菜单内存功能页.png', import.meta.url).href,
  1: new URL('../assets/7daystodieGamePhoto/游戏绘图ESP功能演示.png', import.meta.url).href,
  2: new URL('../assets/7daystodieGamePhoto/游戏菜单追踪自瞄页.png', import.meta.url).href,
  3: new URL('../assets/7daystodieGamePhoto/游戏菜单技能页.png', import.meta.url).href,
  4: new URL('../assets/7daystodieGamePhoto/启动器启动完成.png', import.meta.url).href,
  5: new URL('../assets/7daystodieGamePhoto/游戏菜单设置页.png', import.meta.url).href,
  6: new URL('../assets/7daystodieGamePhoto/游戏触摸自瞄功能演示.png', import.meta.url).href,
}

const currentImage = computed(() => screenshotMap[activeTab.value] || screenshotMap[0])

// Auto-rotate carousel every 4 seconds
let timer: ReturnType<typeof setInterval> | null = null

function startAutoPlay() {
  stopAutoPlay()
  timer = setInterval(() => {
    activeTab.value = (activeTab.value + 1) % tabs.value.length
  }, 3000)
}

function stopAutoPlay() {
  if (timer) {
    clearInterval(timer)
    timer = null
  }
}

function switchTab(index: number) {
  activeTab.value = index
  startAutoPlay()
}

function pauseAutoPlay() {
  stopAutoPlay()
}

function resumeAutoPlay() {
  startAutoPlay()
}

onMounted(() => startAutoPlay())
onUnmounted(() => stopAutoPlay())
</script>

<template>
  <section id="showcase" class="scroll-enter py-24 px-6">
    <div class="max-w-[1200px] mx-auto">
      <!-- Section header -->
      <p
        class="mb-2"
        style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: var(--font-mono); letter-spacing: 2px;"
      >
        {{ t('showcase.sectionTag') }}
      </p>
      <h2
        class="mb-2"
        style="font-size: 36px; font-family: var(--font-heading); font-weight: 700; color: var(--text-primary);"
      >
        {{ t('showcase.title') }}
      </h2>
      <p
        class="mb-14"
        style="font-size: 14px; color: var(--text-secondary);"
      >
        {{ t('showcase.subtitle') }}
      </p>

      <div class="flex gap-8">
        <!-- Left: Tab list - prominent and visible -->
        <div
          class="w-[300px] shrink-0 flex flex-col gap-2"
          @mouseenter="pauseAutoPlay"
          @mouseleave="resumeAutoPlay"
        >
          <button
            v-for="(tab, i) in tabs"
            :key="tab.num"
            class="showcase-tab text-left px-5 py-4 rounded-lg transition-all duration-200 relative"
            :class="{ 'showcase-tab--active': activeTab === i }"
            @click="switchTab(i)"
          >
            <!-- Active indicator bar -->
            <div
              class="absolute left-0 top-0 bottom-0 w-[3px] rounded-r transition-all duration-300"
              :style="{
                background: activeTab === i ? 'linear-gradient(180deg, #00E5FF, #9D4EDD)' : 'rgba(255,255,255,0.08)',
              }"
            />
            <div class="flex items-center gap-3 mb-1">
              <span
                class="font-mono text-[12px] font-bold"
                :style="{ color: activeTab === i ? '#00E5FF' : 'rgba(255,255,255,0.4)', letterSpacing: '1px' }"
              >
                {{ tab.num }}
              </span>
              <span
                class="font-bold text-[16px]"
                :style="{ color: activeTab === i ? '#FFFFFF' : 'rgba(255,255,255,0.6)' }"
              >
                {{ tab.title }}
              </span>
            </div>
            <span
              class="font-mono text-[11px] tracking-wider"
              :style="{ color: activeTab === i ? '#00E5FF' : 'rgba(255,255,255,0.3)' }"
            >
              {{ tab.subtitle }}
            </span>
            <!-- Progress bar for active tab -->
            <div
              v-if="activeTab === i"
              class="mt-3 h-[2px] rounded-full overflow-hidden"
              style="background: rgba(0,229,255,0.1);"
            >
              <div class="showcase-progress" />
            </div>
          </button>
        </div>

        <!-- Right: Screenshot display -->
        <div class="flex-1 min-w-0">
          <div class="monitor-frame">
            <!-- Top bar -->
            <div class="monitor-topbar">
              <div class="flex gap-[6px]">
                <span class="monitor-dot" style="background: #FF6B6B" />
                <span class="monitor-dot" style="background: #FFD700" />
                <span class="monitor-dot" style="background: #00FF88" />
              </div>
              <div class="monitor-url">
                {{ t('showcase.urlBar') }}
              </div>
              <div class="monitor-status">
                <span class="w-1.5 h-1.5 rounded-full inline-block" style="background: #00FF88; animation: pulse-dot 1.5s ease-in-out infinite;" />
                <span>{{ t('showcase.modInstalled') }}</span>
              </div>
            </div>

            <!-- Screenshot area with crossfade -->
            <div class="monitor-screen">
              <transition name="carousel-fade" mode="out-in">
                <img
                  :key="activeTab"
                  :src="currentImage"
                  :alt="tabs[activeTab]?.title || 'Screenshot'"
                  class="monitor-screenshot"
                />
              </transition>
              <!-- Scanline overlay -->
              <div class="monitor-scanline" />
            </div>
          </div>

          <!-- Dot indicators below monitor -->
          <div class="flex items-center justify-center gap-2 mt-4">
            <button
              v-for="i in tabs.length"
              :key="i"
              class="carousel-dot"
              :class="{ 'carousel-dot--active': activeTab === i - 1 }"
              @click="switchTab(i - 1)"
            />
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
/* Tab styles */
.showcase-tab {
  background: transparent;
  border: 1px solid transparent;
  cursor: pointer;
  outline: none;
}

.showcase-tab:hover {
  background: rgba(255, 255, 255, 0.04);
  border-color: rgba(0, 229, 255, 0.08);
}

.showcase-tab--active {
  background: rgba(0, 229, 255, 0.08);
  border-color: rgba(0, 229, 255, 0.2);
}

/* Progress bar animation */
.showcase-progress {
  height: 100%;
  background: linear-gradient(90deg, #00E5FF, #9D4EDD);
  border-radius: 1px;
  animation: progress-fill 3s linear infinite;
}

@keyframes progress-fill {
  from { width: 0%; }
  to { width: 100%; }
}

/* Carousel crossfade */
.carousel-fade-enter-active,
.carousel-fade-leave-active {
  transition: opacity 400ms ease;
}
.carousel-fade-enter-from,
.carousel-fade-leave-to {
  opacity: 0;
}

/* Dot indicators */
.carousel-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  border: none;
  background: rgba(255, 255, 255, 0.15);
  cursor: pointer;
  transition: all 200ms ease;
  padding: 0;
}

.carousel-dot:hover {
  background: rgba(0, 229, 255, 0.4);
}

.carousel-dot--active {
  background: #00E5FF;
  box-shadow: 0 0 8px rgba(0, 229, 255, 0.5);
  width: 24px;
  border-radius: 4px;
}

/* Monitor frame */
.monitor-frame {
  border: 6px solid #1A1F2E;
  border-radius: 10px;
  overflow: hidden;
  position: relative;
  box-shadow:
    inset 0 0 0 1px #00E5FF30,
    0 0 40px rgba(0, 229, 255, 0.06);
}

.monitor-topbar {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 8px 14px;
  background: #1A1F2E;
  border-bottom: 1px solid #00E5FF26;
}

.monitor-dot {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  display: inline-block;
}

.monitor-url {
  font-family: Consolas, 'JetBrains Mono', monospace;
  font-size: 11px;
  color: rgba(255, 255, 255, 0.4);
  background: rgba(0, 0, 0, 0.3);
  padding: 2px 10px;
  border-radius: 4px;
  flex: 1;
}

.monitor-status {
  display: flex;
  align-items: center;
  gap: 6px;
  font-family: Consolas, 'JetBrains Mono', monospace;
  font-size: 10px;
  color: #00FF88;
  letter-spacing: 1px;
}

.monitor-screen {
  position: relative;
  background: #0A0E17;
  overflow: hidden;
  min-height: 300px;
}

.monitor-screenshot {
  display: block;
  width: 100%;
  height: auto;
  object-fit: contain;
}

.monitor-scanline {
  position: absolute;
  inset: 0;
  background: repeating-linear-gradient(
    0deg,
    transparent,
    transparent 2px,
    rgba(0, 229, 255, 0.02) 2px,
    rgba(0, 229, 255, 0.02) 4px
  );
  pointer-events: none;
  z-index: 3;
}

@media (max-width: 768px) {
  .flex.gap-8 {
    flex-direction: column;
  }
  .w-\[280px\] {
    width: 100%;
    flex-direction: row;
    overflow-x: auto;
    gap: 4px;
  }
}
</style>
