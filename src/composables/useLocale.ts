import { computed } from 'vue'
import { useI18n } from 'vue-i18n'

export function useLocale() {
  const { locale } = useI18n()

  const isZh = computed(() => locale.value === 'zh')

  function toggleLocale() {
    locale.value = locale.value === 'zh' ? 'en' : 'zh'
  }

  return { locale, isZh, toggleLocale }
}
