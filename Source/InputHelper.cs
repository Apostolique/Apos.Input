using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Apos.Input {
    /// <summary>
    /// Unorganized helper functions for Apos.Input.
    /// Also holds some important variables for this library to work correctly.
    /// </summary>
    public static class InputHelper {

        /// <value>Pass your game class here.</value>
        public static Game Game { get; set; }
        /// <summary>
        /// Checks the game previous was active state.
        /// </summary>
        public static bool OldIsActive => _oldIsActive;
        /// <summary>
        /// Checks if the game is active. Usually this is when the game window has focus.
        /// </summary>
        public static bool IsActive => _newIsActive;
        /// <summary>
        /// The game window.
        /// </summary>
        public static GameWindow Window => Game.Window;
        /// <summary>
        /// The game window's width.
        /// </summary>
        public static int WindowWidth => Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
        /// <summary>
        /// The game window's height.
        /// </summary>
        public static int WindowHeight => Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
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
        /// An array with all gamepads' deadzone settings.
        /// </summary>
        public static GamePadDeadZone[] GamePadDeadZone => _gamePadDeadZone;
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
        public static List<TextInputEventArgs> TextEvents => _textEvents;
        /// <summary>
        /// Maps a MouseButton to a function that can extract a specific ButtonState from a MouseState.
        /// </summary>
        public static Dictionary<MouseButton, Func<MouseState, ButtonState>> MouseButtons => _mouseButtons;
        /// <summary>
        /// Maps a GamePadButton to a function that can extract a specific ButtonState from a GamePadState.
        /// </summary>
        public static Dictionary<GamePadButton, Func<GamePadState[], int, ButtonState>> GamePadButtons => _gamePadButtons;
        /// <summary>
        /// Used by conditions to know if they've been consumed this frame.
        /// </summary>
        public static uint CurrentFrame => _currentFrame;

        /// <summary>
        /// Call Setup in the game's LoadContent.
        /// </summary>
        /// <param name="game">Your game object.</param>
        public static void Setup(Game game) {
            Game = game;

            _newIsActive = Game.IsActive;
            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();
            _touchPanelCapabilities = TouchPanel.GetCapabilities();

            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _gamePadDeadZone[i] = Microsoft.Xna.Framework.Input.GamePadDeadZone.None;
                _newGamepad[i] = GamePad.GetState(i, _gamePadDeadZone[i]);
                _gamePadCapabilities[i] = GamePad.GetCapabilities(i);
            }

            _newTouchCollection = TouchPanel.GetState();

            Window.TextInput += ProcessTextInput;
        }

        /// <summary>
        /// Call this at the beginning of your update loop.
        /// </summary>
        public static void UpdateSetup() {
            _oldIsActive = _newIsActive;
            _oldMouse = _newMouse;
            _oldKeyboard = _newKeyboard;
            _newGamepad.CopyTo(_oldGamePad, 0);

            _newIsActive = Game.IsActive;
            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();

            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _newGamepad[i] = GamePad.GetState(i, GamePadDeadZone[i]);
                _gamePadCapabilities[i] = GamePad.GetCapabilities(i);
            }

            _newTouchCollection = TouchPanel.GetState();
            _touchPanelCapabilities = TouchPanel.GetCapabilities();

            _currentFrame++;
        }
        /// <summary>
        /// Call this at the end of your update loop.
        /// </summary>
        public static void UpdateCleanup() {
            _textEvents.Clear();
        }

        private static void ProcessTextInput(object sender, TextInputEventArgs e) {
            _textEvents.Add(e);
        }

        /// <summary>
        /// The previous active state.
        /// </summary>
        private static bool _oldIsActive;
        /// <summary>
        /// The current active state.
        /// </summary>
        private static bool _newIsActive;
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
        private static GamePadState[] _oldGamePad = new GamePadState[GamePad.MaximumGamePadCount];
        /// <summary>
        /// An array with all gamepads' current states.
        /// </summary>
        private static GamePadState[] _newGamepad = new GamePadState[GamePad.MaximumGamePadCount];
        /// <summary>
        /// An array with all gamepads' info.
        /// </summary>
        private static GamePadCapabilities[] _gamePadCapabilities = new GamePadCapabilities[GamePad.MaximumGamePadCount];
        /// <summary>
        /// An array with all gamepads' deadzone settings.
        /// </summary>
        private static GamePadDeadZone[] _gamePadDeadZone = new GamePadDeadZone[GamePad.MaximumGamePadCount];
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
        private static List<TextInputEventArgs> _textEvents = new List<TextInputEventArgs>();
        private static Dictionary<MouseButton, Func<MouseState, ButtonState>> _mouseButtons = new Dictionary<MouseButton, Func<MouseState, ButtonState>> {
            {MouseButton.LeftButton, s => s.LeftButton},
            {MouseButton.MiddleButton, s => s.MiddleButton},
            {MouseButton.RightButton, s => s.RightButton},
            {MouseButton.XButton1, s => s.XButton1},
            {MouseButton.XButton2, s => s.XButton2},
        };
        private static Dictionary<GamePadButton, Func<GamePadState[], int, ButtonState>> _gamePadButtons = new Dictionary<GamePadButton, Func<GamePadState[], int, ButtonState>> {
            {GamePadButton.A, (s, i) => s[i].Buttons.A},
            {GamePadButton.B, (s, i) => s[i].Buttons.B},
            {GamePadButton.Back, (s, i) => s[i].Buttons.Back},
            {GamePadButton.X, (s, i) => s[i].Buttons.X},
            {GamePadButton.Y, (s, i) => s[i].Buttons.Y},
            {GamePadButton.Start, (s, i) => s[i].Buttons.Start},
            {GamePadButton.LeftShoulder, (s, i) => s[i].Buttons.LeftShoulder},
            {GamePadButton.LeftStick, (s, i) => s[i].Buttons.LeftStick},
            {GamePadButton.RightShoulder, (s, i) => s[i].Buttons.RightShoulder},
            {GamePadButton.RightStick, (s, i) => s[i].Buttons.RightStick},
            {GamePadButton.BigButton, (s, i) => s[i].Buttons.BigButton},
            {GamePadButton.Down, (s, i) => s[i].DPad.Down},
            {GamePadButton.Left, (s, i) => s[i].DPad.Left},
            {GamePadButton.Right, (s, i) => s[i].DPad.Right},
            {GamePadButton.Up, (s, i) => s[i].DPad.Up},
        };
        private static uint _currentFrame = 0;
    }
}
