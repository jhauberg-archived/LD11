using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Mantra.Framework;
using Mantra.Framework.Extensions;
using LD11.Visuals;
using Microsoft.Xna.Framework;
using Mantra.XNA;
using Mantra.XNA.Graphics;

namespace LD11.Logicals
{
    class GameStateController : Behavior
    {
        GameState state = GameState.None;

        KeyboardState ksLast;

        void SetPlayingEnabled(bool enabled)
        {
            var specks = Repository.Behaviors.Where
                (
                    x =>
                        x.Group.IndexOf(DirtProducer.DirtSpeck) != -1 &&
                        x is Transform // just to only grab one component from each group
                );

            // remove all previously made dirt specks
            Behavior[] speckTransforms = specks.ToArray();
            for (int i = 0; i < speckTransforms.Length; i++) {
                Repository.Delegater.UnbindAll(speckTransforms[i].Group);
            }

            var results = Repository.Behaviors.Where
                 (
                     x =>
                         x.Group == Groups.DirtProduction ||
                         x.Group == Groups.GameInformation ||
                         x.Group == Groups.Statue ||
                         x.Group.IndexOf(DirtProducer.DirtSpeck) != -1
                 );

            foreach (var result in results) {
                result.Enabled = enabled;

                if (result is Drawable) {
                    ((Drawable)result).Visible = enabled;
                }
            }

            if (enabled) {
                StatueInformation statueInfo = Repository.Get<StatueInformation>(Groups.Statue);

                GameInformation gameInfo = Repository.Get<GameInformation>(Groups.GameInformation);
                gameInfo.ResetTimer();
                gameInfo.IsCountingTime = true;

                DirtProducer producer = Repository.Get<DirtProducer>(Groups.DirtProduction);
                producer.SpawnContinuously = gameInfo.GameMode == GameMode.Regular ? false : true;
                producer.TimeBetweenSpawns = statueInfo.StatueSettings.TimeBetweenSpawns;
                producer.Spawn(statueInfo.StatueSettings.InitialAmountOfSpecks);

                Vector3 cameraOffset = new Vector3(0, 0, 1);
                float distanceToCenter = statueInfo.BoundingSphere.Radius / (float)Math.Sin(MathHelper.PiOver4 / 2);
                Vector3 back = Vector3.Backward;
                back.X = -back.X;
                Repository.Get<Transform>(Groups.Camera).Position = (back * distanceToCenter) + cameraOffset;
            }
        }

        void SetMenuEnabled(bool enabled)
        {
            var results = Repository.Behaviors.Where
                 (
                     x =>
                         x.Group == Groups.MenuStatuePreview ||
                         x.Group == Groups.MenuItemsDisplay
                 );

            foreach (var result in results) {
                result.Enabled = enabled;

                if (result is Drawable) {
                    ((Drawable)result).Visible = enabled;
                }
            }
        }

        void SetIntroEnabled(bool enabled)
        {
            var results = Repository.Behaviors.Where
                 (
                     x =>
                         x.Group == Groups.Intro
                 );

            foreach (var result in results) {
                result.Enabled = enabled;

                if (result is Drawable) {
                    ((Drawable)result).Visible = enabled;
                }
            }
        }

        void ApplyState()
        {
            switch (state) {
                case GameState.Menu: {
                        SetPlayingEnabled(false);
                        SetIntroEnabled(false);

                        SetMenuEnabled(true);
                    } break;
                case GameState.Playing: {
                        SetIntroEnabled(false);
                        SetMenuEnabled(false);

                        SetPlayingEnabled(true);
                    } break;
                case GameState.Intro: {
                        SetPlayingEnabled(false);
                        SetMenuEnabled(false);

                        SetIntroEnabled(true);
                    } break;
            }
        }

        public override void Update(TimeSpan elapsed)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyUp(Keys.Escape) && ksLast.IsKeyDown(Keys.Escape)) {
                GameState = GameState.Menu;

                if (GameContainer.SoundChannel == null) {
                    GameContainer.StartMusic(2);
                }
            }

            ksLast = ks;
        }

        public GameState GameState
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value) {
                    state = value;

                    ApplyState();
                }
            }
        }
    }
}
