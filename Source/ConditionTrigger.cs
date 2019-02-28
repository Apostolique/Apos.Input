using System;
using System.Collections.Generic;

namespace Apos.Input {
    /// <summary>
    /// Used to abstract conditions away.
    /// This makes it possible to create a condition that triggers on
    /// gamepad buttons, keyboard press, or even in game events.
    /// </summary>
    public class ConditionTrigger {

        // Group: Constructors

        /// <summary>
        /// Empty ConditionTrigger.
        /// </summary>
        public ConditionTrigger() {
            _conditions = new List<Func<bool>>();
        }
        /// <summary>
        /// ConditionTrigger with initial values.
        /// </summary>
        /// <param name="conditions">A list of conditions.</param>
        public ConditionTrigger(List<Func<bool>> conditions) {
            _conditions = conditions;
        }

        // Group: Public Functions

        /// <param name="condition">A condition to add.</param>
        public void AddCondition(Func<bool> condition) {
            _conditions.Add(condition);
        }
        /// <returns>Returns true when at least 1 condition is true.</returns>
        public bool IsTriggered() {
            foreach (Func<bool> f in _conditions) {
                if (f()) {
                    return true;
                }
            }
            return false;
        }

        // Group: Private Variables

        /// <summary>
        /// List of conditions.
        /// </summary>
        private List<Func<bool>> _conditions;
    }
}