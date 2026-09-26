# Testing
This guide will show you how to drive your input from a test instead of the real devices.

You can hand `InputHelper` a `SimulatedInput`, set the states you want, then run a frame. There's no need for a game, a window or a graphics device:

```csharp
var input = new SimulatedInput();
InputHelper.Setup(input);

input.Keyboard = new KeyboardState(Keys.Space);
InputHelper.UpdateSetup(new GameTime(TimeSpan.FromMilliseconds(16), TimeSpan.FromMilliseconds(16)));

bool jumped = new KeyboardCondition(Keys.Space).Pressed();
```

Each `UpdateSetup` reads the states once, the same way it reads the devices in a game. So a key that's down for two frames in a row is `Pressed` on the first one and `HeldOnly` on the second.

## Setting the states

The states are the regular MonoGame ones, so you build them with their constructors:

```csharp
input.Keyboard = new KeyboardState(Keys.LeftControl, Keys.C);
input.Mouse = new MouseState(100, 50, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
input.GamePads[0] = new GamePadState(new GamePadThumbSticks(), new GamePadTriggers(), new GamePadButtons(Buttons.A), new GamePadDPad());
input.Touch = new TouchCollection([new TouchLocation(1, TouchLocationState.Pressed, new Vector2(200, 100))]);
```

`IsActive`, `WindowWidth` and `WindowHeight` are there too. Set `IsActive` to false to test what happens when the game loses [focus](../focus/README.md).

Text goes through `Type`. It shows up in `InputHelper.TextEvents` on the next frame:

```csharp
input.Type("hello");
input.Type('\b', Keys.Back);
```

## Time

Durations like `HoldCondition` and `RepeatCondition` read the `GameTime` you pass to `UpdateSetup`. Step it by a fixed amount each frame and the results are exact:

```csharp
TimeSpan time = TimeSpan.Zero;

void Frame(params Keys[] keys) {
    input.Keyboard = new KeyboardState(keys);
    time += TimeSpan.FromMilliseconds(20);
    InputHelper.UpdateSetup(new GameTime(time, TimeSpan.FromMilliseconds(20)));
}
```

Poll durations every frame like a game would. A frame where nobody asked starts them over.

## Your own source

`SimulatedInput` is one implementation of `IInputSource`. You can write your own, for example to replay input you recorded from a real play session.

`InputHelper` is static, so tests that use it can't run at the same time. With xUnit you can turn off parallel tests for the assembly:

```csharp
[assembly: CollectionBehavior(DisableTestParallelization = true)]
```
