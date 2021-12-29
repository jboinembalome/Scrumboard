const plugin = require('tailwindcss/plugin');
const buildMediaQuery = require('tailwindcss/lib/util/buildMediaQuery').default;

const extractConfig = plugin(({
    addVariant,
    theme
}) =>
{
    addVariant('blouppy-tailwind-extracted-config', ({container}) =>
    {
        // Prepare the extracted config variable
        let extractedConfig = '';

        // Breakpoints
        Object.entries(theme('screens')).forEach(([key, value]) =>
        {
            extractedConfig = `${extractedConfig} --breakpoints-${key}:'${buildMediaQuery(value)}';`;
        });

        // Themes
        (theme('blouppy.themes')).forEach((value) =>
        {
            Object.entries(value).forEach(([key, value]) =>
            {
                extractedConfig = `${extractedConfig} --themes-${key}:'${JSON.stringify(value)}';`;
            });
        });

        // Append the extracted config
        container.append(`
            .blouppy-tailwind-extracted-config {
                ${extractedConfig}
            }
        `);
    });
});

module.exports = extractConfig;
