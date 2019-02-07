using System;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Checks various Mouse conditions for a specific button.
    /// </summary>
    public class ActionGamePad {
        // Group: Constructors
        public ActionGamePad(Func<GamePadState[], ButtonState> needButton) {
            _needButton = needButton;
        }

        // Group: Public Functions
        public bool Pressed() {
            return Pressed(_needButton) && InputHelper.IsActive;
        }
        public bool Held() {
            return Held(_needButton) && InputHelper.IsActive;
        }
        public bool HeldOnly() {
            return HeldOnly(_needButton) && InputHelper.IsActive;
        }
        public bool Released() {
            return Released(_needButton) && InputHelper.IsActive;
        }
        public static bool Pressed(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Pressed && button(InputHelper.OldGamePad) == ButtonState.Released;
        }
        public static bool Held(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Pressed;
        }
        public static bool HeldOnly(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Pressed && button(InputHelper.OldGamePad) == ButtonState.Pressed;
        }
        public static bool Released(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Released && button(InputHelper.OldGamePad) == ButtonState.Pressed;
        }

        // Group: Private Variables
        private Func<GamePadState[], ButtonState> _needButton;
    }
}