	//-----------------------------------------------------------------------//
	// <copyright file="Item.cs" company="A.D.F.Software">
	// Copyright "A.D.F.Software" (c) 2014 All Rights Reserved
	// <author>Andrea M. Falappi</author>
	// <date>Wednesday, September 24, 2014 11:36:49 AM</date>
	// </copyright>
	//
	// * NOTICE:  All information contained herein is, and remains
	// * the property of Andrea M. Falappi and its suppliers,
	// * if any.  The intellectual and technical concepts contained
	// * herein are proprietary to A.D.F.Software
	// * and its suppliers and may be covered by World Wide and Foreign Patents,
	// * patents in process, and are protected by trade secret or copyright law.
	// * Dissemination of this information or reproduction of this material
	// * is strictly forbidden unless prior written permission is obtained
	// * from Andrea M. Falappi.
	//-----------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace PrinceGame
{
    public abstract class Item
    {
        public Texture2D Texture;
        public AnimationSequence itemAnimation = new AnimationSequence();
        public TileState itemState = new TileState();
        private SpriteEffects flip = SpriteEffects.None;

        private Position position = new Position(new Vector2(0, 0), new Vector2(0, 0));
        private static List<Sequence> m_itemSequence = new List<Sequence>();
        public List<Sequence> ItemSequence
        {
            get { return m_itemSequence; }
            set { m_itemSequence = value; }
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            float elapsed = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            itemAnimation.UpdateFrameItem(elapsed, ref position, ref flip, ref itemState);
        }

    }
}
