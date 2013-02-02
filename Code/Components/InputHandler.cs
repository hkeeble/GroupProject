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

        /// <summary>
        /// Initiates update states for the mouse and keyboard.
        /// </summary>
        /// <param name="game">The game which you want to attch this to.</param>
        public InputHandler(Game game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Game.Exit();
            base.Update(gameTime);
        }

        /// <summary>
        /// Returns the mouse Y value (vertical).
        /// </summary>
        public static int MouseY
        {
            get { return mouseState.Y; }
        }

        /// <summary>
        /// Returns the mouse X value (horizontal).
        /// </summary>
        public static int MouseX
        {
            get { return mouseState.X; }
        }

        /// <summary>
        /// Returns if the middle mouse button has been released.
        /// </summary>
        public static bool MiddleClickReleased
        {
            get
            {
                return (mouseState.MiddleButton == ButtonState.Released) &&
                (lastMouseState.MiddleButton == ButtonState.Pressed);
            }
        }

        /// <summary>
        /// Returns if the middle mouse button is pressed.
        /// </summary>
        public static bool MiddleClickPressed
        {
            get
            {
                return (mouseState.MiddleButton == ButtonState.Pressed) &&
                    (lastMouseState.MiddleButton == ButtonState.Released);
            }
        }

        /// <summary>
        /// Returns if the middle mouse button is up.
        /// </summary>
        public static bool MiddleClickUp
        {
            get
            {
                return mouseState.MiddleButton == ButtonState.Released;
            }
        }

        /// <summary>
        /// Returns if the middle mouse button is down.
        /// </summary>
        public static bool MiddleClickDown
        {
            get
            {
                return mouseState.MiddleButton == ButtonState.Pressed;
            }
        }

        /// <summary>
        /// Returns if the right mouse button has been released.
        /// </summary>
        public static bool RightClickReleased
        {
            get
            {
                return (mouseState.RightButton == ButtonState.Released) &&
                    (lastMouseState.RightButton == ButtonState.Pressed);
            }
        }

        /// <summary>
        /// Returns if the right mouse button is pressed.
        /// </summary>
        public static bool RightClickPressed
        {
            get
            {
                return (mouseState.RightButton == ButtonState.Pressed) &&
                    (lastMouseState.RightButton == ButtonState.Released);
            }
        }

        /// <summary>
        /// Returns if the right mouse button is up.
        /// </summary>
        public static bool RightClickUp
        {
            get
            {
                return mouseState.RightButton == ButtonState.Released;
            }
        }

        /// <summary>
        /// Returns if the right mouse button is down.
        /// </summary>
        public static bool RightClickDown
        {
            get
            {
                return mouseState.RightButton == ButtonState.Pressed;
            }
        }

        /// <summary>
        /// Returns if the left mouse button has been released.
        /// </summary>
        public static bool LeftClickReleased
        {
            get
            {
                return (mouseState.LeftButton == ButtonState.Released) &&
                    (lastMouseState.LeftButton == ButtonState.Pressed);
            }
        }

        /// <summary>
        /// Returns if the left mouse button is pressed.
        /// </summary>
        public static bool LeftClickPressed
        {
            get
            {
                return (mouseState.LeftButton == ButtonState.Pressed) &&
                    (lastMouseState.LeftButton == ButtonState.Released);
            }
        }

        /// <summary>
        /// Returns if the left mouse button is up.
        /// </summary>
        public static bool LeftClickUp
        {
            get
            {
                return mouseState.LeftButton == ButtonState.Released;
            }
        }

        /// <summary>
        /// Returns if the left mouse button is down.
        /// </summary>
        public static bool LeftClickDown
        {
            get
            {
                return mouseState.LeftButton == ButtonState.Pressed;
            }
        }

        /// <summary>
        /// Returns if the key has been released.
        /// </summary>
        /// <param name="key">Identifies a particual key on the keyboard.</param>
        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
                lastKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns if the key has been pressed.
        /// </summary>
        /// <param name="key">Identifies a particual key on the keyboard.</param>
        public static bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
                lastKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Returns if the key is held down.
        /// </summary>
        /// <param name="key">Identifies a particual key on the keyboard.</param>
        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
    }
}
