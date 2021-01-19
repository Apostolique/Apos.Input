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
        }

        /// <returns>Returns true when the key was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            return Pressed(_key, canConsume) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the key is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return Held(_key, canConsume) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the key was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return HeldOnly(_key, canConsume) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the key was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return Released(_key, canConsume) && InputHelper.IsActive;
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

        private Keys _key;

        /// <summary>Tracks keys being used each frames.</summary>
        protected static Dictionary<Keys, uint> Tracker = new Dictionary<Keys, uint>();
    }
}
