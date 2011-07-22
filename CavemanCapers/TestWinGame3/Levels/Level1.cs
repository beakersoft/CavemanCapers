using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestWinGame3
{
    class Level1 : BaseLevel
    {                
        const int TOTAL_BOLDERS = 3;                //it is always +1 so we can work out the entry point in the loop based on how many times round
        const int BOLDER_LEVEL_GAP = 150;           //gap between the bolders
        const int FIRST_BOLDER_LOC = 150;           //where the fist bolder is
        
        public LevelObjects mBolder;
        public LevelObjects mFlyer;
        
        public void LoadLevelContent(ContentManager theContentManager)
        {
            //objects for each of the things in the level, first the bolders                        
            for (int i = 1; i < TOTAL_BOLDERS; i++)
            {
                mBolder = new LevelObjects();
                if (i == 1)
                {
                    //first object so work out the entry 
                    mBolder.LevelObjectEntry(12, 90, 90, 800, 440, "boulder2", theContentManager, 150f, FIRST_BOLDER_LOC, "Boulder_sheet_new", "Boulder");
                }
                else
                {
                    mBolder.LevelObjectEntry(12, 90, 90, 800, 440, "boulder2", theContentManager, 150f, BOLDER_LEVEL_GAP * i, "Boulder_sheet_new", "Boulder");
                }

                //add to the levels object list
                base.theLevelObjects.Add(mBolder);
            }

            //now the Pterodactyls 454, 454
            mFlyer = new LevelObjects();
            mFlyer.mSpriteEffect = SpriteEffects.FlipHorizontally;      //flip the graphic round so it come in the right way
            mFlyer.LevelObjectEntry(7, 229, 340, 800, 40, "Pterodactyl_Sheet2", theContentManager, 150f, 50, "Pterodactyl_Sheet_XML", "Pterodactyl3_Fly");
            base.theLevelObjects.Add(mFlyer);
            
            base.LevelEnd = 600;
            base.CurrLevelPoss = 0;
            base.LoadLevel(theContentManager);
        }

        public void UpdateLevel1(GameTime theGameTime, int LevelPoint,Rectangle CurrCaveManLoc,Game1 CurrGame)
        {
            //loop through all the bolder objects
            foreach (LevelObjects aCurrentBolder in base.theLevelObjects)
            {
                aCurrentBolder.UpdateObject(theGameTime, LevelPoint, CurrGame);
            }

            base.UpdateLevel(CurrCaveManLoc);                       
        }

        public void DrawLevel(SpriteBatch theSpriteBatch)
        {
            //draw all the bolder items
            foreach (LevelObjects aCurrentBolder in base.theLevelObjects)
            {
                aCurrentBolder.Draw(theSpriteBatch);
            }
        }

    }
}
