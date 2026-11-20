import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // The API runs separately (dotnet run, http profile); the dev server forwards /api to it.
    proxy: {
      '/api': 'http://localhost:5217',
    },
  },
})
