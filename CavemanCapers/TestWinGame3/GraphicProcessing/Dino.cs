using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TestWinGame3
{
    //class to handle the dino sprite, where it is, anmiation, collision etc
    class Dino : Sprite
    {
        const string DINO_SPRITES = "dino_test";            //name of the spritesheet
        const string DINO_SPRITES_NAME = "Allosaur3";       //name of ref in the spritesheet


        public Rectangle CurrDinoRetPoss;                                   //rectangle poss of the Dino, used for collisions
        GetSpriteSheetInfo DinoManSprites = new GetSpriteSheetInfo();       //Spritesheet containing the dinos sprites


        //load the dino objects stuff in
        public void LoadContent(ContentManager theContentManager)
        {
            //load in the info from the spritesheet            
            DinoManSprites.LoadXMLSpriteSheet("Dino_Sheet", theContentManager);

            base.LoadContent(theContentManager, DINO_SPRITES);
        }






    }
}
