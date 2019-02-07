using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Combines ActionMouse to make more complex action triggers.
    /// </summary>
    public class ActionMouseSet {
        // Group: Constructors
        public ActionMouseSet() {
            _needAction = new List<ActionMouse>();
            _notAction = new List<ActionMouse>();
        }
        public ActionMouseSet(List<ActionMouse> needAction, List<ActionMouse> notAction) {
            _needAction = needAction;
            _notAction = notAction;
        }

        // Group: Public Functions
        public ActionMouseSet AddNeed(Func<MouseState, ButtonState> button) {
            return AddNeed(new ActionMouse(button));
        }
        public ActionMouseSet AddNeed(ActionMouse action) {
            _needAction.Add(action);
            return this;
        }
        public ActionMouseSet AddNot(Func<MouseState, ButtonState> button) {
            return AddNot(new ActionMouse(button));
        }
        public ActionMouseSet AddNot(ActionMouse action) {
            _notAction.Add(action);
            return this;
        }
        public bool Pressed() {
            bool pressed = false;
            bool held = true;
            bool notHeld = false;

            foreach (ActionMouse am in _needAction) {
                pressed = pressed || am.Pressed();
                if (pressed) {
                    break;
                }
            }
            foreach (ActionMouse am in _needAction) {
                held = held && am.Held();
                if (!held) {
                    break;
                }
            }
            foreach (ActionMouse am in _notAction) {
                notHeld = notHeld || am.Held();
                if (notHeld) {
                    break;
                }
            }

            return pressed && held && !notHeld;
        }
        public bool Held() {
            bool held = true;
            bool notHeld = false;

            foreach (ActionMouse am in _needAction) {
                held = held && am.Held();
                if (!held) {
                    break;
                }
            }
            foreach (ActionMouse am in _notAction) {
                notHeld = notHeld || am.Held();
                if (notHeld) {
                    break;
                }
            }

            return held && !notHeld;
        }
        public bool HeldOnly() {
            bool held = true;
            bool notHeld = false;

            foreach (ActionMouse am in _needAction) {
                held = held && am.HeldOnly();
                if (!held) {
                    break;
                }
            }
            foreach (ActionMouse am in _notAction) {
                notHeld = notHeld || am.Held();
                if (notHeld) {
                    break;
                }
            }

            return held && !notHeld;
        }
        public bool Released() {
            bool released = false;
            bool held = true;
            bool notHeld = false;

            ActionMouse releasedMouse = null;

            foreach (ActionMouse am in _needAction) {
                released = released || am.Released();
                if (released) {
                    releasedMouse = am;
                    break;
                }
            }
            foreach (ActionMouse am in _needAction) {
                if (am == releasedMouse) {
                    continue;
                }
                held = held && am.Held();
                if (!held) {
                    break;
                }
            }
            foreach (ActionMouse am in _notAction) {
                notHeld = notHeld || am.Held();
                if (notHeld) {
                    break;
                }
            }

            return released && held && !notHeld;
        }

        // Group: Private Variables
        private List<ActionMouse> _needAction;
        private List<ActionMouse> _notAction;
    }
}