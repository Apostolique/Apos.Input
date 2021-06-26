using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Apos.Input.Track {
    /// <summary>
    /// Checks various conditions on a specific mouse button.
    /// Non static methods implicitly make sure that the game is active.
    /// Pressed also makes sure the mouse is inside the window. Otherwise returns false.
    /// </summary>
    public class MouseCondition : ICondition {

        /// <param name="button">The button to operate on.</param>
        public MouseCondition(MouseButton button) {
            _button = button;
        }

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed(bool canConsume = true) {
            return Input.MouseCondition.IsMouseValid && (Pressed(_button, canConsume) || !InputHelper.OldIsActive && Held(_button, canConsume));
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Held(bool canConsume = true) {
            return InputHelper.IsActive && Held(_button, canConsume);
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return InputHelper.IsActive && InputHelper.OldIsActive && HeldOnly(_button, canConsume);
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released(bool canConsume = true) {
            return InputHelper.IsActive && Released(_button, canConsume);
        }
        /// <summary>Mark the condition as used.</summary>
        public void Consume() {
            Consume(_button);
        }

        /// <returns>Returns true when the mouse button was released and is now pressed.</returns>
        public static bool Pressed(MouseButton button, bool canConsume = true) {
            if (IsUnique(button) && Input.MouseCondition.Pressed(button)) {
                if (canConsume)
                    Consume(button);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the mouse button is now pressed.</returns>
        public static bool Held(MouseButton button, bool canConsume = true) {
            if (IsUnique(button) && Input.MouseCondition.Held(button)) {
                if (canConsume)
                    Consume(button);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the mouse button was pressed and is now pressed.</returns>
        public static bool HeldOnly(MouseButton button, bool canConsume = true) {
            if (IsUnique(button) && Input.MouseCondition.HeldOnly(button)) {
                if (canConsume)
                    Consume(button);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the mouse button was pressed and is now released.</returns>
        public static bool Released(MouseButton button, bool canConsume = true) {
            if (IsUnique(button) && Input.MouseCondition.Released(button)) {
                if (canConsume)
                    Consume(button);
                return true;
            }
            return false;
        }
        /// <returns>Returns true when the scroll wheel is scrolled.</returns>
        public static bool Scrolled(bool canConsume = true) {
            if (IsUnique(MouseSensor.ScrollWheel) && Apos.Input.MouseCondition.Scrolled()) {
                if (canConsume)
                    Consume(MouseSensor.ScrollWheel);
                return true;
            }
            return false;
        }
        /// <returns>Returns the difference between last frame and this frame's scroll wheel value.</returns>
        public static int ScrollDelta => Apos.Input.MouseCondition.ScrollDelta;

        /// <returns>Returns true when the mouse pointer is moved.</returns>
        public static bool PointerMoved(bool canConsume = true) {
            if (IsUnique(MouseSensor.Pointer) && Apos.Input.MouseCondition.PointerMoved()) {
                if (canConsume)
                    Consume(MouseSensor.Pointer);
                return true;
            }
            return false;
        }
        ///<returns>Returns the difference between the last frame and this frame's mouse pointer position.</returns>
        public static Point PointerDelta => Apos.Input.MouseCondition.PointerDelta;

        /// <summary>Mark the mouse button as used for this frame.</summary>
        public static void Consume(MouseButton button) {
            ButtonTracker[button] = InputHelper.CurrentFrame;
        }
        /// <summary>Checks if the given mouse button is unique for this frame.</summary>
        public static bool IsUnique(MouseButton button) => !ButtonTracker.ContainsKey(button) || ButtonTracker[button] != InputHelper.CurrentFrame;

        /// <summary>Mark the mouse sensor as used for this frame.</summary>
        public static void Consume(MouseSensor sensor) {
            SensorTracker[sensor] = InputHelper.CurrentFrame;
        }
        /// <summary>Checks if the given mouse sensor is unique for this frame.</summary>
        public static bool IsUnique(MouseSensor sensor) => !SensorTracker.ContainsKey(sensor) || SensorTracker[sensor] != InputHelper.CurrentFrame;

        private MouseButton _button;

        /// <summary>Tracks mouse buttons being used each frames.</summary>
        protected static Dictionary<MouseButton, uint> ButtonTracker = new Dictionary<MouseButton, uint>();
        /// <summary>Tracks mouse sensors being used each frames.</summary>
        protected static Dictionary<MouseSensor, uint> SensorTracker = new Dictionary<MouseSensor, uint>();
    }
}
