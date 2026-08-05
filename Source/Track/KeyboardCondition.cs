using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input.Track {
    /// <summary>
    /// Checks various conditions on a specific keyboard key.
    /// Non static methods only count the key as down while the game is active. Losing focus
    /// reports a release, and coming back with the key still down reports a press.
    /// </summary>
    public class KeyboardCondition : ICondition {

        /// <param name="key">The key to operate on.</param>
        public KeyboardCondition(Keys key) {
            _key = key;
            _condition = new Input.KeyboardCondition(key);
        }

        /// <returns>Returns true when the key was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            return Check(_condition.Pressed(), canConsume);
        }
        /// <returns>Returns true when the key is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return Check(_condition.Held(), canConsume);
        }
        /// <returns>Returns true when the key was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return Check(_condition.HeldOnly(), canConsume);
        }
        /// <returns>Returns true when the key was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return Check(_condition.Released(), canConsume);
        }
        /// <summary>Mark the key as used.</summary>
        public void Consume() {
            Consume(_key);
        }

        /// <returns>Returns true when the key was released and is now pressed.</returns>
        public static bool Pressed(Keys key, bool canConsume = true) {
            if (IsUnique(key) && Input.KeyboardCondition.Pressed(key)) {
                if (canConsume)
                    Consume(key);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the key is now pressed.</returns>
        public static bool Held(Keys key, bool canConsume = true) {
            if (IsUnique(key) && Input.KeyboardCondition.Held(key)) {
                if (canConsume)
                    Consume(key);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the key was pressed and is now pressed.</returns>
        public static bool HeldOnly(Keys key, bool canConsume = true) {
            if (IsUnique(key) && Input.KeyboardCondition.HeldOnly(key)) {
                if (canConsume)
                    Consume(key);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the key was pressed and is now released.</returns>
        public static bool Released(Keys key, bool canConsume = true) {
            if (IsUnique(key) && Input.KeyboardCondition.Released(key)) {
                if (canConsume)
                    Consume(key);
                return true;
            }
            return false;
        }
        /// <summary>Mark the key as used for this frame.</summary>
        public static void Consume(Keys key) {
            Tracker[key] = InputHelper.CurrentFrame;
        }

        /// <summary>Checks if the given key is unique for this frame.</summary>
        public static bool IsUnique(Keys key) => !Tracker.ContainsKey(key) || Tracker[key] != InputHelper.CurrentFrame;

        /// <summary>
        /// The untracked condition answers whether the key is in that state, tracking only decides
        /// whether this instance is the one that gets to see it.
        /// </summary>
        private bool Check(bool state, bool canConsume) {
            if (IsUnique(_key) && state) {
                if (canConsume)
                    Consume(_key);
                return true;
            }
            return false;
        }

        private Keys _key;
        private Input.KeyboardCondition _condition;

        /// <summary>Tracks keys being used each frames.</summary>
        protected static Dictionary<Keys, uint> Tracker = new Dictionary<Keys, uint>();
    }
}
