# Getting started

## Install

Install using the following dotnet command:
```
dotnet add package Apos.Input
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

## Instance usage

Create a keyboard condition:
```csharp
ICondition jump = new KeyboardCondition(Keys.Space);
```

Create a gamepad condition for the first gamepad:
```csharp
ICondition jump = new GamePadCondition(GamePadButton.A, 0);
```

Create a mouse condition:
```csharp
ICondition jump = new MouseCondition(MouseButton.LeftButton);
```

You can combine the 3 using AnyCondition:
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
