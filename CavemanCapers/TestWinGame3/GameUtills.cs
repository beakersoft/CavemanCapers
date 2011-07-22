using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml;

namespace TestWinGame3
{
    //class to get the co-ords ect about a sprite sheet from the info text file
    class GetSpriteSheetInfo
    {
        //where we are holding the info
        public Dictionary<string, Rectangle> spriteSourceRectangles = new Dictionary<string, Rectangle>();
        
        //load the sprite info from a text file
        public void LoadSpriteInfo(string PathToSheet)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathToSheet);

            //open the file
            using (StreamReader reader = new StreamReader(path))
            {
                // while we're not done reading...
                while (!reader.EndOfStream)
                {
                    // get a line
                    string line = reader.ReadLine();

                    // split at the equals sign
                    string[] sides = line.Split('=');

                    // trim the right side and split based on spaces
                    string[] rectParts = sides[1].Trim().Split(' ');

                    // create a rectangle from those parts
                    Rectangle r = new Rectangle(
                       int.Parse(rectParts[0]),
                       int.Parse(rectParts[1]),
                       int.Parse(rectParts[2]),
                       int.Parse(rectParts[3]));

                    // add the name and rectangle to the dictionary
                    spriteSourceRectangles.Add(sides[0].Trim(), r);
                }
            }
        }

        //load the dictonary for the rectangles from an xml file
        public void LoadXMLSpriteSheet(string PathToXML, ContentManager theContentManager)
        {
            spriteSourceRectangles = theContentManager.Load<Dictionary<string, Rectangle>>(PathToXML);
        }

       
    }
}
