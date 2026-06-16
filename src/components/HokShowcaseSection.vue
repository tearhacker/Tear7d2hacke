<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useI18n } from 'vue-i18n'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'

const { t, locale } = useI18n()

const tabs = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  const raw = msgs.hokShowcase?.tabs
  if (!raw || !Array.isArray(raw)) return []
  return raw
})
const activeTab = ref(0)

// Map tabs to real screenshot images
const screenshotMap: Record<number, string> = {
  0: new URL('../assets/honorofkingsPhoto/绘图_metool.jpg', import.meta.url).href,
  1: new URL('../assets/honorofkingsPhoto/向量自瞄展示_metool.jpg', import.meta.url).href,
  2: new URL('../assets/honorofkingsPhoto/触摸功能_metool.jpg', import.meta.url).href,
  3: new URL('../assets/honorofkingsPhoto/自动换装_metool.jpg', import.meta.url).href,
  4: new URL('../assets/honorofkingsPhoto/启动Ui展示_metool.jpg', import.meta.url).href,
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
  <section id="hok-showcase" class="scroll-enter py-24 px-6">
    <div class="max-w-[1200px] mx-auto">
      <!-- Section header -->
      <p
        class="mb-2"
        style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: var(--font-mono); letter-spacing: 2px;"
      >
        {{ t('hokShowcase.sectionTag') }}
      </p>
      <h2
        class="mb-2"
        style="font-size: 36px; font-family: var(--font-heading); font-weight: 700; color: var(--text-primary);"
      >
        {{ t('hokShowcase.title') }}
      </h2>
      <p
        class="mb-14"
        style="font-size: 14px; color: var(--text-secondary);"
      >
        {{ t('hokShowcase.subtitle') }}
      </p>

      <div class="flex gap-8">
        <!-- Left: Tab list -->
        <div
          class="w-[300px] shrink-0 flex flex-col gap-2"
          @mouseenter="pauseAutoPlay"
          @mouseleave="resumeAutoPlay"
        >
          <button
            v-for="(tab, i) in tabs"
            :key="tab.num"
            class="hok-showcase-tab text-left px-5 py-4 rounded-lg transition-all duration-200 relative"
            :class="{ 'hok-showcase-tab--active': activeTab === i }"
            @click="switchTab(i)"
          >
            <!-- Active indicator bar -->
            <div
              class="absolute left-0 top-0 bottom-0 w-[3px] rounded-r transition-all duration-300"
              :style="{
                background: activeTab === i ? 'linear-gradient(180deg, #9D4EDD, #FFD700)' : 'rgba(255,255,255,0.08)',
              }"
            />
            <div class="flex items-center gap-3 mb-1">
              <span
                class="font-mono text-[12px] font-bold"
                :style="{ color: activeTab === i ? '#9D4EDD' : 'rgba(255,255,255,0.4)', letterSpacing: '1px' }"
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
              :style="{ color: activeTab === i ? '#9D4EDD' : 'rgba(255,255,255,0.3)' }"
            >
              {{ tab.subtitle }}
            </span>
            <!-- Progress bar for active tab -->
            <div
              v-if="activeTab === i"
              class="mt-3 h-[2px] rounded-full overflow-hidden"
              style="background: rgba(157,78,221,0.1);"
            >
              <div class="hok-showcase-progress" />
            </div>
          </button>
        </div>

        <!-- Right: Screenshot display -->
        <div class="flex-1 min-w-0">
          <div class="hok-monitor-frame">
            <!-- Top bar -->
            <div class="hok-monitor-topbar">
              <div class="flex gap-[6px]">
                <span class="monitor-dot" style="background: #FF6B6B" />
                <span class="monitor-dot" style="background: #FFD700" />
                <span class="monitor-dot" style="background: #00FF88" />
              </div>
              <div class="hok-monitor-url">
                {{ t('hokShowcase.urlBar') }}
              </div>
              <div class="hok-monitor-status">
                <span class="w-1.5 h-1.5 rounded-full inline-block" style="background: #00FF88; animation: pulse-dot 1.5s ease-in-out infinite;" />
                <span>{{ t('hokShowcase.modInstalled') }}</span>
              </div>
            </div>

            <!-- Screenshot area with crossfade -->
            <div class="hok-monitor-screen">
              <transition name="carousel-fade" mode="out-in">
                <img
                  :key="activeTab"
                  :src="currentImage"
                  :alt="tabs[activeTab]?.title || 'Screenshot'"
                  class="hok-monitor-screenshot"
                />
              </transition>
              <!-- Scanline overlay -->
              <div class="hok-monitor-scanline" />
            </div>
          </div>

          <!-- Dot indicators below monitor -->
          <div class="flex items-center justify-center gap-2 mt-4">
            <button
              v-for="i in tabs.length"
              :key="i"
              class="hok-carousel-dot"
              :class="{ 'hok-carousel-dot--active': activeTab === i - 1 }"
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
.hok-showcase-tab {
  background: transparent;
  border: 1px solid transparent;
  cursor: pointer;
  outline: none;
}

.hok-showcase-tab:hover {
  background: rgba(255, 255, 255, 0.04);
  border-color: rgba(157, 78, 221, 0.1);
}

.hok-showcase-tab--active {
  background: rgba(157, 78, 221, 0.08);
  border-color: rgba(157, 78, 221, 0.25);
}

/* Progress bar animation */
.hok-showcase-progress {
  height: 100%;
  background: linear-gradient(90deg, #9D4EDD, #FFD700);
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
.hok-carousel-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  border: none;
  background: rgba(255, 255, 255, 0.15);
  cursor: pointer;
  transition: all 200ms ease;
  padding: 0;
}

.hok-carousel-dot:hover {
  background: rgba(157, 78, 221, 0.4);
}

.hok-carousel-dot--active {
  background: #9D4EDD;
  box-shadow: 0 0 8px rgba(157, 78, 221, 0.5);
  width: 24px;
  border-radius: 4px;
}

/* Monitor frame — purple/gold theme */
.hok-monitor-frame {
  border: 6px solid #1A1F2E;
  border-radius: 10px;
  overflow: hidden;
  position: relative;
  box-shadow:
    inset 0 0 0 1px #9D4EDD30,
    0 0 40px rgba(157, 78, 221, 0.08);
}

.hok-monitor-topbar {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 8px 14px;
  background: #1A1F2E;
  border-bottom: 1px solid #9D4EDD26;
}

.monitor-dot {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  display: inline-block;
}

.hok-monitor-url {
  font-family: Consolas, 'JetBrains Mono', monospace;
  font-size: 11px;
  color: rgba(255, 255, 255, 0.4);
  background: rgba(0, 0, 0, 0.3);
  padding: 2px 10px;
  border-radius: 4px;
  flex: 1;
}

.hok-monitor-status {
  display: flex;
  align-items: center;
  gap: 6px;
  font-family: Consolas, 'JetBrains Mono', monospace;
  font-size: 10px;
  color: #00FF88;
  letter-spacing: 1px;
}

.hok-monitor-screen {
  position: relative;
  background: #0A0E17;
  overflow: hidden;
  min-height: 300px;
}

.hok-monitor-screenshot {
  display: block;
  width: 100%;
  height: auto;
  object-fit: contain;
}

.hok-monitor-scanline {
  position: absolute;
  inset: 0;
  background: repeating-linear-gradient(
    0deg,
    transparent,
    transparent 2px,
    rgba(157, 78, 221, 0.015) 2px,
    rgba(157, 78, 221, 0.015) 4px
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
