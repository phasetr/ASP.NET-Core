import { defineConfig } from 'vite';

export default defineConfig({
  build: {
    emptyOutDir: true,
    rollupOptions: {
      input: 'src/Main.fs.js',
      output: {
        file: 'src/main.js',
        format: 'iife'
      }
    }
  },
  publicDir: 'public'
});
