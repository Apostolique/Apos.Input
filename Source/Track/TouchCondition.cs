using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input.Touch;

namespace Apos.Input.Track {
    /// <summary>
    /// Checks various conditions on touch screen contacts.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    /// <remarks>
    /// Tracking is per contact, so a panel consuming one finger leaves every other finger free for
    /// whatever is underneath. A single flag saying the frame was consumed can't do that, which is
    /// what makes this worth using once more than one finger is in play.
    /// It's also what decides when a gesture ends. A contact belongs to whoever claimed it, and it
    /// keeps belonging to them until it lifts or something else takes it, so a stray finger landing
    /// somewhere unrelated doesn't interrupt anything.
    /// </remarks>
    public class TouchCondition : ICondition {

        /// <summary>Triggers on any contact that hasn't been used yet this frame.</summary>
        public TouchCondition() { }
        /// <summary>
        /// Claims this many contacts and holds on to them. The gesture ends when one of them lifts
        /// or when a condition checked earlier this frame takes one, and not before.
        /// </summary>
        /// <param name="count">How many contacts the gesture needs.</param>
        /// <remarks>
        /// Extra contacts are left alone, so two fingers claimed by a camera still leaves a third
        /// for whatever wants it. Check the greedier gesture first, since the one that claims first
        /// wins.
        /// A claim needs one contact that just landed, and the rest can already be down. Lifting a
        /// finger off a two finger gesture therefore doesn't hand the leftover one to a one finger
        /// gesture, and putting a second finger back down starts the two finger one again.
        /// </remarks>
        public TouchCondition(int count) {
            _count = count;
        }

        /// <returns>Returns true when a contact started this frame.</returns>
        public bool Pressed(bool canConsume = true) {
            if (_count == AnyCount) {
                return Any(Input.TouchCondition.Pressed, canConsume);
            }
            SyncClaim();
            return Report(_isDown && !_wasDown, canConsume);
        }
        /// <returns>Returns true when a contact is down.</returns>
        public bool Held(bool canConsume = true) {
            if (_count == AnyCount) {
                return Any(Input.TouchCondition.Held, canConsume);
            }
            SyncClaim();
            return Report(_isDown, canConsume);
        }
        /// <returns>Returns true when a contact was down and is still down.</returns>
        public bool HeldOnly(bool canConsume = true) {
            if (_count == AnyCount) {
                return Any(Input.TouchCondition.HeldOnly, canConsume);
            }
            SyncClaim();
            return Report(_isDown && _wasDown, canConsume);
        }
        /// <returns>Returns true when a contact ended this frame.</returns>
        public bool Released(bool canConsume = true) {
            if (_count == AnyCount) {
                return Any(Input.TouchCondition.Released, canConsume);
            }
            SyncClaim();
            return Report(!_isDown && _wasDown, canConsume);
        }
        /// <summary>Mark the contacts this condition is using as used.</summary>
        public void Consume() {
            if (_count == AnyCount) {
                if (_matchedId != NoContact) {
                    Consume(_matchedId);
                }
                return;
            }
            foreach (int id in _owned) {
                Consume(id);
            }
        }

        /// <summary>The contacts this condition currently holds. Empty while it isn't triggered.</summary>
        public IReadOnlyList<int> Owned => _owned;

        /// <returns>Returns true when the contact started this frame.</returns>
        public static bool Pressed(TouchLocation touch, bool canConsume = true) {
            return Check(touch, Input.TouchCondition.Pressed, canConsume);
        }
        /// <returns>Returns true when the contact is down.</returns>
        public static bool Held(TouchLocation touch, bool canConsume = true) {
            return Check(touch, Input.TouchCondition.Held, canConsume);
        }
        /// <returns>Returns true when the contact was down and is still down.</returns>
        public static bool HeldOnly(TouchLocation touch, bool canConsume = true) {
            return Check(touch, Input.TouchCondition.HeldOnly, canConsume);
        }
        /// <returns>Returns true when the contact ended this frame.</returns>
        public static bool Released(TouchLocation touch, bool canConsume = true) {
            return Check(touch, Input.TouchCondition.Released, canConsume);
        }

        /// <summary>Mark the contact as used for this frame.</summary>
        public static void Consume(int touchId) {
            Sync();
            Tracker.Add(touchId);
        }
        /// <summary>Checks if the given contact is unique for this frame.</summary>
        public static bool IsUnique(int touchId) {
            Sync();
            return !Tracker.Contains(touchId);
        }

