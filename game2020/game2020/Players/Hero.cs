﻿using game2020.Animation;
using game2020.Animation.HeroAnimations;
using game2020.Commands;
using game2020.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game2020.Players
{
    public class Hero : ITransform
    {
        public Vector2 Position { get ; set; }
        public Rectangle CollisinRectangle { get; set; }

        Texture2D heroTexture;

        private IInputReader reader;
        private IGameCommand moveCommand;
        IEntityAnimation walkRight, walkLeft, walkDown, walkUp, currentAnimation;

        public Hero(Texture2D texture, IInputReader inputReader)
        {
            this.heroTexture = texture;
            walkRight = new WalkRightAnimation(texture, this);
            walkLeft = new WalkLeftAnimation(texture, this);
            walkDown = new WalkDownAnimation(texture, this);
            walkUp = new WalkUpAnimation(texture, this);
            currentAnimation = walkRight;

            //Read input for hero class
            this.reader = inputReader;

            moveCommand = new MoveCommand();
        }

        private void move(Vector2 _direction)
        {
            if (_direction.X == -1)
                currentAnimation = walkLeft;
            else if (_direction.X == 1)
                currentAnimation = walkRight;
            else if (_direction.Y == -1)
                currentAnimation = walkUp;
            else if (_direction.Y == 1)
                currentAnimation = walkDown;

            moveCommand.Execute(this, _direction);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            var direction = reader.ReadInput();

            if (direction.X != 0 || direction.Y != 0)
            {
                //animatie.Update(gameTime);
                currentAnimation.Update(gameTime);
                move(direction);
            }
        }
    }
}