using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Combines ActionMouseSet in order to trigger when either one is true.
    /// </summary>
    public class ActionMouseComposite {
        //constructors
        public ActionMouseComposite() {
            _actionSets = new List<ActionMouseSet>();
        }
        public ActionMouseComposite(List<ActionMouseSet> actionSets) {
            _actionSets = actionSets;
        }

        //public functions
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
        public bool Holding() {
            bool holding = false;
            foreach (ActionMouseSet ams in _actionSets) {
                holding = ams.Holding();
                if (holding) {
                    break;
                }
            }
            return holding;
        }
        public bool HoldingOnly() {
            bool holdingOnly = false;
            foreach (ActionMouseSet ams in _actionSets) {
                holdingOnly = ams.HoldingOnly();
                if (holdingOnly) {
                    break;
                }
            }
            return holdingOnly;
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

        //private vars
        private List<ActionMouseSet> _actionSets;
    }
}