        private static bool Check(TouchLocation touch, Func<TouchLocation, bool> state, bool canConsume) {
            if (IsUnique(touch.Id) && state(touch)) {
                if (canConsume) {
                    Consume(touch.Id);
                }
                return true;
            }
            return false;
        }
        private bool Any(Func<TouchLocation, bool> state, bool canConsume) {
            _matchedId = NoContact;
            if (!InputHelper.IsActive) return false;

            foreach (TouchLocation t in InputHelper.NewTouch) {
                if (state(t) && IsUnique(t.Id)) {
                    _matchedId = t.Id;
                    if (canConsume) {
                        Consume(t.Id);
                    }
                    return true;
                }
            }
            return false;
        }
        private bool Report(bool state, bool canConsume) {
            if (state && canConsume) {
                Consume();
            }
            return state;
        }
        /// <summary>
        /// Worked out once per frame no matter how many of the four checks get called. A gap in the
        /// frame numbers means this condition went unpolled, so it can't claim to still be holding
        /// anything and starts over.
        /// </summary>
        private void SyncClaim() {
            if (_frame == InputHelper.CurrentFrame) {
                return;
            }
            bool contiguous = _frame == InputHelper.CurrentFrame - 1;
            _frame = InputHelper.CurrentFrame;

            _wasDown = contiguous && _isDown;

            if (!InputHelper.IsActive) {
                _owned.Clear();
                _isDown = false;
                return;
            }

            if (_wasDown && StillOwns()) {
                _isDown = true;
                return;
            }

            // Losing a contact ends the gesture on this frame. Claiming a fresh set waits for the
            // next one, so there's always a frame where Released is true.
            _owned.Clear();
            _isDown = !_wasDown && TryClaim();
        }
        /// <summary>Whether every contact this condition claimed is still down and still its own.</summary>
        private bool StillOwns() {
            foreach (int id in _owned) {
                if (!InputHelper.NewTouch.FindById(id, out TouchLocation t) ||
                    !Input.TouchCondition.Held(t) ||
                    !IsUnique(id)) {
                    return false;
                }
            }
            return _owned.Count == _count;
        }
        /// <summary>
        /// Takes free contacts and leaves any extras alone, but only if one of them just landed.
        /// One fresh contact plus whatever was already down, the same shape as a modifier being
        /// held while another key gets pressed.
        /// Without that, lifting one finger off a two finger gesture would hand the leftover
        /// finger straight to a one finger gesture, which is a stroke nobody started.
        /// </summary>
        private bool TryClaim() {
            _free.Clear();
            foreach (TouchLocation t in InputHelper.NewTouch) {
                if (Input.TouchCondition.Held(t) && IsUnique(t.Id)) {
                    _free.Add(t);
                }
            }
            if (_free.Count < _count) {
                return false;
            }

            int landed = -1;
            for (int i = 0; i < _free.Count; i++) {
                if (Input.TouchCondition.Pressed(_free[i])) {
                    landed = i;
                    break;
                }
            }
            if (landed == -1) {
                return false;
            }

            _owned.Add(_free[landed].Id);
            for (int i = 0; i < _free.Count && _owned.Count < _count; i++) {
                if (i != landed) {
                    _owned.Add(_free[i].Id);
                }
            }
            return true;
        }
        /// <summary>
        /// Touch ids climb forever, unlike keys and buttons, so a tracker that only ever grows
        /// would leak across a long session. Nothing older than this frame matters, so the whole
        /// set is dropped the first time it's touched on a new frame.
        /// </summary>
        private static void Sync() {
            if (_trackerFrame != InputHelper.CurrentFrame) {
                Tracker.Clear();
                _trackerFrame = InputHelper.CurrentFrame;
            }
        }

        private const int NoContact = -1;
        /// <summary>Any single contact, which is what the empty constructor means.</summary>
        private const int AnyCount = -1;

        private int _matchedId = NoContact;
        private int _count = AnyCount;
        private List<int> _owned = new List<int>();
        private List<TouchLocation> _free = new List<TouchLocation>();
        private bool _isDown;
        private bool _wasDown;
        private uint _frame = uint.MaxValue;

        /// <summary>Tracks contacts being used this frame.</summary>
        protected static HashSet<int> Tracker = new HashSet<int>();
        private static uint _trackerFrame = uint.MaxValue;
    }
}
