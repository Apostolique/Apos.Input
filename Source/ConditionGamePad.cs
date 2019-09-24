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
        /// <param name="gamePadIndex">The index of the gamepad to operate on.</param>
        public ConditionGamePad(InputHelper.GamePadButton needButton, int gamePadIndex) {
            _needButton = needButton;
            _gamePadIndex = gamePadIndex;
        }

        // Group: Public Functions

        /// <returns>Returns true when a button was not pressed and is now pressed.</returns>
        public bool Pressed() {
            return Pressed(_needButton, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <returns>Returns true when a button is now pressed.</returns>
        public bool Held() {
            return Held(_needButton, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <returns>Returns true when a button was pressed and is now pressed.</returns>
        public bool HeldOnly() {
            return HeldOnly(_needButton, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <returns>Returns true when a button was pressed and is now not pressed.</returns>
        public bool Released() {
            return Released(_needButton, _gamePadIndex) && InputHelper.IsActive;
        }

        // Group: Static Functions

        /// <returns>Returns true when a button was not pressed and is now pressed.</returns>
        public static bool Pressed(InputHelper.GamePadButton button, int gamePadIndex) {
            return InputHelper.GamePadButtons[button](InputHelper.NewGamePad, gamePadIndex) == ButtonState.Pressed &&
                   InputHelper.GamePadButtons[button](InputHelper.OldGamePad, gamePadIndex) == ButtonState.Released;
        }
        /// <returns>Returns true when a button is now pressed.</returns>
        public static bool Held(InputHelper.GamePadButton button, int gamePadIndex) {
            return InputHelper.GamePadButtons[button](InputHelper.NewGamePad, gamePadIndex) == ButtonState.Pressed;
        }
        /// <returns>Returns true when a button was pressed and is now pressed.</returns>
        public static bool HeldOnly(InputHelper.GamePadButton button, int gamePadIndex) {
            return InputHelper.GamePadButtons[button](InputHelper.NewGamePad, gamePadIndex) == ButtonState.Pressed &&
                   InputHelper.GamePadButtons[button](InputHelper.OldGamePad, gamePadIndex) == ButtonState.Pressed;
        }
        /// <returns>Returns true when a button was pressed and is now not pressed.</returns>
        public static bool Released(InputHelper.GamePadButton button, int gamePadIndex) {
            return InputHelper.GamePadButtons[button](InputHelper.NewGamePad, gamePadIndex) == ButtonState.Released &&
                   InputHelper.GamePadButtons[button](InputHelper.OldGamePad, gamePadIndex) == ButtonState.Pressed;
        }

        // Group: Private Variables

        /// <summary>
        /// The button that will be checked.
        /// </summary>
        private InputHelper.GamePadButton _needButton;
        /// <summary>
        /// The index for the gamepad that will be checked.
        /// </summary>
        private int _gamePadIndex;
    }
}