using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific gamepad button.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    public class GamePadCondition : ICondition {

        // Group: Constructors

        /// <param name="button">The button to operate on.</param>
        /// <param name="gamePadIndex">The index of the gamepad to operate on.</param>
        public GamePadCondition(GamePadButton button, int gamePadIndex) {
            _button = button;
            _gamePadIndex = gamePadIndex;
        }

        // Group: Public Functions

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed() {
            return Pressed(_button, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held() {
            return Held(_button, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly() {
            return HeldOnly(_button, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released() {
            return Released(_button, _gamePadIndex) && InputHelper.IsActive;
        }

        // Group: Static Functions

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public static bool Pressed(GamePadButton button, int gamePadIndex) {
            return InputHelper.GamePadButtons[button](InputHelper.NewGamePad, gamePadIndex) == ButtonState.Pressed &&
                   InputHelper.GamePadButtons[button](InputHelper.OldGamePad, gamePadIndex) == ButtonState.Released;
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public static bool Held(GamePadButton button, int gamePadIndex) {
            return InputHelper.GamePadButtons[button](InputHelper.NewGamePad, gamePadIndex) == ButtonState.Pressed;
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public static bool HeldOnly(GamePadButton button, int gamePadIndex) {
            return InputHelper.GamePadButtons[button](InputHelper.NewGamePad, gamePadIndex) == ButtonState.Pressed &&
                   InputHelper.GamePadButtons[button](InputHelper.OldGamePad, gamePadIndex) == ButtonState.Pressed;
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public static bool Released(GamePadButton button, int gamePadIndex) {
            return InputHelper.GamePadButtons[button](InputHelper.NewGamePad, gamePadIndex) == ButtonState.Released &&
                   InputHelper.GamePadButtons[button](InputHelper.OldGamePad, gamePadIndex) == ButtonState.Pressed;
        }

        // Group: Private Variables

        /// <summary>
        /// The button that will be checked.
        /// </summary>
        private GamePadButton _button;
        /// <summary>
        /// The index for the gamepad that will be checked.
        /// </summary>
        private int _gamePadIndex;
    }
}