using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TestWinGame3
{
    class CaveMan : Sprite
    {
        const string CAVEMAN_SPRITES = "caveman_sheet";
        const string RUN_SPRITE_REF = "Caveman_Run";                            //name of the refernce in the sprite sheet
        const int RUNNING_SPRITES = 6;                                          //how man sprites we have to run
        const int CAVEMAN_HEIGHT = 200;                                         //height of the caveman
        const int CAVEMAN_HEIGHT_JUMP = 180;                                    //height when jumping
        const int CAVEMAN_WIDTH_WALKING = 100;                                  //width of rect when in walking state
        const int CAVEMAN_WIDTH_JUMPING = 100;                                  //width of rect when in jumping state
        const int CAVEMAN_START_X = 150;            
        const int CAVEMAN_START_Y = 325;                
        const float RUNNING_ANIM_INT = 150f;                                    //how long between the sprite swap when running
        const float COLLIDED_ANIM_INT = 150F;                                   //how long to be in the air when collided
        const int CAVEMAN_DUCKING_HEIGHT = 100;                                 //what to size the collision rectangle to when ducking

        //JUMPING CONSTS
        const int CAVEMAN_JUMP_MOVE_UP = -1;
        const int CAVEMAN_JUMP_MOVE_DOWN = 1;
        const int CAVEMAN_JUMP_SPEED = 180;
        const int CAVEMAN_JUMP_HIGHT = 170;
        const int JUMP_SWAP = 150;
                        
        public Rectangle CurrRetPoss;           //rectangle poss of the caveman, used for collisions
        public bool ScrollTheBackGround;        //are we at a point where we want to scroll the background image

        State mCurrentState;
        State LastState;
        GetSpriteSheetInfo CavManSprites = new GetSpriteSheetInfo();
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        KeyboardState mPreviousKeyboardState;
        float timer = 0f;
        int currentFrame = 1;

        //track the cavemans jump
        Vector2 mStartingPosition = Vector2.Zero;

        //states the sprite can be in (walking, dead, ducking etc)
        enum State
        {
            Walking,
            Jumping,
            Ducking,
            Collided,
            Standing,
            Down
        }

        //load the content in
        public void LoadContent(ContentManager theContentManager)
        {
            //load in the info from the spritesheet            
            CavManSprites.LoadXMLSpriteSheet("Caveman_sheet_XML", theContentManager);

            Position = new Vector2(CAVEMAN_START_X, CAVEMAN_START_Y);            
            Source = CavManSprites.spriteSourceRectangles["Caveman_Standing"];
            CurrRetPoss = new Rectangle((int)Position.X - CAVEMAN_WIDTH_WALKING, (int)Position.Y, CAVEMAN_WIDTH_WALKING, CAVEMAN_HEIGHT);
            mCurrentState = State.Standing;
            ScrollTheBackGround = false;
            base.LoadContent(theContentManager, CAVEMAN_SPRITES);
        }

        //update the movement of the man
        public void Update(GameTime theGameTime, bool CaveManColl)
        {
            //Increase the timer by the number of milliseconds since update was last called
            timer += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
                        
            if (CaveManColl)
            {
                //update the collision image
                if (LastState != State.Collided)
                {
                    LastState = mCurrentState;
                    mCurrentState = State.Collided;                    
                }                
                UpdateCollision();
            }
            else
            {
                //no collision, default state to Standing and check for a run
                if (mCurrentState == State.Down)
                    mCurrentState = State.Standing;
                UpdateWalkingMovement(aCurrentKeyboardState);
                UpdateJump(aCurrentKeyboardState);
                UpdateDuck(aCurrentKeyboardState);
            }

            //save the last ketboard state
            mPreviousKeyboardState = aCurrentKeyboardState;

            //update the possition 
            base.Position.X = Position.X;
            base.Position.Y = Position.Y; 
            base.Update(theGameTime, mSpeed, mDirection);
        }

        /// <summary>
        /// update the walking movement
        /// </summary>
        /// <param name="theGameTime"></param>
        /// <param name="aCurrentKeyboardState"></param>
        private void UpdateWalkingMovement(KeyboardState aCurrentKeyboardState)
        {                        
            if (mCurrentState == State.Walking || mCurrentState == State.Standing)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                //we are walking
                if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {                    
                    //if we have gone over the timer, then time to change the image
                    if (timer > RUNNING_ANIM_INT)
                    {
                        currentFrame++;
                        timer = 0f;
                    }
                    //if we are on the last frame then reset it
                    if (currentFrame == RUNNING_SPRITES)
                            currentFrame = 1;

                    //update the level poss and the state
                    mCurrentState = State.Walking;
                    LastState = State.Walking;
                    base.mLevelPos++;
                    ScrollTheBackGround = true;

                    //update the source rectangle
                    Source = CavManSprites.spriteSourceRectangles[RUN_SPRITE_REF + currentFrame];
                }
                else
                {
                    //not pressed so make him stand
                    Source = CavManSprites.spriteSourceRectangles["Caveman_Standing"];
                    LastState = State.Walking;
                    mCurrentState = State.Standing;
                    ScrollTheBackGround = false;
                }                 
                                
                //update the current poss rectangle
                CurrRetPoss = new Rectangle((int)Position.X + (CAVEMAN_WIDTH_WALKING /2), (int)Position.Y, CAVEMAN_WIDTH_WALKING, CAVEMAN_HEIGHT);                                                                
            }                      
        }
        
        /// <summary>
        /// update the collision sprites, and set the states. 
        /// Work out if we were jumping at the time
        /// </summary>
        private void UpdateCollision()
        {
            if (mCurrentState == State.Collided)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;
                Position.Y = CAVEMAN_START_Y;
                LastState = State.Collided;
                mCurrentState = State.Down;
                Source = CavManSprites.spriteSourceRectangles["Caveman_Collision"];
                timer = 0f;
                CurrRetPoss = new Rectangle((int)Position.X + (CAVEMAN_WIDTH_WALKING / 2), (int)Position.Y, CAVEMAN_WIDTH_WALKING, CAVEMAN_HEIGHT);
                ScrollTheBackGround = false;
            }
            else if (mCurrentState == State.Down)
            {
                if (timer > COLLIDED_ANIM_INT)
                {
                    //we need to make sure the caveman is on the floor after being hit                    
                    mCurrentState = State.Walking;
                    mDirection = Vector2.Zero;
                    Source = CavManSprites.spriteSourceRectangles["Caveman_Knocked Down"];                    
                    timer = 0f;
                    CurrRetPoss = new Rectangle((int)Position.X + (CAVEMAN_WIDTH_WALKING / 2), (int)Position.Y, CAVEMAN_WIDTH_WALKING, CAVEMAN_HEIGHT);
                    ScrollTheBackGround = false;
                }                                               
            }                       
        }

        /// <summary>
        /// Update the jump state of the caveman
        /// </summary>
        /// <param name="aCurrentKeyboardState">State of the keyboard</param>
        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            //check for the space key and the fact we are in a state where we can jump
            if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                if (mCurrentState == State.Walking || mCurrentState == State.Standing)
                {
                    Jump();
                }                
            }

            //now check for the jump status, and ajust the poss
            if (mCurrentState == State.Jumping)
            {
                //on the way down so put the jump into negative
                if (mStartingPosition.Y - Position.Y > CAVEMAN_JUMP_HIGHT)
                {
                    mDirection.Y = CAVEMAN_JUMP_MOVE_DOWN;
                    

                    if (LastState == State.Standing)                    
                            Source = CavManSprites.spriteSourceRectangles["Caveman_Standing Jump2"];
                    else if (LastState == State.Walking)
                            Source = CavManSprites.spriteSourceRectangles["Caveman_Running Jump2"];
                }
                
                if (Position.Y > mStartingPosition.Y)
                {
                    Position.Y = mStartingPosition.Y;
                    mCurrentState = State.Walking;
                    ScrollTheBackGround = true;
                    mDirection = Vector2.Zero;
                }

                //update the current poss rectangle and the level poss
                CurrRetPoss = new Rectangle((int)Position.X + (CAVEMAN_WIDTH_JUMPING / 2), (int)Position.Y, CAVEMAN_WIDTH_JUMPING, CAVEMAN_HEIGHT_JUMP);
                if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    base.mLevelPos++;
                    ScrollTheBackGround = true;
                }
                    
            }    
        }

        /// <summary>
        /// Make the caveman start a jump
        /// </summary>
        private void Jump()
        {
            if (mCurrentState != State.Jumping)
            {                
                mStartingPosition = Position;
                mDirection.Y = CAVEMAN_JUMP_MOVE_UP;
                mSpeed = new Vector2(CAVEMAN_JUMP_SPEED, CAVEMAN_JUMP_SPEED);

                //swap the sprite to the first jumper
                if (mCurrentState == State.Standing)
                    Source = CavManSprites.spriteSourceRectangles["Caveman_Standing Jump1"];
                else if (mCurrentState == State.Walking)
                    Source = CavManSprites.spriteSourceRectangles["Caveman_Running Jump1"];

                //update the state
                LastState = mCurrentState;
                mCurrentState = State.Jumping;
            }
        }


        /// <summary>
        /// Update the ducking state of the caveman
        /// </summary>
        /// <param name="aCurrentKeyboardState">State of the keyboard</param>
        private void UpdateDuck(KeyboardState aCurrentKeyboardState)
        {
            if (aCurrentKeyboardState.IsKeyDown(Keys.RightShift) == true)
            {
                //start the ducking
                Duck();
            }
            else
            {
                if (mCurrentState == State.Ducking)
                {
                    //if we are ducking, see if we need to stop
                    Source = CavManSprites.spriteSourceRectangles["Caveman_Standing"];
                    LastState = mCurrentState;
                    mCurrentState = State.Standing;
                }
            }            
        }

        /// <summary>
        /// Make the caveman start to duck
        /// </summary>
        private void Duck()
        {
            if (mCurrentState == State.Standing)
            {
                //no movement
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                //swap to the ducking sprite
                Source = CavManSprites.spriteSourceRectangles["Caveman_Duck"];
                LastState = mCurrentState;
                mCurrentState = State.Ducking;
                CurrRetPoss = new Rectangle((int)Position.X + (CAVEMAN_WIDTH_WALKING / 2), (int)Position.Y + CAVEMAN_DUCKING_HEIGHT, CAVEMAN_WIDTH_WALKING, CAVEMAN_DUCKING_HEIGHT);                
                ScrollTheBackGround = false;
            }
        }
        
    }
}
