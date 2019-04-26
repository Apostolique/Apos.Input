using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Stores a key with the character it produces.
    /// </summary>
    public struct KeyCharacter {

        // Group: Constructors

        /// <param name="key">The key that was pressed on the keyboard.</param>
        /// <param name="character">A char that can be used in a string or textbox.</param>
        public KeyCharacter(Keys key, char character) {
            Key = key;
            Character = character;
        }

        // Group: Public Variables

        /// <value>The key that was pressed on the keyboard.</value>
        public Keys Key {
            get;
            set;
        }
        /// <value>A char that can be used in a string or textbox.</value>
        public char Character {
            get;
            set;
        }
    }
}