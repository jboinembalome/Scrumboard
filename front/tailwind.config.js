const path = require('path');
const colors = require('tailwindcss/colors');
const generatePalette = require(path.resolve(__dirname, ('src/app/shared/tailwind/utils/generate-palette')));

/**
 * Custom palettes
 *
 * Uses the generatePalette helper method to generate
 * Tailwind-like color palettes automatically
 */
const customPalettes = {
  brand: generatePalette('#2196F3')
};

/**
* Themes
*/
const themes = {
  // Default theme is required for theming system to work correctly
  'default': {
    primary: {
      ...colors.indigo,
      DEFAULT: colors.indigo[600]
    },
    accent: {
      ...colors.slate,
      DEFAULT: colors.slate[800]
    },
    warn: {
      ...colors.red,
      DEFAULT: colors.red[600]
    },
    'on-warn': {
      500: colors.red['50']
    }
  },
  // Rest of the themes will use the 'default' as the base theme
  // and extend them with their given configuration
  'brand': {
    primary: customPalettes.brand
  },
  'indigo': {
    primary: {
      ...colors.teal,
      DEFAULT: colors.teal[600]
    }
  },
  'rose': {
    primary: colors.rose
  },
  'purple': {
    primary: {
      ...colors.purple,
      DEFAULT: colors.purple[600]
    }
  },
  'amber': {
    primary: colors.amber
  }
};

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
      current: 'currentColor',
      black: colors.black,
      white: colors.white,
      gray: colors.slate,
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
  variants: {
    extend: {
      backgroundColor: ['disabled'],
      textColor: ['disabled'],
      opacity: ['disabled'],
    }
  },
  plugins: [
    // Blouppy - Tailwind plugins
    require(path.resolve(__dirname, ('src/app/shared/tailwind/plugins/extract-config'))),
    require(path.resolve(__dirname, ('src/app/shared/tailwind/plugins/utilities'))),
    require(path.resolve(__dirname, ('src/app/shared/tailwind/plugins/icon-size'))),
    require(path.resolve(__dirname, ('src/app/shared/tailwind/plugins/theming')))({themes})
  ],
};