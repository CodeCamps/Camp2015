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
        Attack1,
        Attack2, // TODO: Homework
    }

    public class Actor
    {
        public static Dictionary<PlayerIndex, Actor> Actors =
            new Dictionary<PlayerIndex, Actor>();

        public Actions CurrentAction = Actions.Idle;
        public const double FRAME_TIME = 0.2;
        public int CurrentFrame = 0;
        public double CurrentFrameDuration = 0.0;
        public Dictionary<Actions, List<Texture2D>> Frames =
            new Dictionary<Actions, List<Texture2D>>();

        public Vector2 Location = Vector2.Zero;
        public bool FacingLeft = false;
        public PlayerIndex PlayerIndex = PlayerIndex.One;
        public int Health = 100;
        public int Damage = 25;

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
            Actions action = Actions.Idle;

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
                action = Actions.Walk;
                if (gamepad.ThumbSticks.Left.X < 0)
                {
                    FacingLeft = true;
                }
                else
                {
                    FacingLeft = false;
                }
            }

            if (gamepad.ThumbSticks.Left.Y != 0.0f)
            {
                Location.Y += -gamepad.ThumbSticks.Left.Y * 5.0f;
            }

            if (gamepad.Buttons.B == ButtonState.Pressed)
            {
                action = Actions.Attack1;
            }

            if (action == Actions.Attack1)
            {
                if (Collision(PlayerIndex.One))
                {
                    Actors[PlayerIndex.One].Health -= Damage;
                }
                if (Collision(PlayerIndex.Two))
                {
                    Actors[PlayerIndex.Two].Health -= Damage;
                }
                if (Collision(PlayerIndex.Three))
                {
                    Actors[PlayerIndex.Three].Health -= Damage;
                }
                if (Collision(PlayerIndex.Four))
                {
                    Actors[PlayerIndex.Four].Health -= Damage;
                }
            }

            StartAnimation(action);
        }

        public virtual void Draw(GameTime gametime, SpriteBatch batch)
        {
            Color tint = Color.White;
            if (Health < 100)
            {
                tint = Color.Red;
            }

            batch.Draw(
                Frames[CurrentAction][CurrentFrame], // texture2D
                Location, // screen location
                null, // source rectangle
                tint, // tint
                0.0f, // rotation
                Vector2.Zero, // origin
                1.0f, // scale
                FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // effect
                0.0f); // depth
        }

        public bool Collision(PlayerIndex player)
        {
            bool result = false;
            Actor opponent = Actors[player];
            if (opponent != null && player != PlayerIndex)
            {
                Rectangle rectOpponent = opponent.Frames[opponent.CurrentAction][0].Bounds;
                rectOpponent.X = (int)opponent.Location.X;
                rectOpponent.Y = (int)opponent.Location.Y;
                Rectangle rectMe = Frames[CurrentAction][0].Bounds;
                rectMe.X = (int)Location.X;
                rectMe.Y = (int)Location.Y;
                result = rectMe.Intersects(rectOpponent);
            }
            return result;
        }
    }
}
