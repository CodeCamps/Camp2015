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

            Frames.Add(Actions.Walk, new List<Texture2D>());
            Frames[Actions.Walk].Add(content.Load<Texture2D>("vikingwalk0001"));
            Frames[Actions.Walk].Add(content.Load<Texture2D>("vikingwalk0003"));
            Frames[Actions.Walk].Add(content.Load<Texture2D>("vikingwalk0005"));
            Frames[Actions.Walk].Add(content.Load<Texture2D>("vikingwalk0007"));
            Frames[Actions.Walk].Add(content.Load<Texture2D>("vikingwalk0009"));
            Frames[Actions.Walk].Add(content.Load<Texture2D>("vikingwalk0011"));
            Frames[Actions.Walk].Add(content.Load<Texture2D>("vikingwalk0013"));
            Frames[Actions.Walk].Add(content.Load<Texture2D>("vikingwalk0015"));

            Frames.Add(Actions.JumpAttack, new List<Texture2D>());
            Frames[Actions.JumpAttack].Add(content.Load<Texture2D>("vikingjumpswingA0001"));
            Frames[Actions.JumpAttack].Add(content.Load<Texture2D>("vikingjumpswingA0002"));
            Frames[Actions.JumpAttack].Add(content.Load<Texture2D>("vikingjumpswingA0003"));
            Frames[Actions.JumpAttack].Add(content.Load<Texture2D>("vikingjumpswingA0004"));
            Frames[Actions.JumpAttack].Add(content.Load<Texture2D>("vikingjumpswingA0005"));
            Frames[Actions.JumpAttack].Add(content.Load<Texture2D>("vikingjumpswingA0006"));

            Frames.Add(Actions.Run, new List<Texture2D>());
            Frames[Actions.Run].Add(content.Load<Texture2D>("vikingrun0001"));
            Frames[Actions.Run].Add(content.Load<Texture2D>("vikingrun0003"));
            Frames[Actions.Run].Add(content.Load<Texture2D>("vikingrun0005"));
            Frames[Actions.Run].Add(content.Load<Texture2D>("vikingrun0007"));
            Frames[Actions.Run].Add(content.Load<Texture2D>("vikingrun0009"));
            Frames[Actions.Run].Add(content.Load<Texture2D>("vikingrun0011"));

            base.LoadContent(content);
        }
    }
}
