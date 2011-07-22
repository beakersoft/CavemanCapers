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
        private Sprite mBackGroundImage;        
        private Game1 mGame;
        private KeyboardState mLastState;
        private GetSpriteSheetInfo StartMenuSprites = new GetSpriteSheetInfo();
        public Vector2 Position = new Vector2(210, 350);

        private Texture2D mStartSpriteTexture;
        private Rectangle SourceStart,SourceOptions,SourceLoad,SourceExit;
        OptionSelected CurrSelection;

        enum OptionSelected
        {
            StartGame,
            Loadgame,
            Options,
            Exit
        }

        //load all the bits in we need
        public StartScreen(Game1 game)
        {
            //load the text graphics in       
            StartMenuSprites.LoadSpriteInfo("Content\\Game_Text.txt");
            this.mGame = game;
            mBackGroundImage = new Sprite();
            mBackGroundImage.LoadContent(mGame.Content, "TitleScreen");

            mStartSpriteTexture = mGame.Content.Load<Texture2D>("GameText");
            SourceStart = StartMenuSprites.spriteSourceRectangles["Game Text_Start Game copy"];
            SourceOptions = StartMenuSprites.spriteSourceRectangles["Game Text_Options"];
            SourceLoad = StartMenuSprites.spriteSourceRectangles["Game Text_Load Game"];
            SourceExit = StartMenuSprites.spriteSourceRectangles["Game Text_Exit"];

            CurrSelection = OptionSelected.StartGame;
            mLastState = Keyboard.GetState();
        }

        //update routine, check the keyboard input to see if we are starting the game, or going to load/options etc
        public void Update(GameTime theGameTime)
        {
            KeyboardState CurrKeyBoardState = Keyboard.GetState();  
    
            //first check to see if enter has been hit, if it has do the option we are on
            if (CurrKeyBoardState.IsKeyDown(Keys.Enter) && mLastState.IsKeyUp(Keys.Enter))
            {
                switch (CurrSelection)
                {
                    case OptionSelected.StartGame:
                        //start a new game from the begining
                        mGame.StartGame();
                        break;
                    case OptionSelected.Loadgame:
                        break;
                    case OptionSelected.Options:
                        break;
                    case OptionSelected.Exit:
                        //quit the game
                        mGame.Exit();
                        break;
                    default:
                        break;
                }                                    
            }
            
            //now check for the game state so we know the menu option, then check for up/down key and change the state
            switch (CurrSelection)
            {
                case OptionSelected.StartGame:
                    if (CurrKeyBoardState.IsKeyDown(Keys.Down) && mLastState.IsKeyUp(Keys.Down))
                    {
                        SourceStart = StartMenuSprites.spriteSourceRectangles["Game Text_Start Game"];
                        SourceLoad = StartMenuSprites.spriteSourceRectangles["Game Text_Load Game copy"];
                        CurrSelection = OptionSelected.Loadgame;
                    }
                    break;
                case OptionSelected.Loadgame:
                    if (CurrKeyBoardState.IsKeyDown(Keys.Down) && mLastState.IsKeyUp(Keys.Down))
                    {
                        SourceLoad = StartMenuSprites.spriteSourceRectangles["Game Text_Load Game"];
                        SourceOptions = StartMenuSprites.spriteSourceRectangles["Game Text_Options copy"];                        
                        CurrSelection = OptionSelected.Options;
                    }
                    if (CurrKeyBoardState.IsKeyDown(Keys.Up) && mLastState.IsKeyUp(Keys.Up))
                    {
                        SourceLoad = StartMenuSprites.spriteSourceRectangles["Game Text_Load Game"];
                        SourceStart = StartMenuSprites.spriteSourceRectangles["Game Text_Start Game copy"];
                        CurrSelection = OptionSelected.StartGame;
                    }
                    break;
                case OptionSelected.Options:

                    if (CurrKeyBoardState.IsKeyDown(Keys.Down) && mLastState.IsKeyUp(Keys.Down))
                    {
                        SourceExit = StartMenuSprites.spriteSourceRectangles["Game Text_Exit copy"];
                        SourceOptions = StartMenuSprites.spriteSourceRectangles["Game Text_Options"];
                        CurrSelection = OptionSelected.Exit;
                    }
                    if (CurrKeyBoardState.IsKeyDown(Keys.Up) && mLastState.IsKeyUp(Keys.Up))
                    {
                        SourceOptions = StartMenuSprites.spriteSourceRectangles["Game Text_Options"];
                        SourceLoad = StartMenuSprites.spriteSourceRectangles["Game Text_Load Game copy"];
                        CurrSelection = OptionSelected.Loadgame;
                    }
                    break;
                case OptionSelected.Exit:
                    if (CurrKeyBoardState.IsKeyDown(Keys.Up) && mLastState.IsKeyUp(Keys.Up))
                    {
                        SourceExit = StartMenuSprites.spriteSourceRectangles["Game Text_Exit"];
                        SourceOptions = StartMenuSprites.spriteSourceRectangles["Game Text_Options copy"];
                        CurrSelection = OptionSelected.Options;
                    }
                    break;
                default:
                    break;
            }
            



            //remember the keyboard state
            mLastState = CurrKeyBoardState;
        }

        //draw the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            mBackGroundImage.Draw(spriteBatch);

            //start option
            spriteBatch.Draw(mStartSpriteTexture, Position, SourceStart,
                Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            //load
            spriteBatch.Draw(mStartSpriteTexture, new Vector2(210, 400), SourceLoad,
                Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0); 

            //options
            spriteBatch.Draw(mStartSpriteTexture, new Vector2(210, 450), SourceOptions,
                Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            //exit
            spriteBatch.Draw(mStartSpriteTexture, new Vector2(210, 500), SourceExit,
                Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);                           
        }

    }
}
