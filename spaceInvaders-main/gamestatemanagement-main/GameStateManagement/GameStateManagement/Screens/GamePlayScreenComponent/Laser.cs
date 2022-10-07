using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Screens.GamePlayScreenComponent
{
    internal class Laser
    {
        private ContentManager content;

        private Enemy enemy;

        private Player player;

        // Sprites
        private Texture2D LaserTexture;
        
        // Laser Variablen
        private List<Vector2> laserShots = new List<Vector2>();

        private float laserSpeed = 10f;

        // Sound Effekte
        private SoundEffect laserSound;
        private SoundEffect explosionSound;

        public Laser(ContentManager content, Enemy enemy, Player player)
        {
            this.content = content;
            this.enemy = enemy;
            this.player = player;
        }

        public void LoadContent()
        {

            // Texturen laden
            LaserTexture = content.Load<Texture2D>("laser");


            // TODO
            // Sounds laden
            explosionSound = content.Load<SoundEffect>("explosion");
            laserSound = content.Load<SoundEffect>("laserfire");

        }

        

        

        public void FireLaser()
        {
            // aktuelle Position des Schiffes auf dem Bildschirm speichern
            Vector2 position = player.getShipPosition();

            // Laserschuss vor das Schiff mittig platzieren
            position.Y -= player.getShipTexture().Height / 2;
            position.X -= LaserTexture.Width / 2;

            // Position in der Liste speichern
            laserShots.Add(position);

            PlayLaserSound();
        }

        public void UpdateLaserShots()
        {
            int laserIndex = 0;

            while (laserIndex < laserShots.Count)
            {
                // hat der Schuss den Bildschirm verlassen?
                if (laserShots[laserIndex].Y < 0)
                {
                    laserShots.RemoveAt(laserIndex);
                }
                else
                {
                    // Position des Schusses aktualiesieren
                    Vector2 pos = laserShots[laserIndex];
                    pos.Y -= laserSpeed;
                    laserShots[laserIndex] = pos;

                    // Überprüfen ob ein Treffer vorliegt
                    int enemyIndex = 0;

                    while (enemyIndex < enemy.enemyPositions.Count)
                    {
                        // Abstand zwischen Feind-Position und Schuss-Position ermitteln
                        float distance = Vector2.Distance(enemy.enemyPositions[enemyIndex], laserShots[laserIndex]);

                        // Treffer?
                        if (distance < enemy.getEnemyRadius())
                        {
                            // Schuss entfernen
                            laserShots.RemoveAt(laserIndex);
                            // Feind entfernen
                            enemy.enemyPositions.RemoveAt(enemyIndex);
                            // Punkte erhöhen
                            player.playerScore++;

                            PlayExplosionSound();

                            // Schleife verlassen
                            break;
                        }
                        else
                        {
                            enemyIndex++;
                        }
                    }
                    laserIndex++;
                }
            }
        }


        public void PlayExplosionSound()
        {
            // TODO
            // Explosions WAV abspielen
            explosionSound.Play();
        }

        public void PlayLaserSound()
        {
            // TODO
            // Laserschuss WAV abspielen
            laserSound.Play();
        }

        public void DrawLaser(SpriteBatch _spriteBatch)
        {
            // TODO
            // Die Liste mit den Laser-Schüssen (laserShots) durchlaufen
            // und alle Schüsse (LaserTexture) zeichnen
            foreach (Vector2 v in laserShots)
            {
                _spriteBatch.Draw(LaserTexture, v, Color.White);
            }

        }
    }
}
