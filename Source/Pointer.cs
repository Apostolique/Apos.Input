using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Apos.Input {
    /// <summary>Where a pointer position came from.</summary>
    public enum PointerSource {
        /// <summary>The mouse.</summary>
        Mouse,
        /// <summary>A touch screen contact.</summary>
        Touch
    }

    /// <summary>
    /// One position for the mouse and the touch screen together, so game code doesn't branch on
    /// which one is in use. Follows the oldest contact while something is touching the screen,
    /// and the mouse otherwise.
    /// </summary>
    /// <remarks>
    /// AnyCondition already combines a MouseCondition and a TouchCondition into one boolean, so
    /// this covers the other half of the port.
    /// </remarks>
    public static class Pointer {
        /// <summary>The pointer's current position.</summary>
        public static Vector2 Position => _position;
        /// <summary>The pointer's previous position.</summary>
        public static Vector2 OldPosition => _oldPosition;
        /// <summary>The difference between the last frame and this frame's position.</summary>
        public static Vector2 Delta => _position - _oldPosition;
        /// <returns>Returns true when the pointer moved.</returns>
        public static bool Moved() => Delta != Vector2.Zero;
        /// <summary>
        /// Which device the position came from. Touch has no hover, so anything that only makes
        /// sense under a cursor can check for Mouse before drawing itself.
        /// </summary>
        public static PointerSource Source => _source;

        internal static void Setup() {
            _position = InputHelper.NewMouse.Position.ToVector2();
            _oldPosition = _position;
            _source = PointerSource.Mouse;
            _primaryId = NoContact;
        }

        internal static void Update() {
            _oldPosition = _position;

            bool wasTouch = _source == PointerSource.Touch;
            int oldPrimaryId = _primaryId;

            if (TouchCondition.TryGetPrimary(out TouchLocation primary)) {
                _position = primary.Position;
                _source = PointerSource.Touch;
                _primaryId = primary.Id;
            } else {
                _primaryId = NoContact;

                // Sticky on purpose. A finger coming off the screen shouldn't teleport the pointer
                // to wherever the mouse happens to be sitting, so the mouse has to move to take it
                // back.
                Vector2 mouse = InputHelper.NewMouse.Position.ToVector2();
                if (!wasTouch || mouse != InputHelper.OldMouse.Position.ToVector2()) {
                    _position = mouse;
                    _source = PointerSource.Mouse;
                }
            }

            // A new contact, or the pointer changing hands, would otherwise report the jump between
            // two unrelated positions as a delta and drag whatever is following it across the
            // screen.
            if (_primaryId != oldPrimaryId || wasTouch != (_source == PointerSource.Touch)) {
                _oldPosition = _position;
            }
        }

        private const int NoContact = -1;

        private static Vector2 _position;
        private static Vector2 _oldPosition;
        private static PointerSource _source;
        private static int _primaryId = NoContact;
    }
}
