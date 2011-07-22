using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestWinGame3
{
    class GameScreenInfo
    {
        SpriteFont TimerFont;
        string time = "";        
        TimeSpan timer, eTime, cTime;
        
        Vector2 tmrPoss = new Vector2(10, 10);              //poss show the game timer
        Vector2 CaveInfoPoss = new Vector2(10, 30);         //poss to hold info about where the caveman is
        Vector2 LevelWherePoss = new Vector2(10, 50);       //poss to hold where in the level we are
        Vector2 LevelDinoWherePoss = new Vector2(10, 70);   //poss to show where in the level the dino is   
        Vector2 EndOfLevelMessage = new Vector2(350, 10);   //poss to show the end of level message

        private string CaveManPossX,CaveManPossY;
        private int CaveManLevelPoss, DinoLevelPoss;
        public bool HasFinishedLevel;
        
        //load the content
        public void LoadContent(ContentManager theContentManager)
        {
            cTime = new TimeSpan(0, 0, 0);
            TimerFont = theContentManager.Load<SpriteFont>("Courier New");
        }

        public void Update(GameTime theGameTime, string mCaveManPossX, string mCaveManPossY, int mCaveManLevelPoss,int mDinoPoss)
        {
            UpdateGameTime(theGameTime);
            CaveManPossX = mCaveManPossX;
            CaveManPossY = mCaveManPossY;
            CaveManLevelPoss = mCaveManLevelPoss;
            DinoLevelPoss = mDinoPoss;
        }

        //Draw the info to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.DrawString(TimerFont, time.ToString(), tmrPoss, Color.White);
            theSpriteBatch.DrawString(TimerFont, "Poss X: " + CaveManPossX + " Poss Y: " + CaveManPossY, CaveInfoPoss, Color.White);
            theSpriteBatch.DrawString(TimerFont, "Caveman Level Possition: " + CaveManLevelPoss.ToString(),LevelWherePoss, Color.White);
            theSpriteBatch.DrawString(TimerFont, "Dino Level Possition: " + DinoLevelPoss.ToString(), LevelDinoWherePoss, Color.White);

            if (HasFinishedLevel)
            {
                string LevelMessage;

                if (DinoLevelPoss < CaveManLevelPoss)
                    LevelMessage = "You made it to the end ok!!!";
                else
                    LevelMessage = "The dino caught up with you!!!";

                theSpriteBatch.DrawString(TimerFont, LevelMessage, EndOfLevelMessage, Color.Red);
            }
        }

        //work out the string of the curr time ready to draw it
        private void UpdateGameTime(GameTime theGameTime)
        {
            timer += theGameTime.ElapsedGameTime;
            eTime = theGameTime.TotalGameTime;
            if (eTime.Minutes < 10)
                time = "Time - 0" + (int)(eTime.Minutes);
            else
                time = "Time - " + (int)(eTime.Minutes);
            if (eTime.Seconds < 10)
                time += ":0" + (int)(eTime.Seconds);
            else
                time += ":" + (int)(eTime.Seconds);
        }
    }
}
