using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific gamepad button.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    public class GamePadCondition : ICondition {

        /// <param name="button">The button to operate on.</param>
        /// <param name="gamePadIndex">The index of the gamepad to operate on.</param>
        public GamePadCondition(GamePadButton button, int gamePadIndex) {
            _button = button;
            _gamePadIndex = gamePadIndex;
        }

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            return Pressed(_button, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return Held(_button, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return HeldOnly(_button, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return Released(_button, _gamePadIndex) && InputHelper.IsActive;
        }
        /// <summary>Does nothing since this condition isn't tracked.</summary>
        public void Consume() { }

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
