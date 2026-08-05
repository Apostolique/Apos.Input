using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific keyboard key.
    /// Non static methods only count the key as down while the game is active. Losing focus
    /// reports a release, and coming back with the key still down reports a press.
    /// </summary>
    public class KeyboardCondition : ICondition {

        /// <param name="key">The key to operate on.</param>
        public KeyboardCondition(Keys key) {
            _key = key;
        }

        /// <returns>Returns true when the key was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            return Down() && !WasDown();
        }
        /// <returns>Returns true when the key is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return Down();
        }
        /// <returns>Returns true when the key was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return Down() && WasDown();
        }
        /// <returns>Returns true when the key was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return !Down() && WasDown();
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
        /// A key only counts as down while the game is active, which is what turns a focus change
        /// into a press or a release on its own. Waiting for the device to report it doesn't work
        /// since the platform decides what happens to a held input when focus goes away.
        /// </summary>
        private bool Down() => InputHelper.IsActive && Held(_key);
        /// <summary>Whether the key counted as down last frame.</summary>
        private bool WasDown() => InputHelper.OldIsActive && InputHelper.OldKeyboard.IsKeyDown(_key);

        /// <summary>
        /// The key that will be checked.
        /// </summary>
        private Keys _key;
    }
}
