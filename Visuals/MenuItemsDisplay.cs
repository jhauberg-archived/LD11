using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mantra.Framework;
using LD11.Logicals;
using Mantra.XNA.Graphics;

namespace LD11.Visuals
{
    class MenuItemsDisplay : Drawable
    {
        const string SelectGameMode = "Press <SPACE> to switch game mode:";
        const string Play = "Press <ENTER> to play!";

        [Dependency(Group = Groups.GameStateControl)]
        GameStateController stateController = null;
        [Dependency(Group = Groups.GameInformation)]
        GameInformation gameInformation = null;
        [Dependency(Group = Groups.Statue)]
        StatueInformation statueInformation = null;

        [Dependency(Group = Groups.MenuStatuePreview)]
        StatueInformation menuStateInfo = null;

        /* mode selection */
        bool regular = true;

        float modeYGravity = 5;
        float modeYPosition = 135;
        float modeYVelocity = 0;
        float modeYFloor = 135;

        /* statue selection */
        int selectedStatueIndex = 0;

        Texture2D arrowleft, arrowright;
        Texture2D longbar;

        KeyboardState ksLast;

        public override void Initialize()
        {
            base.Initialize();

            arrowleft = GameContainer.ContentManager.Load<Texture2D>("nectar\\ui\\arrow_left");
            arrowright = GameContainer.ContentManager.Load<Texture2D>("nectar\\ui\\arrow_right");
            longbar = GameContainer.ContentManager.Load<Texture2D>("nectar\\ui\\long_bar");
        }

        public override void Update(TimeSpan elapsed)
        {
            modeYVelocity += modeYGravity * 0.08f;
            modeYPosition += modeYVelocity;

            if (modeYPosition > modeYFloor) {
                modeYVelocity = -5;
                modeYPosition = modeYFloor;
            }

            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyUp(Keys.Space) && ksLast.IsKeyDown(Keys.Space)) {
                regular = !regular;

                gameInformation.GameMode = regular ? GameMode.Regular : GameMode.Invasion;

                modeYVelocity = 0;
                modeYPosition = modeYFloor;
            }

            if (ks.IsKeyUp(Keys.Left) && ksLast.IsKeyDown(Keys.Left)) {
                selectedStatueIndex--;
            }
            if (ks.IsKeyUp(Keys.Right) && ksLast.IsKeyDown(Keys.Right)) {
                selectedStatueIndex++;
            }

            if (selectedStatueIndex < 0) {
                selectedStatueIndex = GameContainer.StatueSettings.Length - 1;
            } else if (selectedStatueIndex > GameContainer.StatueSettings.Length - 1) {
                selectedStatueIndex = 0;
            }

            statueInformation.StatueSettings = GameContainer.StatueSettings[selectedStatueIndex];
            menuStateInfo.StatueSettings = GameContainer.StatueSettings[selectedStatueIndex];

            if (ks.IsKeyUp(Keys.Enter) && ksLast.IsKeyDown(Keys.Enter)) {
                stateController.GameState = GameState.Playing;
            }

            ksLast = ks;
        }

        public override void Draw()
        {
            GraphicsDevice device = GameContainer.Graphics.GraphicsDevice;
            SpriteBatch sprite = GameContainer.sprite;

            SpriteFont calibri18 = GameContainer.calibri18;

            /* mode selection */
            Vector2 selectModeSize = GameContainer.calibri16.MeasureString(SelectGameMode);
            Vector2 selectModePosition = new Vector2(
                (device.Viewport.Width / 2) - (selectModeSize.X / 2),
                45);

            sprite.Draw(longbar, new Vector2((device.Viewport.Width / 2) - (longbar.Width / 2), selectModePosition.Y - (longbar.Height / 4)), Color.White);

            sprite.DrawString(GameContainer.calibri16, SelectGameMode, selectModePosition + Vector2.One, Color.Black);
            sprite.DrawString(GameContainer.calibri16, SelectGameMode, selectModePosition, Color.White);

            string regularText = GameMode.Regular.ToString();
            Vector2 regularTextSize = calibri18.MeasureString(regularText);
            Vector2 regularTextPosition = new Vector2(
                (device.Viewport.Width / 2) - (regularTextSize.X) - 25,
                regular ? modeYPosition : modeYFloor);

            sprite.DrawString(calibri18, regularText, regularTextPosition + Vector2.One, Color.Black);
            sprite.DrawString(calibri18, regularText, regularTextPosition, regular ? Color.Coral : Color.White);

            string invasionText = GameMode.Invasion.ToString();
            Vector2 invasionTextSize = calibri18.MeasureString(invasionText);
            Vector2 invasionTextPosition = new Vector2(
                (device.Viewport.Width / 2) + (invasionTextSize.X) - 50,
                regular ? modeYFloor : modeYPosition);

            sprite.DrawString(calibri18, invasionText, invasionTextPosition + Vector2.One, Color.Black);
            sprite.DrawString(calibri18, invasionText, invasionTextPosition, regular ? Color.White : Color.Coral);

            /* statue selection */
            string statueName = GameContainer.StatueSettings[selectedStatueIndex].DisplayName;

            if (statueName.Length > 10) {
                statueName = statueName.Substring(0, 8) + "..";
            }

            Vector2 statueNameSize = calibri18.MeasureString(statueName);
            Vector2 statueNamePosition = new Vector2((device.Viewport.Width / 2) - (statueNameSize.X / 2), device.Viewport.Height - statueNameSize.Y - 125);

            sprite.DrawString(calibri18, statueName, statueNamePosition + Vector2.One, Color.Black);
            sprite.DrawString(calibri18, statueName, statueNamePosition, Color.White);

            sprite.Draw(arrowleft, new Vector2(300, statueNamePosition.Y), Color.White);
            sprite.Draw(arrowright, new Vector2(465, statueNamePosition.Y), Color.White);

            Vector2 playTextSize = calibri18.MeasureString(Play);
            Vector2 playTextPosition = new Vector2((device.Viewport.Width / 2) - (playTextSize.X / 2), device.Viewport.Height - playTextSize.Y - 35);

            sprite.Draw(longbar, new Vector2((device.Viewport.Width / 2) - (longbar.Width / 2), playTextPosition.Y - (longbar.Height / 4)), Color.Gray);

            sprite.DrawString(calibri18, Play, playTextPosition + Vector2.One, Color.Black);
            sprite.DrawString(calibri18, Play, playTextPosition, Color.White);
        }
    }
}
