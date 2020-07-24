# Apos.Input
Input library for MonoGame.

[![Discord](https://img.shields.io/discord/355231098122272778.svg)](https://discord.gg/N9t26Uv)

## Documentation

* [API](https://apostolique.github.io/Apos.Input/)

## Build

[![NuGet](https://img.shields.io/nuget/v/Apos.Input.svg)](https://www.nuget.org/packages/Apos.Input/) [![NuGet](https://img.shields.io/nuget/dt/Apos.Input.svg)](https://www.nuget.org/packages/Apos.Input/)

## Features

* Mouse, Keyboard, GamePad buttons

## Usage samples

In your game's `Initialize()`, pass the game class to `InputHelper.Setup()`:

```csharp
protected override void Initialize() {
    InputHelper.Setup(this);
}
```

In your game's `Update(GameTime gameTime)`, call the two functions:

```csharp
protected override void Update(GameTime gametime) {
    //Call UpdateSetup at the start.
    InputHelper.UpdateSetup();

    //...

    //Call UpdateCleanup at the end.
    InputHelper.UpdateCleanup();
}
```

```csharp
//Create a condition to toggle fullscreen.
//It should work on either Alt keys with Enter.
var toggleFullScreen = new AllCondition(
    new AnyCondition(
        new KeyboardCondition(Keys.LeftAlt),
        new KeyboardCondition(Keys.RightAlt)
    ),
    new KeyboardCondition(Keys.Enter)
);

//To check if toggleFullscreen is triggered:
if (toggleFullscreen.Pressed()) {
    //Do the fullscreen change.
}
```

## Other projects you might like

* [Apos.Gui](https://github.com/Apostolique/Apos.Gui) - UI library for MonoGame.
* [Apos.History](https://github.com/Apostolique/Apos.History) - A C# library that makes it easy to handle undo and redo.
* [Apos.Content](https://github.com/Apostolique/Apos.Content) - Content builder library for MonoGame.
* [Apos.Framework](https://github.com/Apostolique/Apos.Framework) - Game architecture for MonoGame.
* [AposGameStarter](https://github.com/Apostolique/AposGameStarter) - MonoGame project starter. Common files to help create a game faster.
