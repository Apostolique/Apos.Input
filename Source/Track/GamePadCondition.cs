using System.Collections.Generic;

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
        }

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            return Pressed(_button, _gamePadIndex, canConsume) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return Held(_button, _gamePadIndex, canConsume) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return HeldOnly(_button, _gamePadIndex, canConsume) && InputHelper.IsActive;
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return Released(_button, _gamePadIndex, canConsume) && InputHelper.IsActive;
        }
        /// <summary>Mark the condition as used.</summary>
        public void Consume() {
            Tracker[(_button, _gamePadIndex)] = InputHelper.CurrentFrame;
        }

        /// <returns>Returns true when the mouse button was released and is now pressed.</returns>
        public static bool Pressed(GamePadButton button, int gamePadIndex, bool canConsume = true) {
            if (IsUnique(button, gamePadIndex) && Input.GamePadCondition.Pressed(button, gamePadIndex)) {
                if (canConsume)
                    Consume(button, gamePadIndex);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the gamepad button is now pressed.</returns>
        public static bool Held(GamePadButton button, int gamePadIndex, bool canConsume = true) {
            if (IsUnique(button, gamePadIndex) && Input.GamePadCondition.Held(button, gamePadIndex)) {
                if (canConsume)
                    Consume(button, gamePadIndex);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the gamepad button was pressed and is now pressed.</returns>
        public static bool HeldOnly(GamePadButton button, int gamePadIndex, bool canConsume = true) {
            if (IsUnique(button, gamePadIndex) && Input.GamePadCondition.HeldOnly(button, gamePadIndex)) {
                if (canConsume)
                    Consume(button, gamePadIndex);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the gamepad button was pressed and is now released.</returns>
        public static bool Released(GamePadButton button, int gamePadIndex, bool canConsume = true) {
            if (IsUnique(button, gamePadIndex) && Input.GamePadCondition.Released(button, gamePadIndex)) {
                if (canConsume)
                    Consume(button, gamePadIndex);
                return true;
            }
            return false;
        }
        /// <summary>Mark the gamepad button as used for this frame.</summary>
        public static void Consume(GamePadButton button, int gamePadIndex) {
            Tracker[(button, gamePadIndex)] = InputHelper.CurrentFrame;
        }
        /// <summary>Checks if the given gamepad button is unique for this frame.</summary>
        public static bool IsUnique(GamePadButton button, int gamePadIndex) => !Tracker.ContainsKey((button, gamePadIndex)) || Tracker[(button, gamePadIndex)] != InputHelper.CurrentFrame;

        private GamePadButton _button;
        private int _gamePadIndex;

        /// <summary>Tracks buttons being used each frames.</summary>
        protected static Dictionary<(GamePadButton, int), uint> Tracker = new Dictionary<(GamePadButton, int), uint>();
    }
}
