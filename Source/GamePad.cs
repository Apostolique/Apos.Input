namespace Apos.Input {
    /// <summary>Gamepad buttons.</summary>
    public enum GamePadButton {
        /// <summary>A button.</summary>
        A,
        /// <summary>B button.</summary>
        B,
        /// <summary>Back button.</summary>
        Back,
        /// <summary>X button.</summary>
        X,
        /// <summary>Y button.</summary>
        Y,
        /// <summary>Start button.</summary>
        Start,
        /// <summary>LeftShould button.</summary>
        LeftShoulder,
        /// <summary>LeftStick button.</summary>
        LeftStick,
        /// <summary>RightShould button.</summary>
        RightShoulder,
        /// <summary>RightStick button.</summary>
        RightStick,
        /// <summary>BigButton button.</summary>
        BigButton,
        /// <summary>DPad down button.</summary>
        Down,
        /// <summary>DPad left button.</summary>
        Left,
        /// <summary>DPad right button.</summary>
        Right,
        /// <summary>DPad up button.</summary>
        Up,
        /// <summary>Left thumbstick pushed left. See <see cref="InputHelper.ThumbstickThreshold"/>.</summary>
        LeftThumbstickLeft,
        /// <summary>Left thumbstick pushed right.</summary>
        LeftThumbstickRight,
        /// <summary>Left thumbstick pushed up.</summary>
        LeftThumbstickUp,
        /// <summary>Left thumbstick pushed down.</summary>
        LeftThumbstickDown,
        /// <summary>Right thumbstick pushed left.</summary>
        RightThumbstickLeft,
        /// <summary>Right thumbstick pushed right.</summary>
        RightThumbstickRight,
        /// <summary>Right thumbstick pushed up.</summary>
        RightThumbstickUp,
        /// <summary>Right thumbstick pushed down.</summary>
        RightThumbstickDown
    }
    /// <summary>Gamepad sensors.</summary>
    public enum GamePadSensor {
        /// <summary>Left analog trigger.</summary>
        LeftTrigger,
        /// <summary>Right analog trigger.</summary>
        RightTrigger,
        /// <summary>Left analog thumbstick.</summary>
        LeftStick,
        /// <summary>Right analog thumbstick.</summary>
        RightStick
    }
}
