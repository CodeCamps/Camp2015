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
    public class Screen
    {
        public static Stack<Screen> Screens = new Stack<Screen>();
        
        public virtual void Show(ContentManager Content)
        {
            Screens.Push(this);
        }

        public virtual void DismissingScreen() { }
        public static void Dismiss()
        {
            Screens.Peek().DismissingScreen();
            Screens.Pop();

            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var gamePadState = GamePad.GetState(player);
                gamepadsPrevious[player] = gamePadState;
                gamepads[player] = gamePadState;
            }
        }

        protected static Dictionary<PlayerIndex, GamePadState> gamepads =
            new Dictionary<PlayerIndex, GamePadState>();
        protected static Dictionary<PlayerIndex, GamePadState> gamepadsPrevious =
            new Dictionary<PlayerIndex, GamePadState>();

        public virtual void Update(GameTime gameTime) { }
        public static void DoUpdate(GameTime gameTime)
        {
            if (gamepads.Count == 0)
            {
                gamepads.Add(PlayerIndex.One, new GamePadState());
                gamepads.Add(PlayerIndex.Two, new GamePadState());
                gamepads.Add(PlayerIndex.Three, new GamePadState());
                gamepads.Add(PlayerIndex.Four, new GamePadState());
            }

            bool dismissScreen = false;
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                gamepadsPrevious[player] = gamepads[player];
                gamepads[player] = GamePad.GetState(player);

                if (gamepads[player].Buttons.Back == ButtonState.Pressed)
                {
                    if (gamepadsPrevious[player].Buttons.Back == ButtonState.Released)
                    {
                        dismissScreen = true;
                    }
                }
            }

            if (dismissScreen) { Dismiss(); }

            if (Screens.Count > 0)
            {
                Screens.Peek().Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch) { }
        public static void DoDraw(GameTime gameTime, SpriteBatch batch)
        {
            if (Screens.Count > 0)
            {
                Screens.Peek().Draw(gameTime, batch);
            }
        }
    }
}
