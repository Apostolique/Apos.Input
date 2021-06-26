using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific mouse button.
    /// Non static methods implicitly make sure that the game is active.
    /// Pressed also makes sure the mouse is inside the window. Otherwise returns false.
    /// </summary>
    public class MouseCondition : ICondition {

        /// <param name="button">The button to operate on.</param>
        public MouseCondition(MouseButton button) {
            _button = button;
        }

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            return IsMouseValid && (Pressed(_button) || !InputHelper.OldIsActive && Held(_button));
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return InputHelper.IsActive && Held(_button);
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return InputHelper.IsActive && InputHelper.OldIsActive && HeldOnly(_button);
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return InputHelper.IsActive && Released(_button);
        }
        /// <summary>Does nothing since this condition isn't tracked.</summary>
        public void Consume() { }

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public static bool Pressed(MouseButton button) {
            return
                InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Pressed &&
                InputHelper.MouseButtons[button](InputHelper.OldMouse) == ButtonState.Released;
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public static bool Held(MouseButton button) {
            return InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Pressed;
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public static bool HeldOnly(MouseButton button) {
            return
                InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Pressed &&
                InputHelper.MouseButtons[button](InputHelper.OldMouse) == ButtonState.Pressed;
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public static bool Released(MouseButton button) {
            return
                InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Released &&
                InputHelper.MouseButtons[button](InputHelper.OldMouse) == ButtonState.Pressed;
        }
        /// <returns>Returns true when the scroll wheel is scrolled.</returns>
        public static bool Scrolled() => ScrollDelta != 0;
        /// <returns>Returns the difference between last frame and this frame's scroll wheel value.</returns>
        public static int ScrollDelta => InputHelper.NewMouse.ScrollWheelValue - InputHelper.OldMouse.ScrollWheelValue;

        /// <returns>Returns true when the mouse pointer is moved.</returns>
        public static bool PointerMoved() => PointerDelta != Point.Zero;
        ///<returns>Returns the difference between the last frame and this frame's mouse pointer position.</returns>
        public static Point PointerDelta => InputHelper.NewMouse.Position - InputHelper.OldMouse.Position;

        /// <returns>Returns true when the mouse is within the game window and active.</returns>
        public static bool IsMouseValid =>
            InputHelper.IsActive &&
            0 <= InputHelper.NewMouse.X && InputHelper.NewMouse.X <= InputHelper.WindowWidth &&
            0 <= InputHelper.NewMouse.Y && InputHelper.NewMouse.Y <= InputHelper.WindowHeight;

        /// <summary>
        /// The button that will be checked.
        /// </summary>
        private MouseButton _button;
    }
}
