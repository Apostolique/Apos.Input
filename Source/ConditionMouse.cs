using System;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific mouse button.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    public class ConditionMouse : ICondition {

        // Group: Constructors

        /// <param name="needButton">The button to operate on.</param>
        public ConditionMouse(InputHelper.MouseButton needButton) {
            _needButton = needButton;
        }

        // Group: Public Functions

        /// <returns>Returns true when a button was not pressed and is now pressed.</returns>
        public bool Pressed() {
            return Pressed(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        /// <returns>Returns true when a button is now pressed.</returns>
        public bool Held() {
            return Held(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        /// <returns>Returns true when a button was pressed and is now pressed.</returns>
        public bool HeldOnly() {
            return HeldOnly(_needButton) && IsMouseValid(InputHelper.IsActive);
        }
        /// <returns>Returns true when a button was pressed and is now not pressed.</returns>
        public bool Released() {
            return Released(_needButton) && IsMouseValid(InputHelper.IsActive);
        }

        // Group: Static Functions

        /// <returns>Returns true when a button was not pressed and is now pressed.</returns>
        public static bool Pressed(InputHelper.MouseButton button) {
            return InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Pressed &&
                   InputHelper.MouseButtons[button](InputHelper.OldMouse) == ButtonState.Released;
        }
        /// <returns>Returns true when a button is now pressed.</returns>
        public static bool Held(InputHelper.MouseButton button) {
            return InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Pressed;
        }
        /// <returns>Returns true when a button was pressed and is now pressed.</returns>
        public static bool HeldOnly(InputHelper.MouseButton button) {
            return InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Pressed &&
                   InputHelper.MouseButtons[button](InputHelper.OldMouse) == ButtonState.Pressed;
        }
        /// <returns>Returns true when a button was pressed and is now not pressed.</returns>
        public static bool Released(InputHelper.MouseButton button) {
            return InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Released &&
                   InputHelper.MouseButtons[button](InputHelper.OldMouse) == ButtonState.Pressed;
        }
        /// <returns>Returns true when the mouse is within the game window.</returns>
        public static bool IsMouseValid(bool IsActive) {
            if (IsActive &&
                InputHelper.NewMouse.X >= 0 && InputHelper.NewMouse.X <= InputHelper.WindowWidth &&
                InputHelper.NewMouse.Y >= 0 && InputHelper.NewMouse.Y <= InputHelper.WindowHeight) {
                return true;
            }
            return false;
        }

        // Group: Private Variables

        /// <summary>
        /// The button that will be checked.
        /// </summary>
        private InputHelper.MouseButton _needButton;
    }
}