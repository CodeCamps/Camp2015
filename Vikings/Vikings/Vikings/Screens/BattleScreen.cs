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
                }
            }

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(music);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // our cool stuff
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var actor = Actors.Actor.Actors[player];
                if (actor != null && gamepads[player].IsConnected)
                {
                    actor.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // our cool stuff
            spriteBatch.Begin();
            spriteBatch.Draw(texArena, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var actor = Actors.Actor.Actors[player];
                if (actor != null && gamepads[player].IsConnected)
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
