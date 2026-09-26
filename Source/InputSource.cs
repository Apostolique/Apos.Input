using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Apos.Input {
    /// <summary>
    /// Where <see cref="InputHelper"/> reads the devices from. Pass one to <see cref="InputHelper.Setup(IInputSource)"/>
    /// to drive the input from a test or a replay instead of the real devices.
    /// </summary>
    public interface IInputSource {
        /// <summary>Whether the game has focus.</summary>
        bool IsActive { get; }
        /// <summary>The back buffer's width in pixels.</summary>
        int WindowWidth { get; }
        /// <summary>The back buffer's height in pixels.</summary>
        int WindowHeight { get; }
        /// <summary>The mouse's state this frame.</summary>
        MouseState GetMouse();
        /// <summary>The keyboard's state this frame.</summary>
        KeyboardState GetKeyboard();
        /// <summary>A gamepad's state this frame.</summary>
        GamePadState GetGamePad(int index);
        /// <summary>The touch screen's contacts this frame.</summary>
        TouchCollection GetTouch();
        /// <summary>The text typed since the last frame. Called once per frame.</summary>
        IEnumerable<TextInputEventArgs> TakeTextInput();
    }

    /// <summary>
    /// Input that you set by hand. Set the states, call <see cref="InputHelper.UpdateSetup"/>, then check your conditions.
    /// </summary>
    /// <example>
    /// <code>
    /// var input = new SimulatedInput();
    /// InputHelper.Setup(input);
    ///
    /// input.Keyboard = new KeyboardState(Keys.Space);
    /// InputHelper.UpdateSetup(gameTime);
    /// bool jumped = KeyboardCondition.Pressed(Keys.Space);
    /// </code>
    /// </example>
    public class SimulatedInput : IInputSource {
        /// <summary>Whether the game has focus. Defaults to true.</summary>
        public bool IsActive { get; set; } = true;
        /// <summary>Defaults to 800.</summary>
        public int WindowWidth { get; set; } = 800;
        /// <summary>Defaults to 480.</summary>
        public int WindowHeight { get; set; } = 480;
        /// <summary>The mouse's state, read on the next UpdateSetup.</summary>
        public MouseState Mouse { get; set; }
        /// <summary>The keyboard's state, read on the next UpdateSetup.</summary>
        public KeyboardState Keyboard { get; set; }
        /// <summary>One state per gamepad index.</summary>
        public GamePadState[] GamePads { get; } = new GamePadState[GamePad.MaximumGamePadCount];
        /// <summary>The touch screen's contacts, read on the next UpdateSetup.</summary>
        public TouchCollection Touch { get; set; } = new TouchCollection([]);

        /// <summary>Queues text for the next frame, the way the window's TextInput event does.</summary>
        public void Type(string text) {
            foreach (char c in text) {
                _text.Add(TextEvent(c, Keys.None));
            }
        }
        /// <summary>Queues a key that sends a text event, like backspace or enter.</summary>
        public void Type(char character, Keys key) {
            _text.Add(TextEvent(character, key));
        }

        /// <inheritdoc/>
        public MouseState GetMouse() => Mouse;
        /// <inheritdoc/>
        public KeyboardState GetKeyboard() => Keyboard;
        /// <inheritdoc/>
        public GamePadState GetGamePad(int index) => GamePads[index];
        /// <inheritdoc/>
        public TouchCollection GetTouch() => Touch;
        /// <inheritdoc/>
        public IEnumerable<TextInputEventArgs> TakeTextInput() {
            var text = _text.ToArray();
            _text.Clear();
            return text;
        }

        private static TextInputEventArgs TextEvent(char character, Keys key) {
#if KNI
            return new TextInputEventArgs(key, character);
#else
            return new TextInputEventArgs(character, key);
#endif
        }

        private readonly List<TextInputEventArgs> _text = [];
    }
}
