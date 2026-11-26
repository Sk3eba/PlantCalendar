import { svelte } from '@sveltejs/vite-plugin-svelte'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [svelte()],
  server: {
    proxy: {
      '/plants': 'http://localhost:5251',
      '/species': 'http://localhost:5251',
    },
  },
})
