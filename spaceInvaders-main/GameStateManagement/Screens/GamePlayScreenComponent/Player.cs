using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Screens.GamePlayScreenComponent
{
    internal class Player
    {
        private ContentManager content;
        private ScreenManager screenManager;

        private Vector2 playerPosition = new Vector2(100, 100);

        // Sprites
        private Texture2D ShipTexture;

        // Raumschiff Variablen
        private Vector2 shipPosition;

        private float shipSpeed = 5f;

        // Spieler-Punkte und Zeichenposition der Punkte
        public int playerScore;

        public Player(ContentManager content, ScreenManager screenManager)
        {
            this.content = content;
            this.screenManager = screenManager;
        }


        public Vector2 getPlayerPosition()
        {
            return playerPosition;
        }

        public Texture2D getShipTexture()
        {
            return ShipTexture;
        }

        public Vector2 getShipPosition()
        {
            return shipPosition;
        }

        public float getShipSpeed()
        {
            return shipSpeed;
        }

        /*
        public int getPlayerScore()
        {
            return this.playerScore;
        }

        public void setPlayerScore(int score)
        {
            this.playerScore = score;
        }
        */
        public void LoadContent()
        {

            // Texturen laden
            ShipTexture = content.Load<Texture2D>("ship");

            // Das Raumschiff positionieren
            shipPosition.X = screenManager.GraphicsDevice.Viewport.Width / 2;
            shipPosition.Y = screenManager.GraphicsDevice.Viewport.Height - 100;

        }


        public void MoveShipLeft()
        {
            // TODO
            // Schiff nach links bewegen und verhindern, 
            // dass das Schiff den Bildschirm verlässt

            shipPosition.X -= shipSpeed;


        }

        public void MoveShipRight()
        {
            // TODO
            shipPosition.X += shipSpeed;

            // Schiff nach rechts bewegen und verhindern, 
            // dass das Schiff den Bildschirm verlässt
        }

        public void DrawSpaceShip(SpriteBatch _spriteBatch)
        {
            // TODO


            _spriteBatch.Draw(ShipTexture, shipPosition, Color.White);


            // Das Schiff mittig an den Koordinaten des Schiffes (shipPosition) zeichnen
        }

    }



}
