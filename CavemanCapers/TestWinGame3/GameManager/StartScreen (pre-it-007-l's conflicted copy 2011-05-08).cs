using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TestWinGame3.GameManager
{
    //class to handle the state of the start (welcome screen)
    class StartScreen
    {
        const float PRESS_ENTER_ANIM_INT = 150F;

        private Sprite mBackGroundImage;
        private Sprite mPressEnterMsg;
        private Game1 mGame;
        private KeyboardState mLastState;


        //load all the bits in we need
        public StartScreen(Game1 game)
        {
            this.mGame = game;
            mBackGroundImage = new Sprite();
            mPressEnterMsg = new Sprite();
            mBackGroundImage.LoadContent(mGame.Content, "TitleScreen");
            mPressEnterMsg.LoadContent(mGame.Content, "pressStart");
            mPressEnterMsg.Position.X = 130;
            mPressEnterMsg.Position.Y = 550;
                      
            mLastState = Keyboard.GetState();
        }

        //update routine, check the keyboard input to see if we are starting the game, or going to load/options etc
        public void Update()
        {
            KeyboardState CurrKeyBoardState = Keyboard.GetState();

            if (CurrKeyBoardState.IsKeyDown(Keys.Enter) && mLastState.IsKeyUp(Keys.Enter))
            {
                //new keypress of enter, so start the game
                mGame.StartGame();
            }

            mLastState = CurrKeyBoardState;
        }

        //draw the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            mBackGroundImage.Draw(spriteBatch);
            mPressEnterMsg.Draw(spriteBatch);
        }

    }
}
