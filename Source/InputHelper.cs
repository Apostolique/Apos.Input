using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Apos.Input {
    /// <summary>
    /// Goal: Unorganized helper functions for Apos.Input.
    /// </summary>
    public static class InputHelper {
        // Group: Public Variables
        public static Game Game;
        public static bool IsActive => Game.IsActive;
        public static GameWindow Window => Game.Window;
        public static int WindowWidth => Window.ClientBounds.Width;
        public static int WindowHeight => Window.ClientBounds.Height;
        public static MouseState OldMouse => _oldMouse;
        public static MouseState NewMouse => _newMouse;
        public static KeyboardState OldKeyboard => _oldKeyboard;
        public static KeyboardState NewKeyboard => _newKeyboard;
        public static TouchCollection NewTouchCollection => _newTouchCollection;
        public static TouchPanelCapabilities TouchPanelCapabilities => _touchPanelCapabilities;
        public static GamePadState[] OldGamePad => _oldGamePad;
        public static GamePadState[] NewGamePad => _newGamepad;
        public static GamePadCapabilities[] GamePadCapabilities => _gamePadCapabilities;
        public static List<TextInputEventArgs> TextEvents => _textEvents;

        // Group: Public Functions
        public static void UpdateSetup() {
            if (!_initiated) {
                setup();
            }

            _oldMouse = _newMouse;
            _oldKeyboard = _newKeyboard;
            _oldGamePad = _newGamepad;

            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();

            _newTouchCollection = TouchPanel.GetState();
            _touchPanelCapabilities = TouchPanel.GetCapabilities();

            _newGamepad = new GamePadState[GamePad.MaximumGamePadCount];
            _gamePadCapabilities = new GamePadCapabilities[GamePad.MaximumGamePadCount];
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _newGamepad[i] = GamePad.GetState(i);
            }
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _gamePadCapabilities[i] = GamePad.GetCapabilities(i);
            }
        }
        public static void Update() {
            _textEvents.Clear();
        }

        // Group: Private Functions
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

            Window.TextInput += processTextInput;
            _textEvents = new List<TextInputEventArgs>();
        }
        private static void processTextInput(object sender, TextInputEventArgs e) {
            _textEvents.Add(e);
        }

        // Group: Private Variables
        private static bool _initiated = false;
        private static MouseState _oldMouse;
        private static MouseState _newMouse;
        private static KeyboardState _oldKeyboard;
        private static KeyboardState _newKeyboard;
        private static TouchCollection _newTouchCollection;
        private static TouchPanelCapabilities _touchPanelCapabilities;
        private static GamePadState[] _oldGamePad;
        private static GamePadState[] _newGamepad;
        private static GamePadCapabilities[] _gamePadCapabilities;
        private static List<TextInputEventArgs> _textEvents;
    }
}