# Tracking
This guide will show you how to stop two conditions from firing on the same button press.

Say you have a `run` and a `walk`, and both use the `Right` key:

```csharp
ICondition run =
    new AllCondition(
        new KeyboardCondition(Keys.LeftShift),
        new KeyboardCondition(Keys.Right)
    );
ICondition walk = new KeyboardCondition(Keys.Right);

if (run.Held()) {

    // Run while the buttons are held.

}
if (walk.Held()) {

    // Walk while the right button is held.

}
```

Holding shift and right runs and walks at the same time, because nothing tells `walk` that `run` already claimed the key.

## Consuming an input

The tracking system fixes that. It lives in its own namespace, and swapping the conditions over is the whole change:

```csharp
ICondition run =
    new AllCondition(
        new KeyboardCondition(Keys.LeftShift),
        new Track.KeyboardCondition(Keys.Right)
    );
ICondition walk = new Track.KeyboardCondition(Keys.Right);
```

Now `run` marks `Right` as used for the frame the moment it triggers, and `walk` sees that it's taken and stays quiet. Order decides who wins, so check the more specific condition first.

Tracking is opt in on purpose. A condition that isn't tracked never consumes anything and never gets blocked, so you only pay for it where you actually have a conflict.

## Doing it yourself

Every check takes a `canConsume` you can turn off, which lets you look without claiming:

```csharp
if (jump.Pressed(false)) {

    // Decide whether you want it.

    jump.Consume();
}
```

That's how `AnyCondition` and `AllCondition` work internally. They check their children with `canConsume` off, then consume the whole group only once the group itself triggers, so a half matched `AllCondition` doesn't eat a key it didn't use.

The static forms take the same parameter, and `Consume` and `IsUnique` are public if you want to drive it by hand:

```csharp
if (Track.KeyboardCondition.IsUnique(Keys.Right) && somethingElse) {
    Track.KeyboardCondition.Consume(Keys.Right);
}
```

## How it knows the frame changed

`InputHelper.CurrentFrame` counts up once per `UpdateSetup`. Consuming stamps the key with that number, and `IsUnique` is just a check that the stamp isn't this frame's. Nothing needs clearing, since last frame's stamps can never match again.

Touch works differently, because touch ids climb forever and a tracker that only grows would leak over a long session. Read [Touch](../touch/README.md#tracking-contacts).

## Limits

Tracking is per frame and per input, and it has no idea what your game means. It stops two conditions from reading the same key, and that's all. It won't help you decide which of two overlapping UI panels should get a click, since both are asking about the same button and the first one to ask wins.

If you need layering, order your checks so the top layer asks first. That's the whole mechanism.

## Follow up

[Touch](../touch/README.md), a guide on touch screen contacts, where the tracking system keys on the contact instead of on a button.
