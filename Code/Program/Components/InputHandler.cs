using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VOiD.Components
{
    public class InputHandler : GameComponent
    {
        static KeyboardState keyboardState;
        static KeyboardState lastKeyboardState;
        static MouseState mouseState;
        static MouseState lastMouseState;

        public InputHandler(Game game)
            : base(game)
        {
            keyboardState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        public static int MouseY
        {
            get { return mouseState.Y; }
        }

        public static int MouseX
        {
            get { return mouseState.X; }
        }

        public static bool MiddleClickReleased
        {
            get
            {
                return (mouseState.MiddleButton == ButtonState.Released) &&
                (lastMouseState.MiddleButton == ButtonState.Pressed);
            }
        }

        public static bool MiddleClickPressed
        {
            get
            {
                return (mouseState.MiddleButton == ButtonState.Pressed) &&
                    (lastMouseState.MiddleButton == ButtonState.Released);
            }
        }

        public static bool MiddleClickUp
        {
            get
            {
                return mouseState.MiddleButton == ButtonState.Released;
            }
        }

        public static bool MiddleClickDown
        {
            get
            {
                return mouseState.MiddleButton == ButtonState.Pressed;
            }
        }

        public static bool RightClickReleased
        {
            get
            {
                return (mouseState.RightButton == ButtonState.Released) &&
                    (lastMouseState.RightButton == ButtonState.Pressed);
            }
        }

        public static bool RightClickPressed
        {
            get
            {
                return (mouseState.RightButton == ButtonState.Pressed) &&
                    (lastMouseState.RightButton == ButtonState.Released);
            }
        }

        public static bool RightClickUp
        {
            get
            {
                return mouseState.RightButton == ButtonState.Released;
            }
        }

        public static bool RightClickDown
        {
            get
            {
                return mouseState.RightButton == ButtonState.Pressed;
            }
        }
        

        public static bool LeftClickReleased
        {
            get
            {
                return (mouseState.LeftButton == ButtonState.Released) &&
                    (lastMouseState.LeftButton == ButtonState.Pressed);
            }
        }

        public static bool LeftClickPressed
        {
            get
            {
                return (mouseState.LeftButton == ButtonState.Pressed) &&
                    (lastMouseState.LeftButton == ButtonState.Released);
            }
        }

        public static bool LeftClickUp
        {
            get
            {
                return mouseState.LeftButton == ButtonState.Released;
            }
        }

        public static bool LeftClickDown
        {
            get
            {
                return mouseState.LeftButton == ButtonState.Pressed;
            }
        }

        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
                lastKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
                lastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
    }
}
