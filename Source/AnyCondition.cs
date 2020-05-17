namespace Apos.Input {
    /// <summary>
    /// Combines a bunch of ICondition in order to trigger when at least one is true.
    /// </summary>
    /// <see cref="AllCondition"/>
    public class AnyCondition : ICondition {

        // Group: Constructors

        /// <summary>
        /// AnyCondition with initial ICondition array or empty.
        /// </summary>
        /// <param name="conditions">An array of ICondition.</param>
        public AnyCondition(params ICondition[] conditions) {
            _conditions = conditions;
        }

        // Group: Public Functions

        /// <returns>
        /// Returns true when at least one condition triggers as pressed.
        /// </returns>
        public bool Pressed() {
            bool pressed = false;
            foreach (ICondition cs in _conditions) {
                pressed = cs.Pressed();
                if (pressed) {
                    break;
                }
            }
            return pressed;
        }
        /// <returns>
        /// Returns true when at least one condition triggers as held.
        /// </returns>
        public bool Held() {
            bool held = false;
            foreach (ICondition cs in _conditions) {
                held = cs.Held();
                if (held) {
                    break;
                }
            }
            return held;
        }
        /// <returns>
        /// Returns true when at least one condition triggers as held only.
        /// </returns>
        public bool HeldOnly() {
            bool heldOnly = false;
            foreach (ICondition cs in _conditions) {
                heldOnly = cs.HeldOnly();
                if (heldOnly) {
                    break;
                }
            }
            return heldOnly;
        }
        /// <returns>
        /// Returns true when at least one condition triggers as released.
        /// </returns>
        public bool Released() {
            bool released = false;
            foreach (ICondition cs in _conditions) {
                released = cs.Released();
                if (released) {
                    break;
                }
            }
            return released;
        }

        // Group: Private Variables

        /// <summary>
        /// An array of ICondition.
        /// </summary>
        private ICondition[] _conditions;
    }
}