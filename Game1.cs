using System;
using System.Linq;
using System.Collections.Generic;
using ConsoleApplication3.Abilities;
using ConsoleApplication3.Conditions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConsoleApplication3
{
    /*
     * Animated Sprites - http://www.gamedev.net/topic/586876-pow-studios-free-sprite-animations/
     * Tile Kit - https://www.youtube.com/watch?v=lnRRDSYgdmw
     * Dungeon Crawl - http://opengameart.org/content/dungeon-crawl-32x32-tiles
     * HUD - http://opengameart.org/content/shiny-hud
     * Tile Inspiration - http://www.david-amador.com/2013/12/quest-of-dungeons-development-update/
     * Fonts - http://www.fontsquirrel.com/ - http://www.fontsquirrel.com/fonts/exo
     * Sprite Inspiration - http://forums.terraria.org/index.php?threads/elemental.2304/
     * 
     * Interesting drop off with - sin((x+1)^x) in range of 0..1
     */


    /*
     * Stage 1
     *   Showing Enemey Info
     *   -Showing Party Info
     *   Determining what is in the surrounding area
     *   Controlling a Unit
     *   Camera Work to Center on the target and player
     *   
     * Starge 2
     *   Input Commands
     *   Targeting Enemies
     * 
     * 
     * 
     * 
     */

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Random rand = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont arialFont;
        Level level = new Level();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            var enemies = new List<Character>();
            for (var i = 0; i < 5; i++)
            {
                var ch = CreateEnemy("Enemy " + i);

                ch.ActionList.Add(new Action2(Condition.FoeNearest, new AbilityAttack()));
                //ch.ActionList.Add(Action.HealSelf);
                enemies.Add(ch);
            }

            //enemies[enemies.Count - 1].ActionList.Insert(0, new Action()
            //{
            //    Ability = new AbilityHeal(),
            //    Condition = new ConditionHPLessThan(75),
            //    Target = new TargetAliveEnemy(),
            //});
            level.allCharacters.AddRange(enemies);
            //level.enemy = new CharacterGroup(enemies.ToArray());

            var players = new List<Character>();
            for (var i = 0; i < 3; i++)
            {
                var ch = CreatePlayer("Player " + i);
                if (i > 0)
                    ch.ActionList.Add(new Action2(Condition.FoePartyLeaderTarget, new AbilityAttack()));

                ch.ActionList.Add(new Action2(Condition.FoeNearest, new AbilityAttack()));
                //ch.ActionList.Add(Action.HealSelf);
                players.Add(ch);
            }

            //players[0].APRechargeRate = 1.5f;
            players[players.Count - 1].ActionList.Insert(0, new Action2(Condition.AllyAnyLessThan80, new AbilityHeal()));
            level.allCharacters.AddRange(players);
            //level.player = new CharacterGroup(players.ToArray());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            arialFont = Content.Load<SpriteFont>("Fonts/FiraSans");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here
            var foes = level.Foes.Where(x => x.IsAlive).Count();
            //if (rand.Next(0, 12) > foes)
            {
                if (rand.NextFloat() < 0.005f + gameTime.TotalGameTime.TotalSeconds * gameTime.TotalGameTime.TotalSeconds / 10000f / 60f)
                {
                    var ch = CreateEnemy("Enemy ");
                    ch.ActionList.Add(new Action2(Condition.FoeNearest, new AbilityAttack()));
                    level.allCharacters.Add(ch);
                }
            }

            foreach (var enemy in level.Foes)
            {
                if (enemy.IsAlive)
                    enemy.Turn(level, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            foreach (var player in level.Allies)
            {
                if (player.IsAlive)
                    player.Turn(level, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var chars = new List<Character>(level.Characters);
            for (var i = 0; i < chars.Count; i++)
            {
                for (var k = i + 1; k < chars.Count; k++)
                {
                    var left = chars[i];
                    var right = chars[k];

                    var distSq = (left.Size * left.Size + right.Size * right.Size) * 4;
                    var distance = Vector2.DistanceSquared(left.Position, right.Position);
                    if (distance < distSq)
                    {
                        var amount = 1f - (distSq / distance);
                        var dir = left.Position - right.Position;
                        dir.Normalize();

                        left.Position += dir * dt * 100;
                        right.Position -= dir * dt * 100;
                    }
                }
            }

            //Console.WriteLine();


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (var enemy in level.Foes)
                enemy.Draw(spriteBatch);

            foreach (var player in level.Allies)
                player.Draw(spriteBatch);


            spriteBatch.End();

            DrawHUD();

            base.Draw(gameTime);
        }

        private void DrawHUD()
        {
            spriteBatch.Begin();
            var index = 0;
            var scale = 1f;
            var lineHeight = arialFont.LineSpacing * scale;
            var meterHeight = 10;
            var meterSize = new Vector2(100, 0);
            var chargeMeterOffset = new Vector2(100, lineHeight * 0.5f) * scale;
            var healthMeterOffset = new Vector2(225, lineHeight * 0.5f) * scale;
            //var manaMeterOffset = new Vector2(350, lineHeight * 0.5f) * scale;
            var actionTextOffset = new Vector2(350, 0) * scale;

            var apLength = new Vector2(arialFont.MeasureString("AP: ").X, 0);

            foreach (var player in level.Allies)
            {
                var linePosition = new Vector2(0, index * lineHeight);

                spriteBatch.DrawString(arialFont, player.Name, linePosition, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

                spriteBatch.DrawLine(linePosition + chargeMeterOffset, linePosition + chargeMeterOffset + meterSize, Color.Gray, meterHeight);
                spriteBatch.DrawLine(linePosition + chargeMeterOffset, linePosition + chargeMeterOffset + meterSize * (player.AP / 100f), Color.Green, meterHeight);

                spriteBatch.DrawLine(linePosition + healthMeterOffset, linePosition + healthMeterOffset + meterSize, Color.Gray, meterHeight);
                spriteBatch.DrawLine(linePosition + healthMeterOffset, linePosition + healthMeterOffset + meterSize * player.HPPercent / 100f, Color.Red, meterHeight);

                spriteBatch.DrawString(arialFont, player.LastAction, linePosition + actionTextOffset, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

                index++;
            }

            index = 0;
            scale = 0.75f;
            var enemyHealth = new Vector2(100, 0);
            foreach (var enemy in level.Foes)
            {
                if (enemy.IsAlive)
                {
                    var linePosition = new Vector2(0, index * lineHeight + 200);

                    spriteBatch.DrawString(arialFont, enemy.Name, linePosition, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

                    spriteBatch.DrawLine(linePosition, linePosition + meterSize, Color.Gray, meterHeight);
                    spriteBatch.DrawLine(linePosition, linePosition + meterSize * enemy.HPPercent / 100f, Color.Red, meterHeight);

                    index++;
                }
            }

            spriteBatch.End();

        }

        public Character CreateEnemy(string name)
        {
            var ch = new Character()
            {
                Name = name,
                Faction = Faction.Foe,
                Position = new Vector2(rand.NextFloat(0, 600), rand.NextFloat(0, 600)),
                IsAlive = true
            };

            var stats = ch.Stats;
            var hp = stats.AddBaseStat("hp_now", 50, 3);
            stats.AddBaseStat("hp_max", hp);

            var mp = stats.AddBaseStat("mp_now", 25, 3);
            stats.AddBaseStat("mp_max", mp);

            stats.AddBaseStat("attack_damage", 3, 3);
            stats.AddBaseStat("strength", 5, 3);
            stats.AddBaseStat("vitality", 5, 3);
            stats.AddBaseStat("intellect", 5, 3);
            stats.AddBaseStat("mind", 5, 3);
            stats.AddBaseStat("speed", 3, 2);

            return ch;
        }


        public Character CreatePlayer(string name)
        {
            var ch = new Character()
            {
                Name = name,
                Faction = Faction.Ally,
                Position = new Vector2(rand.NextFloat(0, 25) + 425, rand.NextFloat(0, 600)),
                IsAlive = true
            };

            var stats = ch.Stats;
            var hp = stats.AddBaseStat("hp_now", 70, 3);
            stats.AddBaseStat("hp_max", hp);

            var mp = stats.AddBaseStat("mp_now", 35, 3);
            stats.AddBaseStat("mp_max", mp);

            stats.AddBaseStat("attack_damage", 6, 4);
            stats.AddBaseStat("strength", 5, 3);
            stats.AddBaseStat("vitality", 5, 3);
            stats.AddBaseStat("intellect", 5, 3);
            stats.AddBaseStat("mind", 5, 3);
            stats.AddBaseStat("speed", 3, 2);

            return ch;
        }

    }
}
