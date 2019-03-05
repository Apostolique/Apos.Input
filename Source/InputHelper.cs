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
            _oldGamePad = _newGamepad;

            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();

            _newGamepad = new GamePadState[GamePad.MaximumGamePadCount];
            _gamePadCapabilities = new GamePadCapabilities[GamePad.MaximumGamePadCount];
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
        public static void Update() {
            _textEvents.Clear();
        }

        // Group: Private Functions

        /// <summary>
        /// This is called automatically on the first update setup call.
        /// </summary>
        private static void setup() {
            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();
            TouchPanel.GetCapabilities();

            _newGamepad = new GamePadState[GamePad.MaximumGamePadCount];
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _newGamepad[i] = GamePad.GetState(i);
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

            _textEvents = new List<KeyCharacter>();
        }
        /// <summary>
        /// This function receives TextInput events from the game window.
        /// </summary>
        /// <param name="sender">This gets ignored.</param>
        /// <param name="e">Contains a character and a key.</param>
        private static void processTextInput(object sender, TextInputEventArgs e) {
            _textEvents.Add(new KeyCharacter(e.Key, e.Character));
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
        private static List<KeyCharacter> _textEvents;
    }
}