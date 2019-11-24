using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Combines a bunch of ICondition to make complex condition sequences.
    /// Operates over conditions that are needed and conditions that must not be activated.
    /// </summary>
    public class ConditionSet {

        // Group: Constructors

        /// <summary>
        /// Empty ConditionSet.
        /// </summary>
        public ConditionSet() : this(new List<ICondition>(), new List<ICondition>()) { }
        /// <summary>
        /// ConditionSet with initial needed conditions.
        /// </summary>
        public ConditionSet(List<ICondition> needConditions) : this(needConditions, new List<ICondition>()) { }
        /// <summary>
        /// ConditionSet with initial values.
        /// </summary>
        /// <param name="needConditions">A list of needed conditions.</param>
        /// <param name="notConditions">A list of condition that must not be activated.</param>
        public ConditionSet(List<ICondition> needConditions, List<ICondition> notConditions) {
            _needConditions = needConditions;
            _notConditions = notConditions;
        }

        // Group: Public Functions

        /// <summary>
        /// This implicitly creates a ConditionKeyboard.
        /// </summary>
        /// <param name="key">Adds a new needed key.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ConditionSet AddNeed(Keys key) {
            return AddNeed(new ConditionKeyboard(key));
        }
        /// <summary>
        /// This implicitly creates a ConditionMouse.
        /// </summary>
        /// <param name="button">Adds a new needed mouse button.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ConditionSet AddNeed(MouseButton button) {
            return AddNeed(new ConditionMouse(button));
        }
        /// <summary>
        /// This implicitly creates a ConditionGamePad.
        /// </summary>
        /// <param name="button">Adds a new needed gamepad button.</param>
        /// <param name="gamePadIndex">The index of the gamepad to operate on.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ConditionSet AddNeed(GamePadButton button, int gamePadIndex) {
            return AddNeed(new ConditionGamePad(button, gamePadIndex));
        }
        /// <param name="condition">Adds a condition that is needed.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ConditionSet AddNeed(ICondition condition) {
            _needConditions.Add(condition);
            return this;
        }
        /// <summary>
        /// This implicitly creates a ConditionKeyboard.
        /// </summary>
        /// <param name="key">Adds a key that must not be pressed.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ConditionSet AddNot(Keys key) {
            return AddNot(new ConditionKeyboard(key));
        }
        /// <summary>
        /// This implicitly creates a ConditionMouse.
        /// </summary>
        /// <param name="button">Adds a mouse button that must not be pressed.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ConditionSet AddNot(MouseButton button) {
            return AddNot(new ConditionMouse(button));
        }
        /// <summary>
        /// This implicitly creates a ConditionGamePad.
        /// </summary>
        /// <param name="button">Adds a gamepad button that must not be pressed.</param>
        /// <param name="gamePadIndex">The index of the gamepad to operate on.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ConditionSet AddNot(GamePadButton button, int gamePadIndex) {
            return AddNot(new ConditionGamePad(button, gamePadIndex));
        }
        /// <param name="condition">Adds a condition that must not be activated.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ConditionSet AddNot(ICondition condition) {
            _notConditions.Add(condition);
            return this;
        }
        /// <returns>
        /// Returns true when all the needed condition are held and at least 1 triggers as pressed.
        /// Always returns false when at least 1 not needed condition is held.
        /// </returns>
        public bool Pressed() {
            bool pressed = false;
            bool held = true;
            bool notHeld = false;

            foreach (ICondition c in _needConditions) {
                pressed = pressed || c.Pressed();
                if (pressed) {
                    break;
                }
            }
            foreach (ICondition c in _needConditions) {
                held = held && c.Held();
                if (!held) {
                    break;
                }
            }
            foreach (ICondition c in _notConditions) {
                notHeld = notHeld || c.Held();
                if (notHeld) {
                    break;
                }
            }

            return pressed && held && !notHeld;
        }
        /// <returns>
        /// Returns true when all the needed condition are held.
        /// Always returns false when at least 1 not needed condition is held.
        /// </returns>
        public bool Held() {
            bool held = true;
            bool notHeld = false;

            foreach (ICondition c in _needConditions) {
                held = held && c.Held();
                if (!held) {
                    break;
                }
            }
            foreach (ICondition c in _notConditions) {
                notHeld = notHeld || c.Held();
                if (notHeld) {
                    break;
                }
            }

            return held && !notHeld;
        }
        /// <returns>
        /// Returns true when all the needed conditions were held and are now held.
        /// Always returns false when at least 1 not needed condition is held.
        /// </returns>
        public bool HeldOnly() {
            bool held = true;
            bool notHeld = false;

            foreach (ICondition c in _needConditions) {
                held = held && c.HeldOnly();
                if (!held) {
                    break;
                }
            }
            foreach (ICondition c in _notConditions) {
                notHeld = notHeld || c.Held();
                if (notHeld) {
                    break;
                }
            }

            return held && !notHeld;
        }
        /// <returns>
        /// Returns true when at least 1 needed condition is released and the other needed condition are held.
        /// Always returns false when at least 1 not needed condition is held.
        /// </returns>
        public bool Released() {
            bool released = false;
            bool held = true;
            bool notHeld = false;

            foreach (ICondition c in _needConditions) {
                released = released || c.Released();
                if (released) {
                    break;
                }
            }
            foreach (ICondition c in _needConditions) {
                held = held && (c.Held() || c.Released());
                if (!held) {
                    break;
                }
            }
            foreach (ICondition c in _notConditions) {
                notHeld = notHeld || c.Held();
                if (notHeld) {
                    break;
                }
            }

            return released && held && !notHeld;
        }

        // Group: Private Variables

        /// <summary>
        /// List of Condition that are needed.
        /// </summary>
        private List<ICondition> _needConditions;
        /// <summary>
        /// List of Condition that must never be held.
        /// </summary>
        private List<ICondition> _notConditions;
    }
}