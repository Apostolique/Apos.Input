using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Combines ActionKeyboards to make complex triggers.
    /// Operates over keys that are needed and keys that must not be pressed.
    /// </summary>
    public class ActionKeyboardSet {

        // Group: Constructors

        /// <summary>
        /// Empty ActionKeyboardSet.
        /// </summary>
        public ActionKeyboardSet() {
            _needAction = new List<ActionKeyboard>();
            _notAction = new List<ActionKeyboard>();
        }
        /// <summary>
        /// ActionKeyboardSet with initial values.
        /// </summary>
        /// <param name="needAction">A list of needed keys.</param>
        /// <param name="notAction">A list of keys that must never be pressed.</param>
        public ActionKeyboardSet(List<ActionKeyboard> needAction, List<ActionKeyboard> notAction) {
            _needAction = needAction;
            _notAction = notAction;
        }

        // Group: Public Functions

        /// <summary>
        /// This implicitly creates an ActionKeyboard.
        /// </summary>
        /// <param name="key">Adds a new needed key.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ActionKeyboardSet AddNeed(Keys key) {
            return AddNeed(new ActionKeyboard(key));
        }
        /// <param name="action">Adds an ActionKeyboard that is needed.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ActionKeyboardSet AddNeed(ActionKeyboard action) {
            _needAction.Add(action);
            return this;
        }
        /// <summary>
        /// This implicitly creates an ActionKeyboard.
        /// </summary>
        /// <param name="key">Adds a key that must not be pressed.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ActionKeyboardSet AddNot(Keys key) {
            return AddNot(new ActionKeyboard(key));
        }
        /// <param name="action">Adds an ActionKeyboard that must not be pressed.</param>
        /// <returns>Returns itself for easy function chaining.</returns>
        public ActionKeyboardSet AddNot(ActionKeyboard action) {
            _notAction.Add(action);
            return this;
        }
        /// <returns>
        /// Returns true when all the needed keys are held and at least 1 triggers as pressed.
        /// Always returns false when at least 1 not needed key is held.
        /// </returns>
        public bool Pressed() {
            bool pressed = false;
            bool held = true;
            bool notHeld = false;

            foreach (ActionKeyboard ak in _needAction) {
                pressed = pressed || ak.Pressed();
                if (pressed) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _needAction) {
                held = held && ak.Held();
                if (!held) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _notAction) {
                notHeld = notHeld || ak.Held();
                if (notHeld) {
                    break;
                }
            }

            return pressed && held && !notHeld;
        }
        /// <returns>
        /// Returns true when all the needed keys are held.
        /// Always returns false when at least 1 not needed key is held.
        /// </returns>
        public bool Held() {
            bool held = true;
            bool notHeld = false;

            foreach (ActionKeyboard ak in _needAction) {
                held = held && ak.Held();
                if (!held) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _notAction) {
                notHeld = notHeld || ak.Held();
                if (notHeld) {
                    break;
                }
            }

            return held && !notHeld;
        }
        /// <returns>
        /// Returns true when all the needed keys were held and are now held.
        /// Always returns false when at least 1 not needed key is held.
        /// </returns>
        public bool HeldOnly() {
            bool held = true;
            bool notHeld = false;

            foreach (ActionKeyboard ak in _needAction) {
                held = held && ak.HeldOnly();
                if (!held) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _notAction) {
                notHeld = notHeld || ak.Held();
                if (notHeld) {
                    break;
                }
            }

            return held && !notHeld;
        }
        /// <returns>
        /// Returns true when at least 1 needed key is released and the other needed keys are held.
        /// Always returns false when at least 1 not needed key is held.
        /// </returns>
        public bool Released() {
            bool released = false;
            bool held = true;
            bool notHeld = false;

            foreach (ActionKeyboard ak in _needAction) {
                released = released || ak.Released();
                if (released) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _needAction) {
                held = held && (ak.Held() || ak.Released());
                if (!held) {
                    break;
                }
            }
            foreach (ActionKeyboard ak in _notAction) {
                notHeld = notHeld || ak.Held();
                if (notHeld) {
                    break;
                }
            }

            return released && held && !notHeld;
        }

        // Group: Private Variables

        /// <summary>
        /// List of ActionKeyboards that are needed.
        /// </summary>
        private List<ActionKeyboard> _needAction;
        /// <summary>
        /// List of ActionKeyboards that must never be held.
        /// </summary>
        private List<ActionKeyboard> _notAction;
    }
}