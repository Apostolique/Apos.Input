# Text input
This guide will show you how to read typed characters.

Conditions answer questions about buttons. Typing is a different problem, because what a key produces depends on the keyboard layout, on dead keys, and on whether shift is down. `KeyboardCondition(Keys.A)` tells you a physical key went down. It can't tell you the player meant `à`.

`InputHelper.TextEvents` is the list of characters that arrived this frame:

```csharp
foreach (TextInputEventArgs e in InputHelper.TextEvents) {
    _text += e.Character;
}
```

It plugs into MonoGame's `Window.TextInput`, so the layout, the modifiers, and the dead keys are already resolved by the time you see a character. An `AZERTY` player pressing the key where `Q` sits on `QWERTY` gets an `a`, which is what they typed and what you want in a text box.

The list gets cleared in `UpdateCleanup`, so read it during your update and don't hold on to it.

## Backspace isn't a character

Editing keys don't arrive as text, so those stay conditions:

```csharp
ICondition backspace = new RepeatCondition(new KeyboardCondition(Keys.Back), 400, 60);

if (backspace.Pressed() && _text.Length > 0) {
    _text = _text.Remove(_text.Length - 1);
}
```

Wrapping it in a [RepeatCondition](../durations/README.md) is what makes holding backspace delete more than one character, which is what everyone expects a text field to do.

## Limits

There's no soft keyboard. On a phone there's no hardware keyboard to produce these events and no MonoGame API to raise an on screen one, so a mobile build that needs typing has to draw its own keys and append to the string itself.

There's no caret, no selection, and no clipboard. This is the character stream and nothing more. A full text box is a UI problem, and [Apos.Gui](https://github.com/Apostolique/Apos.Gui) has one built on top of this.

## Follow up

[Getting started](../getting-started/README.md), if you came in partway through and want the setup.
