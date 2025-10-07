using System;
using Clengine.External.SDL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Clengine {
    public class ClengineCore : Game {
        public static ClengineCore Instance { get; private set; }


        public static GraphicsDeviceManager Graphics { get; private set; }
        public static new GraphicsDevice GraphicsDevice { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static new ContentManager Content { get; private set; }


        public static GameTime LogicGameTime { get; protected set; }
        public static float LogicDeltaTime => LogicGameTime.ElapsedGameTime.Milliseconds / 1000.0f;


        public static GameTime RenderGameTime { get; protected set; }
        public static float RenderDeltaTime => RenderGameTime.ElapsedGameTime.Milliseconds / 1000.0f;


        public static ref readonly Rectangle RenderDestRect => ref Instance._renderDestRect;
        public static bool WindowHasFocus => Instance.IsActive;


        public float OperatingSystemScale { get; protected set; }
        public int VirtualWidth { get; protected set; }
        public int VirtualHeight { get; protected set; }
        public bool IsResizingWindow { get; protected set; }


        protected Rectangle _renderDestRect = Rectangle.Empty;
        protected RenderTarget2D _renderTarget2D;

        public ClengineCore(string title, int virtualWidth, int virtualHeight, bool fullScreen) {
            if (Instance != null) {
                throw new InvalidOperationException("Only a single Core instance can be created");
            }

            Instance = this;
            Graphics = new GraphicsDeviceManager(this);
            IntPtr sdlHandle = Window.Handle;
            OperatingSystemScale = GetDisplayScale(sdlHandle);

            GraphicsAdapter adapter = GraphicsAdapter.DefaultAdapter;

            Graphics.PreferredBackBufferWidth = (int)(adapter.CurrentDisplayMode.Width - adapter.CurrentDisplayMode.Width * 0.33f);// (int)(virtualWidth * OperatingSystemScale);
            Graphics.PreferredBackBufferHeight = (int)(adapter.CurrentDisplayMode.Height - adapter.CurrentDisplayMode.Height * 0.33f);// (int)(virtualHeight * OperatingSystemScale);

            OperatingSystemScale = 1f / OperatingSystemScale;

            Graphics.IsFullScreen = fullScreen;
            Graphics.ApplyChanges();

            Window.Title = title;

            Content = base.Content;
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
        }

        public static void EnableVSYNC() {
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.ApplyChanges();
        }

        public static void DisableVSYNC() {
            Graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.ApplyChanges();
        }

        public static void CapFrameRateAt(int fps) {
            Instance.IsFixedTimeStep = true;
            Instance.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / fps);
        }

        public static float GetFrameRate() {
            return 1.0f / LogicDeltaTime; 
        }

        public static void SetWindowTitle(string title) {
            Instance.Window.Title = title;
        }

        protected override void Initialize() {
            base.Initialize();

            GraphicsDevice = base.GraphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            DisableVSYNC();
            CapFrameRateAt(60);
            InitializeRenderTarget();
        }

        private void InitializeRenderTarget() {
            _renderTarget2D = new RenderTarget2D(GraphicsDevice, VirtualWidth, VirtualHeight);
            Window.ClientSizeChanged += OnWindowResize;
            CalculateRenderRectangle();
        }

        private void OnWindowResize(object sender, EventArgs e) {
            if (!IsResizingWindow)
                CalculateRenderRectangle();
        }

        private void CalculateRenderRectangle() {
            IsResizingWindow = true;
            float windowWidth = Window.ClientBounds.Width;
            float windowHeight = Window.ClientBounds.Height;

            float scaleX = windowWidth / _renderTarget2D.Width;
            float scaleY = windowHeight / _renderTarget2D.Height;
            float scale = scaleX < scaleY ? scaleX : scaleY;

            _renderDestRect.Width = (int)(_renderTarget2D.Width * scale);
            _renderDestRect.Height = (int)(_renderTarget2D.Height * scale);

            _renderDestRect.X = (int)(windowWidth - _renderDestRect.Width) / 2;
            _renderDestRect.Y = (int)(windowHeight - _renderDestRect.Height) / 2;
            IsResizingWindow = false;
        }

        private float GetDisplayScale(IntPtr sdlWindowHandle) {
            int displayIndex = SDL.SDL_GetWindowDisplayIndex(sdlWindowHandle);
            if (displayIndex < 0)
                return 1f;

            if (SDL.SDL_GetDisplayDPI(displayIndex, out float ddpi, out float hdpi, out float vdpi) == 0) {
                return ddpi / 96f;
            } else {
                return 1f;
            }
        }
    }
}