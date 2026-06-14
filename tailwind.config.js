/** @type {import('tailwindcss').Config} */

export default {
  darkMode: "class",
  content: ["./index.html", "./src/**/*.{js,ts,vue}"],
  theme: {
    container: {
      center: true,
    },
    extend: {
      colors: {
        'deep-space': '#0A0E17',
        'carbon': '#1A1F2E',
        'cyan-brand': '#00E5FF',
        'purple-brand': '#9D4EDD',
        'online': '#00FF88',
        'alert': '#FF6B6B',
        'warn': '#FFD700',
        'text-primary': '#FFFFFF',
        'text-secondary': '#FFFFFFB3',
        'text-weak': '#FFFFFF59',
        'border-main': '#00E5FF26',
        'border-strong': '#00E5FF66',
      },
      fontFamily: {
        'display': ['Orbitron', 'Consolas', 'monospace'],
        'heading': ['JetBrains Mono', 'Consolas', 'monospace'],
        'body': ['Inter', 'Noto Sans SC', 'PingFang SC', 'sans-serif'],
        'mono': ['Consolas', 'JetBrains Mono', 'monospace'],
      },
      borderRadius: {
        'card': '8px',
        'btn': '6px',
        'cta': '28px',
        'pill': '18px',
      },
      animation: {
        'scanline': 'scanline 2.6s linear infinite',
        'breathe': 'breathe 2.2s ease-in-out infinite',
        'pulse-dot': 'pulse-dot 1.5s ease-in-out infinite',
        'radar-sweep': 'radar-sweep 2.8s linear infinite',
        'float-up': 'float-up 0.45s ease-out forwards',
        'glow-pulse': 'glow-pulse 2.2s ease-in-out infinite',
      },
    },
  },
  plugins: [],
};
