using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mantra.Framework;
using LD11.Visuals;
using Mantra.XNA;

namespace LD11.Logicals
{
    class GameInformation : Behavior
    {
        [Dependency(Group = Groups.Statue)]
        StatueInformation statueInformation = null;

        GameMode gameMode = GameMode.Regular;

        bool isCountingTime = true;

        DateTime from;
        TimeSpan spent;

        float coverage;

        public override void Initialize()
        {
            base.Initialize();

            ResetTimer();
        }

        public override void Update(TimeSpan elapsed)
        {
            if (IsCountingTime) {
                spent = DateTime.Now - from;
            }

            var results = Repository.Behaviors.Where
            (
                x =>
                    x.Group.IndexOf(DirtProducer.DirtSpeck) != -1 &&
                    x is Transform
            );

            int dirtSpecks = 0;

            Behavior[] transforms = results.ToArray();

            for (int i = 0; i < transforms.Length; i++) {
                Transform dirtSpeckTransform = transforms[i] as Transform;

                // if a speck is scaling upwards then let it count as 2 specks
                dirtSpecks +=
                    dirtSpeckTransform.Scale.X > 1.75f ?
                        2 :
                        1;
            }

            int maxDirtPerModel = statueInformation.StatueSettings.MaximumAmountOfDirtPerTriangle * statueInformation.Triangles.Count;

            coverage = 100.0f * dirtSpecks / maxDirtPerModel;

            if (coverage <= 0.0f) {
                // won
                if (gameMode == GameMode.Regular) {
                    statueInformation.StatueSettings.RecordRegular = spent;
                } else if (gameMode == GameMode.Invasion) {
                    statueInformation.StatueSettings.RecordInvasion = spent;
                }

                IsCountingTime = false;
                Repository.Get<DirtProducer>(Groups.DirtProduction).SpawnContinuously = false;
            } else if (coverage >= 100.0f) {
                // lost
                coverage = 100;

                IsCountingTime = false;
                Repository.Get<DirtProducer>(Groups.DirtProduction).SpawnContinuously = false;
            }
        }

        public void ResetTimer()
        {
            from = DateTime.Now;
        }

        public GameMode GameMode
        {
            get
            {
                return gameMode;
            }
            set
            {
                if (gameMode != value) {
                    gameMode = value;
                }
            }
        }

        public float Coverage
        {
            get
            {
                return coverage;
            }
        }

        public bool IsCountingTime
        {
            get
            {
                return isCountingTime;
            }
            set
            {
                if (isCountingTime != value) {
                    isCountingTime = value;

                    from = DateTime.Now;
                }
            }
        }

        public TimeSpan TotalTimeSpent
        {
            get
            {
                return spent;
            }
        }
    }
}
