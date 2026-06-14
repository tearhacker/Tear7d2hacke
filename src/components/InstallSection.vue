<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useI18n } from 'vue-i18n'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'

const { t, locale } = useI18n()

const steps = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  const raw = msgs.install?.steps
  if (!raw || !Array.isArray(raw)) return []
  return raw as Array<{num: string; title: string; desc: string}>
})

// ===== Blood Scratch Game =====
type Phase = 'idle' | 'scratching' | 'revealed'
const phase = ref<Phase>('idle')
const bloodLevel = ref(0) // 0~100, blood fills up then user scratches to reveal
const scratchProgress = ref(0) // 0~100, how much scratched
const isScratching = ref(false)
const secretKeys = ref<Array<{gameName: string; gameVersion: string; secretKey: string; expireTime: string | null}>>([])
const copied = ref(false)
const bloodFilling = ref(false)
const keyLoaded = ref(false)

let fillInterval = 0

// Fetch game keys from API
async function fetchGameKey() {
  try {
    const res = await fetch('/api/v1/gamekeys?gameName=七日杀')
    const json = await res.json()
    if (json.code === 0 && Array.isArray(json.data) && json.data.length > 0) {
      secretKeys.value = json.data
      keyLoaded.value = true
    }
  } catch {
    // API unavailable, keep empty
  }
}

function startGame() {
  phase.value = 'idle'
  bloodLevel.value = 0
  scratchProgress.value = 0
  isScratching.value = false
  copied.value = false
  bloodFilling.value = true

  // Blood fill animation
  fillInterval = window.setInterval(() => {
    bloodLevel.value += 2
    if (bloodLevel.value >= 100) {
      bloodLevel.value = 100
      bloodFilling.value = false
      phase.value = 'scratching'
      window.clearInterval(fillInterval)
    }
  }, 30)
}

function handleScratch(e: MouseEvent | TouchEvent) {
  if (phase.value !== 'scratching') return
  isScratching.value = true
  scratchProgress.value = Math.min(100, scratchProgress.value + 8)
  if (scratchProgress.value >= 100) {
    phase.value = 'revealed'
  }
}

function handleScratchMove(e: MouseEvent | TouchEvent) {
  if (!isScratching.value || phase.value !== 'scratching') return
  scratchProgress.value = Math.min(100, scratchProgress.value + 3)
  if (scratchProgress.value >= 100) {
    phase.value = 'revealed'
    isScratching.value = false
  }
}

function handleScratchEnd() {
  isScratching.value = false
}

function copyKey(key: string) {
  navigator.clipboard.writeText(key).then(() => {
    copied.value = true
    setTimeout(() => { copied.value = false }, 2000)
  })
}

function resetGame() {
  window.clearInterval(fillInterval)
  startGame()
}

onMounted(() => {
  fetchGameKey()
  startGame()
})

onUnmounted(() => {
  window.clearInterval(fillInterval)
})
</script>

