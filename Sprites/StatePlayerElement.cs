using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PrinceGame
{
    public class StatePlayerElement : StateElement
    {

        private Enumeration.State _state;

        public Enumeration.State state
        {
            get { return _state; }
            set { _state = value; }
        }

        public StatePlayerElement()
        {
            _state = Enumeration.State.none;
            Priority = Enumeration.PriorityState.Normal;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority__1)
        {
            _state = state;
            Priority = priority__1;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority__1, Vector2 offSet__2)
        {
            _state = state;
            Priority = priority__1;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = offSet__2;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority__1, System.Nullable<bool> stoppable__2)
        {
            _state = state;
            Priority = priority__1;
            Stoppable = stoppable__2;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority__1, Enumeration.SequenceReverse reverse__2)
        {
            _state = state;
            Priority = priority__1;
            Reverse = reverse__2;
            OffSet = Vector2.Zero;
        }


        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority__1, System.Nullable<bool> stoppable__2, Enumeration.SequenceReverse reverse__3)
        {
            _state = state;
            Priority = priority__1;
            Stoppable = stoppable__2;
            Reverse = reverse__3;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority__1, System.Nullable<bool> stoppable__2, Enumeration.SequenceReverse reverse__3, Vector2 offSet__4)
        {
            _state = state;
            Priority = priority__1;
            Stoppable = stoppable__2;
            Reverse = reverse__3;
            OffSet = offSet__4;
        }

        public StatePlayerElement(Enumeration.State state)
        {
            _state = state;
            Priority = Enumeration.PriorityState.Normal;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
            IfTrue = false;
        }

        public StatePlayerElement(Enumeration.State state, bool iftrue__1)
        {
            _state = state;
            Priority = Enumeration.PriorityState.Normal;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
            IfTrue = iftrue__1;
        }


        public static new Enumeration.State Parse(string name)
        {
            return (Enumeration.State)Enum.Parse(typeof(Enumeration.State), name.ToLower());
        }



    }
}
