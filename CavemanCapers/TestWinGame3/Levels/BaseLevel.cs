using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestWinGame3
{
    class BaseLevel
    {
        //where we are in the level
        public int CurrLevelPoss;

        //where the  dino is in the level
        public int DinoCurrentPoss;

        //have we got to the end
        public bool AreAtLevelEnd;

        //list of the level objects
        public List<LevelObjects> theLevelObjects = new List<LevelObjects>();

        //possision of the caveman in the level
        public Vector2 CaveManPoss;

        //has the caveman collied with something
        public bool CaveManColl;
                
        //the start poss of the level, always going to be 0 (on the x axis)
        private int mLevelStart;
        public int LevelStart
        {
            get { return mLevelStart; }

            set { mLevelStart = 0; }
        }

        //the end poss of the level
        private int mLevelEnd;
        public int LevelEnd 
        {
            get { return mLevelEnd; }

            set
            {
                mLevelEnd = value;
            }
        }
        
        public void LoadLevel(ContentManager theContentManager)
        {            
            CaveManColl = false;
            AreAtLevelEnd = false;
            DinoCurrentPoss = -200;     //poss of the dino, he starts back from the caveman            
        }

        public void UpdateLevel(Rectangle CaveManRectangle)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            foreach (LevelObjects aCurrentObject in theLevelObjects)
            {
                if (aCurrentObject.CheckHasCol)
                    CheckCaveManCol(CaveManRectangle, aCurrentObject.CurrRetPoss);
                else
                    CaveManColl = false;
                
                //if we have a hit, drop right out
                if (CaveManColl) break;
            }
                                      
            //check for end of level
            if (CurrLevelPoss >= mLevelEnd)
                AreAtLevelEnd = true;
            else
                //increment the dinos poss, he never stops running!!
                DinoCurrentPoss++;
        }

        //check to see if we have hit a sprite, if we have set the collision to true
        private void CheckCaveManCol(Rectangle CaveManRectangle, Rectangle SpriteToCheck)
        {
            if (SpriteToCheck.Intersects(CaveManRectangle))
                CaveManColl = true;
            else
                CaveManColl = false;                                   
        }                
    }
}
