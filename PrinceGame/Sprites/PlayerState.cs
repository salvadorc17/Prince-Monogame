using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;

namespace PrinceGame
{
    public class PlayerState
    {
        private const int iSize = 2;

        private Queue<StatePlayerElement> data = new Queue<StatePlayerElement>(iSize);
        public PlayerState()
        {
            Add(new StatePlayerElement());
        }

        public Enumeration.State Next()
        {
            return Next(Value().state);
        }
        public Enumeration.State Next(Enumeration.State state)
        {
            switch (state)
            {
                case Enumeration.State.freefall:
                    return Enumeration.State.crouch;
                default:
                    return Enumeration.State.stand;
            }

        }


        public void Add(Enumeration.State state)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state));
        }

        public void Add(Enumeration.State state, bool ifTrue)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, ifTrue));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, offSet));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, reverse));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal, offSet));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable, Enumeration.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, reverse));
        }


        public void Add(Enumeration.State state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable, Enumeration.SequenceReverse reverse, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, reverse, offSet));
        }


        public void Add(StatePlayerElement stateElement)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(stateElement);
        }

        public StatePlayerElement Previous()
        {
            if (data.Count == 0)
            {
                Add(new StatePlayerElement());
            }

            return data.First();
        }


        public StatePlayerElement Value()
        {
            if (data.Count == 0)
            {
                Add(new StatePlayerElement());
            }
            return data.Last();
        }

        public void Clear()
        {
            data.Clear();
        }

        public IEnumerable<StateElement> GetData()
        {
            // Need to go via array because Queue does not implement IList<T>
            // which ReadOnlyCollection's ctor takes.
            return new ReadOnlyCollection<StateElement>(data.ToArray());
        }
    }
}
