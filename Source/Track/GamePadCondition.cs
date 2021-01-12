using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input.Track {
    /// <summary>
    /// Checks various conditions on a specific gamepad button.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    public class GamePadCondition : ICondition {

        /// <param name="button">The button to operate on.</param>
        /// <param name="gamePadIndex">The index of the gamepad to operate on.</param>
        public GamePadCondition(GamePadButton button, int gamePadIndex) {
            _button = button;
            _gamePadIndex = gamePadIndex;
            _condition = new Input.GamePadCondition(_button, _gamePadIndex);

            if (!Tracker.ContainsKey((_button, _gamePadIndex))) {
                Tracker.Add((_button, _gamePadIndex), 0);
            }
        }

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            if (isUnique) {
                bool pressed = _condition.Pressed();

                if (canConsume && pressed) {
                    Consume();
                }
                return pressed;
            }
            return false;
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            if (isUnique) {
                bool held = _condition.Held();

                if (canConsume && held) {
                    Consume();
                }
                return held;
            }
            return false;
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            if (isUnique) {
                bool held = _condition.HeldOnly();

                if (canConsume && held) {
                    Consume();
                }
                return held;
            }
            return false;
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            if (isUnique) {
                bool released = _condition.Released();

                if (canConsume && released) {
                    Consume();
                }
                return released;
            }
            return false;
        }
        /// <summary>Mark the condition as used.</summary>
        public void Consume() {
            Tracker[(_button, _gamePadIndex)] = InputHelper.CurrentFrame;
        }

        private bool isUnique => Tracker[(_button, _gamePadIndex)] != InputHelper.CurrentFrame;

        private GamePadButton _button;
        private int _gamePadIndex;
        private Input.GamePadCondition _condition;

        /// <summary>
        /// Tracks buttons being used each frames.
        /// </summary>
        protected static Dictionary<(GamePadButton, int), uint> Tracker = new Dictionary<(GamePadButton, int), uint>();
    }
}
