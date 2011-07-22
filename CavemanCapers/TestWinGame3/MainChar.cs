using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestWinGame3
{
    class MainChar : Sprite
    {        
        const float ANI_INTERVAL = 150f;            //interval between sprites        
        const int SPRITE_WIDTH = 213;               //width of one of the sprites
        const int SPRITE_HEIGHT = 510;              //height of one of the sprites
        const int TOT_SPRITES = 7;                  //total number of sprites in the sheet   
        const int POSS_X = 400;                     //X poss of the holding rectangle
        const int POSS_Y = 465;                     //Y poss of the holding rectangle
        const int JUMP_HIGHT = 100;

        private Texture2D m_texture;                //Texture to load the image into
        private Rectangle m_charRectangle;          //where the background image possitioned on the screen
        KeyboardState m_currentState;               //current state of the keyboard
        KeyboardState m_previousState;              //previous keyboard state
        private Vector2 origin;                     //where the image will be
        float timer = 0f;
        int currentFrame = 1;

        //states the sprite can be in (walking, dead, stuck etc)
        enum State
        {
            Running,
            Jumping,
            Ducking,
            Standing
        }

        State mCurrentState = State.Standing;

        //load in the stuff in
        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            Position = new Vector2(POSS_X, POSS_Y);

            //this is the rectangle we are going to fit the image in            
            m_texture = backgroundTexture; 
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(m_texture, new Vector2(POSS_X, POSS_Y), m_charRectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }

        //public update method, calls the other funtions
        public void Update(GameTime theGametime) 
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
           
            //Keyboard States
            m_previousState = m_currentState;
            m_currentState = Keyboard.GetState();
                                       
            //update all the states
            CharStateMove(theGametime);            
            CharStanding();
            
        }

        //set the stat of the char based on if we are holding right
        private void CharStateMove(GameTime theGameTime)
        {
            if (m_currentState.IsKeyDown(Keys.Right))
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

                //build the new rectangle
                m_charRectangle = new Rectangle((currentFrame * SPRITE_WIDTH), 0, SPRITE_WIDTH, SPRITE_HEIGHT);
                origin = new Vector2(m_charRectangle.Width / 2, m_charRectangle.Height / 2);
                State mCurrentState = State.Running;
            }            
        }

        //set the man back to staning if nothing is pressed
        private void CharStanding()
        {
            if (!m_currentState.IsKeyDown(Keys.Right))
            {
                //reset the frame back to the start
                currentFrame = 1;

                //draw only the first (standing) sprite
                m_charRectangle = new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
                origin = new Vector2(m_charRectangle.Width / 2, m_charRectangle.Height / 2);

                State mCurrentState = State.Standing;
            }            
        }
                  



    }
}
