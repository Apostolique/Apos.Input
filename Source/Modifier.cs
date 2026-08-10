namespace Apos.Input {
    /// <summary>
    /// Wraps a condition to act as a modifier. Always returns false for Pressed and Released.
    /// </summary>
    public class Modifier : ICondition {
        /// <param name="condition">The condition to wrap.</param>
        public Modifier(ICondition condition) {
            _condition = condition;
        }

        /// <returns>Always returns false.</returns>
        public bool Pressed(bool canConsume = true) {
            return false;
        }
        /// <returns>Returns true when the wrapped condition is held.</returns>
        public bool Held(bool canConsume = true) {
            return _condition.Held(canConsume);
        }
        /// <returns>Returns true when the wrapped condition was held and is now held.</returns>
        public bool HeldOnly(bool canConsume = true) {
            return _condition.HeldOnly(canConsume);
        }
        /// <returns>Always returns false.</returns>
        public bool Released(bool canConsume = true) {
            return false;
        }
        /// <summary>Marks the wrapped condition as used.</summary>
        public void Consume() {
            _condition.Consume();
        }

        private ICondition _condition;
    }
}
