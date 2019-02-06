using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Apos.Input {
    /// <summary>
    /// Goal: Combines ActionGamePad to make more complex action triggers.
    /// </summary>
    public class ActionGamePadSet {
        // Group: Constructors
        public ActionGamePadSet() {
            _needAction = new List<ActionGamePad>();
            _notAction = new List<ActionGamePad>();
        }
        public ActionGamePadSet(List<ActionGamePad> needAction, List<ActionGamePad> notAction) {
            _needAction = needAction;
            _notAction = notAction;
        }

        // Group: Public Functions
        public ActionGamePadSet AddNeed(Func<GamePadState[], ButtonState> button) {
            return AddNeed(new ActionGamePad(button));
        }
        public ActionGamePadSet AddNeed(ActionGamePad action) {
            _needAction.Add(action);
            return this;
        }
        public ActionGamePadSet AddNot(Func<GamePadState[], ButtonState> button) {
            return AddNot(new ActionGamePad(button));
        }
        public ActionGamePadSet AddNot(ActionGamePad action) {
            _notAction.Add(action);
            return this;
        }
        public bool Pressed() {
            bool pressed = false;
            bool holding = true;
            bool notHolding = false;

            foreach (ActionGamePad ag in _needAction) {
                pressed = pressed || ag.Pressed();
                if (pressed) {
                    break;
                }
            }
            foreach (ActionGamePad ag in _needAction) {
                holding = holding && ag.Holding();
                if (!holding) {
                    break;
                }
            }
            foreach (ActionGamePad ag in _notAction) {
                notHolding = notHolding || ag.Holding();
                if (notHolding) {
                    break;
                }
            }

            return pressed && holding && !notHolding;
        }
        public bool Holding() {
            bool holding = true;
            bool notHolding = false;

            foreach (ActionGamePad ag in _needAction) {
                holding = holding && ag.Holding();
                if (!holding) {
                    break;
                }
            }
            foreach (ActionGamePad ag in _notAction) {
                notHolding = notHolding || ag.Holding();
                if (notHolding) {
                    break;
                }
            }

            return holding && !notHolding;
        }
        public bool HoldingOnly() {
            bool holding = true;
            bool notHolding = false;

            foreach (ActionGamePad ag in _needAction) {
                holding = holding && ag.HoldingOnly();
                if (!holding) {
                    break;
                }
            }
            foreach (ActionGamePad ag in _notAction) {
                notHolding = notHolding || ag.HoldingOnly();
                if (notHolding) {
                    break;
                }
            }

            return holding && !notHolding;
        }
        public bool Released() {
            bool released = false;
            bool holding = true;
            bool notHolding = false;

            ActionGamePad releasedMouse = null;

            foreach (ActionGamePad ag in _needAction) {
                released = released || ag.Released();
                if (released) {
                    releasedMouse = ag;
                    break;
                }
            }
            foreach (ActionGamePad ag in _needAction) {
                if (ag == releasedMouse) {
                    continue;
                }
                holding = holding && ag.Holding();
                if (!holding) {
                    break;
                }
            }
            foreach (ActionGamePad ag in _notAction) {
                notHolding = notHolding || ag.Holding();
                if (notHolding) {
                    break;
                }
            }

            return released && holding && !notHolding;
        }

        // Group: Private Variables
        private List<ActionGamePad> _needAction;
        private List<ActionGamePad> _notAction;
    }
}