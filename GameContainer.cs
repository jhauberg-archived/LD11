using System;
using System.Collections.Generic;
using System.Linq;

using Mantra.Framework;
using Mantra.Framework.Extensions;

using Mantra.XNA;
using Mantra.XNA.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using LD11.Visuals;
using LD11.Logicals;
using Microsoft.Xna.Framework.Input;

namespace LD11
{
    sealed class Groups
    {
        public const string Camera = "Camera";
        public const string FrameRate = "FPS";

        /* Playing*/
        public const string Statue = "Statue";
        public const string Background = "Background";
        public const string DirtProduction = "Dirt Production";
        public const string GameInformation = "Game Information";

        /* Menu */
        public const string MenuStatuePreview = "Preview Statue";
        public const string MenuItemsDisplay = "Menu Items";

        /* Intro */
        public const string Intro = "Intro";

        public const string GameStateControl = "Game State Control";
    }

    enum GameMode
    {
        Invasion,
        Regular
    }

    enum GameState
    {
        None,
        Intro,
        Menu,
        Playing,
        Won,
        Lost
    }

    class GameContainer : Game
    {
        public static readonly StatueSettings[] StatueSettings =
        {
            new StatueSettings("Cube", "nectar\\geometry\\cube", "nectar\\geometry\\sandish%0", TimeSpan.FromSeconds(1), 16, 80, new Vector3(5)),
            new StatueSettings("Cylindrical", "nectar\\geometry\\cylinder", "nectar\\geometry\\sandish%0", TimeSpan.FromSeconds(0.6), 10, 100, new Vector3(5)),
            new StatueSettings("Banan", "nectar\\geometry\\banan", "nectar\\geometry\\yellowish%0", TimeSpan.FromSeconds(0.6), 6, 100, new Vector3(5)),
            new StatueSettings("Shroom", "nectar\\geometry\\shroom", "nectar\\geometry\\blueish%0", TimeSpan.FromSeconds(0.8), 10, 100, new Vector3(5)),
            new StatueSettings("Snabel", "nectar\\geometry\\snabel", "nectar\\geometry\\greenish%0", TimeSpan.FromSeconds(0.8), 2, 70, new Vector3(2))
        };

        public static GraphicsDeviceManager Graphics;
        public static ContentManager ContentManager;

        public static FMOD.System SoundSystem;
        public static FMOD.Channel SoundChannel;

        public static FMOD.Sound[] tunes = new FMOD.Sound[4];

        public static int tuneIndex = 0;

        List<Mantra.Framework.IUpdateable> modules = new List<Mantra.Framework.IUpdateable>();

        Repository repo;

        Drawing drawing;
        Timing timing;

        FrameRateCounter fps;

        public static SpriteBatch sprite;

        public static SpriteFont calibri16, calibri18;
        public static SpriteFont segoe14;

        public static void StartMusic(int n)
        {
            if (SoundChannel != null) {
                SoundChannel.stop();
            }

            SoundSystem.playSound(FMOD.CHANNELINDEX.FREE, tunes[n], false, ref SoundChannel);
            SoundChannel.setVolume(0.5f);

            tuneIndex = n;
        }

        public GameContainer()
        {
            Graphics = new GraphicsDeviceManager(this);

            Graphics.PreferMultiSampling = true;
            Graphics.PreferredBackBufferHeight = 600;
            Graphics.PreferredBackBufferWidth = 800;
            
            Graphics.MinimumPixelShaderProfile = ShaderProfile.PS_3_0;
            Graphics.MinimumVertexShaderProfile = ShaderProfile.VS_3_0;

            ContentManager = Content;

            Window.Title = "Shaken, not stirred.     (Jacob H. Hansen @ LD11)";

            this.IsMouseVisible = true;

            // fmod bullshit
            FMOD.Factory.System_Create(ref SoundSystem);
            SoundSystem.init(32, FMOD.INITFLAG.NORMAL, (IntPtr)null);

            SoundSystem.createSound("nectar\\music\\shrt-beat2.ogg", FMOD.MODE.HARDWARE, ref tunes[0]);
            SoundSystem.createSound("nectar\\music\\shrt-vildere_lort.ogg", FMOD.MODE.HARDWARE, ref tunes[1]);
            SoundSystem.createSound("nectar\\music\\shrt-sving_om.ogg", FMOD.MODE.HARDWARE, ref tunes[2]);
            SoundSystem.createSound("nectar\\music\\shrt-beat3.ogg", FMOD.MODE.HARDWARE, ref tunes[3]);

            foreach (FMOD.Sound s in tunes) {
                s.setMode(FMOD.MODE.LOOP_NORMAL);
            }
        }

        protected override void Initialize()
        {
            sprite = new SpriteBatch(GraphicsDevice);
            
            segoe14 = Content.Load<SpriteFont>("nectar\\ui\\segoe14");
            calibri16 = Content.Load<SpriteFont>("nectar\\ui\\calibri16");
            calibri18 = Content.Load<SpriteFont>("nectar\\ui\\calibri18");

            InitializeModules();
            InitializeBehaviors();

            base.Initialize();
        }

