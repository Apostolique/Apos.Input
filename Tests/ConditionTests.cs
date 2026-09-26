using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Xunit;

// InputHelper is static so tests can't share the process concurrently.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Apos.Input.Tests {
    public class ConditionTests {
        public ConditionTests() {
            InputHelper.Setup(_input);
        }

        [Fact]
        public void PressedHeldAndReleasedFollowTheKey() {
            var jump = new KeyboardCondition(Keys.Space);

            Frame(Keys.Space);
            Assert.True(jump.Pressed());
            Assert.True(jump.Held());
            Assert.False(jump.HeldOnly());

            Frame(Keys.Space);
            Assert.False(jump.Pressed());
            Assert.True(jump.HeldOnly());

            Frame();
            Assert.True(jump.Released());
            Assert.False(jump.Held());
        }

        [Fact]
        public void KeysDontCountWhileTheGameIsInactive() {
            _input.IsActive = false;
            Frame(Keys.Space);

            Assert.False(new KeyboardCondition(Keys.Space).Held());
        }

        [Fact]
        public void ConsumingHidesTheKeyFromTheNextTrackedCondition() {
            var first = new Track.KeyboardCondition(Keys.Enter);
            var second = new Track.KeyboardCondition(Keys.Enter);

            Frame(Keys.Enter);

            Assert.True(first.Pressed());
            Assert.False(second.Pressed());
        }

        [Fact]
        public void RepeatTriggersAfterTheDelayThenAtEachInterval() {
            var repeat = new RepeatCondition(new Track.KeyboardCondition(Keys.Right), 400, 50);
            var triggers = 0;

            // 30 frames at 20 ms is 600 ms: the first press, then 400, 450, 500, 550 and 600.
            for (int i = 0; i <= 30; i++) {
                Frame(Keys.Right);
                if (repeat.Pressed()) triggers++;
            }

            Assert.Equal(6, triggers);
        }

        [Fact]
        public void RepeatDoesntTriggerAfterSomethingElseConsumedIt() {
            var key = new Track.KeyboardCondition(Keys.Right);
            var repeat = new RepeatCondition(key, 400, 50);

            Frame(Keys.Right);
            Assert.True(repeat.Pressed());
            Assert.False(repeat.Pressed());
        }

        [Fact]
        public void HoldTriggersOnceItWasHeldLongEnough() {
            var hold = new HoldCondition(new KeyboardCondition(Keys.E), 100);

            // It has to be polled every frame, a gap starts the hold over. At 20 ms per frame, 100 ms is the 6th frame.
            for (int i = 0; i < 5; i++) {
                Frame(Keys.E);
                Assert.False(hold.Pressed());
            }
            Frame(Keys.E);
            Assert.True(hold.Pressed());
            Assert.Equal(1f, hold.Progress);
        }

        [Fact]
        public void TypedTextShowsUpForOneFrame() {
            _input.Type("hi");
            Frame();
            Assert.Equal("hi", string.Concat(InputHelper.TextEvents.ConvertAll(e => e.Character)));

            InputHelper.UpdateCleanup();
            Frame();
            Assert.Empty(InputHelper.TextEvents);
        }

        [Fact]
        public void PointerFollowsTouchThenGoesBackToTheMouse() {
            _input.Mouse = Mouse(10, 10);
            _input.Touch = new TouchCollection([new TouchLocation(1, TouchLocationState.Pressed, new Vector2(200, 100))]);
            Frame();
            Assert.Equal(PointerSource.Touch, Pointer.Source);
            Assert.Equal(new Vector2(200, 100), Pointer.Position);

            _input.Touch = new TouchCollection([]);
            _input.Mouse = Mouse(20, 30);
            Frame();
            Frame();
            Assert.Equal(PointerSource.Mouse, Pointer.Source);
            Assert.Equal(new Vector2(20, 30), Pointer.Position);
        }

        private static MouseState Mouse(int x, int y) =>
            new(x, y, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

        private void Frame(params Keys[] keys) {
            _input.Keyboard = new KeyboardState(keys);
            _time += TimeSpan.FromMilliseconds(20);
            InputHelper.UpdateSetup(new GameTime(_time, TimeSpan.FromMilliseconds(20)));
        }

        private readonly SimulatedInput _input = new();
        private TimeSpan _time = TimeSpan.Zero;
    }
}
