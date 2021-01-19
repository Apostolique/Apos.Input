using System.Collections.Generic;

namespace Apos.Input.Track {
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
            return Pressed(_button, canConsume) && Input.MouseCondition.IsMouseValid;
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return Held(_button, canConsume) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return HeldOnly(_button, canConsume) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return Released(_button, canConsume) && InputHelper.IsActive;
        }
        /// <summary>Mark the condition as used.</summary>
        public void Consume() {
            Consume(_button);
        }

        /// <returns>Returns true when the mouse button was released and is now pressed.</returns>
        public static bool Pressed(MouseButton button, bool canConsume = true) {
            if (IsUnique(button) && Input.MouseCondition.Pressed(button)) {
                if (canConsume)
                    Consume(button);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the mouse button is now pressed.</returns>
        public static bool Held(MouseButton button, bool canConsume = true) {
            if (IsUnique(button) && Input.MouseCondition.Held(button)) {
                if (canConsume)
                    Consume(button);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the mouse button was pressed and is now pressed.</returns>
        public static bool HeldOnly(MouseButton button, bool canConsume = true) {
            if (IsUnique(button) && Input.MouseCondition.HeldOnly(button)) {
                if (canConsume)
                    Consume(button);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the mouse button was pressed and is now released.</returns>
        public static bool Released(MouseButton button, bool canConsume = true) {
            if (IsUnique(button) && Input.MouseCondition.Released(button)) {
                if (canConsume)
                    Consume(button);
                return true;
            }
            return false;
        }
        /// <summary>Mark the mouse button as used for this frame.</summary>
        public static void Consume(MouseButton button) {
            Tracker[button] = InputHelper.CurrentFrame;
        }
        /// <summary>Checks if the given mouse button is unique for this frame.</summary>
        public static bool IsUnique(MouseButton button) => !Tracker.ContainsKey(button) || Tracker[button] != InputHelper.CurrentFrame;

        private MouseButton _button;

        /// <summary>Tracks buttons being used each frames.</summary>
        protected static Dictionary<MouseButton, uint> Tracker = new Dictionary<MouseButton, uint>();
    }
}
