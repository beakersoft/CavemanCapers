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
    //Class to run the main game play screen
    class GamePlayScreen
    {
        private Game1 mGame;
        private Level1 mCurrLevel;
        private CaveMan mCaveManSprite;
        private Dino mDinoSprite;
        private HorizontallyScrollingBackground mScrollingBackground;           //main background
        private GameScreenInfo mGameInfo;

        public GamePlayScreen(Game1 game)
        {
            this.mGame = game;

            mDinoSprite = new Dino();
            mCaveManSprite = new CaveMan();

            //load the level
            mCurrLevel = new Level1();
            mCurrLevel.LoadLevelContent(mGame.Content);            

            //load the content for the background in
            mScrollingBackground = new HorizontallyScrollingBackground(mGame.GraphicsDevice.Viewport);
            mScrollingBackground.AddBackground("Background01");
            mScrollingBackground.AddBackground("Background02");
            mScrollingBackground.AddBackground("Background03");
            mScrollingBackground.LoadContent(mGame.Content);

            //caveman object load
            mCaveManSprite.LoadContent(mGame.Content);

            //dino object load
            mDinoSprite.LoadContent(mGame.Content);

            //game info
            mGameInfo = new GameScreenInfo();
            mGameInfo.LoadContent(mGame.Content);
        }

        public void Update(GameTime gameTime)
        {                        
            //update the level
            mCurrLevel.CurrLevelPoss = mCaveManSprite.mLevelPos;
            mCurrLevel.CaveManPoss = mCaveManSprite.Position;
            mCurrLevel.UpdateLevel1(gameTime, mCaveManSprite.mLevelPos, mCaveManSprite.CurrRetPoss, mGame);

            //update the caveman
            mCaveManSprite.Update(gameTime, mCurrLevel.CaveManColl);

            if (mCurrLevel.AreAtLevelEnd)
            {
                mGameInfo.HasFinishedLevel = true;
            }

            if (mCaveManSprite.ScrollTheBackGround)
            {
                //update the background 
                mScrollingBackground.Update(gameTime, 50, HorizontallyScrollingBackground.HorizontalScrollDirection.Left);
            }

            //update the screen info
            mGameInfo.Update(gameTime, mCaveManSprite.Position.X.ToString(), mCaveManSprite.Position.Y.ToString(), mCaveManSprite.mLevelPos,
                mCurrLevel.DinoCurrentPoss);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int bw = 2; // Border width
            var t = new Texture2D(mGame.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });

            //draw the background
            mScrollingBackground.Draw(spriteBatch);

            //draw the game info
            mGameInfo.Draw(spriteBatch);

            //draw the level info
            mCurrLevel.DrawLevel(spriteBatch);

            //draw the caveman
            mCaveManSprite.Draw(spriteBatch);

            //draw the boarders round the boulder
            spriteBatch.Draw(t, new Rectangle(mCurrLevel.mBolder.CurrRetPoss.Left, mCurrLevel.mBolder.CurrRetPoss.Top, bw, mCurrLevel.mBolder.CurrRetPoss.Height), Color.Black); // Left
            spriteBatch.Draw(t, new Rectangle(mCurrLevel.mBolder.CurrRetPoss.Right, mCurrLevel.mBolder.CurrRetPoss.Top, bw, mCurrLevel.mBolder.CurrRetPoss.Height), Color.Black); // Right
            spriteBatch.Draw(t, new Rectangle(mCurrLevel.mBolder.CurrRetPoss.Left, mCurrLevel.mBolder.CurrRetPoss.Top, mCurrLevel.mBolder.CurrRetPoss.Width, bw), Color.Black); // Top
            spriteBatch.Draw(t, new Rectangle(mCurrLevel.mBolder.CurrRetPoss.Left, mCurrLevel.mBolder.CurrRetPoss.Bottom, mCurrLevel.mBolder.CurrRetPoss.Width, bw), Color.Black); // Bottom

            //draw the boarders round the caveman
            spriteBatch.Draw(t, new Rectangle(mCaveManSprite.CurrRetPoss.Left, mCaveManSprite.CurrRetPoss.Top, bw, mCaveManSprite.CurrRetPoss.Height), Color.Black); // Left
            spriteBatch.Draw(t, new Rectangle(mCaveManSprite.CurrRetPoss.Right, mCaveManSprite.CurrRetPoss.Top, bw, mCaveManSprite.CurrRetPoss.Height), Color.Black); // Right
            spriteBatch.Draw(t, new Rectangle(mCaveManSprite.CurrRetPoss.Left, mCaveManSprite.CurrRetPoss.Top, mCaveManSprite.CurrRetPoss.Width, bw), Color.Black); // Top
            spriteBatch.Draw(t, new Rectangle(mCaveManSprite.CurrRetPoss.Left, mCaveManSprite.CurrRetPoss.Bottom, mCaveManSprite.CurrRetPoss.Width, bw), Color.Black); // Bottom

            //draw boarders round the pterodactyl
            spriteBatch.Draw(t, new Rectangle(mCurrLevel.mFlyer.CurrRetPoss.Left, mCurrLevel.mFlyer.CurrRetPoss.Top, bw, mCurrLevel.mFlyer.CurrRetPoss.Height), Color.Black); // Left
            spriteBatch.Draw(t, new Rectangle(mCurrLevel.mFlyer.CurrRetPoss.Right, mCurrLevel.mFlyer.CurrRetPoss.Top, bw, mCurrLevel.mFlyer.CurrRetPoss.Height), Color.Black); // Right
            spriteBatch.Draw(t, new Rectangle(mCurrLevel.mFlyer.CurrRetPoss.Left, mCurrLevel.mFlyer.CurrRetPoss.Top, mCurrLevel.mFlyer.CurrRetPoss.Width, bw), Color.Black); // Top
            spriteBatch.Draw(t, new Rectangle(mCurrLevel.mFlyer.CurrRetPoss.Left, mCurrLevel.mFlyer.CurrRetPoss.Bottom, mCurrLevel.mFlyer.CurrRetPoss.Width, bw), Color.Black); // Top
        }

    }
}
