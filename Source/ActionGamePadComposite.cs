using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Combines ActionGamePadSet in order to trigger when either one is true.
    /// </summary>
    public class ActionGamePadComposite {
        // Group: Constructors
        public ActionGamePadComposite() {
            _actionSets = new List<ActionGamePadSet>();
        }
        public ActionGamePadComposite(List<ActionGamePadSet> actionSets) {
            _actionSets = actionSets;
        }

        // Group: Public Functions
        public ActionGamePadSet AddSet(Func<GamePadState[], ButtonState> button) {
            ActionGamePadSet newSet = new ActionGamePadSet().AddNeed(button);
            AddSet(newSet);
            return newSet;
        }
        public ActionGamePadComposite AddSet(ActionGamePadSet ams) {
            _actionSets.Add(ams);
            return this;
        }
        public bool Pressed() {
            bool pressed = false;
            foreach (ActionGamePadSet ags in _actionSets) {
                pressed = ags.Pressed();
                if (pressed) {
                    break;
                }
            }
            return pressed;
        }
        public bool Holding() {
            bool holding = false;
            foreach (ActionGamePadSet ags in _actionSets) {
                holding = ags.Holding();
                if (holding) {
                    break;
                }
            }
            return holding;
        }
        public bool HoldingOnly() {
            bool holdingOnly = false;
            foreach (ActionGamePadSet ags in _actionSets) {
                holdingOnly = ags.HoldingOnly();
                if (holdingOnly) {
                    break;
                }
            }
            return holdingOnly;
        }
        public bool Released() {
            bool released = false;
            foreach (ActionGamePadSet ags in _actionSets) {
                released = ags.Released();
                if (released) {
                    break;
                }
            }
            return released;
        }

        // Group: Private Variables
        private List<ActionGamePadSet> _actionSets;
    }
}