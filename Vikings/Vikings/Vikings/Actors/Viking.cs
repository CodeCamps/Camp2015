using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vikings.Actors
{
    public class Viking : Actor
    {
        public override void LoadContent(ContentManager content)
        {
            if (VikingContent.Frames.Count == 0)
            {
                VikingContent.LoadContent(content);
            }
            Frames = VikingContent.Frames;
            texHealthBar = VikingContent.texHealthBar;
            texHealthP1 = VikingContent.texHealthP1;
            texHealthP2 = VikingContent.texHealthP2;
            texHealthP3 = VikingContent.texHealthP3;
            texHealthP4 = VikingContent.texHealthP4;

            base.LoadContent(content);
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            
            if (wasKilled)
            {
                VikingContent.sndThud.Play();
            }
            else if (wasHit)
            {
                int i = random.Next(VikingContent.sndClangs.Count);
                VikingContent.sndClangs[i].Play();
            }
        }
    }
}
