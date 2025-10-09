using Clengine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceBar;

public class Game : ClengineCore {

    public Game() : base("Space Bar", 480, 720, false) {
    }

    protected override void Initialize() {
        base.Initialize();

        Input.Mouse.OnMouseMove += p => {
            System.Console.WriteLine("Mouse moved");
        };
    }

    protected override void LoadContent() {

    }

    protected override void Update(GameTime gameTime) {
        LogicGameTime = gameTime;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // SetWindowTitle(GetFrameRate().ToString());
        Input.Update();

 

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        RenderGameTime = gameTime;
        GraphicsDevice.SetRenderTarget(_renderTarget2D);
        GraphicsDevice.Clear(Color.CornflowerBlue);


        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
        //draw here
        SpriteBatch.End();


        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);


        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
        SpriteBatch.Draw(_renderTarget2D, RenderDestRect, Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);

        base.Draw(gameTime);
    }
}
