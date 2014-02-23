using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Mantra.Framework;
using Mantra.Framework.Extensions;
using Mantra.XNA;
using Mantra.XNA.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD11.Visuals
{
    class StatueInformation : Behavior
    {
        Model model;

        List<Triangle> triangles = new List<Triangle>();
        ReadOnlyCollection<Triangle> trianglesReadOnly;

        BoundingSphere boundaries;

        StatueSettings currentSettings;

        public StatueInformation()
        {
            trianglesReadOnly = new ReadOnlyCollection<Triangle>(triangles);
        }

        public override void Initialize()
        {
            base.Initialize();

            model = GameContainer.ContentManager.Load<Model>(currentSettings.FileName);

            Dictionary<string, object> tagData = (Dictionary<string, object>)model.Tag;

            boundaries = (BoundingSphere)tagData["BoundingSphere"];
            Vector3[] vertices = (Vector3[])tagData["Vertices"];

            triangles.Clear();

            for (int i = 0; i < vertices.Length; i += 3) {
                Triangle t = new Triangle(
                    vertices[i],
                    vertices[i + 1],
                    vertices[i + 2]);

                triangles.Add(t);
            }
        }

        public Model Model
        {
            get
            {
                return model;
            }
        }

        public StatueSettings StatueSettings
        {
            get
            {
                return currentSettings;
            }
            set
            {
                if (currentSettings != value) {
                    currentSettings = value;

                    Initialize();
                }
            }
        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                return boundaries;
            }
        }

        public ReadOnlyCollection<Triangle> Triangles
        {
            get
            {
                return trianglesReadOnly;
            }
        }
    }
}
