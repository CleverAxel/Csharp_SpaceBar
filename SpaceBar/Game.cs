using System;
using Clengine;
using Clengine.Texture;
using Clengine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceBar.Entities.Player;

namespace SpaceBar;

public class Game : ClengineCore {

    private PlayerTopDown _player;


    private Vector2 _position = new Vector2(0, 0);
    private Rectangle _rectangle = new Rectangle(0, 0, 30, 30);
    private Scale _scale = new Scale();

    public Game() : base("Space Bar", 560, 720, false) {
        _scale.SetBaseDimension(_rectangle.Width, _rectangle.Height).SetOrigin(new Vector2(0.5f, 0.5f));
    }

    protected override void Initialize() {
        base.Initialize();
        _player = new PlayerTopDown();
        _player.LoadContent();
    }

    protected override void LoadContent() {
    }

    protected override void Update(GameTime gameTime) {
        LogicGameTime = gameTime;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        SetWindowTitle(GetFrameRate().ToString());
        Input.Update();
        _player.Update();

        
        // _rectangle.X = (int)Math.Round(_position.X);
        // _rectangle.Y = (int)Math.Round(_position.Y);
 

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        RenderGameTime = gameTime;
        GraphicsDevice.SetRenderTarget(_renderTarget2D);
        GraphicsDevice.Clear(new Color(23, 32, 56));


        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
        //draw here
        _player.Draw();
        SpriteBatch.End();


        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);


        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
        SpriteBatch.Draw(_renderTarget2D, RenderDestRect, Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
