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
    internal class Score
    {
        private ContentManager content;

        private Player player;

        // Font
        private SpriteFont spriteFont;
      
        private Vector2 scorePosition;


        public Score(ContentManager content, Player player)
        {
            this.content = content;
            this.player = player;
        }

        public  void LoadContent()
        {
            // Font laden
            spriteFont = content.Load<SpriteFont>("Verdana");

            // Position der Score Ausgabe festlegen
            scorePosition = new Vector2(25, 25);
        }

        public void DrawScore(SpriteBatch _spriteBatch)
        {
            // TODO
            // Die Punkte (playerScore) oben links (scorePosition) anzeigen

            _spriteBatch.DrawString(spriteFont, player.playerScore.ToString(), scorePosition, Color.Red);
        }
    }
}
