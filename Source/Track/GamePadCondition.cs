using System.Collections.Generic;

namespace Apos.Input.Track {
    /// <summary>
    /// Checks various conditions on a specific gamepad button.
    /// Non static methods only count the button as down while the game is active. Losing focus
    /// reports a release, and coming back with the button still down reports a press.
    /// </summary>
    public class GamePadCondition : ICondition {

        /// <param name="button">The button to operate on.</param>
        /// <param name="gamePadIndex">The index of the gamepad to operate on.</param>
        public GamePadCondition(GamePadButton button, int gamePadIndex) {
            _button = button;
            _gamePadIndex = gamePadIndex;
            _condition = new Input.GamePadCondition(button, gamePadIndex);
        }

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            return Check(_condition.Pressed(), canConsume);
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return Check(_condition.Held(), canConsume);
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return Check(_condition.HeldOnly(), canConsume);
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return Check(_condition.Released(), canConsume);
        }
        /// <summary>Mark the condition as used.</summary>
        public void Consume() {
            ButtonTracker[(_button, _gamePadIndex)] = InputHelper.CurrentFrame;
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
            ButtonTracker[(button, gamePadIndex)] = InputHelper.CurrentFrame;
        }
        /// <summary>Checks if the given gamepad button is unique for this frame.</summary>
        public static bool IsUnique(GamePadButton button, int gamePadIndex) => !ButtonTracker.ContainsKey((button, gamePadIndex)) || ButtonTracker[(button, gamePadIndex)] != InputHelper.CurrentFrame;

        /// <summary>Mark the gamepad sensor as used for this frame.</summary>
        public static void Consume(GamePadSensor sensor, int gamePadIndex) {
            SensorTracker[(sensor, gamePadIndex)] = InputHelper.CurrentFrame;
        }
        /// <summary>Checks if the given gamepad sensor is unique for this frame.</summary>
        public static bool IsUnique(GamePadSensor sensor, int gamePadIndex) => !SensorTracker.ContainsKey((sensor, gamePadIndex)) || SensorTracker[(sensor, gamePadIndex)] != InputHelper.CurrentFrame;

        /// <summary>
        /// The untracked condition answers whether the button is in that state, tracking only
        /// decides whether this instance is the one that gets to see it.
        /// </summary>
        private bool Check(bool state, bool canConsume) {
            if (IsUnique(_button, _gamePadIndex) && state) {
                if (canConsume)
                    Consume(_button, _gamePadIndex);
                return true;
            }
            return false;
        }

        private GamePadButton _button;
        private int _gamePadIndex;
        private Input.GamePadCondition _condition;

        /// <summary>Tracks gamepad buttons being used each frames.</summary>
        protected static Dictionary<(GamePadButton, int), uint> ButtonTracker = new Dictionary<(GamePadButton, int), uint>();
        /// <summary>Tracks gamepad sensors being used each frames.</summary>
        protected static Dictionary<(GamePadSensor, int), uint> SensorTracker = new Dictionary<(GamePadSensor, int), uint>();
    }
}
