# Getting started

## Install

Install using the following dotnet command:
```
dotnet add package Apos.Input
```

## Setup

Import the library with:
```csharp
using Apos.Input;
using Track = Apos.Input.Track;
```


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

## Static usage

If you don't want to setup anything, you can use `KeyboardCondition`, `GamePadCondition`, and `MouseCondition` statically.

```csharp
if (KeyboardCondition.Pressed(Keys.Space) ||
    GamePadCondition.Pressed(GamePadButton.A, 0) ||
    MouseCondition.Pressed(MouseButton.LeftButton)) {

    // Do the jump.

}
```

## Instanced usage

Let's create a condition for a `jump`.

Create a keyboard condition:
```csharp
ICondition jump = new KeyboardCondition(Keys.Space);
```

Create a gamepad condition for the first gamepad which is at index 0:
```csharp
ICondition jump = new GamePadCondition(GamePadButton.A, 0);
```

Create a mouse condition:
```csharp
ICondition jump = new MouseCondition(MouseButton.LeftButton);
```

Use `AnyCondition` to combine the three conditions together:
```csharp
ICondition jump =
    new AnyCondition(
        new KeyboardCondition(Keys.Space),
        new GamePadCondition(GamePadButton.A, 0),
        new MouseCondition(MouseButton.LeftButton)
    );
```

To use `jump`, you can do:

```csharp
if (jump.Pressed()) {

    // Do the jump.

}
```

Use `AllCondition` to require multiple keys at the same time.

```csharp
ICondition run =
    new AllCondition(
        new KeyboardCondition(Keys.Shift),
        new KeyboardCondition(Keys.Right)
    );

if (run.Held()) {

    // Run while the buttons are held.

}
```

If you also want to `walk`, you can do:

```csharp
ICondition walk = new KeyboardCondition(Keys.Right);

if (walk.Held()) {

    // Walk while the buttons are held.

}
```

If you combine both, it gives:

```csharp
if (run.Held()) {

    // Run while the buttons are held.

}
if (walk.Held()) {

    // Walk while the buttons are held.

}
```

An issue arises where `walk` will trigger during the `run` since both need the `Right` button. To fix that, you can opt into the tracking system for the `Right` button. The tracking system will make `walk` never trigger during `run`.

```csharp
ICondition run =
    new AllCondition(
        new KeyboardCondition(Keys.Shift),
        new Track.KeyboardCondition(Keys.Right)
    );
ICondition walk = new Track.KeyboardCondition(Keys.Right);
```
