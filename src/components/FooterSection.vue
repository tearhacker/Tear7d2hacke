<script setup lang="ts">
</script>

<template>
  <footer style="background: #070A11; border-top: 1px solid rgba(0,229,255,0.15);">
    <div style="max-width: 1120px; margin: 0 auto; padding: 32px 24px; display: flex; flex-direction: column; align-items: center; gap: 16px;">

      <!-- GitHub Link -->
      <a
        href="https://github.com/tearhacker"
        target="_blank"
        rel="noopener"
        style="display: inline-flex; align-items: center; gap: 8px; text-decoration: none; transition: opacity 0.2s;"
        onmouseenter="this.style.opacity='0.7'"
        onmouseleave="this.style.opacity='1'"
      >
        <!-- GitHub SVG Icon -->
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#00E5FF" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
          <path d="M9 19c-5 1.5-5-2.5-7-3m14 6v-3.87a3.37 3.37 0 0 0-.94-2.61c3.14-.35 6.44-1.54 6.44-7A5.44 5.44 0 0 0 20 4.77 5.07 5.07 0 0 0 19.91 1S18.73.65 16 2.48a13.38 13.38 0 0 0-7 0C6.27.65 5.09 1 5.09 1A5.07 5.07 0 0 0 5 4.77a5.44 5.44 0 0 0-1.5 3.78c0 5.42 3.3 6.61 6.44 7A3.37 3.37 0 0 0 9 18.13V22"/>
        </svg>
        <span style="font-size: 14px; font-family: Consolas, monospace; font-weight: 600; color: #00E5FF; letter-spacing: 1px;">
          tearhacker
        </span>
      </a>

      <!-- Copyright -->
      <p style="font-size: 12px; font-family: Consolas, monospace; color: rgba(255,255,255,0.25); letter-spacing: 0.5px;">
        Copyright &copy; 2026.6 TearHeart Gaming
      </p>
    </div>
  </footer>
</template>
