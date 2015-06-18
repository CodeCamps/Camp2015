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

        public Actions CurrentAction = Actions.Idle;
        public const double DAMAGE_TIME = 2.0;
        public double DamageDuration = 0.0;

        public const double FRAME_TIME = 0.2;
        public int CurrentFrame = 0;
        public double CurrentFrameDuration = 0.0;
        public Dictionary<Actions, List<Texture2D>> Frames =
            new Dictionary<Actions, List<Texture2D>>();

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

        public virtual void Update(GameTime gametime)
        {
            if (Health < 1 && DamageDuration <= 0.0) { GamePad.SetVibration(PlayerIndex, 0, 0); return; }

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

            if (action == Actions.Attack1 || action == Actions.Attack2)
            {
                bool wasHit = false;
                bool wasKilled = false;
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
                if (wasKilled)
                {
                    Screens.BattleScreen.sndThud.Play();
                }
                else if (wasHit)
                {
                    int i = random.Next(Screens.BattleScreen.sndClangs.Count);
                    Screens.BattleScreen.sndClangs[i].Play();
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

            batch.Draw(
                Frames[CurrentAction][CurrentFrame], // texture2D
                Location, // screen location
                null, // source rectangle
                tint, // tint
                0.0f, // rotation
                Origin, // origin
                1.0f, // scale
                FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // effect
                1.0f - (float)Location.Y / Game1.SCREEN_HEIGHT); // depth

            var texHealthBar = Screens.BattleScreen.texHealthBar;
            Rectangle rectBar = new Rectangle(
                    (int)Location.X - texHealthBar.Width / 2,
                    (int)Location.Y - texHealthBar.Height - Frames[CurrentAction][CurrentFrame].Bounds.Height,
                    texHealthBar.Width,
                    texHealthBar.Height);
            Vector2 locBar = new Vector2(rectBar.X, rectBar.Y);
            batch.Draw(texHealthBar, rectBar, null, Color.DarkRed);
            Rectangle rectHealth = rectBar;
            rectHealth.Location = Point.Zero;
            rectBar.Width = rectHealth.Width = 
                (int)Math.Round(rectHealth.Width * (Health / 100.0f));
            batch.Draw(texHealthBar, rectBar, rectHealth, Color.Green);
            Texture2D texPlayer;
            switch (PlayerIndex)
            {
                case PlayerIndex.Two:
                    texPlayer = Screens.BattleScreen.texHealthP2;
                    break;
                case PlayerIndex.Three:
                    texPlayer = Screens.BattleScreen.texHealthP3;
                    break;
                case PlayerIndex.Four:
                    texPlayer = Screens.BattleScreen.texHealthP4;
                    break;
                default:
                    texPlayer = Screens.BattleScreen.texHealthP1;
                    break;
            }
            batch.Draw(texPlayer, locBar, Color.White);
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
