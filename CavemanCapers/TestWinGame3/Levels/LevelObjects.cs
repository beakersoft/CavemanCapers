using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TestWinGame3
{
    class LevelObjects : Sprite
    {                               
        int mTotSprites;                        //how many sprites we have
        int mSpriteHeight;
        int mSpriteWidth;
        int mSpriteStartX;
        int mSpriteStartY;
        string mAssetName;
        string mSpriteKeyName;
        float mAnimiationInterval;              //how long between the sprite swap
        int mLevelEntryPoint;                   //where on the level we want to apear        

        float timer = 0f;
        int currentFrame = 1;
        Vector2 theDirection = new Vector2(-1, 0);
        Vector2 theSpeed = new Vector2(210,0);
        GetSpriteSheetInfo ObjectXMLSprites = new GetSpriteSheetInfo();        

        public Rectangle CurrRetPoss;
        public bool CheckHasCol;
        public bool iSObjectOnScreen;           //bool to see if the object is on screen

        public void LevelObjectEntry(int TotSprites, int SpriteHeight, int SpriteWidth,
            int SpriteStartX, int SpriteStartY, string AssetName, ContentManager theContentManager, 
            float AnimiationInterval, int LevelEntryPoint,string PathToSheetInfo, string SpriteKeyName)
        {
            this.mTotSprites = TotSprites;
            this.mSpriteHeight = SpriteHeight;
            this.mSpriteWidth = SpriteWidth;
            this.mSpriteStartX = SpriteStartX;
            this.mSpriteStartY = SpriteStartY;
            this.mAssetName = AssetName;
            this.mAnimiationInterval = AnimiationInterval;
            this.mLevelEntryPoint = LevelEntryPoint;
            this.mSpriteKeyName = SpriteKeyName;

            //load the contect in from the xml pipeline
            ObjectXMLSprites.LoadXMLSpriteSheet(PathToSheetInfo, theContentManager);

            //setup the initial possitions
            Position = new Vector2(mSpriteStartX, mSpriteStartY);
            base.LoadContent(theContentManager, mAssetName);
            Source = ObjectXMLSprites.spriteSourceRectangles[SpriteKeyName + "1"];
            CurrRetPoss = new Rectangle((int)Position.X, (int)Position.Y, mSpriteWidth, mSpriteHeight);            
            CheckHasCol = false;
            iSObjectOnScreen = false;
        }

        public void UpdateObject(GameTime theGameTime,int LevelPoint, Game1 CurrGame)
        {
            //Increase the timer by the number of milliseconds since update was last called
            timer += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;

            //set the bool to see if the object is on screen, if it is then we want to work with it
            if (LevelPoint >= mLevelEntryPoint && CurrRetPoss.X > (-mSpriteWidth - 5))
                iSObjectOnScreen = true;
            else
                iSObjectOnScreen = false;
             
            if (iSObjectOnScreen)
            {
                UpdateMovement(theGameTime);
                CheckHasCol = true;
                base.Update(theGameTime, theSpeed, theDirection);
            }
            else
            {
                CheckHasCol = false;
            }                         
        }

        private void UpdateMovement(GameTime theGameTime)
        {            
            //if its time to move on then change the frame and reset the counter
            if (timer > mAnimiationInterval)
            {
                currentFrame++;
                timer = 0f;
            }

            //if we are on the last frame then reset it
            if (currentFrame == mTotSprites)
            {
                currentFrame = 1;
            }

            //update the source rectangles            
            Source = ObjectXMLSprites.spriteSourceRectangles[mSpriteKeyName + currentFrame];
            CurrRetPoss = new Rectangle((int)Position.X, (int)Position.Y, mSpriteWidth, mSpriteHeight);
        }        
    }
}
