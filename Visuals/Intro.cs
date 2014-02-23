using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mantra.Framework;
using LD11.Logicals;
using Mantra.XNA.Graphics;

namespace LD11.Visuals
{
    class Intro : Drawable
    {
        [Dependency(Group = Groups.GameStateControl)]
        GameStateController stateController = null;

        const string Nickname = "shrt";
        const string Presents = "presents";
        const string Product = "a 48h production";

        Vector2 nickPos, nickVel;
        Vector2 presentsPos, presentsVel;
        Vector2 prodPos, prodVel;

        float nickSpeed, presentsSpeed, prodSpeed;

        float nickAlpha, presentsAlpha, prodAlpha;
        float nickAlphaIncrease, presentsAlphaIncrease, prodAlphaIncrease;

        bool drawHelp;

        Texture2D help;

        float helpAlpha;

        public override void Initialize()
        {
            base.Initialize();

            help = GameContainer.ContentManager.Load<Texture2D>("nectar\\ui\\help");

            nickPos = new Vector2(125, 250);
            nickVel = new Vector2(1, 0);
            nickSpeed = 105;
            nickAlphaIncrease = 0.7f;

            presentsPos = new Vector2(385, 280);
            presentsVel = new Vector2(-1, 0);
            presentsSpeed = 100;
            presentsAlphaIncrease = 0.4f;

            prodPos = new Vector2(485, 325);
            prodVel = new Vector2(-1, 0);
            prodSpeed = 75;
            prodAlphaIncrease = 0.25f;
        }

        public override void Update(TimeSpan elapsed)
        {
            float t = (float)elapsed.TotalSeconds;

            nickPos += (nickVel * nickSpeed) * t;
            nickAlpha += nickAlphaIncrease * t;

            if (nickAlpha > 1) {
                nickAlphaIncrease = -nickAlphaIncrease + (-0.2f);
            }

            presentsPos += (presentsVel * presentsSpeed) * t;
            presentsAlpha += presentsAlphaIncrease * t;

            if (presentsAlpha > 1) {
                presentsAlphaIncrease = -presentsAlphaIncrease + (-0.35f);
            }

            prodPos += (prodVel * prodSpeed) * t;
            prodAlpha += prodAlphaIncrease * t;

            if (prodAlpha > 1) {
                prodAlphaIncrease = -prodAlphaIncrease + (-0.55f);
            }

            if (prodAlpha < -0.8f) {
                drawHelp = true;

                helpAlpha += 0.8f * t;

                if (prodAlpha < -10) {
                    stateController.GameState = GameState.Menu;
                    GameContainer.StartMusic(2);
                }
            }
        }

        public override void Draw()
        {
            SpriteBatch sprite = GameContainer.sprite;
            GraphicsDevice device = GameContainer.Graphics.GraphicsDevice;

            Color color = Color.White;
            Color shadow = Color.Black;

            Vector4 rgba = color.ToVector4();
            Vector4 rgbaShadow = shadow.ToVector4();

            rgba.W = nickAlpha;
            color = new Color(rgba);
            rgbaShadow.W = nickAlpha;
            shadow = new Color(rgbaShadow);

            sprite.DrawString(GameContainer.calibri18, Nickname, nickPos + Vector2.One, shadow);
            sprite.DrawString(GameContainer.calibri18, Nickname, nickPos, color);

            rgba.W = presentsAlpha;
            color = new Color(rgba);
            rgbaShadow.W = presentsAlpha;
            shadow = new Color(rgbaShadow);

            sprite.DrawString(GameContainer.calibri18, Presents, presentsPos + Vector2.One, shadow);
            sprite.DrawString(GameContainer.calibri18, Presents, presentsPos, color);

            rgba.W = prodAlpha;
            color = new Color(rgba);
            rgbaShadow.W = prodAlpha;
            shadow = new Color(rgbaShadow);

            sprite.DrawString(GameContainer.calibri18, Product, prodPos + Vector2.One, shadow);
            sprite.DrawString(GameContainer.calibri18, Product, prodPos, color);

            if (drawHelp) {
                rgba.W = helpAlpha;
                color = new Color(rgba);

                sprite.Draw(help, new Vector2((device.Viewport.Width / 2) - (help.Width / 2), (device.Viewport.Height / 2) - (help.Height / 2)), color);
            }
        }
    }
}
