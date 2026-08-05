# Getting started
This guide will show you how to get started with the Apos.Input library.

Before you start, make sure that you have a valid MonoGame project. You can create a new project by following this [other guide](https://learn-monogame.github.io/how-to/get-started/).

## Install

Install using the following dotnet command:

```
dotnet add package Apos.Input
```

You can find other ways to install it on the [NuGet page](https://www.nuget.org/packages/Apos.Input/). If you're on [KNI](https://github.com/kniengine/kni), install `Apos.Input.KNI` instead. The two are the same library built against a different framework.

## Setup

Import the library with:

```csharp
using Apos.Input;
using Track = Apos.Input.Track;
```

In your game's `LoadContent()`, pass the game class to `InputHelper.Setup()`:

```csharp
protected override void LoadContent() {
    InputHelper.Setup(this);
}
```

In your game's `Update(GameTime gameTime)`, call the two functions:

```csharp
protected override void Update(GameTime gameTime) {
    // Call UpdateSetup at the start.
    InputHelper.UpdateSetup(gameTime);

    // ...

    // Call UpdateCleanup at the end.
    InputHelper.UpdateCleanup();
}
```

`UpdateSetup` polls every device once and keeps the previous frame's states next to the new ones. That pair is what lets a condition tell a press apart from a hold. `UpdateCleanup` clears the text input events at the end of the frame.

The `GameTime` sets `InputHelper.TotalMS`, the clock that [duration based conditions](../durations/README.md) read. It's required rather than optional, since a condition waiting on a duration would otherwise compare against a clock stuck at 0 and quietly never trigger.

## Your first condition

Make a condition, then ask it whether it triggered:

```csharp
ICondition jump = new KeyboardCondition(Keys.Space);

if (jump.Pressed()) {

    // Do the jump.

}
```

You can ask anywhere in your game code that feels natural, as many times as you want, and you get the same answer for the whole frame. That's what polling buys you over events and callbacks. Nothing to subscribe to, nothing to unsubscribe from, and no handler running at a moment you didn't choose.

## Follow up

[Conditions](../conditions/README.md), a guide that covers the four states every condition has and the devices they read from.
