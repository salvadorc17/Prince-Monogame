using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;

namespace PrinceGame
{
    public class TileState
    {

        private const int iSize = 2;

        private Queue<StateTileElement> data = new Queue<StateTileElement>(iSize);
        public TileState()
        {
            Add(new StateTileElement());
        }

        public Enumeration.StateTile Next()
        {
            return Next(Value().state);
        }
        public Enumeration.StateTile Next(Enumeration.StateTile state)
        {
            switch (state)
            {
                default:
                    //case Enumeration.StateTile.freefall:
                    //    return Enumeration.StateTile.crouch;
                    return Enumeration.StateTile.normal;
            }

        }


        public void Add(Enumeration.StateTile state)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state));
        }

        public void Add(Enumeration.StateTile state, bool ifTrue)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, ifTrue));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, reverse));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal, offSet));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable, Enumeration.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, reverse));
        }


        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, System.Nullable<bool> stoppable, Enumeration.SequenceReverse reverse, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, reverse, offSet));
        }


        public void Add(StateTileElement stateElement)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(stateElement);
        }

        public StateTileElement Previous()
        {
            if (data.Count == 0)
            {
                Add(new StateTileElement());
            }
            return data.First();
        }


        public StateTileElement Value()
        {
            if (data.Count == 0)
            {
                Add(new StateTileElement());
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
