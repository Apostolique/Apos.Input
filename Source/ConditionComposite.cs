using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Combines a bunch of ConditionSet in order to trigger when at least one is true.
    /// </summary>
    /// <see cref="ConditionSet"/>
    public class ConditionComposite {

        // Group: Constructors

        /// <summary>
        /// ConditionComposite with initial ConditionSets or empty.
        /// </summary>
        /// <param name="conditionSets">A list of ConditionSets.</param>
        public ConditionComposite(params ConditionSet[] conditionSets) : this(conditionSets.ToList()) { }
        /// <summary>
        /// ConditionComposite with initial ConditionSets.
        /// </summary>
        /// <param name="conditionSets">A list of ConditionSets.</param>
        public ConditionComposite(List<ConditionSet> conditionSets) {
            _conditionSets = conditionSets;
        }

        // Group: Public Functions

        /// <summary>
        /// This implicitly creates a ConditionSet.
        /// </summary>
        /// <param name="key">A key that will have it's own set.</param>
        /// <returns>Returns the set for easy function chaining.</returns>
        public ConditionSet AddSet(Keys key) {
            ConditionSet newSet = new ConditionSet().AddNeed(key);
            AddSet(newSet);
            return newSet;
        }
        /// <summary>
        /// This implicitly creates a ConditionSet.
        /// </summary>
        /// <param name="button">A mouse button that will have it's own set.</param>
        /// <returns>Returns the set for easy function chaining.</returns>
        public ConditionSet AddSet(MouseButton button) {
            ConditionSet newSet = new ConditionSet().AddNeed(button);
            AddSet(newSet);
            return newSet;
        }
        /// <summary>
        /// This implicitly creates a ConditionSet.
        /// </summary>
        /// <param name="button">A gamepad button that will have it's own set.</param>
        /// <param name="gamePadIndex">The index of the gamepad to operate on.</param>
        /// <returns>Returns the set for easy function chaining.</returns>
        public ConditionSet AddSet(GamePadButton button, int gamePadIndex) {
            ConditionSet newSet = new ConditionSet().AddNeed(button, gamePadIndex);
            AddSet(newSet);
            return newSet;
        }
        /// <param name="cs">A ConditionSet to add.</param>
        /// <returns>Returns this composite for easy function chaining.</returns>
        public ConditionComposite AddSet(ConditionSet cs) {
            _conditionSets.Add(cs);
            return this;
        }
        /// <returns>
        /// Returns true when at least 1 set triggers as pressed.
        /// </returns>
        public bool Pressed() {
            bool pressed = false;
            foreach (ConditionSet cs in _conditionSets) {
                pressed = cs.Pressed();
                if (pressed) {
                    break;
                }
            }
            return pressed;
        }
        /// <returns>
        /// Returns true when at least 1 set triggers as held.
        /// </returns>
        public bool Held() {
            bool held = false;
            foreach (ConditionSet cs in _conditionSets) {
                held = cs.Held();
                if (held) {
                    break;
                }
            }
            return held;
        }
        /// <returns>
        /// Returns true when at least 1 set triggers as held only.
        /// </returns>
        public bool HeldOnly() {
            bool heldOnly = false;
            foreach (ConditionSet cs in _conditionSets) {
                heldOnly = cs.HeldOnly();
                if (heldOnly) {
                    break;
                }
            }
            return heldOnly;
        }
        /// <returns>
        /// Returns true when at least 1 set triggers as released.
        /// </returns>
        public bool Released() {
            bool released = false;
            foreach (ConditionSet cs in _conditionSets) {
                released = cs.Released();
                if (released) {
                    break;
                }
            }
            return released;
        }

        // Group: Private Variables

        /// <summary>
        /// A list of ConditionSet.
        /// </summary>
        private List<ConditionSet> _conditionSets;
    }
}