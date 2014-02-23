using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mantra.Framework;
using Mantra.XNA.Graphics;
using LD11.Logicals;

namespace LD11.Visuals
{
    class GameInformationDisplay : Drawable
    {
        [Dependency(Group = Groups.Statue)]
        StatueInformation statueInformation = null;

        [Dependency]
        GameInformation information = null;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw()
        {
            GraphicsDevice device = GameContainer.Graphics.GraphicsDevice;

            SpriteBatch sprite = GameContainer.sprite;
            SpriteFont segoe14 = GameContainer.segoe14;

            /*
             * Time
             */
            string timeText = String.Format("{0:00}:{1:00}", information.TotalTimeSpent.Minutes, information.TotalTimeSpent.Seconds);

            sprite.DrawString(segoe14, timeText, new Vector2(10, 10) + Vector2.One, Color.Black);
            sprite.DrawString(segoe14, timeText, new Vector2(10, 10), Color.White);

            /* 
             * Record Time
             */
            TimeSpan record = TimeSpan.Zero;

            if (information.GameMode == GameMode.Regular) {
                record = statueInformation.StatueSettings.RecordRegular;
            } else if (information.GameMode == GameMode.Invasion) {
                record = statueInformation.StatueSettings.RecordInvasion;
            }

            if (record.Seconds > 0) {
                string fastestText = "Record:";
                Vector2 fastestTextSize = segoe14.MeasureString(fastestText);

                string recordText = String.Format("{0:00}:{1:00}", record.Minutes, record.Seconds);
                Vector2 recordTextSize = segoe14.MeasureString(recordText);

                sprite.DrawString(segoe14, fastestText, new Vector2(10, device.Viewport.Height - fastestTextSize.Y - recordTextSize.Y) + Vector2.One, Color.Black);
                sprite.DrawString(segoe14, fastestText, new Vector2(10, device.Viewport.Height - fastestTextSize.Y - recordTextSize.Y), Color.White);

                sprite.DrawString(segoe14, recordText, new Vector2(20, device.Viewport.Height - recordTextSize.Y - 5) + Vector2.One, Color.Black);
                sprite.DrawString(segoe14, recordText, new Vector2(20, device.Viewport.Height - recordTextSize.Y - 5), Color.White);
            }

            /*
             * Coverage
             */
            string coverageText = String.Format("{0:0}%", information.Coverage);

            Vector2 coverageTextSize = segoe14.MeasureString(coverageText);
            Vector2 coverageTextPosition = new Vector2(
                (device.Viewport.Width) - (coverageTextSize.X) - 10, 10);

            sprite.DrawString(segoe14, coverageText, coverageTextPosition + Vector2.One, Color.Black);
            sprite.DrawString(segoe14, coverageText, coverageTextPosition, Color.White);
        }
    }
}
