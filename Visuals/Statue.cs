using System;
using System.Collections.Generic;
using System.Linq;

using Mantra.Framework;
using Mantra.Framework.Extensions;
using Mantra.XNA;
using Mantra.XNA.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD11.Visuals
{
    class Statue : Drawable
    {
        [Dependency(Group = Groups.Camera)]
        DefaultCamera camera = null;

        [Dependency]
        Transform transform = null;
        [Dependency]
        StatueInformation information = null;

        Effect lambert;

        EffectParameter fxTexture;
        EffectParameter fxViewInverted;
        EffectParameter fxWorldInvertedTranspose;
        EffectParameter fxWvp;
        EffectParameter fxWorld;

        Vector3 specular = new Vector3(0.6f, 0.6f, 0.6f);
        Vector3 ambient = new Vector3(0.6f, 0.6f, 0.6f);

        Vector3 lampOffset = (Vector3.Backward * 5) + (Vector3.Up * 5);

        Texture2D texture;

        public override void Initialize()
        {
            base.Initialize();

            lambert = GameContainer.ContentManager.Load<Effect>("nectar\\shading\\lambert");
            lambert.CurrentTechnique = lambert.Techniques["Main"];

            fxTexture = lambert.Parameters["ColorTexture"];
            fxViewInverted = lambert.Parameters["ViewIXf"];
            fxWorld = lambert.Parameters["WorldXf"];
            fxWorldInvertedTranspose = lambert.Parameters["WorldITXf"];
            fxWvp = lambert.Parameters["WvpXf"];
        }

        public override void Draw()
        {
            Model model = information.Model;
            
            // bad, bad code!
            texture = GameContainer.ContentManager.Load<Texture2D>(information.StatueSettings.TextureName);
            for (int i = 0; i < model.Meshes.Count; i++) {
                for (int j = 0; j < model.Meshes[i].MeshParts.Count; j++) {
                    model.Meshes[i].MeshParts[j].Effect = lambert;
                }
            }

            fxTexture.SetValue(texture);
            fxViewInverted.SetValue(Matrix.Invert(camera.View));

            fxWorldInvertedTranspose.SetValueTranspose(Matrix.Invert(transform.World));
            fxWvp.SetValue(transform.World * camera.View * camera.Projection);
            fxWorld.SetValue(transform.World);

            lambert.Parameters["Lamp0Pos"].SetValue(
                Matrix.Invert(camera.View).Translation + lampOffset);

            lambert.Parameters["Lamp0Color"].SetValue(specular);
            lambert.Parameters["AmbiColor"].SetValue(ambient);

            lambert.CommitChanges();

            foreach (ModelMesh mesh in model.Meshes) {
                mesh.Draw();
            }
        }
    }
}
