using System;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific gamepad button.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    public class ConditionGamePad : ICondition {

        // Group: Constructors

        /// <param name="needButton">The button to operate on.</param>
        public ConditionGamePad(Func<GamePadState[], ButtonState> needButton) {
            _needButton = needButton;
        }

        // Group: Public Functions

        /// <returns>Returns true when a button was not pressed and is now pressed.</returns>
        public bool Pressed() {
            return Pressed(_needButton) && InputHelper.IsActive;
        }
        /// <returns>Returns true when a button is now pressed.</returns>
        public bool Held() {
            return Held(_needButton) && InputHelper.IsActive;
        }
        /// <returns>Returns true when a button was pressed and is now pressed.</returns>
        public bool HeldOnly() {
            return HeldOnly(_needButton) && InputHelper.IsActive;
        }
        /// <returns>Returns true when a button was pressed and is now not pressed.</returns>
        public bool Released() {
            return Released(_needButton) && InputHelper.IsActive;
        }

        // Group: Static Functions

        /// <returns>Returns true when a button was not pressed and is now pressed.</returns>
        public static bool Pressed(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Pressed && button(InputHelper.OldGamePad) == ButtonState.Released;
        }
        /// <returns>Returns true when a button is now pressed.</returns>
        public static bool Held(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Pressed;
        }
        /// <returns>Returns true when a button was pressed and is now pressed.</returns>
        public static bool HeldOnly(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Pressed && button(InputHelper.OldGamePad) == ButtonState.Pressed;
        }
        /// <returns>Returns true when a button was pressed and is now not pressed.</returns>
        public static bool Released(Func<GamePadState[], ButtonState> button) {
            return button(InputHelper.NewGamePad) == ButtonState.Released && button(InputHelper.OldGamePad) == ButtonState.Pressed;
        }

        // Group: Private Variables

        /// <summary>
        /// The button that will be checked.
        /// </summary>
        private Func<GamePadState[], ButtonState> _needButton;
    }
}