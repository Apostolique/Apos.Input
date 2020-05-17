namespace Apos.Input {
    /// <summary>
    /// Combines a bunch of ICondition. All conditions must be true for this to trigger.
    /// </summary>
    /// <see cref="AnyCondition"/>
    public class AllCondition : ICondition {

        // Group: Constructors

        /// <summary>
        /// AllCondition with initial needed conditions or empty.
        /// </summary>
        public AllCondition(params ICondition[] conditions) {
            _conditions = conditions;
        }

        // Group: Public Functions

        /// <returns>
        /// Returns true when all the needed conditions are held and at least one triggers as pressed.
        /// </returns>
        public bool Pressed() {
            bool pressed = false;
            bool held = true;

            foreach (ICondition c in _conditions) {
                pressed = pressed || c.Pressed();
                if (pressed) {
                    break;
                }
            }
            foreach (ICondition c in _conditions) {
                held = held && c.Held();
                if (!held) {
                    break;
                }
            }

            return pressed && held;
        }
        /// <returns>
        /// Returns true when all the needed conditions are held.
        /// </returns>
        public bool Held() {
            bool held = true;

            foreach (ICondition c in _conditions) {
                held = held && c.Held();
                if (!held) {
                    break;
                }
            }

            return held;
        }
        /// <returns>
        /// Returns true when all the needed conditions were held and are now held.
        /// </returns>
        public bool HeldOnly() {
            bool held = true;

            foreach (ICondition c in _conditions) {
                held = held && c.HeldOnly();
                if (!held) {
                    break;
                }
            }

            return held;
        }
        /// <returns>
        /// Returns true when at least one needed condition is released and the other needed conditions are held.
        /// </returns>
        public bool Released() {
            bool released = false;
            bool held = true;

            foreach (ICondition c in _conditions) {
                released = released || c.Released();
                if (released) {
                    break;
                }
            }
            foreach (ICondition c in _conditions) {
                held = held && (c.Held() || c.Released());
                if (!held) {
                    break;
                }
            }

            return released && held;
        }

        // Group: Private Variables

        /// <summary>
        /// An array of ICondition.
        /// </summary>
        private ICondition[] _conditions;
    }
}