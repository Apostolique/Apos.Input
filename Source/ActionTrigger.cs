using System;
using System.Collections.Generic;

namespace Apos.Input {
    /// <summary>
    /// Used to abstract actions away.
    /// This makes it possible to create an action that triggers on
    /// gamepad buttons, keyboard press, or even in game events.
    /// </summary>
    public class ActionTrigger {

        // Group: Constructors

        public ActionTrigger() {
            _actions = new List<Func<bool>>();
        }
        public ActionTrigger(List<Func<bool>> actions) {
            _actions = actions;
        }

        // Group: Public Functions

        public void AddAction(Func<bool> action) {
            _actions.Add(action);
        }
        public bool IsTriggered() {
            foreach (Func<bool> f in _actions) {
                if (f()) {
                    return true;
                }
            }
            return false;
        }

        // Group: Private Variables

        private List<Func<bool>> _actions;
    }
}