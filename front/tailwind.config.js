const colors = require('tailwindcss/colors');

module.exports = {
  darkMode: 'class',
  content: ['./src/**/*.{html,scss,ts}'],
  important: true,
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
      current: 'currentColor', // Useful to update the color of svg
      white: '#ffffff',
      primary: {
        DEFAULT: '#676dcf',
        50: '#f5f5fc',
        100: '#ebebf9',
        200: '#ced0f2',
        300: '#b5bfeb',
        400: '#8e93dc',
        500: '#676dcf',
        600: '#5052a7',
        700: '#3a3e80',
        800: '#262b5a',
        900: '#131633'
      },
      accent: {
        DEFAULT: '#ff4c4c',
        50: '#fff7f6',
        100: '#ffe6e4',
        200: '#ffa7a3',
        300: '#ffb7b3',
        400: '#ff817f',
        500: '#ff4c4c',
        600: '#e63f40',
        700: '#b73234',
        800: '#8f2728',
        900: '#6d1c1c'
      },
      warn: {
        ...colors.red,
      },
      slate: colors.slate,
      gray: colors.gray,
      red: colors.red,
      orange: colors.orange,
      yellow: colors.yellow,
      green: colors.green,
      blue: colors.blue,
      indigo: colors.indigo,
      purple: colors.purple,
      amber: colors.amber,
      black: colors.black,
      white: colors.white,
      teal: colors.teal,
      violet: colors.violet,
      pink: colors.pink,
      rose: colors.rose
    },
  },
  plugins: [
  ],
};