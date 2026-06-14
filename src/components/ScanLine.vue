<script setup lang="ts">
defineProps<{
  slow?: boolean
}>()
</script>

<template>
  <div
    class="absolute left-0 w-full pointer-events-none md:block hidden"
    :style="{
      height: '2px',
      background: 'var(--cyan)',
      boxShadow: '0 0 6px var(--cyan), 0 0 12px var(--cyan)',
      filter: 'blur(0.5px)',
      animation: `scanline ${slow ? '6s' : '2.6s'} linear infinite`,
      zIndex: 1,
    }"
  />
</template>
