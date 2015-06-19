using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vikings.Screens
{
    public class BattleScreen : Screen
    {
        Actors.Viking player1 = new Actors.Viking();
        Actors.Viking player2 = new Actors.Viking();
        Actors.Viking player3 = new Actors.Viking();
        Actors.Viking player4 = new Actors.Viking();

        public double CountDown;

        public static Texture2D texArena;
        public static Song music;

        private Vector2[] StartLocations = 
        {
            new Vector2(0,0),
            new Vector2(Game1.SCREEN_WIDTH, Game1.SCREEN_HEIGHT),
            new Vector2(0, Game1.SCREEN_HEIGHT),
            new Vector2(Game1.SCREEN_WIDTH, 0),
        };

        private Color[] Colors =
        {
            Color.White,
            Color.Lavender,
            Color.Salmon,
            Color.Turquoise
        };

        public override void Show(ContentManager Content)
        {
            base.Show(Content);

            if (texArena == null)
            {

                texArena = Content.Load<Texture2D>("arena");

                music = Content.Load<Song>("arena-music");

                Actors.Actor.Actors.Add(PlayerIndex.One, player1);
                Actors.Actor.Actors.Add(PlayerIndex.Two, player2);
                Actors.Actor.Actors.Add(PlayerIndex.Three, player3);
                Actors.Actor.Actors.Add(PlayerIndex.Four, player4);
            }

            CountDown = 4.0;
            played3 = played2 = played1 = playedFight = false;

            int i = 0;
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var actor = Actors.Actor.Actors[player];
                if (actor != null)
                {
                    actor.LoadContent(Content);
                    actor.PlayerIndex = player;
                    actor.Location = StartLocations[i++];
                    actor.FacingLeft = actor.Location.X > 0;
                    actor.StartAnimation(Actors.Actions.Idle);
                    actor.Health = 100;
                    actor.Update(new GameTime());
                    actor.StartAnimation(Actors.Actions.Idle);
                }
            }

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(music);
        }

        bool played3 = false;
        bool played2 = false;
        bool played1 = false;
        bool playedFight = false;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // our cool stuff
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var actor = Actors.Actor.Actors[player];
                if (actor != null && (gamepads[player].IsConnected || player == PlayerIndex.Two))
                {
                    actor.IgnoreInput = CountDown > 0.0;
                    actor.Update(gameTime);
                }
            }

            CountDown -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // our cool stuff
            //spriteBatch.Begin();
            //spriteBatch.Draw(texArena, Vector2.Zero, Color.White);
            //spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            spriteBatch.Draw(
                texArena,
                Vector2.Zero,
                null,
                Color.White,
                0.0f,
                Vector2.Zero,
                Vector2.One,
                SpriteEffects.None,
                0.5f);
            Texture2D texCountDown = null;
            if (CountDown < 0.0) 
            {
                //System.Diagnostics.Debug.WriteLine(CountDown);
            }
            else if (CountDown < 1.0)
            {
                texCountDown = Actors.VikingContent.texFight;
                if (!playedFight)
                {
                    Actors.VikingContent.sndFight.Play();
                    playedFight = true;
                }
            }
            else if (CountDown < 2.0)
            {
                texCountDown = Actors.VikingContent.texOne;
                if (!played1)
                {
                    Actors.VikingContent.sndOne.Play();
                    played1 = true;
                }
            }
            else if (CountDown < 3.0)
            {
                texCountDown = Actors.VikingContent.texTwo;
                if (!played2)
                {
                    Actors.VikingContent.sndTwo.Play();
                    played2 = true;
                }
            }
            else if (CountDown < 4.0)
            {
                texCountDown = Actors.VikingContent.texThree;
                if (!played3)
                {
                    Actors.VikingContent.sndThree.Play();
                    played3 = true;
                }
            }

            if (texCountDown != null)
            {
                double fade = CountDown - Math.Floor(CountDown);
                Vector2 origin = new Vector2(128.0f, 128.0f);
                Color tint = new Color(1.0f, 1.0f, 1.0f, (float)fade);
                spriteBatch.Draw(
                    texCountDown,
                    Vector2.One * (float)(Game1.SCREEN_WIDTH / 2),
                    null,
                    tint,
                    (float)fade,
                    origin,
                    Vector2.One - Vector2.One * (float)fade,
                    SpriteEffects.None,
                    0.4f);
            }

            
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var actor = Actors.Actor.Actors[player];
                if (actor != null && (gamepads[player].IsConnected || player == PlayerIndex.Two))
                {
                    actor.Draw(gameTime, spriteBatch);
                }
            }
            spriteBatch.End();
        }

        public override void DismissingScreen()
        {
            MediaPlayer.Stop();
            base.DismissingScreen();
        }
    }
}