        KeyboardState ksLast;

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyUp(Keys.Tab) && ksLast.IsKeyDown(Keys.Tab)) {
                tuneIndex++;

                if (tuneIndex > tunes.Length - 1) {
                    tuneIndex = 0;
                }

                StartMusic(tuneIndex);
            }
            if ((ks.IsKeyUp(Keys.F1) && ksLast.IsKeyDown(Keys.F1)) ||
                (ks.IsKeyUp(Keys.Back) && ksLast.IsKeyDown(Keys.Back))) {
                if (SoundChannel != null) {
                    bool isPlaying = false;
                    SoundChannel.isPlaying(ref isPlaying);

                    if (isPlaying) {
                        SoundChannel.stop();
                    } else {
                        StartMusic(tuneIndex);
                    }
                }
                
            }

            timing.Elapsed = gameTime.ElapsedGameTime;

            for (int i = 0; i < modules.Count; i++) {
                if (modules[i].Enabled) {
                    modules[i].Update();
                }
            }

            ksLast = ks;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            sprite.Begin(SpriteBlendMode.AlphaBlend);
                drawing.Update();
            sprite.End();

            base.Draw(gameTime);
        }

        void InitializeModules()
        {
            repo = new Repository();

            if (modules.Count > 0) {
                modules.Clear();
            }

            drawing = new Drawing();
            drawing.Enabled = false; // manual updating (so it only happens when XNA wants to draw)
            drawing.Subscribe(repo.Delegater);

            timing = new Timing();
            timing.Subscribe(repo.Delegater);

            modules.Add(drawing);
            modules.Add(timing);
        }

        void InitializeBehaviors()
        {
            Delegater delegater = repo.Delegater;

            /*
             * FPS
             */
            fps = new FrameRateCounter();
            delegater.Bind(Groups.FrameRate, fps);

            /* 
             * Camera
             */
            DefaultCamera camera = new DefaultCamera(GraphicsDevice) 
            { 
                ClearColor = Color.CornflowerBlue 
            };

            delegater.Bind(Groups.Camera, camera);

            /* 
             * Background
             */
            // bug: spritebatch.Begin(AlphaBlend) makes sure that this quad gets drawn in the background
            // which means that some important renderstates are getting set, but not by me
            FullScreenQuad background = new FullScreenQuad()
            {
                Top = new Color(25, 31, 32),
                Bottom = new Color(71, 81, 90),
                DrawOrder = 0
            };

            delegater.Bind(Groups.Background, background);

            /*
             * Statue
             */
            StatueInformation statueInformation = new StatueInformation() 
            { 
                StatueSettings = StatueSettings[0]
            };

            delegater.Bind(Groups.Statue, statueInformation);

            Statue statue = new Statue()
            {
                DrawOrder = 2 // to fix some troubles with blobs getting viewed through the statue
            };

            delegater.Bind(Groups.Statue, statue);

            StatueMouseController statueController = new StatueMouseController();
            delegater.Bind(Groups.Statue, statueController);

            /* 
             * Game Information
             */
            GameInformation gameInfo = new GameInformation()
            {
                GameMode = GameMode.Regular
            };

            delegater.Bind(Groups.GameInformation, gameInfo);

            GameInformationDisplay gameInfoDisplay = new GameInformationDisplay()
            {
                DrawOrder = 3
            };

            delegater.Bind(Groups.GameInformation, gameInfoDisplay);

            /*
             * Blob Production
             */
            DirtProducer blobProducer = new DirtProducer();
            delegater.Bind(Groups.DirtProduction, blobProducer);

            /* 
             * Game State Control
             */
            GameStateController stateController = new GameStateController();

            delegater.Bind(Groups.GameStateControl, stateController);

            /*
             * Menu stuff
             */
            StatueInformation menuStatueInformation = new StatueInformation()
            {
                StatueSettings = StatueSettings[0]
            };

            delegater.Bind(Groups.MenuStatuePreview, menuStatueInformation);

            Statue menuStatue = new Statue()
            {
                DrawOrder = 2 
            };

            delegater.Bind(Groups.MenuStatuePreview, menuStatue);

            StatuePreviewSpinner statueSpinner = new StatuePreviewSpinner();

            delegater.Bind(Groups.MenuStatuePreview, statueSpinner);

            MenuItemsDisplay menuItems = new MenuItemsDisplay()
            {
                DrawOrder = 3
            };

            delegater.Bind(Groups.MenuItemsDisplay, menuItems);

            /*
             * Intro
             */
            Intro intro = new Intro();

            delegater.Bind(Groups.Intro, intro);



            // omg, lousy hack.. these two lines makes sure we get to see the damn preview model in menu screen :P
            // im too tired to find the reason.. the code is such a mess already
            stateController.GameState = GameState.Playing;
            stateController.GameState = GameState.Menu;
            stateController.GameState = GameState.Intro;
        }
    }
}
