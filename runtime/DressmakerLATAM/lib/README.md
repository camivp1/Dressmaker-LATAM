# Dressmaker LATAM build references

Place local compile-time DLL references here. Do not commit or fabricate placeholder DLLs.

## `lib/BepInEx/`

Copy from BepInEx 5.4.23.5 Mono:

- `BepInEx.dll`
- `0Harmony.dll`

These usually come from:

```text
BepInEx/core/
```

## `lib/Game/`

Copy from the Dressmaker game managed DLL folder:

- `UnityEngine.CoreModule.dll`
- `Unity.Localization.dll`
- `Unity.Addressables.dll`
- `Unity.ResourceManager.dll`

These usually come from:

```text
Dressmaker_Data/Managed/
```

If compilation later reports missing Unity/BCL assemblies, copy the exact additional DLLs from the same `Dressmaker_Data/Managed/` folder and add an explicit non-private reference to the `.csproj`.
