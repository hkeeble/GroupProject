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


namespace VOiD.Components
{
    public class SpriteManager : GameComponent
    {
        private static SpriteBatch spriteBatch;

        public SpriteManager(Game game)
            : base(game)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        // Summary:
        //     Begins a sprite batch operation using deferred sort and default state objects
        //     (BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None,
        //     RasterizerState.CullCounterClockwise).
        public static void Begin()
        {
            spriteBatch.Begin();
        }
        //
        // Summary:
        //     Begins a sprite batch operation using the specified sort and blend state
        //     object and default state objects (DepthStencilState.None, SamplerState.LinearClamp,
        //     RasterizerState.CullCounterClockwise). If you pass a null blend state, the
        //     default is BlendState.AlphaBlend.
        //
        // Parameters:
        //   sortMode:
        //     Sprite drawing order.
        //
        //   blendState:
        //     Blending options.
        public static void Begin(SpriteSortMode sortMode, BlendState blendState)
        {
            spriteBatch.Begin(sortMode, blendState);
        }
        //
        // Summary:
        //     Begins a sprite batch operation using the specified sort, blend, sampler,
        //     depth stencil and rasterizer state objects. Passing null for any of the state
        //     objects selects the default default state objects (BlendState.AlphaBlend,
        //     SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise).
        //
        // Parameters:
        //   sortMode:
        //     Sprite drawing order.
        //
        //   blendState:
        //     Blending options.
        //
        //   samplerState:
        //     Texture sampling options.
        //
        //   depthStencilState:
        //     Depth and stencil options.
        //
        //   rasterizerState:
        //     Rasterization options.
        public static void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
        {
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState);
        }
        //
        // Summary:
        //     Begins a sprite batch operation using the specified sort, blend, sampler,
        //     depth stencil and rasterizer state objects, plus a custom effect. Passing
        //     null for any of the state objects selects the default default state objects
        //     (BlendState.AlphaBlend, DepthStencilState.None, RasterizerState.CullCounterClockwise,
        //     SamplerState.LinearClamp). Passing a null effect selects the default SpriteBatch
        //     Class shader.
        //
        // Parameters:
        //   sortMode:
        //     Sprite drawing order.
        //
        //   blendState:
        //     Blending options.
        //
        //   samplerState:
        //     Texture sampling options.
        //
        //   depthStencilState:
        //     Depth and stencil options.
        //
        //   rasterizerState:
        //     Rasterization options.
        //
        //   effect:
        //     Effect state options.
        public static void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
        {
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect);
        }
        //
        // Summary:
        //     Begins a sprite batch operation using the specified sort, blend, sampler,
        //     depth stencil, rasterizer state objects, plus a custom effect and a 2D transformation
        //     matrix. Passing null for any of the state objects selects the default default
        //     state objects (BlendState.AlphaBlend, DepthStencilState.None, RasterizerState.CullCounterClockwise,
        //     SamplerState.LinearClamp). Passing a null effect selects the default SpriteBatch
        //     Class shader.
        //
        // Parameters:
        //   sortMode:
        //     Sprite drawing order.
        //
        //   blendState:
        //     Blending options.
        //
        //   samplerState:
        //     Texture sampling options.
        //
        //   depthStencilState:
        //     Depth and stencil options.
        //
        //   rasterizerState:
        //     Rasterization options.
        //
        //   effect:
        //     Effect state options.
        //
        //   transformMatrix:
        //     Transformation matrix for scale, rotate, translate options.
        public static void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        //
        // Summary:
        //     Adds a sprite to a batch of sprites for rendering using the specified texture,
        //     destination rectangle, and color. Reference page contains links to related
        //     code samples.
        //
        // Parameters:
        //   texture:
        //     A texture.
        //
        //   destinationRectangle:
        //     A rectangle that specifies (in screen coordinates) the destination for drawing
        //     the sprite.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        public static void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            spriteBatch.Draw(texture, destinationRectangle, color);
        }
        //
        // Summary:
        //     Adds a sprite to a batch of sprites for rendering using the specified texture,
        //     position and color. Reference page contains links to related code samples.
        //
        // Parameters:
        //   texture:
        //     A texture.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        public static void Draw(Texture2D texture, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, position, color);
        }
        //
        // Summary:
        //     Adds a sprite to a batch of sprites for rendering using the specified texture,
        //     destination rectangle, source rectangle, and color.
        //
        // Parameters:
        //   texture:
        //     A texture.
        //
        //   destinationRectangle:
        //     A rectangle that specifies (in screen coordinates) the destination for drawing
        //     the sprite. If this rectangle is not the same size as the source rectangle,
        //     the sprite will be scaled to fit.
        //
        //   sourceRectangle:
        //     A rectangle that specifies (in texels) the source texels from a texture.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        public static void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }
        //
        // Summary:
        //     Adds a sprite to a batch of sprites for rendering using the specified texture,
        //     position, source rectangle, and color.
        //
        // Parameters:
        //   texture:
        //     A texture.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   sourceRectangle:
        //     A rectangle that specifies (in texels) the source texels from a texture.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        public static void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color);
        }
        //
        // Summary:
        //     Adds a sprite to a batch of sprites for rendering using the specified texture,
        //     destination rectangle, source rectangle, color, rotation, origin, effects
        //     and layer.
        //
        // Parameters:
        //   texture:
        //     A texture.
        //
        //   destinationRectangle:
        //     A rectangle that specifies (in screen coordinates) the destination for drawing
        //     the sprite. If this rectangle is not the same size as the source rectangle,
        //     the sprite will be scaled to fit.
        //
        //   sourceRectangle:
        //     A rectangle that specifies (in texels) the source texels from a texture.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        //
        //   rotation:
        //     Specifies the angle (in radians) to rotate the sprite about its center.
        //
        //   origin:
        //     The sprite origin; the default is (0,0) which represents the upper-left corner.
        //
        //   effects:
        //     Effects to apply.
        //
        //   layerDepth:
        //     The depth of a layer. By default, 0 represents the front layer and 1 represents
        //     a back layer. Use SpriteSortMode if you want sprites to be sorted during
        //     drawing.
        public static void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }
        //
        // Summary:
        //     Adds a sprite to a batch of sprites for rendering using the specified texture,
        //     position, source rectangle, color, rotation, origin, scale, effects, and
        //     layer. Reference page contains links to related code samples.
        //
        // Parameters:
        //   texture:
        //     A texture.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   sourceRectangle:
        //     A rectangle that specifies (in texels) the source texels from a texture.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        //
        //   rotation:
        //     Specifies the angle (in radians) to rotate the sprite about its center.
        //
        //   origin:
        //     The sprite origin; the default is (0,0) which represents the upper-left corner.
        //
        //   scale:
        //     Scale factor.
        //
        //   effects:
        //     Effects to apply.
        //
        //   layerDepth:
        //     The depth of a layer. By default, 0 represents the front layer and 1 represents
        //     a back layer. Use SpriteSortMode if you want sprites to be sorted during
        //     drawing.
        public static void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }
        //
        // Summary:
        //     Adds a sprite to a batch of sprites for rendering using the specified texture,
        //     position, source rectangle, color, rotation, origin, scale, effects and layer.
        //     Reference page contains links to related code samples.
        //
        // Parameters:
        //   texture:
        //     A texture.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   sourceRectangle:
        //     A rectangle that specifies (in texels) the source texels from a texture.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        //
        //   rotation:
        //     Specifies the angle (in radians) to rotate the sprite about its center.
        //
        //   origin:
        //     The sprite origin; the default is (0,0) which represents the upper-left corner.
        //
        //   scale:
        //     Scale factor.
        //
        //   effects:
        //     Effects to apply.
        //
        //   layerDepth:
        //     The depth of a layer. By default, 0 represents the front layer and 1 represents
        //     a back layer. Use SpriteSortMode if you want sprites to be sorted during
        //     drawing.
        public static void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }
        //
        // Summary:
        //     Adds a string to a batch of sprites for rendering using the specified font,
        //     text, position, and color.
        //
        // Parameters:
        //   spriteFont:
        //     A font for diplaying text.
        //
        //   text:
        //     A text string.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        public static void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(spriteFont, text, position, color);
        }
        //
        // Summary:
        //     Adds a string to a batch of sprites for rendering using the specified font,
        //     text, position, and color.
        //
        // Parameters:
        //   spriteFont:
        //     A font for diplaying text.
        //
        //   text:
        //     Text string.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        public static void DrawString(SpriteFont spriteFont, System.Text.StringBuilder text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(spriteFont, text, position, color);
        }
        //
        // Summary:
        //     Adds a string to a batch of sprites for rendering using the specified font,
        //     text, position, color, rotation, origin, scale, effects and layer.
        //
        // Parameters:
        //   spriteFont:
        //     A font for diplaying text.
        //
        //   text:
        //     A text string.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        //
        //   rotation:
        //     Specifies the angle (in radians) to rotate the sprite about its center.
        //
        //   origin:
        //     The sprite origin; the default is (0,0) which represents the upper-left corner.
        //
        //   scale:
        //     Scale factor.
        //
        //   effects:
        //     Effects to apply.
        //
        //   layerDepth:
        //     The depth of a layer. By default, 0 represents the front layer and 1 represents
        //     a back layer. Use SpriteSortMode if you want sprites to be sorted during
        //     drawing.
        public static void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        //
        // Summary:
        //     Adds a string to a batch of sprites for rendering using the specified font,
        //     text, position, color, rotation, origin, scale, effects and layer.
        //
        // Parameters:
        //   spriteFont:
        //     A font for diplaying text.
        //
        //   text:
        //     A text string.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        //
        //   rotation:
        //     Specifies the angle (in radians) to rotate the sprite about its center.
        //
        //   origin:
        //     The sprite origin; the default is (0,0) which represents the upper-left corner.
        //
        //   scale:
        //     Scale factor.
        //
        //   effects:
        //     Effects to apply.
        //
        //   layerDepth:
        //     The depth of a layer. By default, 0 represents the front layer and 1 represents
        //     a back layer. Use SpriteSortMode if you want sprites to be sorted during
        //     drawing.
        public static void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        //
        // Summary:
        //     Adds a string to a batch of sprites for rendering using the specified font,
        //     text, position, color, rotation, origin, scale, effects and layer.
        //
        // Parameters:
        //   spriteFont:
        //     A font for diplaying text.
        //
        //   text:
        //     Text string.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        //
        //   rotation:
        //     Specifies the angle (in radians) to rotate the sprite about its center.
        //
        //   origin:
        //     The sprite origin; the default is (0,0) which represents the upper-left corner.
        //
        //   scale:
        //     Scale factor.
        //
        //   effects:
        //     Effects to apply.
        //
        //   layerDepth:
        //     The depth of a layer. By default, 0 represents the front layer and 1 represents
        //     a back layer. Use SpriteSortMode if you want sprites to be sorted during
        //     drawing.
        public static void DrawString(SpriteFont spriteFont, System.Text.StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        //
        // Summary:
        //     Adds a string to a batch of sprites for rendering using the specified font,
        //     text, position, color, rotation, origin, scale, effects and layer.
        //
        // Parameters:
        //   spriteFont:
        //     A font for diplaying text.
        //
        //   text:
        //     Text string.
        //
        //   position:
        //     The location (in screen coordinates) to draw the sprite.
        //
        //   color:
        //     The color to tint a sprite. Use Color.White for full color with no tinting.
        //
        //   rotation:
        //     Specifies the angle (in radians) to rotate the sprite about its center.
        //
        //   origin:
        //     The sprite origin; the default is (0,0) which represents the upper-left corner.
        //
        //   scale:
        //     Scale factor.
        //
        //   effects:
        //     Effects to apply.
        //
        //   layerDepth:
        //     The depth of a layer. By default, 0 represents the front layer and 1 represents
        //     a back layer. Use SpriteSortMode if you want sprites to be sorted during
        //     drawing.
        public static void DrawString(SpriteFont spriteFont, System.Text.StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        //
        // Summary:
        //     Flushes the sprite batch and restores the device state to how it was before
        //     Begin was called.
        public static void End()
        {
            spriteBatch.End();
        }

    }
}

