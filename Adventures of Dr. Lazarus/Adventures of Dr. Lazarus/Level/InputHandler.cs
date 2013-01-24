using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class InputHandler : Microsoft.Xna.Framework.GameComponent
{
    static KeyboardState keyboardState;
    static KeyboardState lastKeyboardState;
    static GamePadState gamepadState;
    static GamePadState lastGamepadState;

    public InputHandler(Game game)
        : base(game)
    {
        keyboardState = Keyboard.GetState();
        gamepadState = GamePad.GetState(PlayerIndex.One);
    }

    public override void Update(GameTime gameTime)
    {
        lastKeyboardState = keyboardState;
        keyboardState = Keyboard.GetState();
        lastGamepadState = gamepadState;
        gamepadState = GamePad.GetState(PlayerIndex.One);
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