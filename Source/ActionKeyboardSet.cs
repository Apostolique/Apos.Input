using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Combines ActionKeyboard to make more complex action triggers.
    /// </summary>
    public class ActionKeyboardSet {
        // Group: Constructors
        public ActionKeyboardSet() {
            _needAction = new List<ActionKeyboard>();
            _notAction = new List<ActionKeyboard>();
        }
        public ActionKeyboardSet(List<ActionKeyboard> needAction, List<ActionKeyboard> notAction) {
            _needAction = needAction;
            _notAction = notAction;
        }

        // Group: Public Functions
        public ActionKeyboardSet AddNeed(Keys key) {
            return AddNeed(new ActionKeyboard(key));
        }
        public ActionKeyboardSet AddNeed(ActionKeyboard action) {
            _needAction.Add(action);
            return this;
        }
        public ActionKeyboardSet AddNot(Keys key) {
            return AddNot(new ActionKeyboard(key));
        }
        public ActionKeyboardSet AddNot(ActionKeyboard action) {
            _notAction.Add(action);
            return this;
        }
        public bool Pressed() {
            bool pressed = false;
            bool holding = true;
            bool notHolding = false;

            foreach (ActionKeyboard ak in _needAction) {
                pressed = pressed || ak.Pressed();
                if (pressed) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _needAction) {
                holding = holding && ak.Holding();
                if (!holding) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _notAction) {
                notHolding = notHolding || ak.Holding();
                if (notHolding) {
                    break;
                }
            }

            return pressed && holding && !notHolding;
        }
        public bool Holding() {
            bool holding = true;
            bool notHolding = false;

            foreach (ActionKeyboard ak in _needAction) {
                holding = holding && ak.Holding();
                if (!holding) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _notAction) {
                notHolding = notHolding || ak.Holding();
                if (notHolding) {
                    break;
                }
            }

            return holding && !notHolding;
        }
        public bool HoldingOnly() {
            bool holding = true;
            bool notHolding = false;

            foreach (ActionKeyboard ak in _needAction) {
                holding = holding && ak.HoldingOnly();
                if (!holding) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _notAction) {
                notHolding = notHolding || ak.HoldingOnly();
                if (notHolding) {
                    break;
                }
            }

            return holding && !notHolding;
        }
        public bool Released() {
            bool released = false;
            bool holding = true;
            bool notHolding = false;

            foreach (ActionKeyboard ak in _needAction) {
                released = released || ak.Released();
                if (released) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _needAction) {
                holding = holding && (ak.Holding() || ak.Released());
                if (!holding) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _notAction) {
                notHolding = notHolding || ak.Holding();
                if (notHolding) {
                    break;
                }
            }

            return released && holding && !notHolding;
        }

        // Group: Private Variables
        private List<ActionKeyboard> _needAction;
        private List<ActionKeyboard> _notAction;
    }
}