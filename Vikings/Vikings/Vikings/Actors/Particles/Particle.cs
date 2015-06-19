using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vikings.Actors.Particles
{
    public class Particle : Actor
    {
        public Vector2 Location = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Acceleration = Vector2.Zero;
        public Color Tint = Color.White;
        public double Health = 2.0;

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gametime)
        {
            //base.Update(gametime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gametime, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            //base.Draw(gametime, batch);
        }
    }
}
