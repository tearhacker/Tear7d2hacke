<script setup lang="ts">
</script>

<template>
  <div class="relative" :style="{ width: '220px', height: '220px' }">
    <svg
      viewBox="0 0 220 220"
      :style="{ width: '100%', height: '100%' }"
      xmlns="http://www.w3.org/2000/svg"
    >
      <!-- Concentric circles -->
      <circle cx="110" cy="110" r="100" fill="none" stroke="#00E5FF" :stroke-opacity="0.12" stroke-width="0.8" />
      <circle cx="110" cy="110" r="75" fill="none" stroke="#00E5FF" :stroke-opacity="0.18" stroke-width="0.8" />
      <circle cx="110" cy="110" r="50" fill="none" stroke="#00E5FF" :stroke-opacity="0.25" stroke-width="0.8" />
      <circle cx="110" cy="110" r="25" fill="none" stroke="#00E5FF" :stroke-opacity="0.35" stroke-width="0.8" />

      <!-- Crosshair lines -->
      <line x1="10" y1="110" x2="210" y2="110" stroke="#00E5FF" :stroke-opacity="0.15" stroke-width="0.5" />
      <line x1="110" y1="10" x2="110" y2="210" stroke="#00E5FF" :stroke-opacity="0.15" stroke-width="0.5" />

      <!-- Rotating sweep sector -->
      <g
        :style="{
          transformOrigin: '110px 110px',
          animation: 'radar-sweep 2.8s linear infinite',
        }"
      >
        <defs>
          <linearGradient id="sweepGrad" x1="0" y1="0" x2="1" y2="0">
            <stop offset="0%" stop-color="#00E5FF" stop-opacity="0" />
            <stop offset="100%" stop-color="#00E5FF" stop-opacity="0.3" />
          </linearGradient>
        </defs>
        <path
          d="M110,110 L110,10 A100,100 0 0,1 196.6,60 Z"
          fill="url(#sweepGrad)"
        />
        <!-- Sweep line -->
        <line x1="110" y1="110" x2="110" y2="10" stroke="#00E5FF" stroke-opacity="0.6" stroke-width="1" />
      </g>

      <!-- Center dot -->
      <circle cx="110" cy="110" r="3" fill="#00E5FF" :fill-opacity="0.9" />
      <circle cx="110" cy="110" r="6" fill="none" stroke="#00E5FF" :stroke-opacity="0.4" stroke-width="0.5" />

      <!-- L-shaped corner marks -->
      <!-- Top-left -->
      <path d="M16,30 L16,16 L30,16" fill="none" stroke="#00E5FF" stroke-opacity="0.5" stroke-width="1.2" />
      <!-- Top-right -->
      <path d="M190,16 L204,16 L204,30" fill="none" stroke="#00E5FF" stroke-opacity="0.5" stroke-width="1.2" />
      <!-- Bottom-left -->
      <path d="M16,190 L16,204 L30,204" fill="none" stroke="#00E5FF" stroke-opacity="0.5" stroke-width="1.2" />
      <!-- Bottom-right -->
      <path d="M190,204 L204,204 L204,190" fill="none" stroke="#00E5FF" stroke-opacity="0.5" stroke-width="1.2" />
    </svg>
  </div>
</template>
