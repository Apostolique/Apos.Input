using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
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
            return Pressed(_key) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the key is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return Held(_key) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the key was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return HeldOnly(_key) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the key was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return Released(_key) && InputHelper.IsActive;
        }
        /// <summary>Does nothing since this condition isn't tracked.</summary>
        public void Consume() { }

        /// <returns>Returns true when the key was released and is now pressed.</returns>
        public static bool Pressed(Keys key) {
            return InputHelper.NewKeyboard.IsKeyDown(key) && InputHelper.OldKeyboard.IsKeyUp(key);
        }
        /// <returns>Returns true when the key is now pressed.</returns>
        public static bool Held(Keys key) {
            return InputHelper.NewKeyboard.IsKeyDown(key);
        }
        /// <returns>Returns true when the key was pressed and is now pressed.</returns>
        public static bool HeldOnly(Keys key) {
            return InputHelper.NewKeyboard.IsKeyDown(key) && InputHelper.OldKeyboard.IsKeyDown(key);
        }
        /// <returns>Returns true when the key was pressed and is now released.</returns>
        public static bool Released(Keys key) {
            return InputHelper.NewKeyboard.IsKeyUp(key) && InputHelper.OldKeyboard.IsKeyDown(key);
        }

        /// <summary>
        /// The key that will be checked.
        /// </summary>
        private Keys _key;
    }
}
