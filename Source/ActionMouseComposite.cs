using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Combines ActionMouseSet in order to trigger when either one is true.
    /// </summary>
    public class ActionMouseComposite {
        // Group: Constructors
        public ActionMouseComposite() {
            _actionSets = new List<ActionMouseSet>();
        }
        public ActionMouseComposite(List<ActionMouseSet> actionSets) {
            _actionSets = actionSets;
        }

        // Group: Public Functions
        public ActionMouseSet AddSet(Func<MouseState, ButtonState> button) {
            ActionMouseSet newSet = new ActionMouseSet().AddNeed(button);
            AddSet(newSet);
            return newSet;
        }
        public ActionMouseComposite AddSet(ActionMouseSet ams) {
            _actionSets.Add(ams);
            return this;
        }
        public bool Pressed() {
            bool pressed = false;
            foreach (ActionMouseSet ams in _actionSets) {
                pressed = ams.Pressed();
                if (pressed) {
                    break;
                }
            }
            return pressed;
        }
        public bool Held() {
            bool held = false;
            foreach (ActionMouseSet ams in _actionSets) {
                held = ams.Held();
                if (held) {
                    break;
                }
            }
            return held;
        }
        public bool HeldOnly() {
            bool heldOnly = false;
            foreach (ActionMouseSet ams in _actionSets) {
                heldOnly = ams.HeldOnly();
                if (heldOnly) {
                    break;
                }
            }
            return heldOnly;
        }
        public bool Released() {
            bool released = false;
            foreach (ActionMouseSet ams in _actionSets) {
                released = ams.Released();
                if (released) {
                    break;
                }
            }
            return released;
        }

        // Group: Private Variables
        private List<ActionMouseSet> _actionSets;
    }
}