# Conditions
This guide will show you the four states every condition has, and the devices they read from.

A button is either `on` or `off`. Comparing last frame against this frame gives four useful questions, and `ICondition` is the interface with all four on it:

```csharp
bool Pressed(bool canConsume = true);
bool Held(bool canConsume = true);
bool HeldOnly(bool canConsume = true);
bool Released(bool canConsume = true);
void Consume();
```

Hold a key down for three frames and they line up like this:

```
frame        1     2     3     4     5
button      off   on    on    on    off
-------------------------------------------
Pressed            x
Held               x     x     x
HeldOnly                 x     x
Released                             x
```

`Pressed` is the moment it becomes `on`, and it triggers on that one frame only. `Held` is true the whole time it stays `on`. `HeldOnly` is the same minus the first frame, which makes it a way to break an action into a start and a continuation. `Released` is the moment it becomes `off`, and it triggers once too.

`Consume` only matters once you opt into [tracking](../tracking/README.md). Until then, `canConsume` does nothing and you can ignore it.

## One condition per device

Each device has its own condition, and they all implement `ICondition`, so they're interchangeable everywhere:

```csharp
ICondition jump = new KeyboardCondition(Keys.Space);
ICondition jump = new MouseCondition(MouseButton.LeftButton);
ICondition jump = new GamePadCondition(GamePadButton.A, 0);
ICondition jump = new TouchCondition();
```

`GamePadCondition` takes the gamepad index, so `0` is the first controller. `AnyGamePadCondition` takes only the button and triggers when any connected gamepad has it, which is what a couch multiplayer menu wants.

`MouseButton` is `LeftButton`, `MiddleButton`, `RightButton`, `XButton1`, and `XButton2`. `GamePadButton` covers the face buttons, `Start`, `Back`, `BigButton`, both shoulders, both stick clicks, and the four `DPad` directions.

Touch has a guide of its own, since where a contact landed matters more than that it happened. Read [Touch](../touch/README.md).

## Combining them

`AnyCondition` is `or`. It triggers when at least one of its conditions does, which is how one action ends up working on every device at once:

```csharp
ICondition jump =
    new AnyCondition(
        new KeyboardCondition(Keys.Space),
        new GamePadCondition(GamePadButton.A, 0),
        new MouseCondition(MouseButton.LeftButton),
        new TouchCondition()
    );
```

`AllCondition` is `and`. Every condition has to trigger for it to trigger, which gives you modifiers:

```csharp
ICondition run =
    new AllCondition(
        new KeyboardCondition(Keys.LeftShift),
        new KeyboardCondition(Keys.Right)
    );
```

Both are `ICondition` themselves, so they nest. A `HoldCondition` wrapping an `AnyCondition` of four devices is a long press that works on all four.

## Static usage

If you don't want to instantiate anything, the same checks are available as static methods:

```csharp
if (KeyboardCondition.Pressed(Keys.Space) ||
    GamePadCondition.Pressed(GamePadButton.A, 0) ||
    MouseCondition.Pressed(MouseButton.LeftButton) ||
    TouchCondition.Pressed()) {

    // Do the jump.

}
```

The static methods report the raw device and ignore whether your game has focus. The instance methods don't, which is the one behaviour difference between the two. Read [Focus](../focus/README.md).

## The mouse has more than buttons

The scroll wheel and the pointer aren't buttons, so they get their own checks:

```csharp
if (MouseCondition.Scrolled()) {
    _zoom += MouseCondition.ScrollDelta;
}
if (MouseCondition.PointerMoved()) {
    _cursor += MouseCondition.PointerDelta.ToVector2();
}
```

`MouseCondition.IsMouseValid` is true while the game has focus and the cursor is inside the window. `Pressed` already checks it for you, so a click that lands outside the window doesn't start anything. `Held` and `Released` deliberately don't, which is what lets a drag keep working after the cursor leaves the window.

## The raw states are there too

`InputHelper` keeps what it polled, in `OldMouse` and `NewMouse`, `OldKeyboard` and `NewKeyboard`, `OldGamePad` and `NewGamePad`. Conditions are built out of these, and you can read them directly when you need something the conditions don't cover:

```csharp
Vector2 stick = InputHelper.NewGamePad[0].ThumbSticks.Left;
```

`InputHelper.GamePadDeadZone` starts at `GamePadDeadZone.None` for every pad and you can set it per pad. `InputHelper.GamePadCapabilities` says what each connected pad actually has.

## Follow up

[Tracking](../tracking/README.md), a guide that shows how to stop two conditions from firing on the same button press.
