using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input.Track {
    /// <summary>
    /// Checks various conditions on a specific keyboard key.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    public class KeyboardCondition : ICondition {

        /// <param name="key">The key to operate on.</param>
        public KeyboardCondition(Keys key) {
            _key = key;
            _condition = new Input.KeyboardCondition(_key);

            if (!Tracker.ContainsKey(_key)) {
                Tracker.Add(_key, 0);
            }
        }

        /// <returns>Returns true when the key was not pressed and is now pressed.</returns>
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
        /// <returns>Returns true when the key is now pressed.</returns>
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
        /// <returns>Returns true when the key was pressed and is now pressed.</returns>
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
        /// <returns>Returns true when the key was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            if (isUnique) {
                bool release = _condition.Released();

                if (canConsume && release) {
                    Consume();
                }
                return release;
            }
            return false;
        }
        /// <summary>Mark the condition as used.</summary>
        public void Consume() {
            Tracker[_key] = InputHelper.CurrentFrame;
        }

        private bool isUnique => Tracker[_key] != InputHelper.CurrentFrame;

        private Keys _key;
        private Input.KeyboardCondition _condition;

        /// <summary>
        /// Tracks keys being used each frames.
        /// </summary>
        protected static Dictionary<Keys, uint> Tracker = new Dictionary<Keys, uint>();
    }
}
