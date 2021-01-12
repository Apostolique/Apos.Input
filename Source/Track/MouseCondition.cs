using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

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
            _condition = new Input.MouseCondition(_button);

            if (!Tracker.ContainsKey(_button)) {
                Tracker.Add(_button, 0);
            }
        }

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            if (isUnique) {
                bool pressed = _condition.Pressed();

                if (canConsume && pressed) {
                    Consume();
                }
                return pressed;
            }
            return false;
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            if (isUnique) {
                bool held = _condition.Held();

                if (canConsume && held) {
                    Consume();
                }
                return held;
            }
            return false;
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            if (isUnique) {
                bool held = _condition.HeldOnly();

                if (canConsume && held) {
                    Consume();
                }
                return held;
            }
            return false;
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            if (isUnique) {
                bool released = _condition.Released();

                if (canConsume && released) {
                    Consume();
                }
                return released;
            }
            return false;
        }
        /// <summary>Mark the condition as used.</summary>
        public void Consume() {
            Tracker[_button] = InputHelper.CurrentFrame;
        }

        private bool isUnique => Tracker[_button] != InputHelper.CurrentFrame;

        private MouseButton _button;
        private Input.MouseCondition _condition;

        /// <summary>
        /// Tracks buttons being used each frames.
        /// </summary>
        protected static Dictionary<MouseButton, uint> Tracker = new Dictionary<MouseButton, uint>();
    }
}
