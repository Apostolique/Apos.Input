namespace Apos.Input
{
    /// <summary>
    /// A condition that tracks multiple inputs along a single dimension.
    /// </summary>
    public class PairCondition : ICondition {
        readonly ICondition _negativeCondition;

        readonly ICondition _positiveCondition;

        /// <summary>
        /// Creates a paired condition from the two inputs.
        /// </summary>
        /// <param name="negative">The negative-associated input.</param>
        /// <param name="positive">The positive-associated input.</param>
        public PairCondition(ICondition negative, ICondition positive) {
            _negativeCondition = negative;
            _positiveCondition = positive;
        }
        
        /// <returns>Returns true when a condition was false and is now true.</returns>
        public bool Pressed(bool canConsume = true) {
            return _negativeCondition.Pressed(canConsume) || _positiveCondition.Pressed(canConsume);
        }

        /// <returns>Returns true when a condition is now true.</returns>
        public bool Held(bool canConsume = true) {
            return _negativeCondition.Held(canConsume) || _positiveCondition.Held(canConsume);
        }

        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return _negativeCondition.HeldOnly(canConsume) || _positiveCondition.HeldOnly(canConsume);
        }

        /// <returns>Returns true when a condition was true and is now false.</returns>
        public bool Released(bool canConsume = true) {
            return _negativeCondition.Released(canConsume) || _positiveCondition.Released(canConsume);
        }

        /// <summary>Does nothing since this condition isn't tracked.</summary>
        public void Consume() { }

        /// <summary>
        /// Gets the value of this condition.
        /// </summary>
        /// <returns>-1 if the negative input is held; 1 if the positive input is held; 0 otherwise.</returns>
        public float GetValue () {
            // TODO: How do we want to handle when both inputs are active?
            if (_negativeCondition.Held()) {
                return -1;
            }
            else if (_positiveCondition.Held()) {
                return 1;
            }
            else {
                return 0;
            }
        }
    }
}
