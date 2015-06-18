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
        public static Texture2D texProgress;

        public static Texture2D texHealthBar;
        public static Texture2D texHealthP1;
        public static Texture2D texHealthP2;
        public static Texture2D texHealthP3;
        public static Texture2D texHealthP4;

        public static Song music;

        public static List<SoundEffect> sndClangs = new List<SoundEffect>();
        //public static List<SoundEffect> sndGrunts = new List<SoundEffect>();
        public static List<SoundEffect> sndTaunts = new List<SoundEffect>();
        public static SoundEffect sndThud;

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

            texArena = Content.Load<Texture2D>("arena");
            texProgress = Content.Load<Texture2D>("debug-square-8x8");

            texHealthBar = Content.Load<Texture2D>("health/health-bar");
            texHealthP1 = Content.Load<Texture2D>("health/player-1");
            texHealthP2 = Content.Load<Texture2D>("health/player-2");
            texHealthP3 = Content.Load<Texture2D>("health/player-3");
            texHealthP4 = Content.Load<Texture2D>("health/player-4");

            music = Content.Load<Song>("arena-music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(music);

            sndClangs.Add(Content.Load<SoundEffect>("sounds/sfx/clang-1"));
            sndClangs.Add(Content.Load<SoundEffect>("sounds/sfx/clang-2"));
            sndClangs.Add(Content.Load<SoundEffect>("sounds/sfx/clang-3"));
            sndClangs.Add(Content.Load<SoundEffect>("sounds/sfx/grunt-1"));
            sndClangs.Add(Content.Load<SoundEffect>("sounds/sfx/grunt-2"));

            sndTaunts.Add(Content.Load<SoundEffect>("sounds/taunts/taunt-1"));
            sndTaunts.Add(Content.Load<SoundEffect>("sounds/taunts/taunt-2"));
            sndTaunts.Add(Content.Load<SoundEffect>("sounds/taunts/taunt-3"));
            sndTaunts.Add(Content.Load<SoundEffect>("sounds/taunts/taunt-4"));
            sndTaunts.Add(Content.Load<SoundEffect>("sounds/taunts/taunt-5"));
            sndTaunts.Add(Content.Load<SoundEffect>("sounds/taunts/taunt-6"));
            sndTaunts.Add(Content.Load<SoundEffect>("sounds/taunts/taunt-7"));
            sndTaunts.Add(Content.Load<SoundEffect>("sounds/taunts/taunt-8"));

            sndThud = Content.Load<SoundEffect>("sounds/sfx/thud");

            Actors.Actor.Actors.Add(PlayerIndex.One, player1);
            Actors.Actor.Actors.Add(PlayerIndex.Two, player2);
            Actors.Actor.Actors.Add(PlayerIndex.Three, player3);
            Actors.Actor.Actors.Add(PlayerIndex.Four, player4);

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
                }
            }
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
