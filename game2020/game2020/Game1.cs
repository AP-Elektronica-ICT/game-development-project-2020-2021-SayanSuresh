﻿using Game1;
using game2020.Animation.HeroAnimations;
using game2020.Collision;
using game2020.Commands;
using game2020.GameScreen;
using game2020.Input;
using game2020.Interfaces;
using game2020.Players;
using game2020.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RefactoringCol;
using System;
using System.Diagnostics;

namespace game2020
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        IScreenUpdater screenUpdater;
        IGameCommand gameCommand;

        Camera camera;
        CollisionManager collisionManager;

        Tiles tile;
        Level1 lv1;

        private Texture2D textureHero;
        private Hero hero;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenUpdater = new ScreenUpdate();
            //screenUpdater.UpdateScreen(_graphics, 1280, 720);
            //screenUpdater.UpdateScreen(_graphics, 1480, 620);

            tile = new Tiles();

            gameCommand = new MoveCommand();

            collisionManager = new CollisionManager(new CollisionHelper());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            camera = new Camera(GraphicsDevice.Viewport);

            Tiles.Content = Content;
            //tile.Content = Content;
            lv1 = new Level1();

            textureHero = Content.Load<Texture2D>("Players/thief");


            InitialzeGameObjects();
        }

        private void InitialzeGameObjects()
        {
            hero = new Hero(textureHero, new KeyBoardReader(), gameCommand);
            hero.HeroWalkAnimation(new WalkRightAnimation(textureHero, hero), new WalkLeftAnimation(textureHero, hero),
                                   new WalkUpAnimation(textureHero, hero), new WalkDownAnimation(textureHero, hero));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            hero.Update(gameTime);

            foreach (CollisionTiles tile in lv1.CollisionTiles)
            {
                collisionManager.UpdateCollision(hero.CollisionRectangle, tile.Rectangle, lv1.Width, lv1.Height, hero);
                camera.Update(hero.Position, lv1.Width, lv1.Height);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BurlyWood);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred,
                               BlendState.AlphaBlend,
                               null, null, null, null,
                               camera.Transform);

            lv1.Draw(_spriteBatch);

            hero.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
