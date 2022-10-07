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
    internal class Enemy
    {
        private ContentManager content;

        //Enemy position
        private Vector2 enemyPosition = new Vector2(100, 100);

        // Sprites
        private Texture2D EnemyTexture;

        // Gegner Variablen
        public readonly List<Vector2> enemyPositions = new List<Vector2>();

        private Vector2 enemyStartPosition = new Vector2(100, 100);
        private float enemyRadius;
        private float enemySpeed = 1f;
        private Color enemyColor;

        // Zufallszahlen
        private Random random = new Random();

        public Enemy(ContentManager content)
        {
            this.content = content;
        }

        public float getEnemyRadius()
        {
            return enemyRadius;
        }



        public void LoadContent()
        {
            // Texturen laden
            EnemyTexture = content.Load<Texture2D>("enemy");

            // Radius der Feinde festlegen
            if (EnemyTexture != null)
            {
                if (EnemyTexture.Width > EnemyTexture.Height)
                {
                    enemyRadius = EnemyTexture.Width;
                }
                else
                {
                    enemyRadius = EnemyTexture.Height;
                }

                // Gegner erzeugen
                CreateEnemies();
            }
        }

        public void CreateEnemies()
        {
            // Feinde erzeugen
            Vector2 position = enemyStartPosition;
            position.X -= EnemyTexture.Width / 2;

            // Eine Zufallszahl zwischen 3 und 10 ermitteln
            int count = random.Next(3, 11);

            // Gegener erzeugen
            for (int i = 0; i < count; i++)
            {
                enemyPositions.Add(position);
                position.X += EnemyTexture.Width + 15f;
            }

            // Farbwert ändern
            switch (count)
            {
                case 3:
                    enemyColor = Color.Red;
                    break;
                case 4:
                    enemyColor = Color.Green;
                    break;
                case 5:
                    enemyColor = Color.Yellow;
                    break;
                case 6:
                    enemyColor = Color.Blue;
                    break;
                case 7:
                    enemyColor = Color.Magenta;
                    break;
                case 8:
                    enemyColor = Color.Yellow;
                    break;
                case 9:
                    enemyColor = Color.White;
                    break;
                case 10:
                    enemyColor = Color.DarkGreen;
                    break;
                default:
                    break;
            }
        }

        public void UpdateEnemies()
        {
            // Startposition verändern
            enemyStartPosition.X += enemySpeed;

            // Bewegungsrichtung umkehren
            if (enemyStartPosition.X > 250)
            {
                enemySpeed *= -1;
            }
            else if (enemyStartPosition.X < 100f)
            {
                enemySpeed *= -1;
            }

            // Alle Feinde abgeschossen? Dann Neue Gegener
            if (enemyPositions.Count == 0 && EnemyTexture != null)
            {
                CreateEnemies();
            }

            // Aktualisieren
            for (int i = 0; i < enemyPositions.Count; i++)
            {
                Vector2 position = enemyPositions[i];
                position.X += enemySpeed;
                enemyPositions[i] = position;
            }
        }

        public void DrawEnemy(SpriteBatch _spriteBatch)
        {
            // TODO
            // Die Liste mit allen Gegnern (enemyPositions) durchlaufen
            // und alle Feinde (EnemyTexture) zeichnen

            foreach (Vector2 v in enemyPositions)
            {
                _spriteBatch.Draw(EnemyTexture, v, Color.White);
            }
        }


    }
}
