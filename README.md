# UnityFastCompiler
**Take back control of your Unity workflow.** Stop automatic recompilation on save, prevent Domain Reload freezes, and compile only when *YOU* want to. 

By default, Unity freezes and recompiles every time you save a C# script. If your project is medium/large, this means losing focus constantly. FastCompiler is a simple, lightweight Editor extension that kills the auto-compile and lets you trigger it manually via a Toolbar button or a hotkey (`F5`).

<img width="475" height="132" alt="image" src="https://github.com/user-attachments/assets/d2a745b3-fee7-45ed-bb87-17f0049481f0" />


## Installation
1. Download the two scripts: `FastCompileManager.cs` and `FastCompileToolbar.cs`.
2. Place them inside the `Assets/Editor` folder in your Unity project. *(If the `Editor` folder doesn't exist, simply create it).*
3. Open Unity project, then `Unity>Edit>Preferences>Assets Pipeline` and change this value *Auto Refresh = Disabled*
4. (Optional) Disable reload scene and domain on play
5. That's it!
