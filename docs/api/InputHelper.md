# InputHelper

This static class holds all the data required to handle inputs. It takes care of saving the previous and new input states.

## Source code

Read the [source code](https://github.com/Apostolique/Apos.Input/blob/master/Source/InputHelper.cs).

## Game

```csharp
public static Game Game {
    get;
    set;
}
```

Your game class is available here. This gets initialized by the `Setup` function.

## IsActive

```csharp
public static bool IsActive => Game.IsActive;
```

This is useful to disable inputs when the game is not active.

## Window

```csharp
public static GameWindow Window => Game.Window;
```

This comes from `Game.Window`.

## WindowWidth

```csharp
public static int WindowWidth => Window.ClientBounds.Width;
```

This comes from `Window.ClientBounds.Width`. Used so that `Pressed` mouse inputs are limited to the inside of the window.

## WindowHeight

```csharp
public static int WindowHeight => Window.ClientBounds.Height;
```

This comes from `Window.ClientBounds.Height`. Used so that `Pressed` mouse inputs are limited to the inside of the window.

## OldMouse

```csharp
public static MouseState OldMouse => _oldMouse;
```

This is set during `UpdateSetup()`. Used by MouseCondition in `Pressed`, `HeldOnly`, `Released`.

## NewMouse

```csharp
public static MouseState NewMouse => _newMouse;
```

This is set during `UpdateSetup()`. Used by the MouseCondition `Pressed`, `Held`, `HeldOnly`, `Released`.

## OldKeyboard

```csharp
public static KeyboardState OldKeyboard => _oldKeyboard;
```

This is set during `UpdateSetup()`. Used by KeyboardCondition in `Pressed`, `HeldOnly`, `Released`.

## NewKeyboard
```csharp
public static KeyboardState NewKeyboard => _newKeyboard;
```

This is set during `UpdateSetup()`. Used by the KeyboardCondition `Pressed`, `Held`, `HeldOnly`, `Released`.

## OldGamePad

```csharp
public static GamePadState[] OldGamePad => _oldGamePad;
```

This is set during `UpdateSetup()`. Used by GamePadCondition and AnyGamePadCondition in `Pressed`, `HeldOnly`, `Released`.

Contains all the previous gamepad states.

## NewGamePad

```csharp
public static GamePadState[] NewGamePad => _newGamepad;
```

This is set during `UpdateSetup()`. Used by GamePadCondition and AnyGamePadCondition `Pressed`, `Held`, `HeldOnly`, `Released`.

Contains all the current gamepad states.

## GamePadCapabilities

```csharp
public static GamePadCapabilities[] GamePadCapabilities => _gamePadCapabilities;
```

An array with all the info about each connected gamepad.

## GamePadDeadZone

```csharp
public static GamePadDeadZone[] GamePadDeadZone => _gamePadDeadZone;
```

Initialized to `GamePadDeadZone.None` for each gamepad. You can use this to set the dead zone for any gamepads.

## NewTouchCollection

```csharp
public static TouchCollection NewTouchCollection => _newTouchCollection;
```

A touch collection that holds the previous and current touch locations.

## TouchPanelCapabilities

```csharp
public static TouchPanelCapabilities TouchPanelCapabilities => _touchPanelCapabilities;
```

Gives info about a touch panel.

## TextEvents

```csharp
public static List<TextInputEventArgs> TextEvents => _textEvents;
```

Used to handle text input from any keyboard layouts. This is useful when coding textboxes. This plugs into the MonoGame `Window.TextInput`. It gets cleared every frame during `UpdateCleanup()`. The way to use this is to iterate over the list every frame. Each element will contain a char to write.

## MouseButtons

```csharp
public static Dictionary<MouseButton, Func<MouseState, ButtonState>> MouseButtons => _mouseButtons;
```

Maps a MouseButton to a function that can extract a specific ButtonState from a MouseState.

## GamePadButtons

```csharp
public static Dictionary<GamePadButton, Func<GamePadState[], int, ButtonState>> GamePadButtons => _gamePadButtons;
```

Maps a GamePadButton to a function that can extract a specific ButtonState from a GamePadState.

## CurrentFrame

```csharp
public static uint CurrentFrame => _currentFrame;
```

Used by conditions to know if they've been consumed this frame.

## Setup

```csharp
public static void Setup(Game game);
```

Call this in the game's LoadContent. This initializes everything this library needs to function correctly.

## UpdateSetup

```csharp
public static void UpdateSetup();
```

Call this at the beginning of your update loop. This sets up everything needed to use inputs for this frame.

## UpdateCleanup

```csharp
public static void UpdateCleanup();
```
Call this at the end of your update loop. This clears the `TextEvents` list.
