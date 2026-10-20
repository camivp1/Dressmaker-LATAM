# Dressmaker LATAM

## Español

Dressmaker LATAM es una localización completa al español latinoamericano (`es-419`) para **Dressmaker**.

Autora/mantenedora: `camivp1`.

El proyecto incluye traducciones finales para:

| Tabla | Estado |
| --- | ---: |
| UI | 221/221 |
| Tutorial | 73/73 |
| Content | 1941/1941 |
| Dialogue | 3677/3677 |
| Total | 5912/5912 |

### Enfoque de traducción

- Español latinoamericano natural, con buena lectura para Chile y Latinoamérica.
- Adaptación contextual en vez de calcos literales.
- Conservación del tono, humor, rimas y juegos de palabras cuando corresponde.
- Terminología de costura revisada en contexto.
- Neutralidad de género para el personaje jugador cuando el original inglés no especifica género.
- Nombres propios, nombres estilizados y términos internos se conservan cuando traducirlos sería menos claro o incorrecto.

### Requisito técnico

La versión runtime está pensada para:

- BepInEx 5.4.23.5
- Windows x64
- Unity Mono

El plugin registra el locale `es-419`, carga las String Tables desde CSV externos y reutiliza los assets gráficos ingleses para las Asset Tables del locale LATAM.

### Instalación para jugar/probar

1. Instala BepInEx 5.4.23.5 Windows x64 en la carpeta raíz de Dressmaker, o usa un ZIP release autocontenido cuando esté disponible.
2. Copia `DressmakerLATAM.dll` y los CSV runtime (`UI.csv`, `Tutorial.csv`, `Content.csv`, `Dialogue.csv`) en:

   ```text
   BepInEx/plugins/DressmakerLATAM/
   ```

3. Inicia Dressmaker desde Steam.
4. Selecciona `Español (Latinoamérica)` en las opciones del juego.

### Desinstalación

Elimina la carpeta:

```text
BepInEx/plugins/DressmakerLATAM/
```

Si instalaste BepInEx solo para este mod y quieres quitarlo por completo, elimina también los archivos de BepInEx de la carpeta raíz del juego.

### CSV públicos

Los CSV públicos versionados están en `localization/` y contienen únicamente:

```csv
ID,Español
```

Los CSV editoriales completos con `source_text` no forman parte del repositorio público.

### Reportar bugs

Al reportar un problema, incluye:

- captura de pantalla o descripción clara;
- dónde ocurrió;
- texto observado y texto esperado, si aplica;
- `BepInEx/LogOutput.log`, si existe.

### Estado actual

Versión pública actual: `v1.0.2`. La traducción completa está cerrada y validada; el build incluye correcciones de QA y fallback de Asset Tables a los assets ingleses.

### Licencias y atribución

El código original del plugin Dressmaker LATAM está bajo licencia MIT; ver [LICENSE](LICENSE).

Dressmaker y sus contenidos originales pertenecen a sus respectivos propietarios. Las traducciones se distribuyen como parte de este mod/localización y no implican propiedad sobre el texto original del juego.

No se incluyen traducciones de mods de terceros en este repositorio.

## English

Dressmaker LATAM is a complete Latin American Spanish (`es-419`) localization for **Dressmaker**.

Author/maintainer: `camivp1`.

Current translation status:

| Table | Status |
| --- | ---: |
| UI | 221/221 |
| Tutorial | 73/73 |
| Content | 1941/1941 |
| Dialogue | 3677/3677 |
| Total | 5912/5912 |

### Translation approach

- Natural Latin American Spanish, with readability for Chile and wider LATAM.
- Contextual localization instead of literal conversion.
- Tone, humor, rhymes, and wordplay adapted where needed.
- Sewing terminology reviewed in context.
- Gender-neutral phrasing for the player character when the English source is neutral.
- Proper names, stylized names, and internal strings are preserved when translation would be unclear or misleading.

### Technical requirement

The runtime plugin targets:

- BepInEx 5.4.23.5
- Windows x64
- Unity Mono

The plugin registers the `es-419` locale, loads String Tables from external CSV files, and reuses English graphical assets for localized Asset Tables.

### Installation

1. Install BepInEx 5.4.23.5 Windows x64 into the Dressmaker game root, or use a self-contained release ZIP when available.
2. Copy `DressmakerLATAM.dll` and the runtime CSV files (`UI.csv`, `Tutorial.csv`, `Content.csv`, `Dialogue.csv`) into:

   ```text
   BepInEx/plugins/DressmakerLATAM/
   ```

3. Launch Dressmaker from Steam.
4. Select `Español (Latinoamérica)` in the game options.

### Uninstall

Delete:

```text
BepInEx/plugins/DressmakerLATAM/
```

If BepInEx was installed only for this mod, remove the BepInEx files from the game root as well.

### Public CSV files

The public CSV files are versioned under `localization/` and contain only:

```csv
ID,Español
```

Full editorial CSV files with `source_text` are not part of the public repository.

### Bug reports

Please include a screenshot or clear description, where it happened, the observed/expected text if relevant, and `BepInEx/LogOutput.log` if available.

### Current status

Current public version: `v1.0.2`. The full translation is complete and validated; the build includes QA text fixes and Asset Table fallback to English assets.

### License and attribution

The original Dressmaker LATAM plugin code is licensed under MIT; see [LICENSE](LICENSE).

Dressmaker and its original content belong to their respective rights holders. The translations are distributed as part of this mod/localization and do not imply ownership of the original game text.

No third-party mod translations are included in this repository.
