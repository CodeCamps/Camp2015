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

        public override void Show(ContentManager Content)
        {
            base.Show(Content);
            contentManager = Content;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bool start = false;
            bool exit = false;
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var gamepad = gamepads[player];
                if (gamepad.IsConnected && gamepad.IsButtonDown(Buttons.A))
                {
                    if (gamepadsPrevious[player].IsButtonUp(Buttons.A))
                    {
                        start = true;
                    }
                }
                if (gamepad.IsConnected && gamepad.IsButtonDown(Buttons.Start))
                {
                    if (gamepadsPrevious[player].IsButtonUp(Buttons.Start))
                    {
                        start = true;
                    }
                }
                if (gamepad.IsConnected && gamepad.IsButtonDown(Buttons.Back))
                {
                    if (gamepadsPrevious[player].IsButtonUp(Buttons.Back))
                    {
                        exit = true;
                    }
                }
            }

            if (start)
            {
                Screens.BattleScreen screen = new Screens.BattleScreen();
                screen.Show(contentManager);
            }
            else if (exit)
            {
                Game1.Instance.Exit();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            base.Draw(gameTime, batch);
        }

        public override void DismissingScreen()
        {
            MediaPlayer.Stop();
            base.DismissingScreen();
        }
    }
}
