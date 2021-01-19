# ICondition

An ICondition is an abstraction over button-based input. A button can pressed or not pressed which maps to `on` or `off` respectively. By comparing it's previous and current states, we get the following cases.

In the following functions, when `canConsume` is `true`, it means that the `Consume()` function gets called implicitly if the condition is part of the tracking system.

## Source code

Read the [source code](https://github.com/Apostolique/Apos.Input/blob/master/Source/ICondition.cs).

---

## Pressed

```csharp
bool Pressed(bool canConsume = true);
```
Previous state is `off`, current state is `on`. Triggers only on the first frame that the current state becomes `on`.

---

## Held

```csharp
bool Held(bool canConsume = true);
```
Previous state ignored, current state is `on`. Triggers every frame for as long as the current state stays `on`.

---

## HeldOnly

```csharp
bool HeldOnly(bool canConsume = true);
```
Previous state is `on`, current state is `on`. Since `HeldOnly` doesn't get triggered on the same frame as `Pressed` it's useful as a way to break an action into multiple steps.

---

## Released

```csharp
bool Released(bool canConsume = true);
```
Previous state is `on`, current state is `off`. Triggers only on the first frame that the current state becomes `off` after it was `on`.

---

## Consume

```csharp
void Consume();
```
Only useful when using the tracking system. Marks the condition as used for that frame. This is called by the other functions implicitly if their `canConsume` is `true`. If you set `canConsume` to false, you are expected to call `Consume` yourself.
