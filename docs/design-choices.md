# Design choices

## Polling

This library's goal is to enable a [polling-based](https://en.wikipedia.org/wiki/Polling_(computer_science)) paradigm on input handling. [Inputs](https://en.wikipedia.org/wiki/Input_device) come from mouse, keyboard, gamepad, touch screen, etc. Polling means that you can check for an input's current state anywhere that feels natural in your game code instead of managing them through [events and callbacks](https://en.wikipedia.org/wiki/Event_(computing)).

## Abstraction

For an input button, the state can either be `on` or `off`. It's useful to know when the state becomes `on` and when the state becomes `off`. This library defines the moment a state becomes on as `Pressed` and the moment the state becomes off as `Released`. It also defines the exclusive state in between those as `HeldOnly`. When either `Pressed` or `HeldOnly` are true, this is defined as `Held`. These are included in the `ICondition` interface.

## Tracking

This library comes with a built-in tracking system. This allows you to resolve conflicts between different ICondition instances that reuse the same keys or buttons. The tracking system is opt-in and separated into it's own namespace at `Apos.Input.Track`. To accommodate it, ICondition offers an optional parameter `canConsume` in `Pressed(canConsume = true)`, `Held(canConsume = true)`, `HeldOnly(canConsume = true)`, and `Released(canConsume = true)`. This optional parameter allows you to handle the tracking yourself by setting `canConsume` to false and calling `Consume()` manually.
