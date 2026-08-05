using Microsoft.Xna.Framework.Input.Touch;

namespace Apos.Input {
    /// <summary>
    /// Checks various conditions on touch screen contacts.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    /// <remarks>
    /// A contact has no fixed identity the way a mouse button does, so this condition doesn't take
    /// one. The instance triggers on any contact, which is what composing with AnyCondition wants.
    /// Anything positional is answered by passing a TouchLocation to the static methods, since
    /// where a contact lands is a question about the game rather than about the device.
    /// </remarks>
    public class TouchCondition : ICondition {

        /// <summary>Triggers on any contact.</summary>
        public TouchCondition() { }

        /// <returns>Returns true when a contact started this frame.</returns>
        public bool Pressed(bool canConsume = true) {
            return InputHelper.IsActive && Pressed();
        }
        /// <returns>Returns true when a contact is down.</returns>
        public bool Held(bool canConsume = true) {
            return InputHelper.IsActive && Held();
        }
        /// <returns>Returns true when a contact was down and is still down.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return InputHelper.IsActive && HeldOnly();
        }
        /// <returns>Returns true when a contact ended this frame.</returns>
        public bool Released(bool canConsume = true) {
            return InputHelper.IsActive && Released();
        }
        /// <summary>Does nothing since this condition isn't tracked.</summary>
        public void Consume() { }

        /// <returns>Returns true when the contact started this frame.</returns>
        public static bool Pressed(TouchLocation touch) {
            return touch.State == TouchLocationState.Pressed;
        }
        /// <returns>Returns true when the contact is down. Moved means still down, not moving.</returns>
        public static bool Held(TouchLocation touch) {
            return touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved;
        }
        /// <returns>Returns true when the contact was down and is still down.</returns>
        public static bool HeldOnly(TouchLocation touch) {
            return touch.State == TouchLocationState.Moved;
        }
        /// <returns>Returns true when the contact ended this frame.</returns>
        public static bool Released(TouchLocation touch) {
            return touch.State == TouchLocationState.Released;
        }

        /// <returns>Returns true when at least one contact started this frame.</returns>
        public static bool Pressed() {
            foreach (TouchLocation t in InputHelper.NewTouch) {
                if (Pressed(t)) {
                    return true;
                }
            }
            return false;
        }
        /// <returns>Returns true when at least one contact is down.</returns>
        public static bool Held() {
            foreach (TouchLocation t in InputHelper.NewTouch) {
                if (Held(t)) {
                    return true;
                }
            }
            return false;
        }
        /// <returns>Returns true when at least one contact was down and is still down.</returns>
        public static bool HeldOnly() {
            foreach (TouchLocation t in InputHelper.NewTouch) {
                if (HeldOnly(t)) {
                    return true;
                }
            }
            return false;
        }
        /// <returns>Returns true when at least one contact ended this frame.</returns>
        public static bool Released() {
            foreach (TouchLocation t in InputHelper.NewTouch) {
                if (Released(t)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The oldest contact that is currently down, which is the one Pointer follows.
        /// </summary>
        /// <returns>Returns false when nothing is touching the screen.</returns>
        public static bool TryGetPrimary(out TouchLocation touch) {
            bool found = false;
            touch = default;

            foreach (TouchLocation t in InputHelper.NewTouch) {
                if (Held(t) && (!found || t.Id < touch.Id)) {
                    touch = t;
                    found = true;
                }
            }
            return found;
        }
        /// <returns>Returns the number of contacts that are currently down.</returns>
        public static int Count => CountDown(InputHelper.NewTouch);
        /// <returns>Returns the number of contacts that were down last frame.</returns>
        public static int OldCount => CountDown(InputHelper.OldTouch);
        /// <returns>Returns true when the device has a touch panel that is reporting.</returns>
        public static bool IsTouchValid => InputHelper.IsActive && InputHelper.NewTouch.IsConnected;

        private static int CountDown(TouchCollection touches) {
            int count = 0;
            foreach (TouchLocation t in touches) {
                if (Held(t)) {
                    count++;
                }
            }
            return count;
        }
    }
}
