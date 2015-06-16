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
            Frames.Add(Actions.Idle, new List<Texture2D>());
            Frames[Actions.Idle].Add(content.Load<Texture2D>("viking_idle0001"));
            Frames[Actions.Idle].Add(content.Load<Texture2D>("viking_idle0009"));
            Frames[Actions.Idle].Add(content.Load<Texture2D>("viking_idle0019"));
            Frames[Actions.Idle].Add(content.Load<Texture2D>("viking_idle0029"));
            Frames[Actions.Idle].Add(content.Load<Texture2D>("viking_idle0039"));
            Frames[Actions.Idle].Add(content.Load<Texture2D>("viking_idle0049"));
            Frames[Actions.Idle].Add(content.Load<Texture2D>("viking_idle0059"));
            base.LoadContent(content);
        }
    }
}
