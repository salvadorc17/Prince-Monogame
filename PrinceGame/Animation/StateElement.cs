using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PrinceGame
{
    public abstract class StateElement
    {
        private Enumeration.State _state = Enumeration.State.none;
        private Enumeration.PriorityState _priorityState;
        private System.Nullable<bool> _stoppable;
        private Enumeration.SequenceReverse _reverse;
        private Vector2 _offset;
        private bool _ifTrue = false;

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null)
                {
                    _name = string.Empty;
                }
                else
                {
                    _name = value;
                }
            }
        }


        public bool IfTrue
        {
            get { return _ifTrue; }
            set { _ifTrue = value; }
        }

        public Vector2 OffSet
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public Enumeration.SequenceReverse Reverse
        {
            get { return _reverse; }
            set { _reverse = value; }
        }

        public System.Nullable<bool> Stoppable
        {
            get { return _stoppable; }
            set { _stoppable = value; }
        }

        public Enumeration.State state
        {
            get { return _state; }
            set { _state = value; }
        }

        public Enumeration.PriorityState Priority
        {
            get { return _priorityState; }
            set { _priorityState = value; }
        }

        public StateElement()
        {
            _state = Enumeration.State.none;
            _priorityState = Enumeration.PriorityState.Normal;
            _reverse = Enumeration.SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(Enumeration.State state, Enumeration.PriorityState priority)
        {
            _state = state;
            _priorityState = priority;
            _reverse = Enumeration.SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(Enumeration.State state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable)
        {
            _state = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = Enumeration.SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(Enumeration.State state, Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            _state = state;
            _priorityState = priority;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }


        public StateElement(Enumeration.State state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable, Enumeration.SequenceReverse reverse)
        {
            _state = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }

        public StateElement(Enumeration.State state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable, Enumeration.SequenceReverse reverse, Vector2 offSet)
        {
            _state = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = offSet;
        }
        public StateElement(Enumeration.State state)
        {
            _state = state;
            _priorityState = Enumeration.PriorityState.Normal;
            _reverse = Enumeration.SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public static Enumeration.StateTile Parse(string name)
        {
            return (Enumeration.StateTile)Enum.Parse(typeof(Enumeration.StateTile), name.ToLower());
        }



    }

}
