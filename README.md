# Apos.Gui
Input library for MonoGame.

[![Discord](https://img.shields.io/discord/355231098122272778.svg)](https://discord.gg/N9t26Uv)

## Documentation

* [API](https://apostolique.github.io/Apos.Input/)

## Build

### [NuGet](https://www.nuget.org/packages/Apos.Input/) [![NuGet](https://img.shields.io/nuget/v/Apos.Input.svg)](https://www.nuget.org/packages/Apos.Input/) [![NuGet](https://img.shields.io/nuget/dt/Apos.Input.svg)](https://www.nuget.org/packages/Apos.Input/)

## Features

* Mouse, Keyboard, GamePad buttons

## Usage samples

```csharp
//Create an action to toggle fullscreen.
//It should work on both Alt keys and Enter.
var toggleFullScreen = new ActionKeyboardComposite();
toggleFullscreen.AddSet(Keys.Enter).AddNeed(Keys.LeftAlt);
toggleFullscreen.AddSet(Keys.Enter).AddNeed(Keys.RightAlt);

//To check if toggleFullscreen is triggered:
if (toggleFullscreen.Pressed()) {
    //Do the fullscreen change.
}
```
