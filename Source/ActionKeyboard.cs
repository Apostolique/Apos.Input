﻿using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on a specific keyboard key.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    public class ActionKeyboard {

        // Group: Constructors

        /// <param name="needKey">The key to operate on.</param>
        public ActionKeyboard(Keys needKey) {
            _needKey = needKey;
        }

        // Group: Public Functions

        /// <returns>Returns true when a key was not pressed and is now pressed.</returns>
        public bool Pressed() {
            return Pressed(_needKey) && InputHelper.IsActive;
        }
        /// <returns>Returns true when a key is now pressed.</returns>
        public bool Holding() {
            return Holding(_needKey) && InputHelper.IsActive;
        }
        /// <returns>Returns true when a key was pressed and is now pressed.</returns>
        public bool HoldingOnly() {
            return HoldingOnly(_needKey) && InputHelper.IsActive;
        }
        /// <returns>Returns true when a key was pressed and is now not pressed.</returns>
        public bool Released() {
            return Released(_needKey) && InputHelper.IsActive;
        }

        // Group: Static Functions

        /// <returns>Returns true when a key was released and is now pressed.</returns>
        public static bool Pressed(Keys key) {
            return InputHelper.NewKeyboard.IsKeyDown(key) && InputHelper.OldKeyboard.IsKeyUp(key);
        }
        /// <returns>Returns true when a key is now pressed.</returns>
        public static bool Holding(Keys key) {
            return InputHelper.NewKeyboard.IsKeyDown(key);
        }
        /// <returns>Returns true when a key was pressed and is now pressed.</returns>
        public static bool HoldingOnly(Keys key) {
            return InputHelper.NewKeyboard.IsKeyDown(key) && InputHelper.OldKeyboard.IsKeyDown(key);
        }
        /// <returns>Returns true when a key was pressed and is now released.</returns>
        public static bool Released(Keys key) {
            return InputHelper.NewKeyboard.IsKeyUp(key) && InputHelper.OldKeyboard.IsKeyDown(key);
        }

        // Group: Private Variables

        /// <summary>
        /// The key that will be checked.
        /// </summary>
        private Keys _needKey;
    }
}