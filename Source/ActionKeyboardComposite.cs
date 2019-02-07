using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Combines ActionKeyboardSets in order to trigger when at least one is true.
    /// </summary>
    /// <see cref="ActionKeyboardSet"/>
    public class ActionKeyboardComposite {

        // Group: Constructors

        /// <summary>
        /// Empty ActionKeyboardComposite.
        /// </summary>
        public ActionKeyboardComposite() {
            _actionSets = new List<ActionKeyboardSet>();
        }
        /// <summary>
        /// ActionKeyboardComposite with initial ActionKeyboardSets.
        /// </summary>
        /// <param name="actionSets">A list of ActionKeyboardSets.</param>
        public ActionKeyboardComposite(List<ActionKeyboardSet> actionSets) {
            _actionSets = actionSets;
        }

        // Group: Public Functions

        /// <summary>
        /// This implicitly creates an ActionKeyboardSet.
        /// </summary>
        /// <param name="key">A key that will have it's own set.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ActionKeyboardSet AddSet(Keys key) {
            ActionKeyboardSet newSet = new ActionKeyboardSet().AddNeed(key);
            AddSet(newSet);
            return newSet;
        }
        /// <param name="aks">An ActionKeyboardSet to add.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ActionKeyboardComposite AddSet(ActionKeyboardSet aks) {
            _actionSets.Add(aks);
            return this;
        }
        /// <returns>
        /// Returns true when at least 1 set triggers as pressed.
        /// </returns>
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
        /// <returns>
        /// Returns true when at least 1 set triggers as held.
        /// </returns>
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
        /// <returns>
        /// Returns true when at least 1 set was held and is now held.
        /// </returns>
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
        /// <returns>
        /// Returns true when at least 1 set triggers as released.
        /// </returns>
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

        /// <summary>
        /// A list of ActionKeyboardSets.
        /// </summary>
        private List<ActionKeyboardSet> _actionSets;
    }
}