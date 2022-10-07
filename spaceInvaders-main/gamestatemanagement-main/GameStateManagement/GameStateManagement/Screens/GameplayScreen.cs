#region File Description

//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using GameStateManagement.Screens.GamePlayScreenComponent;
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

        //player object
        private Player player;

        //Enemy object
        private Enemy enemy;

        //StarField Object
        private StarField starField;

        //Score Object
        private Score score;

        //Laser object
        private Laser laser;


        private float pauseAlpha;

        #endregion Fields

        #region Initialization


        // Tastatur abfragen
        private KeyboardState currentKeyboardState;

        private KeyboardState previousKeyboardState;

        // Sprites
        /*
         * Ersetzt
        private Texture2D LaserTexture;

        // Laser Variablen
        private List<Vector2> laserShots = new List<Vector2>();

        private float laserSpeed = 10f;
       
        // Sound Effekte
        private SoundEffect laserSound;
        private SoundEffect explosionSound;
        */

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

            //Texturen for Player laden und das Raumschiff positionieren
            player = new Player(content, ScreenManager);
            player.LoadContent();

            // Texturen for Enemy laden und Radius der Feinde festlegen
            enemy = new Enemy(content);
            enemy.LoadContent();

            //Texturen for Background laden
            starField = new StarField(content);
            starField.LoadContent();

            //Texturen for Score laden
            score = new Score(content,player);
            score.LoadContent();

            //Texturen for Laser laden
            if (player == null)
            {
                Console.WriteLine("Null...............................,,,,,,,,,,,,,,,,,,,,,,,,,,.,.");
            }
            laser = new Laser(content, enemy, player);
            laser.LoadContent();

            // TODO
            // Texturen laden
            /* Ersetzt
            LaserTexture = content.Load<Texture2D>("laser");


            // TODO
            // Sounds laden
            explosionSound = content.Load<SoundEffect>("explosion");
            laserSound = content.Load<SoundEffect>("laserfire");
            */
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion Initialization


        //#region Input Operationen

        public bool IsNewKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) &&
                    !previousKeyboardState.IsKeyDown(key);
        }

        /* Ersetzt
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

        #endregion


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

                    while (enemyIndex < Enemy.enemyPositions.Count)
                    {
                        // Abstand zwischen Feind-Position und Schuss-Position ermitteln
                        float distance = Vector2.Distance(Enemy.enemyPositions[enemyIndex], laserShots[laserIndex]);

                        // Treffer?
                        if (distance < enemy.getEnemyRadius())
                        {
                            // Schuss entfernen
                            laserShots.RemoveAt(laserIndex);
                            // Feind entfernen
                            Enemy.enemyPositions.RemoveAt(enemyIndex);
                            // Punkte erhöhen
                            Player.playerScore++;

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
        #endregion
        */
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
                    player.MoveShipLeft();
                }

                // Right
                if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    player.MoveShipRight();
                }

                // Space
                if (IsNewKeyPressed(Keys.Space))
                {
                    //Ersetzt//FireLaser();
                    laser.FireLaser();
                }

                previousKeyboardState = currentKeyboardState;

                enemy.UpdateEnemies();

                //Ersetzt//UpdateLaserShots();
                laser.UpdateLaserShots();


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

                //playerPosition += movement * 2;
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
            starField.DrawBackground(_spriteBatch);

            // Das Schiff zeichnen
            player.DrawSpaceShip(_spriteBatch);

            // Laser zeichnen
            //Ersetzt//DrawLaser(_spriteBatch);
            laser.DrawLaser(_spriteBatch);

            // Feinde zeichnen
            enemy.DrawEnemy(_spriteBatch);

            // Punkte anzeigen
            score.DrawScore(_spriteBatch);

            _spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        
        /*
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
        */

        #endregion Update and Draw
    }
}