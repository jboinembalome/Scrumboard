const colors = require('tailwindcss/colors')

module.exports = {
  darkMode: 'class',
  important: true,
  purge: {
    enabled: true,
    content: ['./src/**/*.{html,scss,ts}'],
  },
  theme: {
    cursor: {
      auto: 'auto',
      default: 'default',
      pointer: 'pointer',
      wait: 'wait',
      text: 'text',
      move: 'move',
      'not-allowed': 'not-allowed',
      crosshair: 'crosshair',
      'zoom-in': 'zoom-in',
      grab: 'grab'
    },
    extend: {

    },
    colors: {
      transparent: 'transparent',
      current: 'currentColor',
      black: colors.black,
      white: colors.white,
      gray: colors.blueGray,
      teal: colors.teal,
      green: colors.green,
      amber: colors.amber,
      orange: colors.orange,
      violet: colors.violet,
      purple: colors.purple,
      pink: colors.pink,
      rose: colors.rose,
      indigo: colors.indigo,
      red: colors.red,
      yellow: colors.yellow,
      blue: colors.blue
    }
  },
  variants: {},
  plugins: [
    require('@tailwindcss/forms'),
  ],
};