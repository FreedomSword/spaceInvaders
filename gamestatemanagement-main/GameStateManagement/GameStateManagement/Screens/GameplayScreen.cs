#region File Description

//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

#endregion Using Statements

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    internal class GameplayScreen : GameScreen
    {
        #region Fields
        private ContentManager content;
        private SpriteFont gameFont;

        private Vector2 playerPosition = new Vector2(100, 100);
        private Vector2 enemyPosition = new Vector2(100, 100);



        private float pauseAlpha;

        #endregion Fields

        #region Initialization


        // Font
        private SpriteFont spriteFont;



        // Zufallszahlen
        private Random random = new Random();

        // Tastatur abfragen
        private KeyboardState currentKeyboardState;

        private KeyboardState previousKeyboardState;

        // Sprites
        private Texture2D ShipTexture;

        private Texture2D StarTexture;
        private Texture2D LaserTexture;
        private Texture2D EnemyTexture;

        // Raumschiff Variablen
        private Vector2 shipPosition;

        private float shipSpeed = 5f;

        // Laser Variablen
        private List<Vector2> laserShots = new List<Vector2>();

        private float laserSpeed = 10f;

        // Gegner Variablen
        private readonly List<Vector2> enemyPositions = new List<Vector2>();

        private Vector2 enemyStartPosition = new Vector2(100, 100);
        private float enemyRadius;
        private float enemySpeed = 1f;
        private Color enemyColor;

        // Sound Effekte
        private SoundEffect laserSound;
        private SoundEffect explosionSound;


        // Spieler-Punkte und Zeichenposition der Punkte
        private int playerScore;

        private Vector2 scorePosition;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);



            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();




            // TODO
            // Texturen laden
            ShipTexture = content.Load<Texture2D>("ship");
            StarTexture = content.Load<Texture2D>("starfield");
            LaserTexture = content.Load<Texture2D>("laser");
            EnemyTexture = content.Load<Texture2D>("enemy");


            // TODO
            // Font laden
            spriteFont = content.Load<SpriteFont>("Verdana");

            // TODO
            // Sounds laden
            explosionSound = content.Load<SoundEffect>("explosion");
            laserSound = content.Load<SoundEffect>("laserfire");


            // Das Raumschiff positionieren
            shipPosition.X = ScreenManager.GraphicsDevice.Viewport.Width / 2;
            shipPosition.Y = ScreenManager.GraphicsDevice.Viewport.Height - 100;

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

            // Position der Score Ausgabe festlegen
            scorePosition = new Vector2(25, 25);
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion Initialization


        #region Gegner erzeugen

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

        #endregion

        #region Input Operationen

        public bool IsNewKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) &&
                    !previousKeyboardState.IsKeyDown(key);
        }

        public void FireLaser()
        {
            // aktuelle Position des Schiffes auf dem Bildschirm speichern
            Vector2 position = shipPosition;

            // Laserschuss vor das Schiff mittig platzieren
            position.Y -= ShipTexture.Height / 2;
            position.X -= LaserTexture.Width / 2;

            // Position in der Liste speichern
            laserShots.Add(position);

            PlayLaserSound();
        }

        #endregion

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

        #region Update von Lasern und Gegnern

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

                    while (enemyIndex < enemyPositions.Count)
                    {
                        // Abstand zwischen Feind-Position und Schuss-Position ermitteln
                        float distance = Vector2.Distance(enemyPositions[enemyIndex], laserShots[laserIndex]);

                        // Treffer?
                        if (distance < enemyRadius)
                        {
                            // Schuss entfernen
                            laserShots.RemoveAt(laserIndex);
                            // Feind entfernen
                            enemyPositions.RemoveAt(enemyIndex);
                            // Punkte erhöhen
                            playerScore++;

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

        #endregion

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
        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }

            if (IsActive)
            {
                currentKeyboardState = Keyboard.GetState();

                // Left
                if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    MoveShipLeft();
                }

                // Right
                if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    MoveShipRight();
                }

                // Space
                if (IsNewKeyPressed(Keys.Space))
                {
                    FireLaser();
                }

                previousKeyboardState = currentKeyboardState;

                UpdateEnemies();

                UpdateLaserShots();


            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    movement.X--;
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    movement.X++;
                }

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    movement.Y--;
                }

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    movement.Y++;
                }

                Vector2 thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                {
                    movement.Normalize();
                }

                playerPosition += movement * 2;
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch _spriteBatch = ScreenManager.SpriteBatch;

            _spriteBatch.Begin();

            // Hintergrund zeichnen
            DrawBackground(_spriteBatch);

            // Das Schiff zeichnen
            DrawSpaceShip(_spriteBatch);

            // Laser zeichnen
            DrawLaser(_spriteBatch);

            // Feinde zeichnen
            DrawEnemy(_spriteBatch);

            // Punkte anzeigen
            DrawScore(_spriteBatch);

            
            _spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void DrawBackground(SpriteBatch _spriteBatch)
        {
            // TODO
            // Die Sternenfeld Grafik an der Position 0,0 zeichnen
            _spriteBatch.Draw(StarTexture, new Vector2(0, 0), Color.White);
        }

        private void DrawSpaceShip(SpriteBatch _spriteBatch)
        {
            // TODO


            _spriteBatch.Draw(ShipTexture, shipPosition, Color.White);


            // Das Schiff mittig an den Koordinaten des Schiffes (shipPosition) zeichnen
        }

        private void DrawLaser(SpriteBatch _spriteBatch)
        {
            // TODO
            // Die Liste mit den Laser-Schüssen (laserShots) durchlaufen
            // und alle Schüsse (LaserTexture) zeichnen
            foreach (Vector2 v in laserShots)
            {
                _spriteBatch.Draw(LaserTexture, v, Color.White);
            }

        }

        private void DrawEnemy(SpriteBatch _spriteBatch)
        {
            // TODO
            // Die Liste mit allen Gegnern (enemyPositions) durchlaufen
            // und alle Feinde (EnemyTexture) zeichnen

            foreach (Vector2 v in enemyPositions)
            {
                _spriteBatch.Draw(EnemyTexture, v, Color.White);
            }
        }

        private void DrawScore(SpriteBatch _spriteBatch)
        {
            // TODO
            // Die Punkte (playerScore) oben links (scorePosition) anzeigen

            _spriteBatch.DrawString(spriteFont, playerScore.ToString(), scorePosition, Color.Red);
        }

        #endregion Update and Draw
    }
}