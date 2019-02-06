using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Combines ActionKeyboardSet in order to trigger when either one is true.
    /// </summary>
    public class ActionKeyboardComposite {
        // Group: Constructors
        public ActionKeyboardComposite() {
            _actionSets = new List<ActionKeyboardSet>();
        }
        public ActionKeyboardComposite(List<ActionKeyboardSet> actionSets) {
            _actionSets = actionSets;
        }

        // Group: Public Functions
        public ActionKeyboardSet AddSet(Keys key) {
            ActionKeyboardSet newSet = new ActionKeyboardSet().AddNeed(key);
            AddSet(newSet);
            return newSet;
        }
        public ActionKeyboardComposite AddSet(ActionKeyboardSet aks) {
            _actionSets.Add(aks);
            return this;
        }
        public bool Pressed() {
            bool pressed = false;
            foreach (ActionKeyboardSet aks in _actionSets) {
                pressed = aks.Pressed();
                if (pressed) {
                    break;
                }
            }
            return pressed;
        }
        public bool Holding() {
            bool holding = false;
            foreach (ActionKeyboardSet aks in _actionSets) {
                holding = aks.Holding();
                if (holding) {
                    break;
                }
            }
            return holding;
        }
        public bool HoldingOnly() {
            bool holdingOnly = false;
            foreach (ActionKeyboardSet aks in _actionSets) {
                holdingOnly = aks.HoldingOnly();
                if (holdingOnly) {
                    break;
                }
            }
            return holdingOnly;
        }
        public bool Released() {
            bool released = false;
            foreach (ActionKeyboardSet aks in _actionSets) {
                released = aks.Released();
                if (released) {
                    break;
                }
            }
            return released;
        }

        // Group: Private Variables
        private List<ActionKeyboardSet> _actionSets;
    }
}