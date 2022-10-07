using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Screens.GamePlayScreenComponent
{
    internal class StarField
    {
        private ContentManager content;

        // Sprites
        private Texture2D StarTexture;

        public StarField(ContentManager content)
        {
            this.content = content;
        }

        public  void LoadContent()
        {
            StarTexture = content.Load<Texture2D>("starfield");
        }

        public void DrawBackground(SpriteBatch _spriteBatch)
        {
            // TODO
            // Die Sternenfeld Grafik an der Position 0,0 zeichnen
            _spriteBatch.Draw(StarTexture, new Vector2(0, 0), Color.White);
        }
    }
}
