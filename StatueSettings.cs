using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LD11
{
    class StatueSettings
    {
        string name, filename;
        string texturename;

        int initialSpeckAmount;

        Vector3 maxSpeckScale;

        TimeSpan timeBetweenSpawns; // only in invasion mode
        int maxDirtPerTri;

        TimeSpan recordRegular, recordInvasion;

        public StatueSettings(string name, string filename, string texturename)
            : this(name, filename, texturename, TimeSpan.Zero, 5, 25, new Vector3(2)) { }

        public StatueSettings(string name, string filename, string texturename, TimeSpan timeBetweenSpawns, int maxDirtPerTri, int initialSpeckAmount, Vector3 maxSpeckScale)
        {
            this.name = name;
            this.filename = filename;
            this.texturename = texturename;

            this.timeBetweenSpawns = timeBetweenSpawns;
            this.maxDirtPerTri = maxDirtPerTri;
            this.initialSpeckAmount = initialSpeckAmount;

            this.maxSpeckScale = maxSpeckScale;
        }

        public Vector3 MaximumDirtScale
        {
            get
            {
                return maxSpeckScale;
            }
        }

        public string DisplayName
        {
            get
            {
                return name;
            }
        }

        public string FileName
        {
            get
            {
                return filename;
            }
        }

        public string TextureName
        {
            get
            {
                return texturename;
            }
        }

        public TimeSpan TimeBetweenSpawns
        {
            get
            {
                return timeBetweenSpawns;
            }
        }

        public TimeSpan RecordRegular
        {
            get
            {
                return recordRegular;
            }
            set
            {
                recordRegular = value;
            }
        }

        public TimeSpan RecordInvasion
        {
            get
            {
                return recordInvasion;
            }
            set
            {
                recordInvasion = value;
            }
        }

        public int InitialAmountOfSpecks
        {
            get
            {
                return initialSpeckAmount;
            }
            set
            {
                initialSpeckAmount = value;
            }
        }

        public int MaximumAmountOfDirtPerTriangle
        {
            get
            {
                return maxDirtPerTri;
            }
        }
    }
}
