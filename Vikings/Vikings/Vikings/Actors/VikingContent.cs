using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vikings.Actors
{
    public class VikingContent
    {
        public static List<SoundEffect> sndClangs = new List<SoundEffect>();
        //public static List<SoundEffect> sndGrunts = new List<SoundEffect>();
        public static List<SoundEffect> sndTaunts = new List<SoundEffect>();
        public static SoundEffect sndThud;

        public static Texture2D texSquare;
        public static Texture2D texCircle;

        public static Texture2D texHealthBar;
        public static Texture2D texHealthP1;
        public static Texture2D texHealthP2;
        public static Texture2D texHealthP3;
        public static Texture2D texHealthP4;

        public static Dictionary<Actions, List<Texture2D>> Frames =
            new Dictionary<Actions, List<Texture2D>>();

        public static void LoadContent(ContentManager content)
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

            Frames.Add(Actions.Attack1, new List<Texture2D>());
            Frames[Actions.Attack1].Add(content.Load<Texture2D>("vikingswingA0001"));
            Frames[Actions.Attack1].Add(content.Load<Texture2D>("vikingswingA0003"));
            Frames[Actions.Attack1].Add(content.Load<Texture2D>("vikingswingA0005"));
            Frames[Actions.Attack1].Add(content.Load<Texture2D>("vikingswingA0007"));
            Frames[Actions.Attack1].Add(content.Load<Texture2D>("vikingswingA0009"));
            Frames[Actions.Attack1].Add(content.Load<Texture2D>("vikingswingA0011"));
            Frames[Actions.Attack1].Add(content.Load<Texture2D>("vikingswingA0013"));
            Frames[Actions.Attack1].Add(content.Load<Texture2D>("vikingswingA0015"));

            Frames.Add(Actions.Attack2, new List<Texture2D>());
            Frames[Actions.Attack2].Add(content.Load<Texture2D>("vikingswingB0001"));
            Frames[Actions.Attack2].Add(content.Load<Texture2D>("vikingswingB0003"));
            Frames[Actions.Attack2].Add(content.Load<Texture2D>("vikingswingB0005"));
            Frames[Actions.Attack2].Add(content.Load<Texture2D>("vikingswingB0007"));
            Frames[Actions.Attack2].Add(content.Load<Texture2D>("vikingswingB0009"));
            Frames[Actions.Attack2].Add(content.Load<Texture2D>("vikingswingB0011"));
            Frames[Actions.Attack2].Add(content.Load<Texture2D>("vikingswingB0013"));
            Frames[Actions.Attack2].Add(content.Load<Texture2D>("vikingswingB0015"));
            Frames[Actions.Attack2].Add(content.Load<Texture2D>("vikingswingB0017"));

            texSquare = content.Load<Texture2D>("debug-square-8x8");
            texCircle = content.Load<Texture2D>("debug-circle-8x8");

            texHealthBar = content.Load<Texture2D>("health/health-bar");
            texHealthP1 = content.Load<Texture2D>("health/player-1");
            texHealthP2 = content.Load<Texture2D>("health/player-2");
            texHealthP3 = content.Load<Texture2D>("health/player-3");
            texHealthP4 = content.Load<Texture2D>("health/player-4");

            sndClangs.Add(content.Load<SoundEffect>("sounds/sfx/clang-1"));
            sndClangs.Add(content.Load<SoundEffect>("sounds/sfx/clang-2"));
            sndClangs.Add(content.Load<SoundEffect>("sounds/sfx/clang-3"));
            sndClangs.Add(content.Load<SoundEffect>("sounds/sfx/grunt-1"));
            sndClangs.Add(content.Load<SoundEffect>("sounds/sfx/grunt-2"));

            sndTaunts.Add(content.Load<SoundEffect>("sounds/taunts/taunt-1"));
            sndTaunts.Add(content.Load<SoundEffect>("sounds/taunts/taunt-2"));
            sndTaunts.Add(content.Load<SoundEffect>("sounds/taunts/taunt-3"));
            sndTaunts.Add(content.Load<SoundEffect>("sounds/taunts/taunt-4"));
            sndTaunts.Add(content.Load<SoundEffect>("sounds/taunts/taunt-5"));
            sndTaunts.Add(content.Load<SoundEffect>("sounds/taunts/taunt-6"));
            sndTaunts.Add(content.Load<SoundEffect>("sounds/taunts/taunt-7"));
            sndTaunts.Add(content.Load<SoundEffect>("sounds/taunts/taunt-8"));

            sndThud = content.Load<SoundEffect>("sounds/sfx/thud");
        }
    }
}
