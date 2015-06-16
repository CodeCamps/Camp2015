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
    public enum Actions
    {
        Idle,
        Walk,
        Run,
        JumpAttack,
    }

    public class Actor
    {
        public Actions CurrentAction = Actions.Idle;
        public const double FRAME_TIME = 0.2;
        public int CurrentFrame = 0;
        public double CurrentFrameDuration = 0.0;
        public Dictionary<Actions, List<Texture2D>> Frames =
            new Dictionary<Actions, List<Texture2D>>();

        public Vector2 Location = Vector2.Zero;
        public bool FacingLeft = false;
        public PlayerIndex PlayerIndex = PlayerIndex.One;

        public virtual void LoadContent(ContentManager content) { }
        
        public virtual void StartAnimation(Actions action)
        {
            if (action != CurrentAction)
            {
                CurrentAction = action;
                CurrentFrame = 0;
                CurrentFrameDuration = 0.0;
            }
        }

        public virtual void Update(GameTime gametime)
        {
            CurrentFrameDuration += gametime.ElapsedGameTime.TotalSeconds;
            if (CurrentFrameDuration >= FRAME_TIME)
            {
                CurrentFrame = (CurrentFrame + 1) % Frames[CurrentAction].Count;
                CurrentFrameDuration = 0.0;
            }

            var gamepad = GamePad.GetState(PlayerIndex);
            if (gamepad.ThumbSticks.Left.X != 0.0f)
            {
                Location.X += gamepad.ThumbSticks.Left.X * 5.0f;
                StartAnimation(Actions.Walk);
                if (gamepad.ThumbSticks.Left.X < 0)
                {
                    FacingLeft = true;
                }
                else
                {
                    FacingLeft = false;
                }
            }
            else
            {
                StartAnimation(Actions.Idle);
            }

            if (gamepad.ThumbSticks.Left.Y != 0.0f)
            {
                Location.Y += -gamepad.ThumbSticks.Left.Y * 5.0f;
            }
        }

        public virtual void Draw(GameTime gametime, SpriteBatch batch)
        {
            batch.Draw(
                Frames[CurrentAction][CurrentFrame], // texture2D
                Location, // screen location
                null, // source rectangle
                Color.White, // tint
                0.0f, // rotation
                Vector2.Zero, // origin
                1.0f, // scale
                FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // effect
                0.0f); // depth
        }
    }
}
