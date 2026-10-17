# Changelog

## v1.0.1

- Updated compatibility with the current Dressmaker runtime tables.
- Added newly introduced UI, tutorial, content, and panel strings.
- Updated totals to UI 221, Tutorial 73, Content 1941, and Dialogue 3677 (5912 total).
- Fixed localized Asset Table handling so English assets are reused correctly for `es-419`.
- Registered `es-419` before saved-locale lookup and displayed it as `Español (Latinoamérica)`.
- Improved translation naturalness and tutorial/tooltips during QA.
- Preserved neutral phrasing where the English source does not specify player gender.
- Corrected `GrabAll` to `Devolver todos los patrones al inventario`.
- Improved the grain-line wording and fixed two piece names that began with commas.

The broader `Panel/*` comma/order audit remains outside this release.

## v1.0.0 - Proposed

- Complete Latin American Spanish translation: 5886/5886 strings.
- UI translated: 208/208.
- Tutorial translated: 71/71.
- Content translated: 1930/1930.
- Dialogue translated: 3677/3677.
- Added runtime support for locale `es-419`.
- Added BepInEx 5.4.23.5 x64 compatible plugin.
- Added CSV-based runtime loading for Unity Localization String Tables.
- Added Asset Table fallback to English graphical assets for localized images.
- Included QA corrections for terminology, gender-neutral wording, and dialogue polish.
