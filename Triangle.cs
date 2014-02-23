using System;

using Microsoft.Xna.Framework;

namespace LD11
{
    struct Triangle
    {
        public Vector3 A, B, C;

        Vector3 normal;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            this.A = a;
            this.B = b;
            this.C = c;

            normal = CalculateNormal(a, b, c);
        }

        public Vector3 Normal
        {
            get
            {
                return normal;
            }
        }

        public static Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            return 
                Vector3.Normalize(
                    Vector3.Cross(
                        a - b,
                        c - b));
        }

        public static Vector3 GetRandomPointInside(Random random, Vector3 a, Vector3 b, Vector3 c)
        {
            float a1 = (float)random.NextDouble();
            float a2 = (float)random.NextDouble();

            if (a1 + a2 > 1.0f) {
                a1 = 1.0f - a1;
                a2 = 1.0f - a2;
            }

            float a3 = 1.0f - a1 - a2;

            return
                a1 * a +
                a2 * b +
                a3 * c;

            //"P = a1*V1 + a2*V2 + a3*V3
            //pick both a1 and a2 randomly in [0,1]. If a1 + a2 > 1 then set a1 = 1 - a1 and a2 = 1 - a2. Now (in either case), set a3 = 1 - a1 - a2."
        }
    }
}
