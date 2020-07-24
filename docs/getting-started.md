# Getting started

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
