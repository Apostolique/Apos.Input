using System;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Checks various Mouse conditions for a specific button.
    /// </summary>
    public class ActionMouse {
        //constructors
        public ActionMouse(Func<MouseState, ButtonState> needButton) {
            _needButton = needButton;
        }

        //public functions
        public bool Pressed() {
            return Pressed(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        public bool Holding() {
            return Holding(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        public bool HoldingOnly() {
            return HoldingOnly(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        public bool Released() {
            return Released(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        public static bool Pressed(Func<MouseState, ButtonState> button) {
            return button(InputHelper.NewMouse) == ButtonState.Pressed && button(InputHelper.OldMouse) == ButtonState.Released;
        }
        public static bool Holding(Func<MouseState, ButtonState> button) {
            return button(InputHelper.NewMouse) == ButtonState.Pressed;
        }
        public static bool HoldingOnly(Func<MouseState, ButtonState> button) {
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

        //private vars
        private Func<MouseState, ButtonState> _needButton;
    }
}