using System;

namespace Apos.Input {
    /// <summary>
    /// Triggers once another condition has been held long enough. It wraps an ICondition rather
    /// than a key, so a long press reads the same on a keyboard, a mouse, a gamepad, or a touch
    /// screen, and it works on a combination too.
    /// </summary>
    /// <remarks>
    /// This is the first thing in the library that reads InputHelper.TotalMS, so the clock has to
    /// be moving for it to do anything.
    /// The hold is measured from the frame the wrapped condition started being held. Skipping a
    /// frame restarts it, since a condition that doesn't get polled can't say how long it's been
    /// down. Check it every frame for as long as you care about the answer.
    /// </remarks>
    public class HoldCondition : ICondition {

        /// <param name="condition">The condition that has to stay held.</param>
        /// <param name="durationMS">How long it has to be held before this triggers.</param>
        public HoldCondition(ICondition condition, double durationMS) {
            _condition = condition;
            _durationMS = durationMS;
        }

        /// <returns>Returns true on the frame the hold reaches its duration.</returns>
        public bool Pressed(bool canConsume = true) {
            Sync();
            return Report(_isDown && !_wasDown, canConsume);
        }
        /// <returns>Returns true while the hold is at or past its duration.</returns>
        public bool Held(bool canConsume = true) {
            Sync();
            return Report(_isDown, canConsume);
        }
        /// <returns>Returns true while the hold is past its duration and already was last frame.</returns>
        public bool HeldOnly(bool canConsume = true) {
            Sync();
            return Report(_isDown && _wasDown, canConsume);
        }
        /// <returns>Returns true on the frame a finished hold gets let go.</returns>
        public bool Released(bool canConsume = true) {
            Sync();
            return Report(!_isDown && _wasDown, canConsume);
        }
        /// <summary>Mark the wrapped condition as used.</summary>
        public void Consume() {
            _condition.Consume();
        }

        /// <summary>
        /// How far along the hold is, from 0 to 1. It reaches 1 on the same frame Pressed
        /// triggers, so a radial menu or a charge bar can draw the wait.
        /// </summary>
        public float Progress {
            get {
                Sync();
                if (!_wasHeld) {
                    return 0f;
                }
                if (_durationMS <= 0) {
                    return 1f;
                }
                return Math.Clamp((float)((InputHelper.TotalMS - _startMS) / _durationMS), 0f, 1f);
            }
        }

        private bool Report(bool state, bool canConsume) {
            if (state && canConsume) {
                _condition.Consume();
            }
            return state;
        }
        /// <summary>
        /// The four checks all read the same state, so it gets worked out once per frame no matter
        /// how many of them get called. A gap in the frame numbers means this condition went
        /// unpolled, so there's no hold left to measure and it starts over.
        /// </summary>
        private void Sync() {
            if (_frame == InputHelper.CurrentFrame) {
                return;
            }
            bool contiguous = _frame == InputHelper.CurrentFrame - 1;
            _frame = InputHelper.CurrentFrame;

            _wasDown = contiguous && _isDown;

            if (_condition.Held(false)) {
                if (!contiguous || !_wasHeld) {
                    _startMS = InputHelper.TotalMS;
                }
                _wasHeld = true;
                _isDown = InputHelper.TotalMS - _startMS >= _durationMS;
            } else {
                _wasHeld = false;
                _isDown = false;
            }
        }

        /// <summary>The condition that has to stay held.</summary>
        private ICondition _condition;
        /// <summary>How long it has to be held, in milliseconds.</summary>
        private double _durationMS;
        /// <summary>When the current hold started, from InputHelper.TotalMS.</summary>
        private double _startMS;
        /// <summary>Whether the wrapped condition was held on the last polled frame.</summary>
        private bool _wasHeld;
        /// <summary>Whether the hold has reached its duration.</summary>
        private bool _isDown;
        /// <summary>Whether the hold had reached its duration on the last polled frame.</summary>
        private bool _wasDown;
        /// <summary>The frame the state was last worked out on.</summary>
        private uint _frame = uint.MaxValue;
    }
}
