# Touch
This guide will show you how to read a touch screen.

A `TouchCondition` triggers on any contact, so it drops into an `AnyCondition` next to the other devices without anything new:

```csharp
ICondition jump =
    new AnyCondition(
        new KeyboardCondition(Keys.Space),
        new TouchCondition()
    );
```

On a machine with no touch screen the panel reports nothing and the condition is just false, so adding one costs those players nothing. A Windows laptop with a touch screen does report contacts through DesktopGL, so this isn't only a phone thing.

## Where the contact landed is your problem

Tapping anywhere is rarely what you want. For a button you need to know where the finger went, and the library won't hit test for you. That's the same deal as the mouse, where you read `InputHelper.NewMouse.Position` and check it against your own rectangle.

Iterate the contacts and do the check yourself:

```csharp
foreach (TouchLocation t in InputHelper.NewTouch) {
    if (TouchCondition.Pressed(t) && jumpButton.Contains(t.Position)) {

        // Do the jump.

    }
}
```

`TouchCondition.Pressed(TouchLocation)`, `Held`, `HeldOnly`, and `Released` are the four states for one contact. The same names with no argument run over every contact, which is the tap anywhere case.

This is deliberate. A rectangle handed to the library would be in screen space, and a game that hit tests through a camera needs world space. Only your game knows which.

## Counting fingers

Give `Track.TouchCondition` a number and it claims that many contacts and holds on to them:

```csharp
ICondition dragCamera = new Track.TouchCondition(2);
ICondition draw = new Track.TouchCondition(1);

if (dragCamera.Held()) {

    // Pan and zoom. This claims two contacts, so draw won't see them.

}
if (draw.Held()) {

    // Draw with whatever finger is left.

}
```

The gesture ends when one of its contacts lifts, or when something checked earlier in the frame takes one. Nothing else ends it. Order is what decides priority, so check the greedier gesture first, and extra contacts are left alone so a third finger is still free for whatever wants it.

`TouchCondition.Count` is how many contacts are down and `OldCount` is how many were down last frame. `TouchCondition.TryGetPrimary(out TouchLocation touch)` gives you the oldest one.

## A stray finger doesn't interrupt anything

A gesture belongs to whoever claimed its contacts, the same way `Right` belongs to `run` until something consumes it. A finger landing somewhere unrelated doesn't end it:

```
f2   finger A lands     contacts=1 | stroke: P=X H=X R=.
f6   finger B lands     contacts=2 | stroke: P=. H=X R=.
f10  finger B lifts     contacts=1 | stroke: P=. H=X R=.
f14  finger A lifts     contacts=0 | stroke: P=. H=. R=X
```

One press and one release across the whole motion, because in that run nothing wanted two fingers. Add the camera, check it first, and the stroke does end, but only because the camera claimed the contacts:

```
f2   finger A lands     contacts=1 | stroke: P=X H=X R=. | camera: P=. H=. R=.
f6   finger B lands     contacts=2 | stroke: P=. H=. R=X | camera: P=X H=X R=.
f10  finger B lifts     contacts=1 | stroke: P=. H=. R=. | camera: P=. H=. R=X
f14  finger A lifts     contacts=0 | stroke: P=. H=. R=. | camera: P=. H=. R=.
```

## A claim needs a finger that just landed

Look at f10 above. The camera ends, one finger is still on the glass, and the stroke doesn't take it. A claim needs at least one contact that landed this frame, and the rest can already be down. That's the same shape as a modifier, which is held before the key it modifies gets pressed.

Without the rule, lifting one finger off a zoom hands the leftover finger straight to the stroke and draws a mark nobody asked for, since two fingers never come off together.

At least one, not all, which is what keeps a two finger gesture easy to start and easy to resume:

```
f2   finger A lands       contacts=1 | stroke: P=X H=X R=. | camera: P=. H=. R=.
f6   finger B lands       contacts=2 | stroke: P=. H=. R=X | camera: P=X H=X R=.
f10  finger B lifts       contacts=1 | stroke: P=. H=. R=. | camera: P=. H=. R=X
f14  finger B lands again contacts=2 | stroke: P=. H=. R=. | camera: P=X H=X R=.
f18  both lift            contacts=0 | stroke: P=. H=. R=. | camera: P=. H=. R=X
```

So you never have to put two fingers down at the same instant. The second one arriving is what starts the gesture, and putting it back down after lifting it starts the gesture again.

