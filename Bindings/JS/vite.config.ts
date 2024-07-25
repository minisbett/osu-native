import { defineConfig } from 'vite'
import dts from 'vite-plugin-dts'
import { cjsInterop } from "vite-plugin-cjs-interop";


export default defineConfig({
  plugins: [
    dts(),
  ],
  build: {
    minify: false,
    target: 'esnext',
    
    lib: {
      entry: 'src/index.ts',
      name: 'smoogipoo.osu-native',
      fileName: 'index',
      formats: ['es', 'cjs'],
    },
    rollupOptions: {
      external: ['./_framework/dotnet.js'],
    }
  }
})