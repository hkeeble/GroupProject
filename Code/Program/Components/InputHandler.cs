using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VOiD.Components
{
    public class InputHandler : GameComponent
    {
        static KeyboardState keyboardState;
        static KeyboardState lastKeyboardState;
        static public MouseState mouseState;

        public InputHandler(Game game)
            : base(game)
        {
            keyboardState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            base.Update(gameTime);
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
