<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import zhMessages from '@/i18n/zh'
import enMessages from '@/i18n/en'

const { t, locale } = useI18n()

const cards = computed(() => {
  const msgs = locale.value === 'zh' ? zhMessages : enMessages
  const raw = msgs.hokFeatures?.cards
  if (!raw || !Array.isArray(raw)) return []
  return raw
})
</script>

<template>
  <section id="project-hok" class="scroll-enter py-24 px-6">
    <div class="max-w-[1120px] mx-auto">
      <p style="font-size: 10px; color: rgba(255,255,255,0.35); font-family: Consolas, monospace; letter-spacing: 2px; margin-bottom: 8px;">
        {{ t('hokFeatures.sectionTag') }}
      </p>
      <h2 style="font-size: 36px; font-weight: 700; color: #E8ECF4; margin-bottom: 8px;">
        {{ t('hokFeatures.title') }}
      </h2>
      <p style="font-size: 14px; color: #B0B8C8; margin-bottom: 56px;">
        {{ t('hokFeatures.subtitle') }}
      </p>

      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div
          v-for="card in cards"
          :key="card.num"
          class="hok-feature-card"
        >
          <!-- Top: Number badge + Title -->
          <div class="card-header">
            <div class="num-badge">{{ card.num }}</div>
            <div class="title-group">
              <h3 class="card-title">{{ card.title }}</h3>
              <span class="card-title-en">{{ card.titleEn }}</span>
            </div>
          </div>

          <!-- Desc -->
          <p class="card-desc">{{ card.desc }}</p>

          <!-- Feature items as styled chips -->
          <div class="items-grid">
            <span v-for="(item, idx) in card.items" :key="idx" class="feature-chip">
              <span class="chip-dot" />
              {{ item }}
            </span>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.hok-feature-card {
  background: rgba(20, 26, 42, 0.75);
  backdrop-filter: blur(12px);
  border: 1px solid rgba(157, 78, 221, 0.15);
  border-radius: 14px;
  padding: 28px;
  transition: transform 0.25s cubic-bezier(0.4, 0, 0.2, 1), border-color 0.25s ease, box-shadow 0.25s ease;
}

.hok-feature-card:hover {
  transform: translateY(-4px);
  border-color: rgba(157, 78, 221, 0.4);
  box-shadow: 0 8px 32px rgba(157, 78, 221, 0.12);
}

.card-header {
  display: flex;
  align-items: center;
  gap: 14px;
  margin-bottom: 14px;
}

.num-badge {
  width: 44px;
  height: 44px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
  font-weight: 900;
  font-family: 'Orbitron', Consolas, monospace;
  color: #9D4EDD;
  background: linear-gradient(135deg, rgba(157, 78, 221, 0.15), rgba(255, 215, 0, 0.08));
  border: 1px solid rgba(157, 78, 221, 0.25);
  flex-shrink: 0;
}

.title-group {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.card-title {
  font-size: 18px;
  font-weight: 700;
  color: #E8ECF4;
  margin: 0;
  line-height: 1.2;
}

.card-title-en {
  font-size: 10px;
  font-family: Consolas, 'JetBrains Mono', monospace;
  color: rgba(157, 78, 221, 0.6);
  letter-spacing: 2px;
  text-transform: uppercase;
}

.card-desc {
  font-size: 13px;
  color: #8A94A8;
  line-height: 1.7;
  margin: 0 0 18px 0;
}

.items-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.feature-chip {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 5px 12px;
  border-radius: 6px;
  font-size: 12px;
  font-family: -apple-system, 'PingFang SC', 'Noto Sans SC', 'Microsoft YaHei', sans-serif;
  color: #C0C8D8;
  background: rgba(157, 78, 221, 0.05);
  border: 1px solid rgba(157, 78, 221, 0.1);
  transition: background 0.2s ease, border-color 0.2s ease, color 0.2s ease;
  cursor: default;
  white-space: nowrap;
}

.feature-chip:hover {
  background: rgba(157, 78, 221, 0.12);
  border-color: rgba(157, 78, 221, 0.3);
  color: #E0E8F4;
}

.chip-dot {
  width: 4px;
  height: 4px;
  border-radius: 50%;
  background: #9D4EDD;
  flex-shrink: 0;
  opacity: 0.7;
}

@media (max-width: 640px) {
  .hok-feature-card {
    padding: 20px;
  }
  .items-grid {
    gap: 6px;
  }
  .feature-chip {
    font-size: 11px;
    padding: 4px 10px;
  }
}
</style>
