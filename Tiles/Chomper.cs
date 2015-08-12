using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace PrinceGame
{
    class Chomper : Tile
    {
        private static List<Sequence> tileSequence = new List<Sequence>();
        public float elapsedTimeOpen = 0;

        public float timeOpen = 2;
        public Enumeration.StateTile State
        {
            get { return tileState.Value().state; }
        }


        public Chomper(RoomNew room, ContentManager Content, Enumeration.TileType tileType, Enumeration.StateTile state, Enumeration.TileType NextTileType__1)
        {
            base.room = room;

            nextTileType = NextTileType__1;
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(tileSequence.GetType());

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString().ToUpper() + "_sequence.xml");

            tileSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in tileSequence)
            {
                s.Initialize(Content);
            }

            //Search in the sequence the right type
            //Sequence result = tileSequence.Find((Sequence s) => s.name.ToUpper() == state.ToString().ToUpper());
            Sequence result = tileSequence.Find((Sequence s) => s.name == state.ToString().ToUpper());

            if (result != null)
            {
                result.frames[0].SetTexture(Content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES[0] + result.frames[0].value));

                collision = result.collision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;


            //change statetile element
            tileState.Value().state = state;
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }

        public void Normal()
        {
            if (tileState.Value().state == Enumeration.StateTile.normal)
            {
                return;
            }

            tileState.Add(Enumeration.StateTile.normal);
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }

        public void Open()
        {
            elapsedTimeOpen = 0;

            if (tileState.Value().state == Enumeration.StateTile.open)
            {
                return;
            }
            if (tileState.Value().state == Enumeration.StateTile.opened)
            {
                return;
            }


            tileState.Add(Enumeration.StateTile.open);
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }

        public void Close()
        {
            elapsedTimeOpen = 0;

            if (tileState.Value().state == Enumeration.StateTile.close)
            {
                return;
            }

            tileState.Add(Enumeration.StateTile.close);
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());

        }
    }

}
