using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PrinceGame
{
    public sealed class Accelerometer
    {
        private Accelerometer()
        {
        }

        // we want to prevent the Accelerometer from being initialized twice.

        private static bool isInitialized = false;
        // whether or not the accelerometer is active

        private static bool isActive = false;
        /// <summary>
        /// Initializes the Accelerometer for the current game. This method can only be called once per game.
        /// </summary>
        public static void Initialize()
        {
            // make sure we don't initialize the Accelerometer twice
            if (isInitialized)
            {
                throw new InvalidOperationException("Initialize can only be called once");
            }


            // remember that we are initialized
            isInitialized = true;
        }

        /// <summary>
        /// Gets the current state of the accelerometer.
        /// </summary>
        /// <returns>A new AccelerometerState with the current state of the accelerometer.</returns>
        public static AccelerometerState GetState()
        {
            // make sure we've initialized the Accelerometer before we try to get the state
            if (!isInitialized)
            {
                throw new InvalidOperationException("You must Initialize before you can call GetState");
            }

            // create a new value for our state
            Vector3 stateValue = new Vector3();


            return new AccelerometerState(stateValue, isActive);
        }
    }

    /// <summary>
    /// An encapsulation of the accelerometer's current state.
    /// </summary>
    public struct AccelerometerState
    {
        /// <summary>
        /// Gets the accelerometer's current value in G-force.
        /// </summary>
        public Vector3 Acceleration
        {
            get { return m_Acceleration; }
            private set { m_Acceleration = value; }
        }

        private Vector3 m_Acceleration;
        /// <summary>
        /// Gets whether or not the accelerometer is active and running.
        /// </summary>
        public bool IsActive
        {
            get { return m_IsActive; }
            private set { m_IsActive = value; }
        }

        private bool m_IsActive;


        public AccelerometerState(Vector3 acceleration, bool isActive)
            : this()
        {
            Acceleration = acceleration;
            IsActive = isActive;
        }

        /// <summary>
        /// Returns a string containing the values of the Acceleration and IsActive properties.
        /// </summary>
        /// <returns>A new string describing the state.</returns>
        public override string ToString()
        {
            return string.Format("Acceleration: {0}, IsActive: {1}", Acceleration, IsActive);
        }
    }



}
