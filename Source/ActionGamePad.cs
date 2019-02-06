using System;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Checks various Mouse conditions for a specific button.
    /// </summary>
    public class ActionGamePad {
        //constructors
        public ActionGamePad(Func<GamePadState[], ButtonState> needButton) {
            _needButton = needButton;
        }

        //public functions
        public bool Pressed() {
            return Pressed(_needButton) && InputHelper.IsActive;
        }
        public bool Holding() {
            return Holding(_needButton) && InputHelper.IsActive;
        }
        public bool HoldingOnly() {
            return HoldingOnly(_needButton) && InputHelper.IsActive;
        }
        public bool Released() {
            return Released(_needButton) && InputHelper.IsActive;
        }
        public static bool Pressed(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Pressed && button(InputHelper.OldGamePad) == ButtonState.Released;
        }
        public static bool Holding(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Pressed;
        }
        public static bool HoldingOnly(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Pressed && button(InputHelper.OldGamePad) == ButtonState.Pressed;
        }
        public static bool Released(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Released && button(InputHelper.OldGamePad) == ButtonState.Pressed;
        }

        //private vars
        private Func<GamePadState[], ButtonState> _needButton;
    }
}