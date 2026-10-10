# Guia de estilo es-419

## Objetivo

Usar espanol latinoamericano neutral, natural para una persona de Chile y Latinoamerica. El texto debe sonar claro, fluido y propio de un videojuego, sin caer en literalismos innecesarios.

## Variante regional

- Evitar espanol peninsular y formas como `vosotros`, `vale`, `ordenador`, `movil`, `coger` o `pulsa`.
- No llenar el texto de chilenismos. La prioridad es un es-419 natural y ampliamente comprensible.
- Priorizar significado, naturalidad, claridad, consistencia terminologica y tono del juego.

## Genero del jugador

- No asignar genero al Dressmaker/personaje jugador cuando el ingles sea neutral.
- Evitar traducciones como `lista`, `satisfecha`, `preparada`, etc. si se refieren al jugador y el ingles no marca genero.
- Reformular de manera natural y neutral. Por ejemplo, para `when ready`, preferir segun contexto `cuando quieras`, `cuando termines` o `cuando este todo listo`.
- Mantener el genero de los NPC cuando el contexto si lo establece.

## Acciones de interfaz

Distinguir siempre estas acciones:

- `click` -> `hacer clic`
- `right click` -> `hacer clic derecho`
- `select` -> `seleccionar`
- `drag` -> `arrastrar`

No sustituir `click` por `seleccionar` salvo que el contexto realmente indique una seleccion y no una accion de clic.

## Interfaz y etiquetas visibles

- Preservar la capitalizacion de nombres de categorias, etiquetas, pestanas o botones visibles del juego cuando funcionen como nombre propio de UI.
- Usar lenguaje natural de videojuegos para botones, menus, opciones e instrucciones.
- No traducir el mismo termino de distintas formas sin una razon contextual clara.

## Marcado y variables

Conservar exactamente:

- placeholders;
- variables;
- tags;
- markup;
- IDs y keys;
- secuencias especiales;
- saltos de linea estructuralmente importantes.

Nunca modificar `source_text`; escribir solo en `es_419`.

## Costura

- `haberdashery` -> `cordoneria`; si se refiere a productos o categoria, usar `cordoneria` o `articulos de cordoneria` segun contexto.
- `draft pattern` -> `trazar el patron` cuando funcione como accion/verbo. Si aparece como sustantivo, marcar para revision contextual.
- `grain line` -> `sentido del hilo`, especialmente para la marca o flecha del patron que indica su orientacion sobre la tela. Ejemplos: `align the grain line` -> `alinear el sentido del hilo`; `horizontal grain line` -> `sentido horizontal del hilo`. No usar `linea de hilo` ni `direccion del hilo`, salvo que `direction` aparezca explicitamente en otro contexto y corresponda hablar de la direccion fisica de la tela.
- En la familia `Sleeve`, usar `sleeve` -> `manga` y mantener el orden natural `Manga + descriptor`. Traducir descriptores claros (`Bell Sleeve` -> `Manga campana`, `Bishop Sleeve` -> `Manga obispo`) y conservar nombres estilizados/modelos cuando corresponda (`Arwen Sleeve` -> `Manga Arwen`).
- Si un termino tecnico de costura es dudoso, marcarlo para revision en vez de inventar una solucion.
## Content textile terminology

- `burlap` -> `yute`.
- `corduroy` -> `cotele`.
- `Cross Patty` -> `cruz patada`.
- `Liberty` se mantiene como nombre/estilo de estampado.
- `Chevron` -> `espiga`.
- `burnout velvet` -> `terciopelo devore`.
- `crushed velvet` -> `terciopelo arrugado`; no usar `terciopelo aplastado`.
- `Scheele's Green` -> `verde de Scheele`.
- `orchil` -> `orchilla`.
- `Ichor` -> `icor`.
- `Violet-Crowned Woodnymph` -> `Ninfa Coronada` cuando se refiera al ave en nombres de tela.
- Si `Continuous` describe un estampado repetido, usar una formulacion natural con `estampado continuo`.
- Mantener nombres propios o de coleccion como Nadine, Rhiannon, Barbara, Bridget, Viviene, Patrice, Alexandria, Birdsong y Strawberry Fields.

