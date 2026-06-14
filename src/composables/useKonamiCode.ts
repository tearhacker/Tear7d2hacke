import { ref, onMounted, onUnmounted } from 'vue'

const KONAMI_CODE = [
  'ArrowUp', 'ArrowUp', 'ArrowDown', 'ArrowDown',
  'ArrowLeft', 'ArrowRight', 'ArrowLeft', 'ArrowRight',
  'KeyB', 'KeyA',
]

export function useKonamiCode(callback: () => void) {
  const index = ref(0)

  function handleKeydown(e: KeyboardEvent) {
    if (e.code === KONAMI_CODE[index.value]) {
      index.value++
      if (index.value === KONAMI_CODE.length) {
        callback()
        index.value = 0
      }
    } else {
      index.value = 0
    }
  }

  onMounted(() => window.addEventListener('keydown', handleKeydown))
  onUnmounted(() => window.removeEventListener('keydown', handleKeydown))
}
