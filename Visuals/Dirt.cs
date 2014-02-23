using System;
using System.Collections.Generic;
using System.Linq;

using Mantra.Framework;
using Mantra.Framework.Extensions;
using Mantra.XNA;
using Mantra.XNA.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LD11.Logicals;

namespace LD11.Visuals
{
    class Dirt : Drawable
    {
        [Dependency(Group = Groups.Camera)]
        DefaultCamera camera = null;

        [Dependency(Group = Groups.Statue)]
        Transform statueTransform = null;

        [Dependency]
        Transform transform = null;

        Model model;

        float alpha = 1;

        // the triangle the blob lives on
        Triangle triangle;

        Vector3 accumulatedForce;

        bool released;
        Vector3 velocity, localPosition;

        Matrix capturedStatueRotation;

        Random rand = new Random();

        public override void Initialize()
        {
            base.Initialize();

            model = GameContainer.ContentManager.Load<Model>("nectar\\geometry\\dirt");

            Repository.Get<StatueMouseController>(Groups.Statue).Shaken += new EventHandler<ShakeEventArgs>(Blob_Shaken);
        }

        void Blob_Shaken(object sender, ShakeEventArgs e)
        {
            Vector3 dirPlane = Vector3.Normalize(
                Vector3.Transform(triangle.Normal, Matrix.CreateFromQuaternion(statueTransform.Rotation)));

            dirPlane.Z = 0;

            float distance = Vector3.Distance(dirPlane, e.Direction);

            if (distance < 1.0f) {
                Vector3 f = e.Force * (1.0f - distance);

                accumulatedForce += f;

                if (accumulatedForce.Length() > 1) {
                    released = true;

                    velocity = dirPlane * (1 + (float)(rand.NextDouble() + 0.1));
                    
                    capturedStatueRotation = Matrix.CreateFromQuaternion(statueTransform.Rotation);
                }
            }
        }

        public override void Update(TimeSpan elapsed)
        {
            accumulatedForce *= 0.65f;

            if (released) {
                localPosition += velocity * (float)elapsed.TotalSeconds;

                alpha -= 0.8f * (float)elapsed.TotalSeconds;
            }

            if (alpha < 0.0f) {
                Repository.Delegater.UnbindAll(Group);
            }
        }
        
        public override void Draw()
        {
            /*
            GameContainer.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            GameContainer.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            GameContainer.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            */
            GameContainer.Graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
            
            foreach (ModelMesh mesh in model.Meshes) {
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.Alpha = alpha;

                    effect.View = camera.View;
                    effect.Projection = camera.Projection;

                    effect.World = 
                        Matrix.CreateScale(transform.Scale) * 
                        Matrix.CreateTranslation(
                            released ?
                                Vector3.Transform(transform.Position, capturedStatueRotation) + localPosition :
                                Vector3.Transform(transform.Position, statueTransform.World));

                    effect.EmissiveColor = Color.Red.ToVector3();
                    effect.DiffuseColor = Color.Gray.ToVector3();
                    effect.AmbientLightColor = Color.Red.ToVector3();
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.Direction = Vector3.Forward;
                    effect.PreferPerPixelLighting = true;
                    effect.DirectionalLight1.Enabled = false;
                    effect.DirectionalLight2.Enabled = false;
                    effect.SpecularPower = 500;
                    effect.SpecularColor = Color.Black.ToVector3();

                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }

            //GameContainer.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
        }

        public Triangle TriangleHost
        {
            get
            {
                return triangle;
            }
            set
            {
                triangle = value;
            }
        }
    }
}
