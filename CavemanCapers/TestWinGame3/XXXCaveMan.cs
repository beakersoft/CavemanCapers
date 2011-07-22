using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestWinGame3
{
    class CaveMan : Sprite
    {
        const string CAVEMAN_ASSETNAME = "arthur_run_75mm_box";
        const string CAVEMAN_SPRITES = "caveman_sheet";
        const int START_POSITION_X = 165;
        const int START_POSITION_Y = 220;           
        const int WIZARD_SPEED = 170;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const int JUMP_HIGHT = 150;
        const int SCREEN_EDGE_LEFT = -35;                           //boundry of the left of the screen
        const int SCREEN_EDGE_RIGHT = 630;
        const float ANI_INTERVAL = 150f;                            //interval between sprites
        const int TOT_SPRITES = 7;                                  //total number of sprites in the sheet
        const int SPRITE_WIDTH = 200;                               //width of one of the sprites
        const int SPRITE_HEIGHT = 200;                              //height of one of the sprites

        const int RECT_POSS_X = 205;
        const int RECT_POSS_Y = 270;
        
        float timer = 0f;
        int currentFrame = 1;
        public Rectangle CurrRetPoss;
        
        //track the jump of the wizard
        Vector2 mStartingPosition = Vector2.Zero;

        //states the sprite can be in (walking, dead, stuck etc)
        enum State
        {
            Walking,
            Jumping,
            Ducking
        }

        State mCurrentState = State.Walking;
        State mSecondState = State.Walking;                                         //so we can have 2 states to check on, to jump while ducking etc

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        KeyboardState mPreviousKeyboardState;

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, CAVEMAN_SPRITES);
            Source = new Rectangle(603, 603, SPRITE_WIDTH, SPRITE_HEIGHT);                                  //start with the standing image
            CurrRetPoss = new Rectangle(RECT_POSS_X, RECT_POSS_Y, 200, 210);
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState, theGameTime);
            UpdateJump(aCurrentKeyboardState);
            UpdateDuck(aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;

            if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
            {
                //update the possition of the char on the level
                base.mLevelPos++;                
            }

            CurrRetPoss = new Rectangle(RECT_POSS_X, RECT_POSS_Y, 200, 210);
            base.Position.X = Position.X;
            base.Position.Y = Position.Y;
            base.Update(theGameTime, mSpeed, mDirection);
        }

        //movement method
        private void UpdateMovement(KeyboardState aCurrentKeyboardState, GameTime theGameTime)
        {
            //check we are in a walking state
            if (mCurrentState == State.Walking)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                //left key and not on the screen boundry
                if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    //Increase the timer by the number of milliseconds since update was last called
                    timer += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;

                    //if its time to move on then chnage the frame and reset the counter
                    if (timer > ANI_INTERVAL)
                    {
                        currentFrame++;
                        timer = 0f;
                    }

                    //if we are on the last frame then reset it
                    if (currentFrame == TOT_SPRITES)
                    {
                        currentFrame = 1;
                    }
                    
                    //update the source rectangle
                    Source = new Rectangle((currentFrame * SPRITE_WIDTH), 0, SPRITE_WIDTH, SPRITE_HEIGHT);
                }
                else
                {
                    //back to default standing
                    currentFrame = 1;
                    Source = new Rectangle(603, 603, SPRITE_WIDTH, SPRITE_HEIGHT);
                }                
            }                        
        }

        //jump methods
        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking | mCurrentState == State.Ducking)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
                {
                    Jump();
                }
            }

            if (mCurrentState == State.Jumping)
            {
                if (mStartingPosition.Y - Position.Y > JUMP_HIGHT)
                {
                    mDirection.Y = MOVE_DOWN;
                }

                if (Position.Y > mStartingPosition.Y)
                {
                    Position.Y = mStartingPosition.Y;
                    mCurrentState = State.Walking;
                    mDirection = Vector2.Zero;
                }
            }
        }

        private void Jump()
        {
            if (mCurrentState != State.Jumping)
            {
                mCurrentState = State.Jumping;
                mStartingPosition = Position;
                mDirection.Y = MOVE_UP;
                mSpeed = new Vector2(WIZARD_SPEED, WIZARD_SPEED);
            }
        }

        private void UpdateDuck(KeyboardState aCurrentKeyboardState)
        {
            if (aCurrentKeyboardState.IsKeyDown(Keys.RightShift) == true)
            {
                Duck();
            }
            else
            {
                StopDucking();
            }
        }

        private void Duck()
        {
            if (mCurrentState == State.Walking)
            {
                //stop it moving
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                //swap to the jumping sprite
                Source = new Rectangle(200, 0, 200, Source.Height);
                mCurrentState = State.Ducking;
                mSecondState = State.Ducking;
            }
            else if (mCurrentState == State.Jumping)
            {
                //we are jumping so just swap the sprite
                Source = new Rectangle(200, 0, 200, Source.Height);
                mSecondState = State.Ducking;
            }
        }

        private void StopDucking()
        {
            if (mCurrentState == State.Ducking | mSecondState == State.Ducking)
            {
                Source = new Rectangle(0, 0, 200, Source.Height);

                //check to see if we are jumping, if we are the state needs to be kept at jumping
                if (mCurrentState != State.Jumping)
                {
                    mCurrentState = State.Walking;
                }
                
            }
        }


    }
}
