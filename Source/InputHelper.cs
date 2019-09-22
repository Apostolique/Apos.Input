using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Apos.Input {
    /// <summary>
    /// Unorganized helper functions for Apos.Input.
    /// Also holds some important variables for this library to work correctly.
    /// </summary>
    public static class InputHelper {

        // Group: Public Variables

        /// <summary>
        /// Available mouse buttons.
        /// </summary>
        public enum MouseButton {
            /// <summary>
            /// Left mouse button.
            /// </summary>
            LeftButton,
            /// <summary>
            /// Middle mouse button.
            /// </summary>
            MiddleButton,
            /// <summary>
            /// Right mouse button.
            /// </summary>
            RightButton,
            /// <summary>
            /// XButton1 mouse button.
            /// </summary>
            XButton1,
            /// <summary>
            /// XButton2 mouse button.
            /// </summary>
            XButton2
        }
        /// <value>Pass your game class here.</value>
        public static Game Game {
            get;
            set;
        }
        /// <summary>
        /// Checks if the game is active. Usually this is when the game window has focus.
        /// </summary>
        public static bool IsActive => Game.IsActive;
        /// <summary>
        /// The game window.
        /// </summary>
        public static GameWindow Window => Game.Window;
        /// <summary>
        /// The game window's width.
        /// </summary>
        public static int WindowWidth => Window.ClientBounds.Width;
        /// <summary>
        /// The game window's height.
        /// </summary>
        public static int WindowHeight => Window.ClientBounds.Height;
        /// <summary>
        /// The mouse's previous state.
        /// </summary>
        public static MouseState OldMouse => _oldMouse;
        /// <summary>
        /// The mouse's current state.
        /// </summary>
        public static MouseState NewMouse => _newMouse;
        /// <summary>
        /// The keyboard's previous state.
        /// </summary>
        public static KeyboardState OldKeyboard => _oldKeyboard;
        /// <summary>
        /// The keyboard's current state.
        /// </summary>
        public static KeyboardState NewKeyboard => _newKeyboard;
        /// <summary>
        /// An array with all gamepads' previous states.
        /// </summary>
        public static GamePadState[] OldGamePad => _oldGamePad;
        /// <summary>
        /// An array with all gamepads' current states.
        /// </summary>
        public static GamePadState[] NewGamePad => _newGamepad;
        /// <summary>
        /// An array with all gamepads' info.
        /// </summary>
        public static GamePadCapabilities[] GamePadCapabilities => _gamePadCapabilities;
        /// <summary>
        /// A touch collection that holds the previous and current touch locations.
        /// </summary>
        public static TouchCollection NewTouchCollection => _newTouchCollection;
        /// <summary>
        /// Gives info about a touch panel.
        /// </summary>
        public static TouchPanelCapabilities TouchPanelCapabilities => _touchPanelCapabilities;
        /// <summary>
        /// Useful for handling text inputs from any keyboard layouts. This is useful when coding textboxes.
        /// </summary>
        public static List<KeyCharacter> TextEvents => _textEvents;
        /// <summary>
        /// Maps a MouseButton to a function that can extract a specific ButtonState from a MouseState.
        /// </summary>
        public static Dictionary<MouseButton, Func<MouseState, ButtonState>> MouseButtons => _mouseButtons;

        // Group: Public Functions

        /// <summary>
        /// Call this at the beginning of your update loop.
        /// </summary>
        public static void UpdateSetup() {
            if (!_initiated) {
                setup();
            }

            _oldMouse = _newMouse;
            _oldKeyboard = _newKeyboard;
            _newGamepad.CopyTo(_oldGamePad, 0);

            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();

            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _newGamepad[i] = GamePad.GetState(i);
            }
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _gamePadCapabilities[i] = GamePad.GetCapabilities(i);
            }

            _newTouchCollection = TouchPanel.GetState();
            _touchPanelCapabilities = TouchPanel.GetCapabilities();
        }
        /// <summary>
        /// Call this at the end of your update loop.
        /// </summary>
        public static void UpdateCleanup() {
            _textEvents.Clear();
        }

        // Group: Private Functions

        /// <summary>
        /// This is called automatically on the first update setup call.
        /// </summary>
        private static void setup() {
            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();
            _touchPanelCapabilities = TouchPanel.GetCapabilities();

            _oldGamePad = new GamePadState[GamePad.MaximumGamePadCount];
            _newGamepad = new GamePadState[GamePad.MaximumGamePadCount];
            _gamePadCapabilities = new GamePadCapabilities[GamePad.MaximumGamePadCount];
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _newGamepad[i] = GamePad.GetState(i);
            }
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _gamePadCapabilities[i] = GamePad.GetCapabilities(i);
            }

            _newTouchCollection = TouchPanel.GetState();

            _initiated = true;

            //This is boring but whatever, it only gets called once.
            //MonoGame doesn't offer TextInput under some platforms.
            Type t = Window.GetType();
            EventInfo e = t.GetEvent("TextInput");
            if (e != null) {
                Window.TextInput += processTextInput;
            }
        }
        /// <summary>
        /// This function receives TextInput events from the game window.
        /// </summary>
        /// <param name="sender">This gets ignored.</param>
        /// <param name="e">Contains a character and a key.</param>
        private static void processTextInput(object sender, object e) {
            Type t = e.GetType();
            PropertyInfo k = t.GetProperty("Key");
            PropertyInfo c = t.GetProperty("Character");
            if (k != null && c != null) {
                _textEvents.Add(new KeyCharacter((Keys)k.GetValue(e), (char)c.GetValue(e)));
            }
        }

        // Group: Private Variables

        /// <summary>
        /// Whether setup has been called or not.
        /// </summary>
        private static bool _initiated = false;
        /// <summary>
        /// The mouse's previous state.
        /// </summary>
        private static MouseState _oldMouse;
        /// <summary>
        /// The mouse's current state.
        /// </summary>
        private static MouseState _newMouse;
        /// <summary>
        /// The keyboard's previous state.
        /// </summary>
        private static KeyboardState _oldKeyboard;
        /// <summary>
        /// The keyboard's current state.
        /// </summary>
        private static KeyboardState _newKeyboard;
        /// <summary>
        /// An array with all gamepads' previous states.
        /// </summary>
        private static GamePadState[] _oldGamePad;
        /// <summary>
        /// An array with all gamepads' current states.
        /// </summary>
        private static GamePadState[] _newGamepad;
        /// <summary>
        /// An array with all gamepads' info.
        /// </summary>
        private static GamePadCapabilities[] _gamePadCapabilities;
        /// <summary>
        /// A touch collection that holds the previous and current touch locations.
        /// </summary>
        private static TouchCollection _newTouchCollection;
        /// <summary>
        /// Gives info about a touch panel.
        /// </summary>
        private static TouchPanelCapabilities _touchPanelCapabilities;
        /// <summary>
        /// Useful for handling text inputs from any keyboard layouts. This is useful when coding textboxes.
        /// </summary>
        private static List<KeyCharacter> _textEvents = new List<KeyCharacter>();
        private static Dictionary<MouseButton, Func<MouseState, ButtonState>> _mouseButtons = new Dictionary<MouseButton, Func<MouseState, ButtonState>> {
            {MouseButton.LeftButton, s => s.LeftButton},
            {MouseButton.MiddleButton, s => s.MiddleButton},
            {MouseButton.RightButton, s => s.RightButton},
            {MouseButton.XButton1, s => s.XButton1},
            {MouseButton.XButton2, s => s.XButton2},
        };
    }
}