namespace Apos.Input {
    /// <summary>
    /// Interface for a condition.
    /// </summary>
    public interface ICondition {
        /// <returns>Returns true when a condition was false and is now true.</returns>
        bool Pressed(bool canConsume = true);
        /// <returns>Returns true when a condition is now true.</returns>
        bool Held(bool canConsume = true);
        /// <returns>Returns true when a condition was true and is now true.</returns>
        bool HeldOnly(bool canConsume = true);
        /// <returns>Returns true when a condition was true and is now false.</returns>
        bool Released(bool canConsume = true);
        /// <summary>Marks a tracked condition as used for this frame so that it doesn't get triggered again.</summary>
        void Consume();
    }
}
