using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific mouse button.
    /// Non static methods only count the button as down while the game is active. Losing focus
    /// reports a release, and clicking the window to come back reports a press.
    /// Pressed also makes sure the mouse is inside the window.
    /// </summary>
    public class MouseCondition : ICondition {

        /// <param name="button">The button to operate on.</param>
        public MouseCondition(MouseButton button) {
            _button = button;
        }

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            return IsMouseValid && Down() && !WasDown();
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
        /// A button only counts as down while the game is active, which is what turns a focus
        /// change into a press or a release on its own. The device can't be relied on for it.
        /// Mouse buttons come from the global cursor state, so they stay down through a focus
        /// loss and report the release whenever the player gets around to letting go.
        /// </summary>
        private bool Down() => InputHelper.IsActive && Held(_button);
        /// <summary>Whether the button counted as down last frame.</summary>
        private bool WasDown() =>
            InputHelper.OldIsActive && InputHelper.MouseButtons[_button](InputHelper.OldMouse) == ButtonState.Pressed;

        /// <summary>
        /// The button that will be checked.
        /// </summary>
        private MouseButton _button;
    }
}
