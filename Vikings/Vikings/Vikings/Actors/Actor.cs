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
        Attack2,
    }

    public class Actor
    {
        public static Dictionary<PlayerIndex, Actor> Actors =
            new Dictionary<PlayerIndex, Actor>();
        
        // set by child (eg. Viking)
        public Dictionary<Actions, List<Texture2D>> Frames;
        public Texture2D texHealthBar;
        public Texture2D texHealthP1;
        public Texture2D texHealthP2;
        public Texture2D texHealthP3;
        public Texture2D texHealthP4;

        public Actions CurrentAction = Actions.Idle;
        public const double DAMAGE_TIME = 2.0;
        public double DamageDuration = 0.0;

        public const double FRAME_TIME = 0.1;
        public int CurrentFrame = 0;
        public double CurrentFrameDuration = 0.0;

        public Vector2 Location = Vector2.Zero;
        public Vector2 Origin = Vector2.Zero;
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

        public bool wasHit = false;
        public bool wasKilled = false;
        public virtual void Update(GameTime gametime)
        {
            wasHit = false;
            wasKilled = false;

            if (Health < 1 && DamageDuration <= 0.0)
            {
                GamePad.SetVibration(PlayerIndex, 0, 0);
                return;
            }

            Actions action = Actions.Idle;

            DamageDuration -= gametime.ElapsedGameTime.TotalSeconds;
            if (DamageDuration < 0.0) { DamageDuration = 0.0; }
            if (DamageDuration > 0.0)
            {
                GamePad.SetVibration(PlayerIndex, 1.0f, 1.0f);
            }

            if (DamageDuration <= 0.0)
            {
                DamageDuration = 0.0;
                GamePad.SetVibration(PlayerIndex, 0.0f, 0.0f);
            }

            CurrentSpriteWidth = Frames[CurrentAction][CurrentFrame].Width;
            CurrentSpriteHeight = Frames[CurrentAction][CurrentFrame].Height;

            CurrentFrameDuration += gametime.ElapsedGameTime.TotalSeconds;
            var inAnimation = false;
            inAnimation =
                CurrentAction == Actions.Attack1 ||
                CurrentAction == Actions.Attack2 ||
                CurrentAction == Actions.JumpAttack;
            if (CurrentFrameDuration >= FRAME_TIME)
            {
                CurrentFrame = (CurrentFrame + 1); // % Frames[CurrentAction].Count;
                switch (CurrentAction)
                {
                    case Actions.Attack1:
                    case Actions.Attack2:
                    case Actions.JumpAttack:
                        action = CurrentAction;
                        if (CurrentFrame >= Frames[CurrentAction].Count)
                        {
                            action = Actions.Idle;
                            CurrentFrame = 0;
                            CurrentFrameDuration = 0.0;
                        }
                        else
                        {
                            CurrentFrameDuration = 0.0;
                            return;
                        }
                        break;
                    default:
                        CurrentFrame = CurrentFrame % Frames[CurrentAction].Count;
                        break;
                }
                CurrentFrameDuration = 0.0;
            }
            else if(inAnimation)
            {
                return;
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

            if (Location.X < CurrentSpriteWidth / 2)
            {
                Location.X = CurrentSpriteWidth / 2;
            }
            else if (Location.X > Game1.SCREEN_WIDTH - CurrentSpriteWidth / 2)
            {
                Location.X = Game1.SCREEN_WIDTH - CurrentSpriteWidth / 2;
            }

            if (Location.Y < 630)
            {
                Location.Y = 630;
            }
            else if (Location.Y > Game1.SCREEN_HEIGHT)
            {
                Location.Y = Game1.SCREEN_HEIGHT;
            }

            if (gamepad.Buttons.B == ButtonState.Pressed)
            {
                action = Actions.Attack1;
            }

            if (gamepad.Buttons.Y == ButtonState.Pressed)
            {
                action = Actions.Attack2;
            }

            if (gamepad.Buttons.A == ButtonState.Pressed)
            {
                action = Actions.JumpAttack;
            }

            if (action == Actions.Attack1 || action == Actions.Attack2 || action == Actions.JumpAttack)
            {
                foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
                {
                    var actor = Actors[player];
                    if (actor != null)
                    {
                        if (Collision(player))
                        {
                            actor.Health -= Damage;
                            actor.DamageDuration = DAMAGE_TIME;
                            wasHit = true;
                            if (actor.Health < 0)
                            {
                                wasKilled = true;
                                actor.Health = 0;
                            }
                        }
                    }
                }
            }

            StartAnimation(action);
        }

        public static Random random = new Random();

        public int CurrentSpriteWidth = 0;
        public int CurrentSpriteHeight = 0;

        public virtual void Draw(GameTime gametime, SpriteBatch batch)
        {
            if (Health < 1 && DamageDuration <= 0.0) { return; }
            Color tint = Color.White;
            if (DamageDuration > 0.0 || Health < 1)
            {
                tint = new Color(255, 255, 255, 128);
            }

            Origin.X = CurrentSpriteWidth / 2;
            Origin.Y = CurrentSpriteHeight;

            var depth = 1.0f - (float)Location.Y / Game1.SCREEN_HEIGHT + 0.04f;

            batch.Draw(
                Frames[CurrentAction][CurrentFrame], // texture2D
                Location, // screen location
                null, // source rectangle
                tint, // tint
                0.0f, // rotation
                Origin, // origin
                1.0f, // scale
                FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // effect
                depth - 0.01f); // depth

            Rectangle rectBar = new Rectangle(
                    (int)Location.X - texHealthBar.Width / 2,
                    (int)Location.Y - texHealthBar.Height - Frames[CurrentAction][CurrentFrame].Bounds.Height,
                    texHealthBar.Width,
                    texHealthBar.Height);
            Vector2 locBar = new Vector2(rectBar.X, rectBar.Y);
            batch.Draw(
                texHealthBar, // texture2D
                rectBar, // screen location
                null, // source rectangle
                Color.DarkRed, // tint
                0.0f, // rotation
                Vector2.Zero, // origin
                SpriteEffects.None, // effect
                depth - 0.02f); // depth

            Rectangle rectHealth = rectBar;
            rectHealth.Location = Point.Zero;
            rectBar.Width = rectHealth.Width = 
                (int)Math.Round(rectHealth.Width * (Health / 100.0f));
            batch.Draw(
                texHealthBar, // texture2D
                rectBar, // screen location
                rectHealth, // source rectangle
                Color.Green, // tint
                0.0f, // rotation
                Vector2.Zero, // origin
                SpriteEffects.None, // effect
                depth - 0.03f); // depth
            Texture2D texPlayer;
            switch (PlayerIndex)
            {
                case PlayerIndex.Two:
                    texPlayer = texHealthP2;
                    break;
                case PlayerIndex.Three:
                    texPlayer = texHealthP3;
                    break;
                case PlayerIndex.Four:
                    texPlayer = texHealthP4;
                    break;
                default:
                    texPlayer = texHealthP1;
                    break;
            }
            batch.Draw(
                texPlayer, // texture2D
                locBar, // screen location
                null, // srcRect
                Color.White, // tint
                0.0f, // rotation
                Vector2.Zero, // origin
                1.0f, // scale
                SpriteEffects.None, // effect
                depth - 0.04f); // depth

            var texShadow = VikingContent.texShadow;
            var locShadow = Location;
            locShadow.X -= texShadow.Bounds.Width / 2;
            locShadow.Y -= texShadow.Bounds.Height;
            batch.Draw(
                texShadow, // texture2D
                locShadow, // screen location
                null, // srcRect
                new Color(255, 255, 255, 128), // tint
                0.0f, // rotation
                Vector2.Zero, // origin
                1.0f, // scale
                SpriteEffects.None, // effect
                depth); // depth
        }

        public bool Collision(PlayerIndex player)
        {
            bool result = false;
            Actor opponent = Actors[player];
            if (opponent != null && player != PlayerIndex)
            {
                bool allowCollision = true;
                if (Math.Abs(Location.Y - opponent.Location.Y) > 10)
                {
                    allowCollision = false;
                }
                if (opponent.Location.X >= Location.X && FacingLeft)
                {
                    allowCollision = false;
                }
                if (opponent.Location.X <= Location.X && !FacingLeft)
                {
                    allowCollision = false;
                }
                if (opponent.DamageDuration > 0.0)
                {
                    allowCollision = false;
                }
                if (DamageDuration > 0.0 || Health < 1)
                {
                    allowCollision = false;
                }

                if (allowCollision)
                {
                    Rectangle rectOpponent = opponent.Frames[opponent.CurrentAction][0].Bounds;
                    rectOpponent.X = (int)opponent.Location.X;
                    rectOpponent.Y = (int)opponent.Location.Y;
                    Rectangle rectMe = Frames[CurrentAction][0].Bounds;
                    rectMe.X = (int)Location.X;
                    rectMe.Y = (int)Location.Y;
                    result = rectMe.Intersects(rectOpponent);
                }
            }
            return result;
        }
    }
}
