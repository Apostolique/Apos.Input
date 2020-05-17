using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific keyboard key.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    public class KeyboardCondition : ICondition {

        // Group: Constructors

        /// <param name="key">The key to operate on.</param>
        public KeyboardCondition(Keys key) {
            _key = key;
        }

        // Group: Public Functions

        /// <returns>Returns true when the key was not pressed and is now pressed.</returns>
        public bool Pressed() {
            return Pressed(_key) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the key is now pressed.</returns>
        public bool Held() {
            return Held(_key) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the key was pressed and is now pressed.</returns>
        public bool HeldOnly() {
            return HeldOnly(_key) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the key was pressed and is now not pressed.</returns>
        public bool Released() {
            return Released(_key) && InputHelper.IsActive;
        }

        // Group: Static Functions

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

        // Group: Private Variables

        /// <summary>
        /// The key that will be checked.
        /// </summary>
        private Keys _key;
    }
}