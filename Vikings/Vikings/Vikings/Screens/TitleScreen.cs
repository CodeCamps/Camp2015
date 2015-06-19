using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Vikings.Screens
{
    public class TitleScreen : Screen
    {
        Actors.Viking player1 = new Actors.Viking();
        Actors.Viking player2 = new Actors.Viking();
        Actors.Viking player3 = new Actors.Viking();

        public static Texture2D texBackground;
        public static Song music;
        private ContentManager contentManager;

        public Texture2D logo;
        public Texture2D pressStart;
        public List<Texture2D> credits = new List<Texture2D>();
        public int CurrentCreditIndex = 0;

        public float xStartLeft;
        public float xStartRight;
        public int runWidth;

        public override void Show(ContentManager Content)
        {
            base.Show(Content);
            contentManager = Content;

            if (credits.Count == 0)
            {
                credits.Add(Content.Load<Texture2D>("credits/aiden"));
                credits.Add(Content.Load<Texture2D>("credits/alexys"));
                credits.Add(Content.Load<Texture2D>("credits/cage"));
                credits.Add(Content.Load<Texture2D>("credits/carlton"));
                credits.Add(Content.Load<Texture2D>("credits/daniel"));
                credits.Add(Content.Load<Texture2D>("credits/ian"));
                credits.Add(Content.Load<Texture2D>("credits/jake"));
                credits.Add(Content.Load<Texture2D>("credits/james"));
                credits.Add(Content.Load<Texture2D>("credits/jess"));
                credits.Add(Content.Load<Texture2D>("credits/lamia"));
                credits.Add(Content.Load<Texture2D>("credits/lauren"));
                credits.Add(Content.Load<Texture2D>("credits/lee"));
                credits.Add(Content.Load<Texture2D>("credits/luke"));
                credits.Add(Content.Load<Texture2D>("credits/pierce"));
                credits.Add(Content.Load<Texture2D>("credits/randon"));
                credits.Add(Content.Load<Texture2D>("credits/addi"));
                credits.Add(Content.Load<Texture2D>("credits/joehall"));

                logo = Content.Load<Texture2D>("logo");
                pressStart = Content.Load<Texture2D>("pressStart");

                player1.LoadContent(Content);
                player2.LoadContent(Content);
                player3.LoadContent(Content);

                music = Content.Load<Song>("title");
            }

            runWidth = Actors.VikingContent.Frames[Actors.Actions.Run][0].Bounds.Width;
            var nameHeight = credits[0].Bounds.Height;
            var runTop = Game1.SCREEN_HEIGHT / 3;
            var runLaneHeight = (Game1.SCREEN_HEIGHT - runTop) / 3;

            xStartLeft = 0 - runWidth;
            xStartRight = Game1.SCREEN_WIDTH + runWidth;

            player1.PlayerIndex = PlayerIndex.One;
            player1.IgnoreInput = true;
            player1.ShowHealth = false;
            player1.FacingLeft = true;
            player1.Location = new Vector2(
                xStartRight,
                runTop + runLaneHeight * 0 + nameHeight);
            
            player2.PlayerIndex = PlayerIndex.One;
            player2.IgnoreInput = true;
            player2.ShowHealth = false;
            player2.FacingLeft = true;
            player2.Location = new Vector2(
                xStartRight,
                runTop + runLaneHeight * 2 + nameHeight);

            player3.PlayerIndex = PlayerIndex.One;
            player3.IgnoreInput = true;
            player3.ShowHealth = false;
            player3.FacingLeft = false;
            player3.Location = new Vector2(
                xStartLeft,
                runTop + runLaneHeight * 1 + nameHeight);

            player1.StartAnimation(Actors.Actions.Run);
            player2.StartAnimation(Actors.Actions.Run);
            player3.StartAnimation(Actors.Actions.Run);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(music);
        }

        bool CanSwapNames = true;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var distance = gameTime.ElapsedGameTime.TotalSeconds * 250.0f;

            player1.Update(gameTime);
            player1.Location.X -= (float)distance;
            player2.Update(gameTime);
            player2.Location.X -= (float)distance;
            player3.Update(gameTime);
            player3.Location.X += (float)distance;

            if (CanSwapNames && player1.Location.X <= 90 + runWidth / 2)
            {
                CurrentCreditIndex += 3;
                CanSwapNames = false;
            }

            if (player1.Location.X <= -runWidth)
            {
                CanSwapNames = true;
                player1.Location.X = xStartRight;
                player2.Location.X = xStartRight;
                player3.Location.X = xStartLeft;
            }

            bool start = false;
            bool exit = false;
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var gamepad = gamepads[player];
                var isConnected = gamepad.IsConnected || player == PlayerIndex.Two;
                if (isConnected && gamepad.IsButtonDown(Buttons.A))
                {
                    if (gamepadsPrevious[player].IsButtonUp(Buttons.A))
                    {
                        start = true;
                    }
                }
                if (isConnected && gamepad.IsButtonDown(Buttons.Start))
                {
                    if (gamepadsPrevious[player].IsButtonUp(Buttons.Start))
                    {
                        start = true;
                    }
                }
                if (isConnected && gamepad.IsButtonDown(Buttons.Back))
                {
                    if (gamepadsPrevious[player].IsButtonUp(Buttons.Back))
                    {
                        exit = true;
                    }
                }
            }

            if (start)
            {
                MediaPlayer.Stop();
                Screens.BattleScreen screen = new Screens.BattleScreen();
                screen.Show(contentManager);
            }
            else if (exit)
            {
                MediaPlayer.Stop();
                Game1.Instance.Exit();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            base.Draw(gameTime, batch);

            var runHeight = Actors.VikingContent.Frames[Actors.Actions.Run][0].Bounds.Height;
            var locName1 = new Vector2(90, player1.Location.Y - runHeight / 2);
            var locName2 = new Vector2(90, player2.Location.Y - runHeight / 2);
            var locName3 = new Vector2(
                Game1.SCREEN_WIDTH - 90 - credits[0].Bounds.Width,
                player3.Location.Y - runHeight / 2);
            var locNameShadowOffset = Vector2.One * 2.0f;
            batch.Begin();

            batch.Draw(logo, Vector2.Zero, Color.White);

            if ((int)(gameTime.TotalGameTime.TotalSeconds * 2.0) % 2 == 1)
            {
                batch.Draw(
                    pressStart,
                    new Vector2(
                        Game1.SCREEN_WIDTH - pressStart.Bounds.Width - 90,
                        Game1.SCREEN_HEIGHT - pressStart.Bounds.Height - 30
                        ),
                    Color.White);
            }

            batch.Draw(
                credits[(CurrentCreditIndex + 0) % credits.Count],
                locName1 + locNameShadowOffset,
                null,
                Color.Black,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);
            batch.Draw(
                credits[(CurrentCreditIndex + 0) % credits.Count],
                locName1,
                null,
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);

            batch.Draw(
                credits[(CurrentCreditIndex + 1) % credits.Count],
                locName2 + locNameShadowOffset,
                null,
                Color.Black,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);
            batch.Draw(
                credits[(CurrentCreditIndex + 1) % credits.Count],
                locName2,
                null,
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);

            batch.Draw(
                credits[(CurrentCreditIndex + 2) % credits.Count],
                locName3 + locNameShadowOffset,
                null,
                Color.Black,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);
            batch.Draw(
                credits[(CurrentCreditIndex + 2) % credits.Count],
                locName3,
                null,
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);

            player1.Draw(gameTime, batch);
            player2.Draw(gameTime, batch);
            player3.Draw(gameTime, batch);

            batch.End();
        }

        public override void DismissingScreen()
        {
            MediaPlayer.Stop();
            base.DismissingScreen();
        }
    }
}
