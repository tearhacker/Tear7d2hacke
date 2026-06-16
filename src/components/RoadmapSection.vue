<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'

const { t, locale } = useI18n()

const items = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  const raw = msgs.roadmap?.items
  if (!raw || !Array.isArray(raw)) return []
  return raw as Array<{time: string; status: string; title: string; desc: string}>
})

const statusConfig: Record<string, { dotClass: string; label: string; labelColor: string }> = {
  done: { dotClass: 'dot-done', label: 'COMPLETED', labelColor: '#00FF88' },
  active: { dotClass: 'dot-active', label: 'IN PROGRESS', labelColor: '#FFD700' },
  plan: { dotClass: 'dot-plan', label: 'PLANNED', labelColor: '#00E5FF' },
}
</script>

<template>
  <section id="roadmap" class="py-24 px-6">
    <div class="max-w-4xl mx-auto">
      <!-- Section Tag -->
      <p
        class="text-center mb-2"
        style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: var(--font-mono); letter-spacing: 2px;"
      >
        {{ t('roadmap.sectionTag') }}
      </p>

      <!-- Title -->
      <h2
        class="text-center mb-2"
        style="font-size: 36px; font-family: Consolas, var(--font-mono); font-weight: 700; color: var(--text-primary);"
      >
        {{ t('roadmap.title') }}
      </h2>

      <!-- Subtitle -->
      <p
        class="text-center mb-16"
        style="font-size: 14px; color: var(--text-secondary);"
      >
        {{ t('roadmap.subtitle') }}
      </p>

      <!-- Timeline -->
      <div class="relative">
        <div
          v-for="(item, i) in items"
          :key="i"
          class="relative flex items-start mb-10 last:mb-0"
        >
          <!-- Left: Time + Dot + Line -->
          <div class="flex flex-col items-center" style="width: 120px; flex-shrink: 0;">
            <!-- Time label -->
            <span
              style="
                font-size: 14px;
                font-family: Consolas, var(--font-mono);
                color: var(--cyan);
                white-space: nowrap;
                margin-bottom: 8px;
              "
            >
              {{ item.time }}
            </span>

            <!-- Status dot -->
            <div
              :class="statusConfig[item.status]?.dotClass"
              class="dot"
            />

            <!-- Vertical dashed line (not on last item) -->
            <div
              v-if="i < (items as any[]).length - 1"
              style="
                width: 1px;
                flex: 1;
                min-height: 40px;
                border-left: 1px dashed #00E5FF40;
                margin-top: 6px;
              "
            />
          </div>

          <!-- Right: Card -->
          <div
            class="cyber-card"
            style="
              flex: 1;
              margin-left: 24px;
              padding: 20px 28px;
              border-radius: 12px;
            "
          >
            <div class="flex items-center justify-between mb-2">
              <h3
                style="font-size: 16px; font-weight: 600; color: var(--text-primary);"
              >
                {{ item.title }}
              </h3>
              <span
                style="
                  font-size: 11px;
                  font-family: var(--font-mono);
                  letter-spacing: 1px;
                  color: v-bind(statusConfig[item.status]?.labelColor);
                "
              >
                {{ statusConfig[item.status]?.label }}
              </span>
            </div>
            <p
              style="font-size: 13px; color: var(--text-secondary); line-height: 1.6;"
            >
              {{ item.desc }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  flex-shrink: 0;
}

.dot-done {
  background: #00FF88;
  box-shadow: 0 0 8px rgba(0, 255, 136, 0.5);
}

.dot-active {
  background: #FFD700;
  box-shadow: 0 0 8px rgba(255, 215, 0, 0.5);
  animation: breathe-dot 2200ms ease-in-out infinite;
}

.dot-plan {
  background: transparent;
  border: 1.5px solid #00E5FF;
}

@keyframes breathe-dot {
  0%, 100% {
    opacity: 1;
    box-shadow: 0 0 8px rgba(255, 215, 0, 0.5);
  }
  50% {
    opacity: 0.4;
    box-shadow: 0 0 16px rgba(255, 215, 0, 0.8);
  }
}
</style>
