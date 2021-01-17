namespace Apos.Input {
    /// <summary>
    /// Combines a bunch of ICondition in order to trigger when at least one is true.
    /// </summary>
    /// <see cref="AllCondition"/>
    public class AnyCondition : ICondition {

        /// <summary>
        /// AnyCondition with initial ICondition array or empty.
        /// </summary>
        /// <param name="conditions">An array of ICondition.</param>
        public AnyCondition(params ICondition[] conditions) {
            _conditions = conditions;
        }

        /// <returns>
        /// Returns true when at least one condition triggers as pressed.
        /// </returns>
        public bool Pressed(bool canConsume = true) {
            bool pressed = false;
            foreach (ICondition cs in _conditions) {
                pressed = cs.Pressed(false);
                if (pressed) {
                    break;
                }
            }

            if (canConsume && pressed) {
                Consume();
            }
            return pressed;
        }
        /// <returns>
        /// Returns true when at least one condition triggers as held.
        /// </returns>
        public bool Held(bool canConsume = true) {
            bool held = false;
            foreach (ICondition cs in _conditions) {
                held = cs.Held(false);
                if (held) {
                    break;
                }
            }

            if (canConsume && held) {
                Consume();
            }
            return held;
        }
        /// <returns>
        /// Returns true when all the needed conditions were held and are now held.
        /// </returns>
        public bool HeldOnly(bool canConsume = true) {
            bool heldOnly = false;
            foreach (ICondition cs in _conditions) {
                heldOnly = cs.HeldOnly(false);
                if (heldOnly) {
                    break;
                }
            }

            if (canConsume && heldOnly) {
                Consume();
            }
            return heldOnly;
        }
        /// <returns>
        /// Returns true when at least one condition triggers as released.
        /// </returns>
        public bool Released(bool canConsume = true) {
            bool released = false;
            foreach (ICondition cs in _conditions) {
                released = cs.Released(false);
                if (released) {
                    break;
                }
            }

            if (canConsume && released) {
                Consume();
            }
            return released;
        }
        /// <summary>Mark all conditions as used.</summary>
        public void Consume() {
            foreach (ICondition c in _conditions) {
                c.Consume();
            }
        }

        /// <summary>
        /// An array of ICondition.
        /// </summary>
        private ICondition[] _conditions;
    }
}
