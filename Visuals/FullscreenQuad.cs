using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Mantra.XNA.Graphics;
using Microsoft.Xna.Framework;

namespace LD11.Visuals
{
    public class FullScreenQuad : Drawable
    {
        VertexDeclaration vertexDeclaration;
        VertexPositionColor[] vertices;

        readonly short[] indices = new short[] 
        { 
            0, 1, 2, 
            2, 3, 0 
        };

        Effect fx;

        Color top, bottom;

        GraphicsDevice device;

        public override void Initialize()
        {
            base.Initialize();

            device = GameContainer.Graphics.GraphicsDevice;

            fx = GameContainer.ContentManager.Load<Effect>("nectar\\shading\\no_transform");
            fx.CurrentTechnique = fx.Techniques["Main"];

            vertexDeclaration = new VertexDeclaration(
                device,
                VertexPositionColor.VertexElements);

            Vector2 min = new Vector2(-1.1f, -1.1f); // 0.1 offset to get rid of edges when using multisampling
            Vector2 max = new Vector2(1, 1);

            vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(
                    new Vector3(max.X, min.Y, 0),
                    Bottom),
                new VertexPositionColor(
                    new Vector3(min.X, min.Y, 0),
                    Bottom),
                new VertexPositionColor(
                    new Vector3(min.X, max.Y, 0),
                    Top),
                new VertexPositionColor(
                    new Vector3(max.X, max.Y, 0),
                    Top)
            };
        }

        public override void Draw()
        {
            fx.Begin();
            for (int i = 0; i < fx.CurrentTechnique.Passes.Count; i++) {
                EffectPass pass = fx.CurrentTechnique.Passes[i];

                pass.Begin();
                device.VertexDeclaration = vertexDeclaration;
                device.DrawUserIndexedPrimitives<VertexPositionColor>
                    (PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
                pass.End();
            }
            fx.End();
        }

        public Color Top
        {
            get
            {
                return top;
            }
            set
            {
                if (top != value) {
                    top = value;

                    if (vertices != null) {
                        vertices[2].Color = top;
                        vertices[3].Color = top;
                    }
                }
            }
        }

        public Color Bottom
        {
            get
            {
                return bottom;
            }
            set
            {
                if (bottom != value) {
                    bottom = value;

                    if (vertices != null) {
                        vertices[0].Color = bottom;
                        vertices[1].Color = bottom;
                    }
                }
            }
        }
    }
}
