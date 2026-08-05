# Durations
This guide will show you how to build a long press and a key repeat.

Both wrap another `ICondition` instead of taking a key, so the same long press works on a keyboard, a gamepad, a mouse, or a touch screen, and on a combination of all four.

## Holding

`HoldCondition` triggers once its inner condition has been held long enough:

```csharp
ICondition radialMenu =
    new HoldCondition(
        new AnyCondition(
            new KeyboardCondition(Keys.Tab),
            new TouchCondition()
        ),
        500
    );

if (radialMenu.Pressed()) {

    // Open the menu after half a second.

}
```

It's a normal condition. Its state is "the inner condition has been held at least this long", run through the same four states from [Conditions](../conditions/README.md), so `Pressed` triggers once when the hold completes and `Released` once when it's let go. You can hand it to an `AnyCondition` like anything else.

Holding a key for 1500ms against a 500ms hold measures out like this:

```
key down            at   2083.3
hold Pressed        at   2583.3  (  500.0 ms after key down, progress 1.00)
hold Released       at   3583.3
key up              at   3583.3  (held 1500.0 ms)

hold.Pressed=1 hold.Released=1
```

`Progress` runs from 0 to 1 along the way and reaches 1 on the same frame `Pressed` triggers, which is what you draw a filling ring with:

```csharp
float fill = radialMenu.Progress;
```

It's linear and raw. If you want it eased, that's a call on your side, since how a charge up should feel belongs to your game and not to an input library.

## Repeating

`RepeatCondition` repeats a condition the way a key repeats while you hold it in a text field. It triggers once right away, waits out the delay, then keeps going on the interval:

```csharp
ICondition nextRow = new RepeatCondition(new KeyboardCondition(Keys.Down), 400, 60);

if (nextRow.Pressed()) {

    // Move down one row, then keep moving while the key stays down.

}
```

With a 400ms delay and a 100ms interval, holding the key produces this:

```
repeat #1  at   2083.3  (    0.0 ms after key down)
repeat #2  at   2483.3  (  400.0 ms after key down,  400.0 since last)
repeat #3  at   2583.3  (  500.0 ms after key down,  100.0 since last)
repeat #4  at   2683.3  (  600.0 ms after key down,  100.0 since last)
```

A frame hitch long enough to owe several repeats pays out one and moves on, so a stall doesn't come back as a burst.

## They read the game clock

Both read `InputHelper.TotalMS`, which `UpdateSetup(gameTime)` sets from `GameTime.TotalGameTime`. You're already passing it, so there's nothing extra to wire up.

That clock doesn't stretch when your game pauses its own simulation. If you scale your world update by your own `dt`, a 500ms hold stays 500ms through slow motion and through a pause.

`TotalMS` has a setter, so you can drive it yourself for replays or a custom clock.

## Check them every frame

Both measure from the frame you started asking. A condition that doesn't get polled has no way to know how long anything has been down, so a gap in the polling starts it over rather than counting time nobody was watching.

Polled every frame, a 500ms hold fires at 500ms. The same condition left unpolled from 200ms to 700ms into the hold fires at 1200ms instead, since it restarts when the polling comes back.

In practice you check a keybind every frame and never notice. It only shows up if you put one inside an `if` that skips frames.

## Limits

`RepeatCondition.Pressed` breaks the usual pairing on purpose. It triggers many times before a single `Released`, which is what a repeat is. `Held`, `HeldOnly`, and `Released` come straight from the wrapped condition.

There's no combo or sequence condition. Pressing A then B within a window is a sequence rather than a duration, and it needs a different machine.

Neither of these is a fighting game input parser. Motion inputs need direction abstraction, leniency, and priority between overlapping moves, and all three depend on the game rather than on the device.

## Follow up

[Text input](../text-input/README.md), a guide on reading typed characters across keyboard layouts.
