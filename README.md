# Apos.Input

Polling input library for MonoGame.

[![Discord](https://img.shields.io/discord/355231098122272778.svg)](https://discord.gg/N9t26Uv)

## Documentation

* [Getting started](https://apostolique.github.io/Apos.Input/getting-started/)
* [Design choices](https://apostolique.github.io/Apos.Input/design-choices/)
* [API](https://apostolique.github.io/Apos.Input/api/)

## Build

[![NuGet](https://img.shields.io/nuget/v/Apos.Input.svg)](https://www.nuget.org/packages/Apos.Input/) [![NuGet](https://img.shields.io/nuget/dt/Apos.Input.svg)](https://www.nuget.org/packages/Apos.Input/)

## Features

* Manages the input states for you every frame
* Mouse, Keyboard, and GamePad buttons abstractions
* Tracking mode so that you don't accidentally consume the same input multiple times
* Static or instanced usage

## Usage samples

In your game's `LoadContent()`, pass the game class to `InputHelper.Setup()`:

```csharp
protected override void LoadContent() {
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
//Create a condition to jump.
//It should work on space, the first gamepad's A button, or the mouse's left button.
ICondition jump =
    new AnyCondition(
        new KeyboardCondition(Keys.Space),
        new GamePadCondition(GamePadButton.A, 0),
        new MouseCondition(MouseButton.LeftButton)
    );
```

```csharp
//To check if the jump is triggered:
if (jump.Pressed()) {
    //Do the jump change.
}
```

## Other projects you might like

* [Apos.Gui](https://github.com/Apostolique/Apos.Gui) - UI library for MonoGame.
* [Apos.History](https://github.com/Apostolique/Apos.History) - A C# library that makes it easy to handle undo and redo.
* [Apos.Content](https://github.com/Apostolique/Apos.Content) - Content builder library for MonoGame.
* [Apos.Framework](https://github.com/Apostolique/Apos.Framework) - Game architecture for MonoGame.
* [AposGameStarter](https://github.com/Apostolique/AposGameStarter) - MonoGame project starter. Common files to help create a game faster.
