using System;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Checks various Mouse conditions for a specific button.
    /// </summary>
    public class ActionMouse {
        // Group: Constructors
        public ActionMouse(Func<MouseState, ButtonState> needButton) {
            _needButton = needButton;
        }

        // Group: Public Functions
        public bool Pressed() {
            return Pressed(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        public bool Held() {
            return Held(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        public bool HeldOnly() {
            return HeldOnly(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        public bool Released() {
            return Released(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        public static bool Pressed(Func<MouseState, ButtonState> button) {
            return button(InputHelper.NewMouse) == ButtonState.Pressed && button(InputHelper.OldMouse) == ButtonState.Released;
        }
        public static bool Held(Func<MouseState, ButtonState> button) {
            return button(InputHelper.NewMouse) == ButtonState.Pressed;
        }
        public static bool HeldOnly(Func<MouseState, ButtonState> button) {
            return button(InputHelper.NewMouse) == ButtonState.Pressed && button(InputHelper.OldMouse) == ButtonState.Pressed;
        }
        public static bool Released(Func<MouseState, ButtonState> button) {
            return button(InputHelper.NewMouse) == ButtonState.Released && button(InputHelper.OldMouse) == ButtonState.Pressed;
        }
        public static bool IsMouseValid(bool IsActive) {
            if (IsActive && InputHelper.NewMouse.X >= 0 && InputHelper.NewMouse.X <= InputHelper.WindowWidth && InputHelper.NewMouse.Y >= 0 && InputHelper.NewMouse.Y <= InputHelper.WindowHeight) {
                return true;
            }
            return false;
        }

        // Group: Private Variables
        private Func<MouseState, ButtonState> _needButton;
    }
}