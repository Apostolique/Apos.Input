using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Checks various keyboard conditions for a specific key.
    /// </summary>
    public class ActionKeyboard {
        //constructors
        public ActionKeyboard(Keys iNeedKey) {
            _needKey = iNeedKey;
        }

        //public functions
        public bool Pressed() {
            return Pressed(_needKey) && InputHelper.IsActive;
        }
        public bool Holding() {
            return Holding(_needKey) && InputHelper.IsActive;
        }
        public bool HoldingOnly() {
            return HoldingOnly(_needKey) && InputHelper.IsActive;
        }
        public bool Released() {
            return Released(_needKey) && InputHelper.IsActive;
        }
        public static bool Pressed(Keys key) {
            return InputHelper.NewKeyboard.IsKeyDown(key) && InputHelper.OldKeyboard.IsKeyUp(key);
        }
        public static bool Holding(Keys key) {
            return InputHelper.NewKeyboard.IsKeyDown(key);
        }
        public static bool HoldingOnly(Keys key) {
            return InputHelper.NewKeyboard.IsKeyDown(key) && InputHelper.OldKeyboard.IsKeyDown(key);
        }
        public static bool Released(Keys key) {
            return InputHelper.NewKeyboard.IsKeyUp(key) && InputHelper.OldKeyboard.IsKeyDown(key);
        }

        //private vars
        private Keys _needKey;
    }
}