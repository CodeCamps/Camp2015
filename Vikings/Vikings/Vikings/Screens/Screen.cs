using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
            if (Screens.Count > 0)
            {
                Screens.Peek().DismissingScreen();
                Screens.Pop();
            }
            else
            {
                Game1.Instance.Exit();
            }

            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var gamePadState = Screen.GetState(player);
                gamepadsPrevious[player] = gamePadState;
                gamepads[player] = gamePadState;
            }
        }

        public static GamePadState GetState(PlayerIndex playerIndex)
        {
            var gamePadState = GamePad.GetState(playerIndex);

            // emulate missing controller 2 with keyboard
            if (playerIndex == PlayerIndex.Two && gamePadState.IsConnected == false)
            {
                var state = Keyboard.GetState();
                var leftThumbStick = new Vector2(
                    state.IsKeyDown(Keys.Left) ? -1.0f :
                    state.IsKeyDown(Keys.Right) ? 1.0f : 0.0f,
                    state.IsKeyDown(Keys.Up) ?    1.0f :
                    state.IsKeyDown(Keys.Down) ? -1.0f : 0.0f);
                var buttons = new List<Buttons>();
                if (state.IsKeyDown(Keys.A)) { buttons.Add(Buttons.B); }
                if (state.IsKeyDown(Keys.S)) { buttons.Add(Buttons.A); }
                if (state.IsKeyDown(Keys.D)) { buttons.Add(Buttons.Y); }
                if (state.IsKeyDown(Keys.Enter)) { buttons.Add(Buttons.RightShoulder); }
                if (state.IsKeyDown(Keys.RightShift)) { buttons.Add(Buttons.LeftShoulder); }
                if (state.IsKeyDown(Keys.Escape)) { buttons.Add(Buttons.Back); }

                gamePadState = new GamePadState(
                    leftThumbStick,
                    Vector2.Zero,
                    0.0f,
                    0.0f,
                    buttons.ToArray());
            }

            return gamePadState;
        }

        public static Dictionary<PlayerIndex, GamePadState> gamepads =
            new Dictionary<PlayerIndex, GamePadState>();
        public static Dictionary<PlayerIndex, GamePadState> gamepadsPrevious =
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
                gamepads[player] = Screen.GetState(player);

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
