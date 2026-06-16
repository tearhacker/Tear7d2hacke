<script setup lang="ts">
defineProps<{
  question: string
  answer: string
  isOpen: boolean
  toggle: () => void
}>()
</script>

<template>
  <div
    style="
      background: #1A1F2E80;
      border-radius: 12px;
      border: 1px solid #00E5FF1A;
      overflow: hidden;
      transition: border-color 250ms ease-out;
    "
    :style="isOpen ? { borderColor: '#00E5FF40' } : {}"
  >
    <!-- Question Row -->
    <button
      class="w-full flex items-center justify-between text-left"
      style="padding: 18px 24px; cursor: pointer; background: none; border: none; outline: none;"
      @click="toggle"
    >
      <span class="flex items-center gap-2">
        <span
          style="
            font-size: 14px;
            font-family: var(--font-mono);
            font-weight: 700;
            color: #9D4EDD;
            flex-shrink: 0;
          "
        >Q.</span>
        <span
          style="
            font-size: 14px;
            font-weight: 600;
            color: var(--text-primary);
          "
        >{{ question }}</span>
      </span>
      <span
        style="
          font-size: 12px;
          color: var(--text-weak);
          transition: transform 250ms ease-out;
          flex-shrink: 0;
          margin-left: 12px;
        "
        :style="{ transform: isOpen ? 'rotate(180deg)' : 'rotate(0deg)' }"
      >▼</span>
    </button>

    <!-- Answer -->
    <div
      style="
        overflow: hidden;
        transition: max-height 250ms ease-out, opacity 250ms ease-out;
      "
      :style="{
        maxHeight: isOpen ? '300px' : '0px',
        opacity: isOpen ? 1 : 0,
      }"
    >
      <div
        style="
          padding: 0 24px 18px 24px;
          display: flex;
          gap: 10px;
        "
      >
        <div
          style="
            width: 2px;
            flex-shrink: 0;
            background: var(--cyan);
            border-radius: 1px;
            align-self: stretch;
          "
        />
        <p
          style="
            font-size: 13px;
            color: rgba(255,255,255,0.7);
            line-height: 1.7;
          "
        >
          {{ answer }}
        </p>
      </div>
    </div>
  </div>
</template>
