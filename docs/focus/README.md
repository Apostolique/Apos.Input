# Focus
This guide will show you what conditions do when your game loses focus.

A button only counts as `on` while your game is active. Alt tabbing away reports `Released`, and coming back with the button still down reports `Pressed`.

That means every `Pressed` gets exactly one matching `Released`, so code that pairs them up never gets stuck holding an input the player already let go of.

```csharp
ICondition drag = new MouseCondition(MouseButton.LeftButton);

if (drag.Pressed()) _dragging = true;
if (drag.Released()) _dragging = false;
```

Without the rule, alt tabbing mid drag leaves `_dragging` true forever.

## Why the device can't be trusted with it

The obvious fix is to wait for the device to report the release. That doesn't work, because the platforms disagree about what a held input does when focus goes away.

Holding a key and a mouse button, then stealing focus, then letting go while unfocused, MonoGame on DesktopGL reports this:

| Device | On the frame focus is lost |
| --- | --- |
| Keyboard | Already up, SDL clears the keyboard |
| Mouse | Still down, and it reports the release a few frames into being unfocused |
| Gamepad | Not attached to a window, so it keeps reporting either way |

Three devices, three answers. Anything that waits for the transition fixes the keyboard and quietly leaves the other two broken.

So the release comes from the focus change instead:

```
Down    = IsActive    && held now
WasDown = OldIsActive && held last frame

Pressed  =  Down && !WasDown        HeldOnly =  Down &&  WasDown
Held     =  Down                    Released = !Down &&  WasDown
```

It's the same four states from [Conditions](../conditions/README.md), measured over "down and focused" rather than "down". Losing focus flips `Down` to false while `WasDown` is still true, which is a `Released` without asking the device anything.

It's also idempotent. A device that reports its own release later can't produce a second one, since `WasDown` has already gone false by then.

## Only the instance methods do this

`KeyboardCondition.Pressed(Keys.Space)` reports the raw device and ignores focus. `new KeyboardCondition(Keys.Space).Pressed()` applies the rule.

That split is on purpose. The statics are the honest device state and stay useful for anything that wants it, including debug overlays and your own state machines. `InputHelper.NewKeyboard` and friends are never rewritten, so what the library polled is always what you read.

`InputHelper.IsActive` and `OldIsActive` are public if you want to make the same distinction yourself.

## Clicking back into the window counts as a click

Coming back with a button already down reports `Pressed`, which is what makes clicking on an unfocused window to focus it register as a click rather than being swallowed.

The mouse has had that behaviour since 2.4.0. It now applies to the keyboard and gamepad too, so all three pair up the same way.

## Limits

A press that starts while your game is unfocused doesn't exist as far as conditions are concerned, and it becomes a `Pressed` on the frame you get focus back. If you care about when the player physically pushed the button rather than when your game could see it, the timestamp you want isn't available.

Focus is per game window, not per control. The library has no idea which of your panels or text boxes should be receiving input, and it doesn't try to.

## Follow up

[Durations](../durations/README.md), a guide on long presses and key repeat, which build on the same four states.