What it costs is that a condition can't adopt a finger already resting on the screen. Something that appears under a held finger waits for the next tap.

## Several one finger actions at once

Claiming is per contact, so one finger holding something and another tapping buttons work at the same time. `Track.TouchCondition` keys on the contact rather than on a button, which a single "this frame was consumed" flag can't do. Hit test first, then consume, so a finger only gets used up once it's landed on something:

```csharp
foreach (TouchLocation t in InputHelper.NewTouch) {
    if (jumpButton.Contains(t.Position) && Track.TouchCondition.Pressed(t)) {

        // Do the jump. The world underneath won't see this finger,
        // but a second finger somewhere else still reaches it.

    }
}
```

That composes with the count form, since both go through the same set of contacts used this frame. A `Track.TouchCondition(1)` holding a dragged object keeps holding it while this loop hands a different finger to a button.

`Track.TouchCondition.Consume(int touchId)` and `IsUnique(int touchId)` are there for handling it yourself, and the no argument instance form triggers on any contact that hasn't been used yet this frame. Touch ids climb forever, unlike keys and buttons, so this tracker drops its whole set the first time it's touched on a new frame rather than stamping ids and keeping them.

The count form takes the first free contacts it finds, which is fine when the gesture doesn't care where it started. When it does care, hit test and hold the id yourself:

```csharp
foreach (TouchLocation t in InputHelper.NewTouch) {
    if (_dragId == -1 && canvas.Contains(t.Position) && Track.TouchCondition.Pressed(t)) {
        _dragId = t.Id;
    }
}

if (InputHelper.NewTouch.FindById(_dragId, out TouchLocation drag)) {
    if (TouchCondition.Released(drag)) {
        _dragId = -1;
    } else {

        // Draw to drag.Position. Every other finger is somebody else's problem.

    }
}
```

`FindById` is on `TouchCollection`. Pair it with `LostTouches` below so the id still gets cleared if that contact ever goes away without a release.

## One pointer for the mouse and the touch screen

Most games have one cursor and don't want to branch on which device moved it. `Pointer` is that position:

```csharp
Vector2 mouse = Pointer.Position;
```

It follows the oldest contact while something is touching the screen and the mouse otherwise. `OldPosition` and `Delta` are there too, and `Moved()` says whether it changed.

It's sticky on purpose. Lifting a finger doesn't snap the position back to wherever the mouse is parked, since the mouse has to actually move to take the pointer back. Without that, every tap would end by teleporting your cursor.

## Touch has no hover

`Pointer.Source` tells you which device the position came from:

```csharp
if (Pointer.Source == PointerSource.Mouse && button.Contains(Pointer.Position)) {

    // Draw the hover fill.

}
```

A cursor is always somewhere, so hover means something. A finger is only somewhere while it's touching, so a hover state driven by `Pointer.Position` latches under wherever you last tapped and stays there.

Gate the visual, not the hit test. Porting a real game turned this up: a button used one `_onlineHovered` flag for both the hover fill and the click check, and gating the whole thing on `Mouse` made the button untappable. Gating only the fill colour is the fix.

## Contacts that end without a release

A contact normally ends by showing up once with a `Released` state, and that's what you get even when the system takes a gesture over, since a cancelled gesture arrives as a release and MonoGame releases every contact itself on an orientation change.

`InputHelper.LostTouches` is the net for anything that still gets through without one. If you hold on to a touch id, you can let go there:

```csharp
foreach (TouchLocation t in InputHelper.LostTouches) {
    if (t.Id == _dragId) {
        _dragId = -1;
    }
}
```

It's usually empty. Reading it costs a loop over a list that has nothing in it.

## Limits

Telling a tap apart from a drag is yours. Fingers jitter more than mice do, so a tap almost never lands on exactly one pixel, and the usual fix is to compare the distance travelled against a threshold. Where that threshold sits is feel, so the library doesn't pick it.

There's no soft keyboard. `InputHelper.TextEvents` reads a hardware keyboard, and bringing up an on screen one is a MonoGame API that doesn't exist yet. A game that types a room code needs its own on screen keys on mobile.

Right click, hover, and keyboard modifiers have no touch equivalent at all. That's a game design problem rather than a library one.

## Follow up

[Focus](../focus/README.md), a guide on what conditions do when your game loses focus.
