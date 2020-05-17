namespace Apos.Input {
    /// <summary>
    /// Interface for a condition.
    /// </summary>
    public interface ICondition {
        /// <returns>Returns true when a condition was false and is now true.</returns>
        bool Pressed();
        /// <returns>Returns true when a condition is now true.</returns>
        bool Held();
        /// <returns>Returns true when a condition was true and is now true.</returns>
        bool HeldOnly();
        /// <returns>Returns true when a condition was true and is now false.</returns>
        bool Released();
    }
}