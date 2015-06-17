using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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
        public static Texture2D texProgress;

        public override void Show(ContentManager Content)
        {
            base.Show(Content);

            player1.LoadContent(Content);
            player1.PlayerIndex = PlayerIndex.One;
            player1.Location = Vector2.Zero;
            player1.FacingLeft = false;
            player1.StartAnimation(Actors.Actions.Idle);

            player2.LoadContent(Content);
            player2.PlayerIndex = PlayerIndex.Two;
            player2.Location = Vector2.One * 2000.0f;
            player2.FacingLeft = true;
            player2.StartAnimation(Actors.Actions.Idle);

            texArena = Content.Load<Texture2D>("arena");
            texProgress = Content.Load<Texture2D>("debug-square-8x8");

            Actors.Actor.Actors.Add(PlayerIndex.One, player1);
            Actors.Actor.Actors.Add(PlayerIndex.Two, player2);
            Actors.Actor.Actors.Add(PlayerIndex.Three, null);
            Actors.Actor.Actors.Add(PlayerIndex.Four, null);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // our cool stuff
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var actor = Actors.Actor.Actors[player];
                if (actor != null)
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
                if (actor != null)
                {
                    actor.Draw(gameTime, spriteBatch);
                }
            }
            spriteBatch.End();
        }

        public override void DismissingScreen()
        {
            base.DismissingScreen();
        }
    }
}
