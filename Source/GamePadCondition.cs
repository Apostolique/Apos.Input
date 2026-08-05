using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific gamepad button.
    /// Non static methods only count the button as down while the game is active. Losing focus
    /// reports a release, and coming back with the button still down reports a press.
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
            return Down() && !WasDown();
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return Down();
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return Down() && WasDown();
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return !Down() && WasDown();
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
        /// A button only counts as down while the game is active, which is what turns a focus
        /// change into a press or a release on its own. A gamepad keeps reporting through a focus
        /// loss since it isn't attached to a window, so the release has to come from here.
        /// </summary>
        private bool Down() => InputHelper.IsActive && Held(_button, _gamePadIndex);
        /// <summary>Whether the button counted as down last frame.</summary>
        private bool WasDown() =>
            InputHelper.OldIsActive &&
            InputHelper.GamePadButtons[_button](InputHelper.OldGamePad, _gamePadIndex) == ButtonState.Pressed;

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
