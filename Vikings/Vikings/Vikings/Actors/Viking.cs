using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public const double TAUNT_DELAY = 3.0;
        public double TauntDuration = 0.0;

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

            if (TauntDuration <= 0.0)
            {
                var gamepad = GamePad.GetState(PlayerIndex); 
                var tauntStupid = 
                    gamepad.IsButtonDown(Buttons.RightShoulder) ||
                    gamepad.Triggers.Right > 0.0f;
                var tauntSerious = 
                    gamepad.IsButtonDown(Buttons.LeftShoulder) ||
                    gamepad.Triggers.Left > 0.0f;
                if (tauntSerious || tauntStupid)
                {
                    TauntDuration = TAUNT_DELAY;
                    if (tauntSerious)
                    {
                        int i = random.Next(VikingContent.sndTauntsSerious.Count);
                        VikingContent.sndTauntsSerious[i].Play();
                    }
                    else if (tauntStupid)
                    {
                        int i = random.Next(VikingContent.sndTauntsStupid.Count);
                        VikingContent.sndTauntsStupid[i].Play();
                    }
                }
            }
            TauntDuration -= gametime.ElapsedGameTime.TotalSeconds;
            if (TauntDuration < 0.0) { TauntDuration = 0.0; }
        }

        public override void Draw(GameTime gametime, SpriteBatch batch)
        {
            base.Draw(gametime, batch);

            if (TauntDuration > 0.0 && Health > 0)
            {
                var depth = 1.0f - (float)Location.Y / Game1.SCREEN_HEIGHT + 0.04f;
                var texBubble = VikingContent.texBubble;
                var locBubble = Location;
                locBubble.X -= texBubble.Bounds.Width / 2;
                locBubble.Y -= 
                    texBubble.Bounds.Height + 
                    Frames[CurrentAction][CurrentFrame].Bounds.Height + 
                    VikingContent.texShadow.Bounds.Height;
                batch.Draw(
                    texBubble, // texture2D
                    locBubble, // screen location
                    null, // srcRect
                    Color.White, // tint
                    0.0f, // rotation
                    Vector2.Zero, // origin
                    1.0f, // scale
                    SpriteEffects.None, // effect
                    depth - 0.01f); // depth
            }
        }
    }
}
