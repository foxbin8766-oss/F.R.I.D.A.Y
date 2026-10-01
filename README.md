# F.R.I.D.A.Y. Iron Man Test System

BONELAB code-mod prototype for an MCU-inspired F.R.I.D.A.Y. development assistant.

## Version 0.1.0
- Startup diagnostics
- On-screen F.R.I.D.A.Y. HUD
- Target detection and distance
- Unity version and scene diagnostics
- F8: toggle HUD
- F9: run diagnostics
- F10: force target scan

## Build
Place these BONELAB reference DLLs in `lib/`:
- `MelonLoader.dll`
- `Assembly-CSharp.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.PhysicsModule.dll`
- `UnityEngine.IMGUIModule.dll`

Then run:

`dotnet build -c Release`

Output:

`bin/Release/net6.0/FRIDAY.dll`

Copy `FRIDAY.dll` to BONELAB's `Mods` folder.

This first build intentionally focuses on the F.R.I.D.A.Y. core. Repulsors, flight, advanced HUD graphics, and voice are planned for later versions.
