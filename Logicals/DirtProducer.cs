using System;
using System.Collections.Generic;
using System.Linq;

using Mantra.Framework;
using LD11.Visuals;
using Mantra.XNA;
using Microsoft.Xna.Framework;

namespace LD11.Logicals
{
    class DirtProducer : Behavior
    {
        [Dependency(Group = Groups.Statue)]
        StatueInformation statueInformation = null;

        public const string DirtSpeck = "Dirt Speck #";

        Random r = new Random();

        uint spawned = 0;

        bool spawnContinuously = false;
        TimeSpan timeBetweenSpawns = TimeSpan.FromSeconds(1);

        DateTime from;

        public override void Initialize()
        {
            base.Initialize();

            from = DateTime.Now;
        }

        public override void Update(TimeSpan elapsed)
        {
            if (spawnContinuously) {
                TimeSpan since = DateTime.Now - from;

                if (since > timeBetweenSpawns) {
                    Spawn(1);
                    from = DateTime.Now;
                }
            }
        }

        public void Spawn(int amount)
        {
            for (int i = 0; i < amount; i++) {
                string name = DirtSpeck + (++spawned);

                Triangle t = statueInformation.Triangles[r.Next(statueInformation.Triangles.Count)];

                Transform transform = new Transform();
                transform.Position = Triangle.GetRandomPointInside(r, 
                    t.A, 
                    t.B, 
                    t.C);

                AddScale grow = new AddScale();
                grow.Maximum = statueInformation.StatueSettings.MaximumDirtScale;
                grow.Minimum = transform.Scale;
                
                float n = (float)r.NextDouble() * 0.05f;

                grow.Amount = new Vector3(n);

                Dirt blob = new Dirt();
                blob.TriangleHost = t;

                Repository.Delegater.Bind(name, transform);
                Repository.Delegater.Bind(name, grow);
                Repository.Delegater.Bind(name, blob);
            }
        }

        public bool SpawnContinuously
        {
            get
            {
                return spawnContinuously;
            }
            set
            {
                spawnContinuously = value;
            }
        }

        public TimeSpan TimeBetweenSpawns
        {
            get
            {
                return timeBetweenSpawns;
            }
            set
            {
                timeBetweenSpawns = value;
            }
        }
    }
}
