namespace Apos.Input {
    /// <summary>
    /// Combines a bunch of ICondition. All conditions must be true for this to trigger.
    /// </summary>
    /// <see cref="AnyCondition"/>
    public class AllCondition : ICondition {

        /// <summary>
        /// AllCondition with initial needed conditions or empty.
        /// </summary>
        public AllCondition(params ICondition[] conditions) {
            _conditions = conditions;
        }

        /// <returns>
        /// Returns true when all the needed conditions are held and at least one triggers as pressed.
        /// </returns>
        public bool Pressed(bool canConsume = true) {
            bool pressed = false;
            bool held = true;

            foreach (ICondition c in _conditions) {
                pressed = pressed || c.Pressed(false);
                if (pressed) {
                    break;
                }
            }
            foreach (ICondition c in _conditions) {
                held = held && c.Held(false);
                if (!held) {
                    break;
                }
            }

            if (canConsume && pressed && held) {
                Consume();
            }
            return pressed && held;
        }
        /// <returns>
        /// Returns true when all the needed conditions are held.
        /// </returns>
        public bool Held(bool canConsume = true) {
            bool held = _conditions.Length > 0;

            foreach (ICondition c in _conditions) {
                held = held && c.Held(false);
                if (!held) {
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
            bool held = _conditions.Length > 0;

            foreach (ICondition c in _conditions) {
                held = held && c.HeldOnly(false);
                if (!held) {
                    break;
                }
            }

            if (canConsume && held) {
                Consume();
            }
            return held;
        }
        /// <returns>
        /// Returns true when at least one needed condition is released and the other needed conditions are held.
        /// </returns>
        public bool Released(bool canConsume = true) {
            bool released = false;
            bool held = true;

            foreach (ICondition c in _conditions) {
                released = released || c.Released(false);
                if (released) {
                    break;
                }
            }
            foreach (ICondition c in _conditions) {
                held = held && (c.Held(false) || c.Released(false));
                if (!held) {
                    break;
                }
            }

            if (canConsume && released && held) {
                Consume();
            }
            return released && held;
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
