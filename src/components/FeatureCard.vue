<script setup lang="ts">
defineProps<{
  num: string
  title: string
  titleEn: string
  desc: string
  items: string[]
}>()
</script>

<template>
  <div class="feature-card">
    <!-- Top: Number badge + Title -->
    <div class="card-header">
      <div class="num-badge">{{ num }}</div>
      <div class="title-group">
        <h3 class="card-title">{{ title }}</h3>
        <span class="card-title-en">{{ titleEn }}</span>
      </div>
    </div>

    <!-- Desc -->
    <p class="card-desc">{{ desc }}</p>

    <!-- Feature items as styled chips -->
    <div class="items-grid">
      <span v-for="(item, idx) in items" :key="idx" class="feature-chip">
        <span class="chip-dot" />
        {{ item }}
      </span>
    </div>
  </div>
</template>

<style scoped>
.feature-card {
  background: rgba(20, 26, 42, 0.75);
  backdrop-filter: blur(12px);
  border: 1px solid rgba(0, 229, 255, 0.12);
  border-radius: 14px;
  padding: 28px;
  transition: transform 0.25s cubic-bezier(0.4, 0, 0.2, 1), border-color 0.25s ease, box-shadow 0.25s ease;
}

.feature-card:hover {
  transform: translateY(-4px);
  border-color: rgba(0, 229, 255, 0.35);
  box-shadow: 0 8px 32px rgba(0, 229, 255, 0.08);
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
  color: #00E5FF;
  background: linear-gradient(135deg, rgba(0, 229, 255, 0.12), rgba(157, 78, 221, 0.08));
  border: 1px solid rgba(0, 229, 255, 0.2);
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
  color: rgba(0, 229, 255, 0.6);
  letter-spacing: 2px;
  text-transform: uppercase;
}

.card-desc {
  font-size: 13px;
  color: #8A94A8;
  line-height: 1.7;
  margin: 0 0 18px 0;
}

/* Feature chips - the key redesign */
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
  background: rgba(0, 229, 255, 0.04);
  border: 1px solid rgba(0, 229, 255, 0.08);
  transition: background 0.2s ease, border-color 0.2s ease, color 0.2s ease;
  cursor: default;
  white-space: nowrap;
}

.feature-chip:hover {
  background: rgba(0, 229, 255, 0.1);
  border-color: rgba(0, 229, 255, 0.25);
  color: #E0E8F4;
}

.chip-dot {
  width: 4px;
  height: 4px;
  border-radius: 50%;
  background: #00E5FF;
  flex-shrink: 0;
  opacity: 0.7;
}

/* Responsive: single column chips on small screens */
@media (max-width: 640px) {
  .feature-card {
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
