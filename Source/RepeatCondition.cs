namespace Apos.Input {
    /// <summary>
    /// Repeats another condition the way a key repeats while you hold it in a text field. It
    /// triggers once right away, waits out the delay, then keeps triggering on the interval.
    /// </summary>
    /// <remarks>
    /// Pressed is the one that repeats, and it triggers many times before a single Released. That
    /// breaks the usual pairing on purpose, it's what a repeat is. Held, HeldOnly, and Released
    /// come straight from the wrapped condition.
    /// This reads InputHelper.TotalMS. Skipping a frame starts the repeat over, the same way
    /// HoldCondition does.
    /// </remarks>
    /// <see cref="HoldCondition"/>
    public class RepeatCondition : ICondition {

        /// <param name="condition">The condition to repeat.</param>
        /// <param name="delayMS">How long to wait after the first trigger before repeating.</param>
        /// <param name="intervalMS">How long between repeats once they start.</param>
        public RepeatCondition(ICondition condition, double delayMS, double intervalMS) {
            _condition = condition;
            _delayMS = delayMS;
            _intervalMS = intervalMS;
        }

        /// <returns>Returns true on the first frame it's held, then again on every repeat.</returns>
        public bool Pressed(bool canConsume = true) {
            Sync();
            if (_triggered && canConsume) {
                _condition.Consume();
            }
            return _triggered;
        }
        /// <returns>Returns true while the wrapped condition is held.</returns>
        public bool Held(bool canConsume = true) {
            return _condition.Held(canConsume);
        }
        /// <returns>Returns true while the wrapped condition was held and is still held.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return _condition.HeldOnly(canConsume);
        }
        /// <returns>Returns true when the wrapped condition gets let go.</returns>
        public bool Released(bool canConsume = true) {
            return _condition.Released(canConsume);
        }
        /// <summary>Mark the wrapped condition as used.</summary>
        public void Consume() {
            _condition.Consume();
        }

        /// <summary>
        /// Worked out once per frame. A gap in the frame numbers means this condition went
        /// unpolled, so the repeat starts over rather than firing for time nobody was watching.
        /// </summary>
        private void Sync() {
            if (_frame == InputHelper.CurrentFrame) {
                return;
            }
            bool contiguous = _frame == InputHelper.CurrentFrame - 1;
            _frame = InputHelper.CurrentFrame;
            _triggered = false;

            if (!_condition.Held(false)) {
                _wasHeld = false;
                return;
            }

            if (!contiguous || !_wasHeld) {
                _wasHeld = true;
                _nextMS = InputHelper.TotalMS + _delayMS;
                _triggered = true;
                return;
            }

            if (InputHelper.TotalMS >= _nextMS) {
                _triggered = true;
                _nextMS += _intervalMS;

                // A hitch long enough to owe several repeats pays out one and moves on, so a stall
                // doesn't come back as a burst.
                if (_nextMS < InputHelper.TotalMS) {
                    _nextMS = InputHelper.TotalMS + _intervalMS;
                }
            }
        }

        /// <summary>The condition being repeated.</summary>
        private ICondition _condition;
        /// <summary>How long to wait after the first trigger, in milliseconds.</summary>
        private double _delayMS;
        /// <summary>How long between repeats, in milliseconds.</summary>
        private double _intervalMS;
        /// <summary>When the next repeat is due, from InputHelper.TotalMS.</summary>
        private double _nextMS;
        /// <summary>Whether the wrapped condition was held on the last polled frame.</summary>
        private bool _wasHeld;
        /// <summary>Whether a trigger landed on this frame.</summary>
        private bool _triggered;
        /// <summary>The frame the state was last worked out on.</summary>
        private uint _frame = uint.MaxValue;
    }
}
