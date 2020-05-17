using System;

namespace Apos.Input {
    /// <summary>
    /// Used to abstract bool functions away.
    /// This makes it possible to combine bool functions and trigger when any of them return true.
    /// </summary>
    public class Trigger {

        // Group: Constructors

        /// <summary>
        /// Trigger with initial values or empty.
        /// </summary>
        /// <param name="triggers">An array of bool functions.</param>
        public Trigger(params Func<bool>[] triggers) {
            _triggers = triggers;
        }

        // Group: Public Functions

        /// <returns>Returns true when at least one function is true.</returns>
        public bool IsTriggered() {
            bool isTriggered = false;
            foreach (Func<bool> f in _triggers) {
                isTriggered = f();
                if (isTriggered) {
                    break;
                }
            }
            return isTriggered;
        }

        // Group: Private Variables

        /// <summary>
        /// List of conditions.
        /// </summary>
        private Func<bool>[] _triggers;
    }
}