<template>
  <section id="install" class="scroll-enter py-24 px-6">
    <div class="max-w-[1120px] mx-auto">
      <!-- Section Tag -->
      <p
        class="mb-2"
        style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: var(--font-mono); letter-spacing: 2px;"
      >
        {{ t('install.sectionTag') }}
      </p>
      <h2
        class="mb-2"
        style="font-size: 36px; font-family: var(--font-heading); font-weight: 700; color: var(--text-primary);"
      >
        {{ t('install.title') }}
      </h2>
      <p
        class="mb-14"
        style="font-size: 14px; color: var(--text-secondary);"
      >
        {{ t('install.subtitle') }}
      </p>

      <div class="grid grid-cols-1 lg:grid-cols-2 gap-8 items-start">
        <!-- Left: Steps -->
        <div class="space-y-4">
          <div
            v-for="step in steps"
            :key="step.num"
            class="flex items-start gap-4 p-4 rounded-lg"
            style="background: #1A1F2EE6; border: 1px solid #00E5FF26;"
          >
            <div
              class="flex-shrink-0 w-10 h-10 rounded-lg flex items-center justify-center font-mono font-bold text-sm"
              style="background: linear-gradient(135deg, #9D4EDD, #00E5FF); color: white;"
            >
              {{ step.num }}
            </div>
            <div>
              <h4 class="font-bold text-white text-base mb-1">{{ step.title }}</h4>
              <p style="font-size: 13px; color: rgba(255,255,255,0.55); line-height: 1.6;">{{ step.desc }}</p>
            </div>
          </div>

          <!-- System Requirements -->
          <div
            class="p-4 rounded-lg"
            style="background: rgba(0,229,255,0.04); border: 1px solid rgba(0,229,255,0.12);"
          >
            <h4 class="font-mono font-bold text-xs tracking-widest mb-3" style="color: #00E5FF;">
              {{ t('install.sysReq.title') }}
            </h4>
            <div class="grid grid-cols-2 gap-2 text-xs font-mono" style="color: rgba(255,255,255,0.6);">
              <div>OS: {{ t('install.sysReq.os') }}</div>
              <div>.NET: {{ t('install.sysReq.dotnet') }}</div>
              <div>Game: {{ t('install.sysReq.game') }}</div>
              <div>Disk: {{ t('install.sysReq.disk') }}</div>
            </div>
          </div>
        </div>

        <!-- Right: Blood Scratch Game -->
        <div class="relative">
          <div
            class="rounded-lg overflow-hidden select-none"
            style="background: #0A0E17; border: 2px solid #8B000050; box-shadow: 0 0 30px rgba(139,0,0,0.15);"
          >
            <!-- Game header -->
            <div
              class="flex items-center justify-between px-4 py-2"
              style="background: #1A0A0A; border-bottom: 1px solid #8B000030;"
            >
              <span class="font-mono text-[10px] tracking-wider" style="color: #FF4444;">
                BLOOD MOON RITUAL
              </span>
              <span class="font-mono text-[10px]" style="color: #FFD700;">
                DAY 7
              </span>
            </div>

            <!-- Game area -->
            <div
              class="relative cursor-pointer"
              style="height: 220px; overflow: hidden; user-select: none;"
              @mousedown="handleScratch"
              @mousemove="handleScratchMove"
              @mouseup="handleScratchEnd"
              @mouseleave="handleScratchEnd"
              @touchstart.prevent="handleScratch"
              @touchmove.prevent="handleScratchMove"
              @touchend="handleScratchEnd"
            >
              <!-- Background: dark wasteland -->
              <div class="absolute inset-0" style="background: linear-gradient(180deg, #0D0A0A 0%, #1A0F0F 40%, #2A1515 100%);" />

              <!-- Zombie silhouettes background -->
              <div class="absolute inset-0 flex items-end justify-around px-4" style="opacity: 0.15;">
                <svg v-for="i in 5" :key="i" width="24" height="40" :style="{ marginBottom: '10px', transform: `scaleX(${i % 2 ? 1 : -1})` }" viewBox="0 0 24 40">
                  <rect x="6" y="0" width="12" height="12" rx="3" fill="#4A7A3A" />
                  <rect x="7" y="4" width="3" height="2" fill="#FF0000" />
                  <rect x="14" y="4" width="3" height="2" fill="#FF0000" />
                  <rect x="4" y="12" width="16" height="16" rx="2" fill="#3A5A2A" />
                  <rect x="5" y="28" width="6" height="12" rx="1" fill="#4A7A3A" />
                  <rect x="13" y="28" width="6" height="12" rx="1" fill="#4A7A3A" />
                </svg>
              </div>

              <!-- Blood fill layer -->
              <div
                v-if="bloodFilling || phase === 'scratching'"
                class="absolute bottom-0 left-0 right-0 transition-all"
                :style="{
                  height: bloodLevel + '%',
                  background: 'linear-gradient(0deg, #8B0000 0%, #B22222 30%, #DC143C 60%, rgba(139,0,0,0.6) 100%)',
                  transition: bloodFilling ? 'none' : 'height 0.3s ease',
                }"
              >
                <!-- Blood drip effect -->
                <div class="absolute top-0 left-0 right-0" style="height: 8px;">
                  <div v-for="i in 12" :key="i" class="absolute" :style="{
                    left: (i * 8.3) + '%',
                    top: '0',
                    width: '3px',
                    height: (4 + Math.random() * 8) + 'px',
                    background: '#DC143C',
                    borderRadius: '0 0 2px 2px',
                    opacity: 0.8,
                  }" />
                </div>
              </div>

              <!-- Scratch layer (blood overlay that gets scratched away) -->
              <div
                v-if="phase === 'scratching'"
                class="absolute inset-0 flex items-center justify-center"
                :style="{
                  clipPath: `inset(0 0 0 0)`,
                }"
              >
                <!-- Blood texture overlay that fades with scratch progress -->
                <div
                  class="absolute inset-0 flex items-center justify-center"
                  :style="{
                    opacity: 1 - (scratchProgress / 100) * 0.85,
                    background: 'radial-gradient(ellipse at center, #8B0000 0%, #5A0000 50%, #3A0000 100%)',
                  }"
                >
                  <span class="font-mono text-xs tracking-widest" style="color: rgba(255,255,255,0.4);">
                    SCRATCH TO REVEAL
                  </span>
                </div>
              </div>

              <!-- Revealed content: Secret Keys -->
              <div
                v-if="phase === 'revealed'"
                class="absolute inset-0 flex flex-col items-center justify-center z-10"
                style="background: radial-gradient(ellipse at center, rgba(0,229,255,0.08) 0%, rgba(10,14,23,0.95) 100%);"
              >
                <!-- Glow ring -->
                <div
                  class="mb-2 w-12 h-12 rounded-full flex items-center justify-center"
                  style="background: radial-gradient(circle, rgba(0,229,255,0.2) 0%, transparent 70%); border: 1px solid rgba(0,229,255,0.3);"
                >
                  <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#00E5FF" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
                    <path d="M21 2l-2 2m-7.61 7.61a5.5 5.5 0 1 1-7.778 7.778 5.5 5.5 0 0 1 7.777-7.777zm0 0L15.5 7.5m0 0l3 3L22 7l-3-3m-3.5 3.5L19 4" />
                  </svg>
                </div>
                <span class="font-mono text-[9px] tracking-widest mb-2" style="color: rgba(0,229,255,0.6);">
                  ACCESS KEY{{ secretKeys.length > 1 ? 'S' : '' }} UNLOCKED
                </span>
                <!-- Keys list -->
                <div class="w-full px-4 space-y-1.5 max-h-[140px] overflow-y-auto" style="scrollbar-width: thin; scrollbar-color: rgba(0,229,255,0.3) transparent;">
                  <div
                    v-for="(k, idx) in secretKeys"
                    :key="idx"
                    class="px-3 py-1.5 rounded flex items-center justify-between gap-2"
                    style="background: rgba(0,229,255,0.06); border: 1px solid rgba(0,229,255,0.15);"
                  >
                    <div class="flex items-center gap-2 min-w-0">
                      <span class="font-mono text-[9px] px-1.5 py-0.5 rounded" style="background: rgba(157,78,221,0.15); color: #9D4EDD; border: 1px solid rgba(157,78,221,0.3);">{{ k.gameVersion }}</span>
                      <span class="font-mono font-bold text-sm tracking-wider truncate" style="color: #00E5FF;">{{ k.secretKey }}</span>
                    </div>
                    <button
                      @click.stop="copyKey(k.secretKey)"
                      class="flex-shrink-0 px-1.5 py-0.5 rounded text-[9px] font-mono transition-all"
                      :style="{
                        background: copied ? 'rgba(0,255,100,0.15)' : 'rgba(0,229,255,0.1)',
                        color: copied ? '#00FF64' : '#00E5FF',
                        border: copied ? '1px solid rgba(0,255,100,0.3)' : '1px solid rgba(0,229,255,0.2)',
                      }"
                    >
                      COPY
                    </button>
                  </div>
                </div>
                <!-- No keys fallback -->
                <div v-if="!keyLoaded" class="px-4 py-2 rounded-lg text-center" style="background: rgba(255,255,255,0.03); border: 1px solid rgba(255,255,255,0.08);">
                  <span class="font-mono text-xs" style="color: rgba(255,255,255,0.3);">暂无可用卡密</span>
                </div>
                <!-- Retry -->
                <button
                  @click.stop="resetGame"
                  class="mt-2 font-mono text-[9px] tracking-wider px-3 py-1 rounded transition-all"
                  style="color: rgba(255,255,255,0.4); border: 1px solid rgba(255,255,255,0.1);"
                >
                  PLAY AGAIN
                </button>
              </div>

              <!-- Idle / filling overlay -->
              <div
                v-if="phase === 'idle'"
                class="absolute inset-0 flex flex-col items-center justify-center z-10"
              >
                <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="#DC143C" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
                  <circle cx="12" cy="12" r="10" />
                  <path d="M12 6v6l4 2" />
                </svg>
                <span class="font-mono text-xs mt-2" style="color: #DC143C;">
                  BLOOD MOON RISING...
                </span>
              </div>

              <!-- Scratch progress bar -->
              <div
                v-if="phase === 'scratching'"
                class="absolute bottom-0 left-0 right-0 z-20"
                style="height: 3px; background: rgba(0,0,0,0.5);"
              >
                <div
                  class="h-full transition-all"
                  :style="{
                    width: scratchProgress + '%',
                    background: 'linear-gradient(90deg, #8B0000, #DC143C, #00E5FF)',
                  }"
                />
              </div>
            </div>
          </div>

          <p
            class="text-center mt-3 font-mono text-[11px]"
            style="color: rgba(255,255,255,0.3);"
          >
            {{ phase === 'idle' ? t('install.gameWait') : phase === 'scratching' ? t('install.gameScratch') : t('install.gameRevealed') }}
          </p>
        </div>
      </div>
    </div>
  </section>
</template>
